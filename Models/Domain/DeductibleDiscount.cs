﻿using Microsoft.EntityFrameworkCore;

namespace RenewalGovernancePremiumValidation.Models.Domain
{
    [Keyless]
    public class deductiblediscount
    {
        public decimal si { get; set; }
        public decimal deductible { get; set; }
        public decimal discount { get; set; }
       
    }
}
