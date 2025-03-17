namespace Flow.Domain.Models.DTO;

public record TransactionAcumulatedOut
{
    public DateTime DateRef { get; set; }
    public int EventCode { get; set; }
    public string? EventDescription { get; set; }
    public string? TransactionDescription { get; set; }
    public decimal BeforeBalance { get; set; }

    public decimal Debit { get; set; }
    public decimal Credit { get; set; }

    public decimal Acumulated { get; set; }

    public DateTime CreatedAt { get; set; }
    public string? Id { get; set; }

}