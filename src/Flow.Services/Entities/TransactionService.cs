using Flow.Domain.Contracts.Repository.Entities;
using Flow.Domain.Contracts.Services;
using Flow.Domain.Models.DTO;
using Flow.Domain.Models.Input;
using Flow.Domain.Models.Output;

namespace Flow.Services.Entities;
public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    /// <summary>
    /// Inclus�o de registro em transa��o
    /// </summary>
    /// <remarks>
    /// Controles nunca poder�o acessar diretamente reposit�rios. Acesso somente pelos servi�os.
    /// </remarks>
    /// <param name="putTransactionIn"></param>
    /// <returns></returns>
    public Task<TraceKeyOut> AddTransactionAsync(PutTransactionIn putTransactionIn)
    {
        var result = _transactionRepository.AddTransactionAsync(putTransactionIn);
        return result;
    }

    /// <summary>
    /// Carga de detalhes de registros de transa��o
    /// </summary>
    /// <param name="getTransactionIn"></param>
    /// <remarks>
    /// Controles nunca poder�o acessar diretamente reposit�rios. Acesso somente pelos servi�os.
    /// </remarks>
    /// <returns></returns>
    public Task<IEnumerable<TransactionDTO>> GetTransactionAsync(GetTransactionIn getTransactionIn)
    {
        var result = _transactionRepository.GetTransactionAsync(getTransactionIn);
        return result;
    }

    /// <summary>
    /// Carga de extrato da conta
    /// </summary>
    /// <param name="getTransactionIn"></param>
    /// <remarks>
    /// Controles nunca poder�o acessar diretamente reposit�rios. Acesso somente pelos servi�os.
    /// Extrato possui em cada linha saldo anterior, d�bito ou cr�dito e saldo resultante.
    /// </remarks>
    /// <returns></returns>
    public Task<IEnumerable<TransactionAcumulatedOut>> GetTransactionAcumulatedAsync(GetTransactionIn getTransactionIn)
    {
        var result = _transactionRepository.GetTransactionAcumulatedAsync(getTransactionIn);
        return result;
    }

}