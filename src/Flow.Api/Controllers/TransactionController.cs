using Flow.Api.Extensions;
using Flow.Api.Validators;
using Flow.Domain.Contracts.Services;
using Flow.Domain.Models.Input;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Flow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// Inclusão das transações no fluxo
    /// </summary>
    /// <param name="putTransactionIn"></param>
    /// <remarks>
    /// Também atualiza saldo automaticamente.
    /// </remarks>
    /// <returns></returns>
    [HttpPut]
    [Route("AddTransactionAsync")]
    public async Task<IActionResult> AddTransactionAsync([FromQuery] PutTransactionIn putTransactionIn) 
    { 
        try
        {
            var validation = new TransactionPutInValidator().Validate(putTransactionIn);
            if (!validation.IsValid)
            {
                return BadRequest(validation);
            }
            var result = await _transactionService.AddTransactionAsync(putTransactionIn);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return ActionResultExtensions.CustomError(this, 500, ex);
        }
    }

    [HttpGet]
    [Route("GetTransactionAsync")]
    public async Task<IActionResult> GetTransactionAsync([FromQuery] GetTransactionIn getTransactionIn)
    {
        try
        {
            var validation = new TransactionGetInValidator().Validate(getTransactionIn);
            if (!validation.IsValid)
            {
                return BadRequest(validation);
            }
            var result = await _transactionService.GetTransactionAsync(getTransactionIn);
            if (result == null || result.Count() == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return ActionResultExtensions.CustomError(this, 500, ex);
        }
    }


    [HttpGet]
    [Route("GetTransactionAcumulatedAsync")]
    public async Task<IActionResult> GetTransactionAcumulatedAsync([FromQuery] GetTransactionIn getTransactionIn)
    {
        try
        {
            var validation = new TransactionGetInValidator().Validate(getTransactionIn);
            if (!validation.IsValid)
            {
                return BadRequest(validation);
            }
            var result = await _transactionService.GetTransactionAcumulatedAsync(getTransactionIn);
            if (result == null || result.Count() == 0)
            {
                return NoContent();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            return ActionResultExtensions.CustomError(this, 500, ex);
        }
    }

}