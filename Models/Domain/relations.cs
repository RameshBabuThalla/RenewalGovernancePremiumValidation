﻿using Microsoft.EntityFrameworkCore;

namespace RenewalGovernancePremiumValidation.Models.Domain
{
    [Keyless]
    public class relations
    {
        public string insured_relation { get; set; }
        public string relation_tag { get; set; }
    }
}
