using System.ComponentModel.DataAnnotations;

namespace RenewalGovernancePremiumValidation.Models.Domain
{
    public class policydocuments
    {
        [Key]
        public Guid object_id { get; set; }
        public string? doctype { get; set; }
        public DateTime? createddate { get; set; }
        public string? policyno { get; set; }
        public string? claimno { get; set; }
        public string? source { get; set; }
        public string? claimreferenceno { get; set; }
        public string? documentformat { get; set; }
        public string? documenttitle { get; set; }
        public string? responsecode { get; set; }
        public string? responsedescription { get; set; }
    }
}
