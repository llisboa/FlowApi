using Flow.Domain.Contracts.Repository.Entities;
using Flow.Domain.Contracts.Services;
using Flow.Repository.FlowDbContext;
using Flow.Repository.FlowDbContext.Entities;
using Flow.Services.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Flow.Service;
public static class StartupService 
{
    /// <summary>
    /// Inclusão de serviços para injeção separadamente, apenas para organização do código
    /// </summary>
    /// <param name="service"></param>
    public static void AddFlowServices(this IServiceCollection service)
    {
        service.AddScoped<IBalanceService, BalanceService>();
        service.AddScoped<ITransactionService, TransactionService>();
    }

    /// <summary>
    /// Inclusão de repositórios para injeção separadamente, apenas para organização do código
    /// </summary>
    /// <param name="service"></param>
    public static void AddFlowRepositories(this IServiceCollection service)
    {
        service.AddScoped<PostgreDbContext>();
        service.AddTransient<ITransactionRepository, TransactionRepository>();
        service.AddTransient<IBalanceRepository, BalanceRepository>();
    }
}