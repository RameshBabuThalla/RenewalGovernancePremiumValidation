﻿namespace RenewalGovernancePremiumValidation.Models.Domain
{
    // Models/InsuranceReport.cs
    public class RMSFormat
    {
        public string? cn_no { get; set; }
        public string policy_no { get; set; }
        public string? endorsement_no { get; set; }
        public string certificate_no { get; set; }
        public string? account_code_bi { get; set; }
        public string? overriding_agent_name { get; set; }
        public string? producer_name_overriding_agent_name { get; set; }
        public string? product_code { get; set; }
        public string? product_name { get; set; }
        public string? name_of_policy { get; set; }
        public DateTime? entry_date { get; set; }
        public DateTime? effective_date { get; set; }
        public DateTime? expiry_date { get; set; }
        public DateTime? policy_renewal_on_expiry_date_1 { get; set; }
        public DateTime? renewal_policy_expiry_date { get; set; }
        public decimal? expiring_policy_gross_premium { get; set; }
        public decimal? expiring_policy_premium { get; set; }
        public decimal? renewal_sum_insured { get; set; }
        public decimal? renewal_gross_premium { get; set; }
        public decimal? renewal_premium_including_st { get; set; }
        public decimal? renewal_ncb { get; set; }
        public string? pol_branch_code { get; set; }
        public string? branch_name { get; set; }
        public string? branch { get; set; }
        public decimal? branch_code { get; set; }
        public string? business_group { get; set; }
        public string? vertical { get; set; }
        public string? sub_vertical { get; set; }
        public string? bdn_name { get; set; }
        public string? sse_name { get; set; }
        public string? lg_code { get; set; }
        public string? los_code { get; set; }
        public string? bdr_code { get; set; }
        public string? name_of_insured { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public string? address3 { get; set; }
        public string? pin_code { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? phone_no { get; set; }
        public string? mobile_no { get; set; }
        public string? home_no { get; set; }
        public string? office_no { get; set; }
        public decimal? claims_amount_paid { get; set; }
        public decimal? os_claim_amount_reserves { get; set; }
        public string? occupancy_details_project_type { get; set; }
        public string? type_of_commodity { get; set; }
        public string? type_of_cargo { get; set; }
        public string? equipment_details { get; set; }
        public string? year_of_manufacturing { get; set; }
        public string? tac_code { get; set; }
        public string? customer_id { get; set; }
        public string? hdfc_bank_location_code { get; set; }
        public string? dl_code { get; set; }
        public string? vehicle_make { get; set; }
        public string? model_desp { get; set; }
        public string? engine_no { get; set; }
        public string? chassis_no { get; set; }
        public string? registration_no { get; set; }
        public decimal? renewal_net_premium { get; set; }
        public decimal? service_tax { get; set; }
        public decimal? total_amount_payable { get; set; }
        public decimal? renewal_idv_sum_insured { get; set; }
        public decimal? ncb_rate_cb { get; set; }
        public string? renewable_product_non_renewable_product { get; set; }
        public string? loan_account_number { get; set; }
        public string? lob { get; set; }
        public string? system { get; set; }
        public string? status { get; set; }
        public string? final_remarks { get; set; }
        public string? dispatch_status { get; set; }
        public string? dispatched { get; set; }
        public DateTime? customer_dispatch_date { get; set; }
        public DateTime? channel_dispatch_date { get; set; }
        public string? awb { get; set; }
        public string? new_sub_vertical { get; set; }
        public string? new_business_group { get; set; }
        public string? active_inactive { get; set; }
        public string? new_dealer_code { get; set; }
        public string? intermediary_name { get; set; }
        public string? new_bdm_name { get; set; }
        public string? new_sse_name { get; set; }
        public string? vertical_receiptant { get; set; }
        public string? dispatch_branch_name { get; set; }
        public string? bosg_name { get; set; }
        public string? cp_sub_type { get; set; }
        public string? sse_code { get; set; }
        public string? bdm_code { get; set; }
        public string? customer_email_id { get; set; }
        public decimal? age_discount { get; set; }
        public decimal? professional_discount { get; set; }
        public string? ncb_premium_desc { get; set; }
        public string? zero_dep_desc { get; set; }
        public string? zero_dep_prem_desc { get; set; }
        public string? ea_prem_desc { get; set; }
        public decimal? loss_down_protection_prem { get; set; }
        public decimal? loss_dwntime_service_tax { get; set; }
        public decimal? loss_dwntime_prem_payable { get; set; }
        public decimal? ncb_protection_prem { get; set; }
        public decimal? ncb_protection_st { get; set; }
        public decimal? ncb_protection_prem_payable { get; set; }
        public decimal? engine_gearbox_prem { get; set; }
        public decimal? engine_gearbox_st { get; set; }
        public decimal? engine_gearbox_prem_payable { get; set; }
        public decimal? cost_of_consumables_prm { get; set; }
        public decimal? cost_consumable_st { get; set; }
        public decimal? cost_consumable_prem_payable { get; set; }
        public string? bdm_mail_id { get; set; }
        public string? sse_mail_id { get; set; }
        public string? policy_branch_code { get; set; }
        public string? branch_manager { get; set; }
        public string? rpm { get; set; }
        public string? sr_rpm { get; set; }
        public string? cluster_head { get; set; }
        public string? circle_head { get; set; }
        public string? rbm_zonal_head { get; set; }
        public string? regional_head { get; set; }
        public string? bbh { get; set; }
        public string? prefix { get; set; }
        public string? batch_id { get; set; }


    }
    // Models/ReportType.cs
    public enum ReportType
    {
        RMSFormatReport,//0
        ClickPSSReport,//1
        PremUploadReport,//2
        AMSBancaReport,//3
        // Add other report types as needed
    }


}
