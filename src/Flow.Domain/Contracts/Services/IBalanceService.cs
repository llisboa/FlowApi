using Flow.Domain.Models.DTO;
using Flow.Domain.Models.Input;

namespace Flow.Domain.Contracts.Services;

public interface IBalanceService
{
    Task<IEnumerable<GetBalanceOut>> GetBalanceAsync(GetBalanceIn getBalanceIn);
}