namespace BankOfLeverx.Core.DTO
{
    public class CustomerPatchDTO
    {
        public string? Name { get; set; }

        public string? Surname { get; set; }

        public int? Category { get; set; }

        public int? Branch { get; set; }
    }
}
