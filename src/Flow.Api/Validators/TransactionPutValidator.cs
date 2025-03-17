using System.Text.Json;
using Flow.Api.Extensions;
using Flow.Domain.Models.Input;
using FluentValidation;

namespace Flow.Api.Validators;

public class TransactionPutInValidator : AbstractValidator<PutTransactionIn>
{
    /// <summary>
    /// Validação da entrada de transação
    /// </summary>
    public TransactionPutInValidator()
    {
        RuleFor(PutTransactionIn => PutTransactionIn.Branch).NotEmpty();
        RuleFor(PutTransactionIn => PutTransactionIn.Account).NotEmpty();
        RuleFor(PutTransactionIn => PutTransactionIn.Amount).NotEmpty();
        RuleFor(PutTransactionIn => PutTransactionIn.DateRef).NotEmpty();
        RuleFor(PutTransactionIn => PutTransactionIn.DebitCredit).NotEmpty();
        RuleFor(PutTransactionIn => PutTransactionIn.Description).MaximumLength(200);
        RuleFor(PutTransactionIn => PutTransactionIn.Details).IsValidJson();
        RuleFor(PutTransactionIn => PutTransactionIn.TraceKey).IsValidGuid();
        RuleFor(PutTransactionIn => PutTransactionIn.DebitCredit).Matches("^[DC]$");
        RuleFor(PutTransactionIn => PutTransactionIn.Amount).GreaterThan(0);
        RuleFor(PutTransactionIn => PutTransactionIn.EventCode).NotEmpty();
    }
}