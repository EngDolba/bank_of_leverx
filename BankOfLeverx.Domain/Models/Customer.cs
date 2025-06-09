namespace BankOfLeverx.Domain.Models;

public class Customer
{
    public required string Name { get; set; }

    public required string Surname { get; set; }
    public int? Key { get; set; }

    public required int Category { get; set; }

    public required int Branch { get; set; }

}
