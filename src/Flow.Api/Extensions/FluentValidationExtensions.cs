using System.Text.Json;
using FluentValidation;

namespace Flow.Api.Extensions;

public static class FluentValidationExtensions
{

    /// <summary>
    /// Trata-se de regra para validação de conteúdo JSON, utilizada pelos validadores(fluentvalidation)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> IsValidJson<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.Must(json =>
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return false;
            }

            try
            {
                JsonDocument.Parse(json);
                return true;
            }
            catch
            {
                return false;
            }
        })
        .WithMessage("{PropertyName} must be valid JSON.");
    }

    /// <summary>
    /// Permite validação de campo GUID informado pelo usuário, tracekey especificamente
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> IsValidGuid<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.Must(value =>
        {
            return Guid.TryParse(value, out _);
        })
        .WithMessage("{PropertyName} must be valid GUID.");
    }
}