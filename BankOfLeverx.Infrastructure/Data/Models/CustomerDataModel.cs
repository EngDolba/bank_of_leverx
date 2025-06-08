namespace BankOfLeverx.Infrastructure.Data.Models;

public class CustomerDataModel
{
    public long Key { get; set; }


    public string Name { get; set; } = string.Empty;


    public string Surname { get; set; } = string.Empty;

    public long Category { get; set; }

    public long Branch { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

}
