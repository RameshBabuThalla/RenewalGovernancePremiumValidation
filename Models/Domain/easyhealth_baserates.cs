﻿namespace RenewalGovernancePremiumValidation.Models.Domain
{
    public class easyhealth_baserates
    {
        public int age { get; set; }
        public int si { get; set; }
        public string variant { get; set; }
        public string tier { get; set; }
        public decimal? one_year { get; set; }
        public decimal? two_years { get; set; }
    }
}
