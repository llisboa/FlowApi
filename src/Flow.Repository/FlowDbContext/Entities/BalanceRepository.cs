using Dapper;
using Flow.Domain.Contracts.Repository.Entities;
using Flow.Domain.Models.DTO;
using Flow.Domain.Models.Input;

namespace Flow.Repository.FlowDbContext.Entities;

public class BalanceRepository : IBalanceRepository
{
    private readonly PostgreDbContext _databaseContext;

    public BalanceRepository(PostgreDbContext databaseContext) => _databaseContext = databaseContext;

    /// <summary>
    /// Carga de dados de saldo (tabela balance)
    /// </summary>
    /// <param name="getBalanceIn"></param>
    /// <returns></returns>
    public async Task<IEnumerable<GetBalanceOut>> GetBalanceAsync(GetBalanceIn getBalanceIn)
    {
        List<string> conditions = new();
        conditions.Add("(dateref between @DateRefStart and @DateRefEnd)");
        if (!String.IsNullOrEmpty(getBalanceIn.Branch)) {
            conditions.Add("branch = @Branch");
        }
        if (!String.IsNullOrEmpty(getBalanceIn.Account)) {
            conditions.Add("account = @Account");
        }
        if (!String.IsNullOrEmpty(getBalanceIn.TraceKey)) {
            conditions.Add("tracekey::text = @TraceKey");
        }

        var sql = """
        select 
            id::text, branch, account, dateref::text, totaldebit, totalcredit, createdat, acumulateddebit, acumulatedcredit, tracekey::text
        from
            flow.balance
        where
        """ + String.Join(" AND ", conditions);

        using var conn = await _databaseContext.GetConnection();
        var result = await conn.QueryAsync<GetBalanceOut>(sql, getBalanceIn);
        return result;
    }

}