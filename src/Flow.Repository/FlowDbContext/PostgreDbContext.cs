using System.Data;
using System.Text.Json.Serialization;
using Flow.Domain.Contracts.Repository.DbContexts;
using Flow.Shared.Cloud.SM;
using Flow.Shared.Database;
using Newtonsoft.Json;
using Npgsql;
using Polly;

namespace Flow.Repository.FlowDbContext;

public class PostgreDbContext : IPostgreDbContext
{
    private readonly DatabaseSettings _databaseSettings;
    private readonly AWSSecretManager _awsSecretManager;

    public IDbConnection connection = default!;

    public PostgreDbContext(DatabaseSettings databaseSettings, AWSSecretManager awsSecretManager)
    {
        _databaseSettings = databaseSettings;
        _awsSecretManager = awsSecretManager;
    }

    /// <summary>
    /// Para obter conexão com a qual será possível manipular o POSTGRESQL
    /// </summary>
    /// <remarks>
    /// A configuração precisa passar pelo BUILD.CONFIGURATION para carga dos parãmetros do arquivo appsettings.json.
    /// </remarks>
    /// <returns></returns>
    public async Task<IDbConnection> GetConnection()
    {
        if (connection == null || connection.State == ConnectionState.Closed)
        {
            if (!string.IsNullOrEmpty(_databaseSettings.SecretId))
            {
                var secretValues = await _awsSecretManager.GetSecretAsync(_databaseSettings.SecretId, "AWSCURRENT");
                JsonConvert.PopulateObject(secretValues, _databaseSettings);
            }
            connection = new NpgsqlConnection(_databaseSettings.ConnectionString);
            await OpenConnection();
        }
        return connection;
    }

    /// <summary>
    /// Rotina para abertura da conexão, no caso de conexão fechada ou expirada
    /// </summary>
    /// <returns></returns>
    private async Task OpenConnection()
    {
        var policy = Policy.Handle<Exception>().Retry(5);
        policy.Execute(() => connection!.Open());
        await Task.Delay(1);
    }
}