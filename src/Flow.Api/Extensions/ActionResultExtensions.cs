using Microsoft.AspNetCore.Mvc;

namespace Flow.Api.Extensions;

/// <summary>
/// Para apresentação de detalhes do erro em log
/// <remarks>
/// O aplicativo não deve enviar detalhes dos erros para o usuário. Apenas um erro tratado deve ser apresentado.
/// No log o desenvolvedor e equipe técnica poderá acessar mais detalhes do erro.
/// Serilog está sendo utilizado para melhor formatação do log, além de ser simples a adaptação para outros formatos, como modelo conteúdo específico para envio ao DATADOG por exemplo.
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