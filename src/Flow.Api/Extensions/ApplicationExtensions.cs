using Amazon.SecretsManager;
using Flow.Service;
using Flow.Shared.Cloud.SM;
using Flow.Shared.Database;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Json;

public static class ApplicationExtensions
{
    /// <summary>
    /// Extens�o utilizada na carga do programa para configura��o do SERILOG
    /// </summary>
    /// <param name="host"></param>
    /// <remars>
    /// No caso de utiliza��o do datadog configurar com a linha .WriteTo.Console(new JsonFormatter()) para o log seja enviado em formato JSON.
    /// DATADOG contabiliza linhas como eventos e caso seja enviado o log em formato texto corrido (sem formata��o JSON) acabar� onerando custos pois uma mensagem apenas poder� resultar em v�rios eventos.
    /// </remars>
    /// <returns></returns>
    public static IHostBuilder ConfigureSerilog(this IHostBuilder host)
    {
        host.UseSerilog((ctx, config) => config
            .Enrich.FromLogContext()
            .Enrich.WithProperty("hostname",
                Environment.GetEnvironmentVariable("HOSTNAME") ??
                Environment.GetEnvironmentVariable("COMPUTERNAME") ??
                "undefined"
            )
            .WriteTo.Console()
            // TODO: Enable for DATADOG -> .WriteTo.Console(new JsonFormatter())
        );
        return host;
    }

    /// <summary>
    /// Para carga dos servi�os em geral para inje��o, inclusive AWS SDK
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var dbSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
        services.AddSingleton(dbSettings!);
        services.AddFlowServices();
        services.AddFlowRepositories();

        var awsOptions = configuration.GetAWSOptions();
        services.AddDefaultAWSOptions(awsOptions)
            .AddAWSService<IAmazonSecretsManager>();

        var awsSecretManager = new AWSSecretManager(awsOptions.CreateServiceClient<IAmazonSecretsManager>());
        services.AddSingleton(awsSecretManager);
    }
}