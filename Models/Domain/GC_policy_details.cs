﻿using System.ComponentModel.DataAnnotations;

namespace RenewalGovernancePremiumValidation.Models.Domain
{
    public class gc_policy_details
    {
        public long batch_id { get; set; }
        public long job_id { get; set; }
        public DateTime notice_date { get; set; }
        public int product_cd { get; set; }
        public string product_name { get; set; }
        public long proposal_no { get; set; }
        public long policy_no_16 { get; set; }
        [Key]
        public long policy_no { get; set; }
        public long hehi_policy_no { get; set; }
        public int banch_code { get; set; }
        public string branch_name { get; set; }
        public string split_flag { get; set; }
        public DateTime policy_start_date { get; set; }
        public DateTime policy_end_date { get; set; }
        public string policy_period { get; set; }
        public string tier { get; set; }
        public string policy_plan { get; set; }
        public string policy_type { get; set; }
        public string family_size { get; set; }
        public int claim_count { get; set; }
        public bool rn_flag { get; set; }
        public bool vip_flag { get; set; }
        public bool age_change_flag { get; set; }
        public bool tier_change_flag { get; set; }
        public bool loading_change_flag { get; set; }
        public string txt_remarks { get; set; }
        public int master_policy_no { get; set; }
        public int customer_uin { get; set; }
        public bool pre_policy_claim_flag { get; set; }
        public bool lifecycle_claim_flag { get; set; }
        public int num_tot_additional_child { get; set; }
        public string gc_reporting_code { get; set; }
        public string remarks { get; set; }
        public string data { get; set; }
        public int pb_1 { get; set; }
        public int pb_2 { get; set; }
        public int pb_3 { get; set; }
        public int pb_4 { get; set; }
        public int pb_5 { get; set; }
        public int pb_6 { get; set; }
        public int pb_7 { get; set; }
        public int pb_8 { get; set; }
        public int pb_9 { get; set; }
        public int pb_10 { get; set; }
        public int pb_11 { get; set; }
        public int pb_12 { get; set; }
        public bool exclusion_1 { get; set; }
        public bool exclusion_2 { get; set; }
        public bool exclusion_3 { get; set; }
        public bool exclusion_4 { get; set; }
        public bool exclusion_5 { get; set; }
        public bool exclusion_6 { get; set; }
        public bool exclusion_7 { get; set; }
        public bool exclusion_8 { get; set; }
        public bool exclusion_9 { get; set; }
        public bool exclusion_10 { get; set; }
        public bool exclusion_11 { get; set; }
        public bool exclusion_12 { get; set; }
        public int si_1 { get; set; }
        public int si_2 { get; set; }
        public int si_3 { get; set; }
        public int si_4 { get; set; }
        public int si_5 { get; set; }
        public int si_6 { get; set; }
        public int si_7 { get; set; }
        public int si_8 { get; set; }
        public int si_9 { get; set; }
        public int si_10 { get; set; }
        public int si_11 { get; set; }
        public int si_12 { get; set; }
        public int cb_1 { get; set; }
        public int cb_2 { get; set; }
        public int cb_3 { get; set; }
        public int cb_4 { get; set; }
        public int cb_5 { get; set; }
        public int cb_6 { get; set; }
        public int cb_7 { get; set; }
        public int cb_8 { get; set; }
        public int cb_9 { get; set; }
        public int cb_10 { get; set; }
        public int cb_11 { get; set; }
        public int cb_12 { get; set; }
        public int sb_1 { get; set; }
        public int sb_2 { get; set; }
        public int sb_3 { get; set; }
        public int sb_4 { get; set; }
        public int sb_5 { get; set; }
        public int sb_6 { get; set; }
        public int sb_7 { get; set; }
        public int sb_8 { get; set; }
        public int sb_9 { get; set; }
        public int sb_10 { get; set; }
        public int sb_11 { get; set; }
        public int sb_12 { get; set; }
        public DateTime first_inception_date_1 { get; set; }
        public DateTime first_inception_date_2 { get; set; }
        public DateTime first_inception_date_3 { get; set; }
        public DateTime first_inception_date_4 { get; set; }
        public DateTime first_inception_date_5 { get; set; }
        public DateTime first_inception_date_6 { get; set; }
        public DateTime first_inception_date_7 { get; set; }
        public DateTime first_inception_date_8 { get; set; }
        public DateTime first_inception_date_9 { get; set; }
        public DateTime first_inception_date_10 { get; set; }
        public DateTime first_inception_date_11 { get; set; }
        public DateTime first_inception_date_12 { get; set; }
        public int co_pay_amt_1 { get; set; }
        public int co_pay_amt_2 { get; set; }
        public int co_pay_amt_3 { get; set; }
        public int co_pay_amt_4 { get; set; }
        public int co_pay_amt_5 { get; set; }
        public int co_pay_amt_6 { get; set; }
        public int co_pay_amt_7 { get; set; }
        public int co_pay_amt_8 { get; set; }
        public int co_pay_amt_9 { get; set; }
        public int co_pay_amt_10 { get; set; }
        public int co_pay_amt_11 { get; set; }
        public int co_pay_amt_12 { get; set; }
        public int co_pay_1 { get; set; }
        public int co_pay_2 { get; set; }
        public int co_pay_3 { get; set; }
        public int co_pay_4 { get; set; }
        public int co_pay_5 { get; set; }
        public int co_pay_6 { get; set; }
        public int co_pay_7 { get; set; }
        public int co_pay_8 { get; set; }
        public int co_pay_9 { get; set; }
        public int co_pay_10 { get; set; }
        public int co_pay_11 { get; set; }  
        public int co_pay_12 { get; set; }
        public string? upsell_type_1 { get; set; }
        public string? upsell_type_2 { get; set; }
        public string? upsell_type_3 { get; set; }
        public int upsell_value_1 {  get; set; }    
        public int upsell_value_2 {  get; set; }
        public int upsell_value_3 {  get; set; }    
        
     
    }
}
