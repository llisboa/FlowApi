
using System.Collections.Generic;
using Flow.Domain.Models.DTO;
using Flow.Domain.Models.Input;
using Flow.Domain.Models.Output;

namespace Flow.Domain.Contracts.Services;

public interface ITransactionService
{
    Task<TraceKeyOut> AddTransactionAsync(PutTransactionIn putTransactionIn);
    Task<IEnumerable<TransactionDTO>> GetTransactionAsync(GetTransactionIn getTransactionIn);
    Task<IEnumerable<TransactionAcumulatedOut>> GetTransactionAcumulatedAsync(GetTransactionIn getTransactionIn);

}