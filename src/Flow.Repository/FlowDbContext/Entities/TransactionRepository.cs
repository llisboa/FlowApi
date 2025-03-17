using Dapper;
using Flow.Domain.Contracts.Repository.Entities;
using Flow.Domain.Models.DTO;
using Flow.Domain.Models.Input;
using Flow.Domain.Models.Output;

namespace Flow.Repository.FlowDbContext.Entities;

public class TransactionRepository : ITransactionRepository
{
    private readonly PostgreDbContext _databaseContext;
    public TransactionRepository(PostgreDbContext databaseContext) => _databaseContext = databaseContext;

    /// <summary>
    /// Gravação de transação, atualização de saldos
    /// </summary>
    /// <param name="putTransactionIn"></param>
    /// <returns></returns>
    // TODO: Ajust name cta TOTAL. Is incorrect...sugest "resume"
    public async Task<TraceKeyOut> AddTransactionAsync(PutTransactionIn putTransactionIn)
    {
        var sql = """
            with ins_trans as (
                insert into flow.transaction (branch,account,
                    amount,debitcredit,
                    eventcode,description,
                    tracekey, details, dateref)
                    select @Branch, @Account, @Amount, 
                    @DebitCredit, @EventCode, @Description,  
                    @TraceKey::uuid, @Details::json, @DateRef
                RETURNING *
            )
            , block_day as (
                select * from flow.balance where (branch,account,dateref) in 
                    (select balance.branch,balance.account,balance.dateref from 
                    ins_trans inner join flow.balance on 
                    ins_trans.branch = balance.branch and
                    ins_trans.account = balance.account and
                    ins_trans.dateref <= balance.dateref) for update 
            )
            , acum as (
                select balance.branch, balance.account, balance.dateref, 
                    balance.acumulateddebit, balance.acumulatedcredit 
                from flow.balance inner join ins_trans
                    on balance.branch=ins_trans.branch and balance.account=ins_trans.account
                    and balance.dateref<ins_trans.dateref
                order by balance.dateref desc limit 1
            )
            , total as (
                select branch, account,
                    case when debitcredit='D' then amount else 0 end totaldebit,
                    case when debitcredit='C' then amount else 0 end totalcredit
                from ins_trans
            )
            , ins_balance as (
                insert into flow.balance(branch,account,dateref,
                    totaldebit,totalcredit,acumulateddebit,acumulatedcredit,tracekey)
                select ins_trans.branch, ins_trans.account,
                    ins_trans.dateref, 
                    total.totaldebit,
                    total.totalcredit,
                    coalesce(acum.acumulateddebit,0)+total.totaldebit,
                    coalesce(acum.acumulatedcredit,0)+total.totalcredit,
                    tracekey
                from 
                    ins_trans 
                    left join acum on ins_trans.branch=acum.branch and ins_trans.account=acum.account
                    join total on ins_trans.branch=total.branch and ins_trans.account=total.account        	
                on conflict (branch,account,dateref)
                do update set 
                    totaldebit = balance.totaldebit + coalesce(EXCLUDED.totaldebit,0),
                    totalcredit = balance.totalcredit + coalesce(EXCLUDED.totalcredit,0),
                    acumulateddebit = balance.acumulateddebit + coalesce(EXCLUDED.totaldebit,0),
                    acumulatedcredit = balance.acumulatedcredit + coalesce(EXCLUDED.totalcredit,0)
                returning *
            ), update_next_balances as (
                update flow.balance 
                set 
                    acumulateddebit = acumulateddebit + total.totaldebit,
                    acumulatedcredit = acumulatedcredit + total.totalcredit
                from ins_trans,total
                where balance.dateref > ins_trans.dateref and
                    balance.branch = ins_trans.branch and
                    balance.account = ins_trans.account
            )
            select id::text, tracekey::text from ins_trans
        """;

        using var conn = await _databaseContext.GetConnection();
        var result = await conn.QuerySingleAsync<TraceKeyOut>(sql, putTransactionIn);
        return result;
    }

    /// <summary>
    /// Carga de dados de transações (tabela transaction)
    /// </summary>
    /// <param name="getTransactionIn"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TransactionDTO>> GetTransactionAsync(GetTransactionIn getTransactionIn)
    {
        List<string> conditions = new();
        conditions.Add("(dateref between @DateRefStart and @DateRefEnd)");
        if (!String.IsNullOrEmpty(getTransactionIn.Branch))
        {
            conditions.Add("branch = @Branch");
        }
        if (!String.IsNullOrEmpty(getTransactionIn.Account))
        {
            conditions.Add("account = @Account");
        }
        if (!String.IsNullOrEmpty(getTransactionIn.TraceKey))
        {
            conditions.Add("tracekey::text = @TraceKey");
        }

        var sql = """
        select 
            id::text, branch, account, amount, debitcredit, eventcode, description, tracekey::text, coalesce(details,'{}')::text details, dateref, createdat
        from
            flow.transaction
        where
        """ + String.Join(" AND ", conditions);

        using var conn = await _databaseContext.GetConnection();
        var result = await conn.QueryAsync<TransactionDTO>(sql, getTransactionIn);
        return result;
    }

    /// <summary>
    /// Consulta de extrato, informa saldo anterior, movimentação e saldo atual, para cada linha
    /// </summary>
    /// <param name="getTransactionIn"></param>
    /// <returns></returns>
    public async Task<IEnumerable<TransactionAcumulatedOut>> GetTransactionAcumulatedAsync(GetTransactionIn getTransactionIn)
    {
        var sql = """
        with before as (
        	select coalesce(acumulatedcredit,0)-coalesce(acumulateddebit,0) balance 
        	from flow.balance where dateref < @DateRefStart::date and branch=@Branch and account=@Account
        	order by dateref desc limit 1
        ),
        transactions as (
        	select 
        		transaction.dateref, transaction.eventcode, event.description eventdescription, 
        		transaction.description transactiondescription,
        		case when debitcredit='D' then -transaction.amount else 0 end debit,
        		case when debitcredit='C' then transaction.amount else 0 end credit,
        		sum(case when debitcredit='D' then -transaction.amount else transaction.amount end)
        		over (order by dateref, transaction.createdat, id) total, 
        		transaction.createdat, transaction.id
        	from flow.transaction 
        		left join flow.event on transaction.eventcode = event.code
        	where branch=@Branch and account=@Account
        	and dateref between @DateRefStart and @DateRefEnd
        )
        select 
        	dateref, eventcode, eventdescription, transactiondescription,
        	coalesce(lag(total) over(order by dateref, createdat, id),0)+coalesce(before.balance,0) beforebalance,
        	debit, credit, total+coalesce(before.balance,0) acumulated, createdat, id::text id
        from transactions left join before on true
        order by dateref, createdat, id
        """;

        using var conn = await _databaseContext.GetConnection();
        var result = await conn.QueryAsync<TransactionAcumulatedOut>(sql, getTransactionIn);
        return result;


    }
}