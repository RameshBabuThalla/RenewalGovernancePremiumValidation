﻿using System.ComponentModel.DataAnnotations;

namespace RenewalGovernancePremiumValidation.Models.Domain
{
    public class screen_access_master
    {
        [Key]
        public int screenid { get; set; }
        public string screen_name { get; set; }
    }
}
