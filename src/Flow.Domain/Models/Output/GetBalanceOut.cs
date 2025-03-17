namespace Flow.Domain.Models.DTO;

public record GetBalanceOut
{
    public string? Id { get; set; }
    public string? Branch { get; set; }
    public string? Account { get; set; }

    public DateTime DateRef { get; set; }

    public decimal TotalDebit { get; set; }
    public decimal TotalCredit { get; set; }

    public decimal AcumulatedDebit { get; set; }
    public decimal AcumulatedCredit { get; set; }

    public decimal AcumulatedBalance
    {
        get
        {
            return AcumulatedCredit - AcumulatedDebit;
        }
    }
    public DateTime CreatedAt { get; set; }
    public string? TraceKey { get; set; }

}