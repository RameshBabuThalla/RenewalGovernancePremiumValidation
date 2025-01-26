using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenewalGovernancePremiumValidation.Model.Domain
{
    public class idst_ora_respo_sms_status
    {
        public string policy_no { get; set; }
        public string? dyn_job_req_id { get; set; }
        public string? dyn_message_content { get; set; }
        public DateTime? event_captured_dt { get; set; }
        public DateTime? src_insert_date { get; set; }
        public string? record_type { get; set; }
        public string? mobile_number { get; set; }
    }
}
