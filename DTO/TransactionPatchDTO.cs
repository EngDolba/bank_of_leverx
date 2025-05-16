namespace bankOfLeverx
{
    public class TransactionPatchDTO
    {

        public  int? AccountKey { get; set; }

        public  bool? IsDebit { get; set; }
        public  string Category { get; set; }
        public  double? Amount { get; set; }
        public  DateTime? Date { get; set; }
        public  string Comment { get; set; }
    }

}
