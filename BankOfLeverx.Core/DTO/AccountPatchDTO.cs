﻿namespace BankOfLeverx.Core.DTO
{
    public class AccountPatchDTO
    {
        public string? Number { get; set; }
        public string? PlanCode { get; set; }
        public double? Balance { get; set; }
        public int? CustomerKey { get; set; }
    }

}
