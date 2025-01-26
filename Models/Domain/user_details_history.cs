using System.ComponentModel.DataAnnotations;

namespace RenewalGovernancePremiumValidation.Models.Domain
{
    public class user_details_history
    {
        [Key]
        public int id { get; set; }
        public int? employee_id { get; set; }
        public string? employee_name { get; set; }
        public string? email_id { get; set; }
        public string? mobile_number { get; set; }
        public string? nt_login_id { get; set; }
        public string? department { get; set; }
        public DateTime? last_login_date { get; set; }
        public string? rg_status { get; set; }
        public string? arm_id { get; set; }
        public string? modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public string? operation { get; set; }
    }
}
