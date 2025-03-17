using Microsoft.AspNetCore.Mvc;

namespace Flow.Api.Extensions;

/// <summary>
/// Para apresenta��o de detalhes do erro em log
/// <remarks>
/// O aplicativo n�o deve enviar detalhes dos erros para o usu�rio. Apenas um erro tratado deve ser apresentado.
/// No log o desenvolvedor e equipe t�cnica poder� acessar mais detalhes do erro.
/// Serilog est� sendo utilizado para melhor formata��o do log, al�m de ser simples a adapta��o para outros formatos, como modelo conte�do espec�fico para envio ao DATADOG por exemplo.
/// </remarks>
/// </summary>
public static class ActionResultExtensions
{
    public static IActionResult CustomError(this ControllerBase controller, int statusCode, Exception? ex = null, string message = "Error")
    {
        Serilog.Log.Error($"{message} in {{controller}} with {{statusCode}}, {{exception}}", controller, statusCode, ex);
        var errorResponse = new
        {
            Error = message,
            StatusCode = statusCode
        };

        return controller.StatusCode(statusCode, errorResponse);
    }
}