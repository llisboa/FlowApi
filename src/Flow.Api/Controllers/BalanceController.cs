using Flow.Api.Extensions;
using Flow.Api.Validators;
using Flow.Domain.Contracts.Services;
using Flow.Domain.Models.Input;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Flow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BalanceController : ControllerBase
{
    private readonly IBalanceService _balanceService;
    public BalanceController(IBalanceService balanceService)
    {
        _balanceService = balanceService;
    }


    /// <summary>
    /// Obtém detalhes dos registros de saldo
    /// </summary>
    /// <param name="getBalanceIn"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetBalanceAsync")]
    public async Task<IActionResult> GetBalanceAsync([FromQuery] GetBalanceIn getBalanceIn)
    {
        try
        {
            var validation = new BalanceGetInValidator().Validate(getBalanceIn);
            if (!validation.IsValid) {
                return BadRequest(validation);
            }
            var result = await _balanceService.GetBalanceAsync(getBalanceIn);
            if (result==null || result.Count()==0)
            {
                return NoContent();
            }
            return Ok(result);
        } catch(Exception ex) {
            return ActionResultExtensions.CustomError(this, 500, ex); 
        }
    }
}