using Flow.Domain.Models.Input;
using FluentValidation;

namespace Flow.Api.Validators;

public class TransactionGetInValidator : AbstractValidator<GetTransactionIn>
{
 
    /// <summary>
    /// Validações dos parãmetros informados para obtenção de detalhes de transação
    /// </summary>
    public TransactionGetInValidator()
    {
        RuleFor(GetTransactionIn => GetTransactionIn.DateRefStart).NotEmpty();
        RuleFor(GetTransactionIn => GetTransactionIn.DateRefEnd).NotEmpty();
        RuleFor(GetTransactionIn => String.IsNullOrEmpty(GetTransactionIn.Branch) == String.IsNullOrEmpty(GetTransactionIn.Account))
            .Must(a => a == true)
            .WithMessage("Branch and account must be null or both filled.");
    }
}