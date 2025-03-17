namespace Flow.Domain.Models.DTO;

public record TransactionDTO
{
    public string? Id { get; set; }
    public string? Branch { get; set; }
    public string? Account { get; set; }

    public decimal Amount { get; set; }

    public string? DebitCredit { get; set; }
    public int EventCode { get; set; }

    public string? Description { get; set; }

    public string? TraceKey { get; set; }
    public string? Details { get; set; }

    public DateTime DateRef { get; set; }
    public DateTime CreatedAt { get; set; }
}