namespace bankOfLeverx.Models
{
    public class LoanDTO
    {
        public required double Amount { get; set; }
        public required DateOnly startDate { get; set; }
        public required DateOnly endDate { get; set; }
        public required string Type { get; set; }
        public required int AccountKey { get; set; }
    }


}
