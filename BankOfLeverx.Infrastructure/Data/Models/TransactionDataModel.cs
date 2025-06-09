namespace BankOfLeverx.Infrastructure.Data.Models;

public class TransactionDataModel

{

    public long Key { get; set; }

    public long AccountKey { get; set; }

    public bool IsDebit { get; set; }

    public string Category { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

}
