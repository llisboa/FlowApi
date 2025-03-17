using Flow.Domain.Models.Input;
using Flow.Repository.FlowDbContext.Entities;
using Microsoft.AspNetCore.Mvc;
using Flow.Repository.FlowDbContext;
using Flow.Services.Entities;
using Flow.Shared.Database;
using Microsoft.Extensions.Configuration;
using Flow.Domain.Models.DTO;
using Flow.Api.Controllers;
using Moq;

namespace Flow.Tests.Api.Controllers;

public class TransactionControllerTests
{
    private readonly TransactionController _transactionController;

    public TransactionControllerTests()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

        var postgreDbContext = new Mock<PostgreDbContext>(databaseSettings!);
        var transactionRepository = new TransactionRepository(postgreDbContext.Object);
        var transactionService = new TransactionService(transactionRepository);
        _transactionController = new TransactionController(transactionService);
    }

    /// <summary>
    /// Verifica bloqueio de consulta sem data
    /// </summary>
    /// <remarks>
    /// Datas são obrigatórias na pesquisa devido ao risco de serem retornados muitos registros.
    /// </remarks>
    /// <returns></returns>
    [Fact]
    public async Task SearchWithoutDatesAsync()
    {
        GetTransactionIn getTransactionIn = new()
        {
            Branch = "1",
            Account = "2",
            DateRefStart = new System.DateTime(2025, 3, 15),
            DateRefEnd = null,
            TraceKey = null
        };

        ObjectResult result = (ObjectResult)await _transactionController.GetTransactionAsync(getTransactionIn);
        Assert.Equal(result.StatusCode, 400);
    }

    /// <summary>
    /// Confirma carga adequada de regisros, busca apenas por data
    /// </summary>
    /// <remarks>
    /// Mesmo não passando tracekey ele será retornado, pois sempre é informado na inserção de transações.
    /// </remarks>
    [Fact]
    public async Task SearcWithDatesandNoTraceKey()
    {
        GetTransactionIn getTransactionIn = new()
        {
            Branch = "1",
            Account = "2",
            DateRefStart = new System.DateTime(2025, 3, 15),
            DateRefEnd = new System.DateTime(2025, 3, 16),
            TraceKey = null
        };

        ObjectResult result = (ObjectResult)await _transactionController.GetTransactionAsync(getTransactionIn);
        IEnumerable<TransactionDTO> items = (IEnumerable<TransactionDTO>)result.Value!;
        Assert.True(items.Count() == 1);
    }


    /// <summary>
    /// Confirma consulta por tracekey
    /// </summary>
    [Fact]
    public async Task SearcWithTraceKey()
    {
        GetTransactionIn getTransactionIn = new()
        {
            Branch = null,
            Account = null,
            DateRefStart = new System.DateTime(2025, 3, 15),
            DateRefEnd = new System.DateTime(2025, 3, 16),
            TraceKey = "b5666143-0f9c-44f0-906e-c496c49ae4e1"
        };

        ObjectResult result = (ObjectResult)await _transactionController.GetTransactionAsync(getTransactionIn);
        IEnumerable<TransactionDTO> items = (IEnumerable<TransactionDTO>)result.Value!;
        Assert.True(items.Count() == 4);
    }



}
