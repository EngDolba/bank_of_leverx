namespace bankOfLeverx.Models
{
    public class LoanPatchDTO
    {
        public  int? Key { get; set; }
        public double? Amount { get; set; }
        public DateOnly? startDate { get; set; }
        public DateOnly? endDate { get; set; }
        public string Type { get; set; }
        public  int? AccountKey { get; set; }
    }


}
