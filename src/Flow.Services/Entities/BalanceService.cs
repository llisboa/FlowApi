using Flow.Domain.Contracts.Repository.Entities;
using Flow.Domain.Contracts.Services;
using Flow.Domain.Models.DTO;
using Flow.Domain.Models.Input;

namespace Flow.Services.Entities;
public class BalanceService : IBalanceService
{
    private readonly IBalanceRepository _balanceRepository;
    public BalanceService(IBalanceRepository balanceRepository)
    {
        _balanceRepository = balanceRepository;
    }

    /// <summary>
    /// Serviço de carga de saldos
    /// </summary>
    /// <remarks>
    /// Controles nunca poderão acessar repositórios. Caminho obrigatório é através dos serviços.
    /// </remarks>
    /// <param name="getBalanceIn"></param>
    /// <returns></returns>
    public async Task<IEnumerable<GetBalanceOut>> GetBalanceAsync(GetBalanceIn getBalanceIn)
    {
        var result = await _balanceRepository.GetBalanceAsync(getBalanceIn);
        return result;
    }
}