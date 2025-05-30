namespace bankOfLeverx
{
    public class Transaction
    {
        public required int Key { get; set; }

        public required int AccountKey { get; set; }

        public required bool IsDebit { get; set; }
        public required string Category { get; set; }
        public required double Amount { get; set; }
        public required DateTime Date { get; set; }
        public required string Comment { get; set; }
    }
   
}
