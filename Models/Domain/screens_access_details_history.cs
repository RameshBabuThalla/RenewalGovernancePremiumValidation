using System.ComponentModel.DataAnnotations;

namespace RenewalGovernancePremiumValidation.Models.Domain
{
    public class screens_access_details_history
    {
        [Key]
        public int recordid { get; set; }
        public int? id { get; set; }
        public int? screenid { get; set; }
        public int? roleid { get; set; }
        public bool? view { get; set; }
        public bool? download { get; set; }
        public bool? submit { get; set; }
        public bool? re_trigger_comm { get; set; }
        public bool? audit { get; set; }
        public bool? downloadrn { get; set; }
        public string? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public string? operation { get; set; }
    }
}
