using RenewalGovernancePremiumValidation.Data;
using RenewalGovernancePremiumValidation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenewalGovernancePremiumValidation.BussinessLogic
{
    public class KotaIPA 
    {
        private readonly HDFCDbContext _context;
        public KotaIPA(HDFCDbContext context)
        {
            _context = context;
        }
        public Task<List<KotaIPAResponse>> PostKotaIPAValues(Kota_ipa kotaIPAData)
        {
            string planName = kotaIPAData.plan_name.Replace(" ", string.Empty);
            string subPlanName = kotaIPAData.sub_plan_name.Replace(" ", string.Empty);

            var premiums = _context.koti_ipa_rates
                .Where(p => p.plan_name.Replace(" ", string.Empty) == planName
                             && p.plan_code == kotaIPAData.plan_code
                             && p.sub_plan_name.Replace(" ", string.Empty) == subPlanName)
                .Select(p => p.total_premiumself_pretax)
                .ToList();
            decimal totalAmount = Convert.ToDecimal(premiums.Sum()); // Sum the premiums
            decimal gst = totalAmount * 0.18m; // Calculate 18% GST
            decimal totalWithGST = totalAmount + gst;

            // Round to 2 decimal places
            totalAmount = Math.Round(totalAmount, 2);
            gst = Math.Round(gst, 2);
            totalWithGST = Math.Round(totalWithGST, 2);
            List<KotaIPAResponse> result = new List<KotaIPAResponse>();
            var response = new KotaIPAResponse
            {
                total_premiumself_pretax = totalAmount,
                gst = gst,
                total = totalWithGST
            };
            result.Add(response);

            return  Task.FromResult(result);


        }
    }
    public class Kota_ipa
    {
        public string plan_name { get; set; }
        public int plan_code { get; set; }
        public string sub_plan_name { get; set; }

    }

    public class KotaIPAResponse
    {
        public decimal? total_premiumself_pretax { get; set; }
        public decimal? gst { get; set; }
        public decimal? total { get; set; }

    }
}
