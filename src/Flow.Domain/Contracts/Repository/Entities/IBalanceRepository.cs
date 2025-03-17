using Flow.Domain.Models.DTO;
using Flow.Domain.Models.Input;

namespace Flow.Domain.Contracts.Repository.Entities;

public interface IBalanceRepository
{
    Task<IEnumerable<GetBalanceOut>> GetBalanceAsync(GetBalanceIn getBalanceIn);
}