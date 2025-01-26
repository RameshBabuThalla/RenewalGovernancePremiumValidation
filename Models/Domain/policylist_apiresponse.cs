using System.ComponentModel.DataAnnotations;

namespace RenewalGovernancePremiumValidation.Models.Domain
{
    public class policylist_apiresponse
    {
        [Key]
        public Guid Object_ID { get; set; }
        public string DocType { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PolicyNo { get; set; }
        public string ClaimNo { get; set; }
        public string Source { get; set; }
        public string ClaimReferenceno { get; set; }
        public string DocumentFormat { get; set; }
        public string DocumentTitle { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
    }
}
