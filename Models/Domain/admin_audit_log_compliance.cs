using System.ComponentModel.DataAnnotations;

namespace RenewalGovernancePremiumValidation.Models.Domain
{
    public class admin_audit_log_compliance
    {
        [Key]
        public int id { get; set; }
        public int? master_table_id { get; set; }
        public string? master_table_name { get; set; }
        public string? master_table_id_description { get; set; }
        public string? master_table_id_mapping { get; set; }
        public int? active { get; set; } = 1;
        public string? operation { get; set; }
        public string? created_by { get; set; }
        public DateTime? created_date { get; set; }
        public string? updated_by { get; set; }
        public DateTime? updated_date { get; set; } 
    }
}
