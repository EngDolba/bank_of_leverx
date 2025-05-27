namespace bankOfLeverx.Models
{
    public class LoanDTO
    {
        public required double Amount { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
        public required double Rate { get; set; }
        public required string Type { get; set; }
        public required int AccountKey { get; set; }
    }


}
