namespace BankOfLeverx.Domain.Models
{
    public class Loan
    {
        public required int Key { get; set; }
        public required double Amount { get; set; }

        public required double InitialAmount { get; set; }

        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required double Rate { get; set; }
        public required string Type { get; set; }
        public required int BankerKey { get; set; }

        public required int AccountKey { get; set; }
    }


}
