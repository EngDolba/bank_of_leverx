namespace bankOfLeverx
{
    public class AccountPatchDTO
    {
        public  string Number { get; set; }
        public  string PlanCode { get; set; }
        public double? Balance { get; set; }
        public  int? CustomerKey { get; set; }
        public  string Branch { get; set; }
    }

}
