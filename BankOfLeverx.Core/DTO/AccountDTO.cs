namespace BankOfLeverx.Core.DTO
{
    public class AccountDTO
    {
        public required string Number { get; set; }
        public required string PlanCode { get; set; }
        public required double Balance { get; set; }
        public required int CustomerKey { get; set; }
    }

}
