using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenewalGovernancePremiumValidation.Model.Domain
{
    public class whatsapp_motor_reminder_status
    {
        public string policy_no  { get; set; }
        public string? job_req_id { get; set; }
        public DateTime? wa_req_sent_date { get; set; }
        public DateTime? src_insert_date { get; set; }
        public string? wa_api_status { get; set; }
        public string? mobile_num { get; set; }
        public string? from_rg_db { get; set; }
    }
}
