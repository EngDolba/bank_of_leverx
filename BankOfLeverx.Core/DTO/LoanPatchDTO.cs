﻿namespace BankOfLeverx.Core.DTO
{
    public class LoanPatchDTO
    {
        public double? Amount { get; set; }
        public double? InitialAmount { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Type { get; set; }
        public double? Rate { get; set; }

        public int? BankerKey { get; set; }

        public int? AccountKey { get; set; }
    }


}
