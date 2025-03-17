using System.Data;

namespace Flow.Domain.Contracts.Repository.DbContexts;

public interface IPostgreDbContext
{
    Task<IDbConnection> GetConnection();
}