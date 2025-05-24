namespace bankOfLeverx
{
    public class AccountDTO
    {
        public required string Number { get; set; }
        public required string PlanCode { get; set; }
        public required double Balance { get; set; }
        public required int    CustomerKey { get; set; }
        public required string Branch { get; set; }
    }

}
