namespace Flow.Shared.Database;

public class DatabaseSettings
{
    private readonly string _connectionStringFormat = "Pooling=true;Server={0};Port={1};Database={2};User Id={3};Password={4};Maximum Pool Size={5};Connection Idle Lifetime={6};Command Timeout={7}";

    public string Server { get; set; } = default!;
    public string Port { get; set; } = default!;
    public string Database { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string MaxPoolSize { get; set; } = default!;
    public string IdleLifetime { get; set; } = default!; 
    public string CommandTimeout { get; set; } = default!;
    public string SecretId { get; set; } = default!;

    /// <summary>
    /// Gabarito para geração de string de conexão do postgresql
    /// </summary>
    public string ConnectionString => string.Format(_connectionStringFormat, Server, Port, Database, UserId, Password, MaxPoolSize, IdleLifetime, CommandTimeout);
}