using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankOfLeverx.Infrastructure.Data.Models;

public class LoanDataModel
{
    public long Key { get; set; }

    public decimal Amount { get; set; }

    public decimal InitialAmount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Rate { get; set; }

    public string Type { get; set; } = string.Empty;

    public long BankerKey { get; set; }

    public long AccountKey { get; set; }
    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

}