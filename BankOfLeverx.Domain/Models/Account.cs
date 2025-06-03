namespace BankOfLeverx.Domain.Models
{
    public class Account
    {
        public required string Number { get; set; }
        public required string PlanCode { get; set; }
        public required double Balance { get; set; }

        public required int CustomerKey { get; set; }
        public int Key { get; set; }
    }

}
