namespace Flow.Domain.Models.Input;

public record GetTransactionIn
{
    public string? Branch { get; set; }
    public string? Account { get; set; }
    public DateTime? DateRefStart { get; set; }
    public DateTime? DateRefEnd { get; set; }

    public string? TraceKey { get; set; }

}