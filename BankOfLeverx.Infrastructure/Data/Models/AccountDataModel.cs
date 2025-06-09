namespace BankOfLeverx.Infrastructure.Data.Models;

public class AccountDataModel
{

    public long Key { get; set; }

    public string Number { get; set; } = string.Empty;

    public string PlanCode { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public long CustomerKey { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

}
