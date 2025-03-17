using Flow.Domain.Models.Input;
using FluentValidation;

namespace Flow.Api.Validators;

/// <summary>
/// Regras aplicadas nos parâmetros informados para obtenção detalhes de saldo
/// </summary>
public class BalanceGetInValidator : AbstractValidator<GetBalanceIn>
{
    public BalanceGetInValidator()
    {
        RuleFor(GetBalanceIn => GetBalanceIn.DateRefStart).NotEmpty();
        RuleFor(GetBalanceIn => GetBalanceIn.DateRefEnd).NotEmpty();
        RuleFor(GetBalanceIn => String.IsNullOrEmpty(GetBalanceIn.Branch) == String.IsNullOrEmpty(GetBalanceIn.Account))
            .Must(a => a==true)
            .WithMessage("Branch and account must be null or both filled.");
    }
}