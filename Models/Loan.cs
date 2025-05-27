namespace bankOfLeverx.Models
{
    public class Loan
    {
        public required int Key { get; set; }
        public required double Amount { get; set; }
        public required DateOnly startDate { get; set; }
        public required DateOnly endDate { get; set; }
        public required string Type { get; set; }
        public required int AccountKey { get; set; }
    }
  
   
}
