namespace BankOfLeverx.Core.DTO
{
    public class CustomerDTO
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required int Category { get; set; }
        public required int Branch { get; set; }
    }

}
