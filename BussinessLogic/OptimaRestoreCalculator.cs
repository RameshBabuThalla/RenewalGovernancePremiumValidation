using RenewalGovernancePremiumValidation.Data;
using RenewalGovernancePremiumValidation.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Npgsql;
using Serilog.Core;
using System.Configuration;
using System.Data;
using System.Reflection;
using DataTable = System.Data.DataTable;


namespace RenewalGovernancePremiumValidation.BussinessLogic
{
    public class OptimaRestoreCalculator
    {
        IEnumerable<OptimaRestoreRNE> OptimaRestoreValidationResult;
        private readonly HDFCDbContext dbContext;
        private readonly ILogger<OptimaRestoreCalculator> _logger;
        public OptimaRestoreCalculator(HDFCDbContext hDFCDbContext, ILogger<OptimaRestoreCalculator> logger)
        {
            // this.dbContext = hDFCDbContext;
            _logger = logger;
        }
        public async Task GetOptimaRestoreValidation(string policyNo, HDFCDbContext dbContext)
        {

            //IEnumerable<OptimaRestoreRNE> OptimaRestoreValidationResultUpSell = Enumerable.Empty<OptimaRestoreRNE>();
            //_logger.LogInformation("GetOptimaRestoreValidation is Started!");
            var orRNEData = await GetGCOptimaRestoreCalculationAsync(policyNo, dbContext, 1, 1000);
            _logger.LogInformation("OptimaRestoreValidation is Started!");
            await OptimaRestoreValidation(orRNEData);


            //        foreach (var row in orRNEData)
            //        {
            //            if (row.upselltype1 == "SI_UPSELL" || row.upselltype2 == "SI_UPSELL" || row.upselltype3 == "SI_UPSELL" || row.upselltype4 == "SI_UPSELL" || row.upselltype5 == "SI_UPSELL" || row.upselltype1 == "UPSELLBASESI_1" || row.upselltype2 == "UPSELLBASESI_1" || row.upselltype3 == "UPSELLBASESI_1" || row.upselltype4 == "UPSELLBASESI_1" || row.upselltype5 == "UPSELLBASESI_1")
            //            {
            //                OptimaRestoreValidationResultUpSell = await OptimaRestoreValidationupsell(orRNEData);
            //                break;
            //            }
            //        }
            //        _logger.LogInformation("OptimaRestoreValidation {result}!");
            //        ORPremiumValidationUpSell objORPremiumValidationUpSell = new ORPremiumValidationUpSell
            //        {
            //            policy_number = OptimaRestoreValidationResult.FirstOrDefault()?.policy_number,
            //            prod_code = OptimaRestoreValidationResult.FirstOrDefault()?.prod_code,
            //            reference_num = OptimaRestoreValidationResult.FirstOrDefault()?.reference_num,
            //            prod_name = OptimaRestoreValidationResult.FirstOrDefault()?.prod_name,
            //            batchid = OptimaRestoreValidationResult.FirstOrDefault()?.batchid,
            //            customer_id = OptimaRestoreValidationResult.FirstOrDefault()?.customer_id,
            //            customername = OptimaRestoreValidationResult.FirstOrDefault()?.customername,
            //            policy_start_date = OptimaRestoreValidationResult.FirstOrDefault()?.policy_start_date,
            //            policy_expiry_date = OptimaRestoreValidationResult.FirstOrDefault()?.policy_expiry_date,
            //            txt_salutation = OptimaRestoreValidationResult.FirstOrDefault()?.txt_salutation,
            //            location_code = OptimaRestoreValidationResult.FirstOrDefault()?.location_code,
            //            txt_apartment = OptimaRestoreValidationResult.FirstOrDefault()?.txt_apartment,
            //            txt_street = OptimaRestoreValidationResult.FirstOrDefault()?.txt_street,
            //            txt_areavillage = OptimaRestoreValidationResult.FirstOrDefault()?.txt_areavillage,
            //            txt_citydistrict = OptimaRestoreValidationResult.FirstOrDefault()?.txt_citydistrict,
            //            txt_state = OptimaRestoreValidationResult.FirstOrDefault()?.txt_state,
            //            state_code = OptimaRestoreValidationResult.FirstOrDefault()?.state_code,
            //            state_regis = OptimaRestoreValidationResult.FirstOrDefault()?.state_regis,
            //            txt_pincode = OptimaRestoreValidationResult.FirstOrDefault()?.txt_pincode,
            //            txt_nationality = OptimaRestoreValidationResult.FirstOrDefault()?.txt_nationality,
            //            split_flag = OptimaRestoreValidationResult.FirstOrDefault()?.split_flag,
            //            txt_family = OptimaRestoreValidationResult.FirstOrDefault()?.txt_family,
            //            policyplan = OptimaRestoreValidationResult.FirstOrDefault()?.policyplan,
            //            policy_type = OptimaRestoreValidationResult.FirstOrDefault()?.policy_type,
            //            policy_period = OptimaRestoreValidationResult.FirstOrDefault()?.policy_period,
            //            verticalname = OptimaRestoreValidationResult.FirstOrDefault()?.verticalname,
            //            vertical_name = OptimaRestoreValidationResult.FirstOrDefault()?.vertical_name,
            //            no_of_members = OptimaRestoreValidationResult.FirstOrDefault()?.no_of_members,
            //            eldest_member = OptimaRestoreValidationResult.FirstOrDefault()?.eldest_member,

            //            loading_per_insured_1 = OptimaRestoreValidationResult.FirstOrDefault()?.loading_per_insured_1,
            //            loading_per_insured_2 = OptimaRestoreValidationResult.FirstOrDefault()?.loading_per_insured_2,
            //            loading_per_insured_3 = OptimaRestoreValidationResult.FirstOrDefault()?.loading_per_insured_3,
            //            loading_per_insured_4 = OptimaRestoreValidationResult.FirstOrDefault()?.loading_per_insured_4,
            //            loading_per_insured_5 = OptimaRestoreValidationResult.FirstOrDefault()?.loading_per_insured_5,
            //            loading_per_insured_6 = OptimaRestoreValidationResult.FirstOrDefault()?.loading_per_insured_6,

            //            upselltype1 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upselltype1,
            //            upselltype2 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upselltype2,
            //            upselltype3 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upselltype3,
            //            upselltype4 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upselltype4,
            //            upselltype5 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upselltype5,

            //            upsellvalue1 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upsellvalue1,
            //            upsellvalue2 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upsellvalue2,
            //            upsellvalue3 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upsellvalue3,
            //            upsellvalue4 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upsellvalue4,
            //            upsellvalue5 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upsellvalue5,

            //            upsellpremium1 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upsellpremium1,
            //            upsellpremium2 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upsellpremium2,
            //            upsellpremium3 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upsellpremium3,
            //            upsellpremium4 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upsellpremium4,
            //            upsellpremium5 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.upsellpremium5,

            //            loyalty_discount = OptimaRestoreValidationResult.FirstOrDefault()?.loyalty_discount,
            //            employee_discount = OptimaRestoreValidationResult.FirstOrDefault()?.employee_discount,
            //            online_discount = OptimaRestoreValidationResult.FirstOrDefault()?.online_discount,
            //            family_discount = OptimaRestoreValidationResult.FirstOrDefault()?.family_discount,
            //            longterm_discount = OptimaRestoreValidationResult.FirstOrDefault()?.longterm_discount,

            //            insuredname_1 = OptimaRestoreValidationResult.FirstOrDefault()?.insuredname_1,
            //            insuredname_2 = OptimaRestoreValidationResult.FirstOrDefault()?.insuredname_2,
            //            insuredname_3 = OptimaRestoreValidationResult.FirstOrDefault()?.insuredname_3,
            //            insuredname_4 = OptimaRestoreValidationResult.FirstOrDefault()?.insuredname_4,
            //            insuredname_5 = OptimaRestoreValidationResult.FirstOrDefault()?.insuredname_5,
            //            insuredname_6 = OptimaRestoreValidationResult.FirstOrDefault()?.insuredname_6,

            //            sum_insured1 = OptimaRestoreValidationResult.FirstOrDefault()?.sum_insured1,
            //            sum_insured2 = OptimaRestoreValidationResult.FirstOrDefault()?.sum_insured2,
            //            sum_insured3 = OptimaRestoreValidationResult.FirstOrDefault()?.sum_insured3,
            //            sum_insured4 = OptimaRestoreValidationResult.FirstOrDefault()?.sum_insured4,
            //            sum_insured5 = OptimaRestoreValidationResult.FirstOrDefault()?.sum_insured5,
            //            sum_insured6 = OptimaRestoreValidationResult.FirstOrDefault()?.sum_insured6,

            //            insured_cb1 = OptimaRestoreValidationResult.FirstOrDefault()?.insured_cb1,
            //            insured_cb2 = OptimaRestoreValidationResult.FirstOrDefault()?.insured_cb2,
            //            insured_cb3 = OptimaRestoreValidationResult.FirstOrDefault()?.insured_cb3,
            //            insured_cb4 = OptimaRestoreValidationResult.FirstOrDefault()?.insured_cb4,
            //            insured_cb5 = OptimaRestoreValidationResult.FirstOrDefault()?.insured_cb5,
            //            insured_cb6 = OptimaRestoreValidationResult.FirstOrDefault()?.insured_cb6,

            //            base_Premium1 = OptimaRestoreValidationResult.FirstOrDefault()?.base_Premium1,
            //            base_Premium2 = OptimaRestoreValidationResult.FirstOrDefault()?.base_Premium2,
            //            base_Premium3 = OptimaRestoreValidationResult.FirstOrDefault()?.base_Premium3,
            //            base_Premium4 = OptimaRestoreValidationResult.FirstOrDefault()?.base_Premium4,
            //            base_Premium5 = OptimaRestoreValidationResult.FirstOrDefault()?.base_Premium5,
            //            base_Premium6 = OptimaRestoreValidationResult.FirstOrDefault()?.base_Premium6,
            //            base_Premium = OptimaRestoreValidationResult.FirstOrDefault()?.base_Premium,

            //            basePremLoading_Insured1 = OptimaRestoreValidationResult.FirstOrDefault()?.basePremLoading_Insured1,
            //            basePremLoading_Insured2 = OptimaRestoreValidationResult.FirstOrDefault()?.basePremLoading_Insured2,
            //            basePremLoading_Insured3 = OptimaRestoreValidationResult.FirstOrDefault()?.basePremLoading_Insured3,
            //            basePremLoading_Insured4 = OptimaRestoreValidationResult.FirstOrDefault()?.basePremLoading_Insured4,
            //            basePremLoading_Insured5 = OptimaRestoreValidationResult.FirstOrDefault()?.basePremLoading_Insured5,
            //            basePremLoading_Insured6 = OptimaRestoreValidationResult.FirstOrDefault()?.basePremLoading_Insured6,
            //            basePrem_Loading = OptimaRestoreValidationResult.FirstOrDefault()?.basePrem_Loading,

            //            orbaseLoading_Premium = OptimaRestoreValidationResult.FirstOrDefault()?.orbaseLoading_Premium,
            //            orloyalty_Discount = OptimaRestoreValidationResult.FirstOrDefault()?.orloyalty_Discount,
            //            oremployee_Discount = OptimaRestoreValidationResult.FirstOrDefault()?.oremployee_Discount,
            //            oronline_Discount = OptimaRestoreValidationResult.FirstOrDefault()?.oronline_Discount,
            //            orfamily_Discount = OptimaRestoreValidationResult.FirstOrDefault()?.orfamily_Discount,
            //            orlongTerm_Discount = OptimaRestoreValidationResult.FirstOrDefault()?.orlongTerm_Discount,
            //            orbase_Premium = OptimaRestoreValidationResult.FirstOrDefault()?.orbase_Premium.HasValue == true
            //? (decimal?)Math.Round(OptimaRestoreValidationResult.FirstOrDefault().orbase_Premium.Value, 2)
            //: (decimal?)null,
            //            unlimited_restore = OptimaRestoreValidationResult.FirstOrDefault()?.unlimited_restore,
            //            finalbase_premium = OptimaRestoreValidationResult.FirstOrDefault()?.finalbase_premium,

            //            hdc_opt = OptimaRestoreValidationResult.FirstOrDefault()?.hdc_opt,
            //            hdc_si = OptimaRestoreValidationResult.FirstOrDefault()?.hdc_si,
            //            hdc_rider_premium = OptimaRestoreValidationResult.FirstOrDefault()?.hdc_rider_premium,
            //            hdc_family_discount = OptimaRestoreValidationResult.FirstOrDefault()?.hdc_family_discount,
            //            hdc_longterm_discount = OptimaRestoreValidationResult.FirstOrDefault()?.hdc_longterm_discount,
            //            hdc_final_premium = OptimaRestoreValidationResult.FirstOrDefault()?.hdc_final_premium,

            //            pr_opt = OptimaRestoreValidationResult.FirstOrDefault()?.pr_opt,
            //            pr_insured_1 = OptimaRestoreValidationResult.FirstOrDefault()?.pr_insured_1,
            //            pr_insured_2 = OptimaRestoreValidationResult.FirstOrDefault()?.pr_insured_2,
            //            pr_insured_3 = OptimaRestoreValidationResult.FirstOrDefault()?.pr_insured_3,
            //            pr_insured_4 = OptimaRestoreValidationResult.FirstOrDefault()?.pr_insured_4,
            //            pr_insured_5 = OptimaRestoreValidationResult.FirstOrDefault()?.pr_insured_5,
            //            pr_insured_6 = OptimaRestoreValidationResult.FirstOrDefault()?.pr_insured_6,
            //            pr_ProtectorRider_Premium = OptimaRestoreValidationResult.FirstOrDefault()?.pr_ProtectorRider_Premium,

            //            pr_loading_insured1 = OptimaRestoreValidationResult.FirstOrDefault()?.pr_loading_insured1,
            //            pr_loading_insured2 = OptimaRestoreValidationResult.FirstOrDefault()?.pr_loading_insured2,
            //            pr_loading_insured3 = OptimaRestoreValidationResult.FirstOrDefault()?.pr_loading_insured3,
            //            pr_loading_insured4 = OptimaRestoreValidationResult.FirstOrDefault()?.pr_loading_insured4,
            //            pr_loading_insured5 = OptimaRestoreValidationResult.FirstOrDefault()?.pr_loading_insured5,
            //            pr_loading_insured6 = OptimaRestoreValidationResult.FirstOrDefault()?.pr_loading_insured6,
            //            pr_protectorriderloading_premium = OptimaRestoreValidationResult.FirstOrDefault()?.pr_protectorriderloading_premium,

            //            pr_BaseLoading_Premium = OptimaRestoreValidationResult.FirstOrDefault()?.pr_BaseLoading_Premium,
            //            pr_Family_Discount = OptimaRestoreValidationResult.FirstOrDefault()?.pr_Family_Discount,
            //            pr_LongTerm_Discount = OptimaRestoreValidationResult.FirstOrDefault()?.pr_LongTerm_Discount,
            //            prpremium_Protector_Rider_Premium = OptimaRestoreValidationResult.FirstOrDefault()?.prpremium_Protector_Rider_Premium,

            //            individual_personalAR_opt = OptimaRestoreValidationResult.FirstOrDefault()?.individual_personalAR_opt,
            //            individual_personalAR_SI = OptimaRestoreValidationResult.FirstOrDefault()?.individual_personalAR_SI,
            //            individual_personalAR_Amt = OptimaRestoreValidationResult.FirstOrDefault()?.individual_personalAR_Amt,
            //            individual_personalAR_LongTermDiscount = OptimaRestoreValidationResult.FirstOrDefault()?.individual_personalAR_LongTermDiscount,
            //            individual_Personal_AccidentRiderPremium = OptimaRestoreValidationResult.FirstOrDefault()?.individual_Personal_AccidentRiderPremium,

            //            criticalAdvantage_Rider_opt = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvantage_Rider_opt,
            //            criticalAdvantageRider_SumInsured_1 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvantageRider_SumInsured_1,
            //            criticalAdvantageRider_SumInsured_2 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvantageRider_SumInsured_2,
            //            criticalAdvantageRider_SumInsured_3 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvantageRider_SumInsured_3,
            //            criticalAdvantageRider_SumInsured_4 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvantageRider_SumInsured_4,
            //            criticalAdvantageRider_SumInsured_5 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvantageRider_SumInsured_5,
            //            criticalAdvantageRider_SumInsured_6 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvantageRider_SumInsured_6,
            //            criticalAdvantageRider_Insured_1 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvantageRider_Insured_1,
            //            criticalAdvantageRider_Insured_2 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvantageRider_Insured_2,
            //            criticalAdvantageRider_Insured_3 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvantageRider_Insured_3,
            //            criticalAdvantageRider_Insured_4 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvantageRider_Insured_4,
            //            criticalAdvantageRider_Insured_5 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvantageRider_Insured_5,
            //            criticalAdvantageRider_Insured_6 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvantageRider_Insured_6,
            //            criticalAdvantage_RiderBase_Premium = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvantage_RiderBase_Premium,

            //            criticalAdvrider_loadinginsured1 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvrider_loadinginsured1,
            //            criticalAdvrider_loadinginsured2 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvrider_loadinginsured2,
            //            criticalAdvrider_loadinginsured3 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvrider_loadinginsured3,
            //            criticalAdvrider_loadinginsured4 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvrider_loadinginsured4,
            //            criticalAdvrider_loadinginsured5 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvrider_loadinginsured5,
            //            criticalAdvrider_loadinginsured6 = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvrider_loadinginsured6,
            //            criticalAdvriderloading = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvriderloading,
            //            criticalAdvriderbase_loading_premium = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvriderbase_loading_premium,
            //            criticalAdvRiderPremium_Family_Discount = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvRiderPremium_Family_Discount,
            //            criticalAdvRiderPremium_LongTerm_Discount = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdvRiderPremium_LongTerm_Discount,
            //            criticalAdv_Rider_Premium = OptimaRestoreValidationResult.FirstOrDefault()?.criticalAdv_Rider_Premium,

            //            net_premium = OptimaRestoreValidationResult.FirstOrDefault()?.net_premium.HasValue == true
            //                          ? (decimal?)Math.Round(OptimaRestoreValidationResult.FirstOrDefault().net_premium.Value, 2)
            //                          : (decimal?)null,
            //            final_Premium = OptimaRestoreValidationResult.FirstOrDefault()?.final_Premium.HasValue == true
            //                          ? (decimal?)Math.Round(OptimaRestoreValidationResult.FirstOrDefault().final_Premium.Value, 2)
            //                          : (decimal?)null,
            //            gst = OptimaRestoreValidationResult.FirstOrDefault()?.gst.HasValue == true
            //                          ? (decimal?)Math.Round(OptimaRestoreValidationResult.FirstOrDefault().gst.Value, 2)
            //                          : (decimal?)null,
            //            cross_Check1 = OptimaRestoreValidationResult.FirstOrDefault()?.cross_Check.HasValue == true
            //                          ? (decimal?)Math.Round(OptimaRestoreValidationResult.FirstOrDefault().cross_Check.Value, 2)
            //                          : (decimal?)null,
            //            cross_Check2 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.cross_Check.HasValue == true
            //                          ? (decimal?)Math.Round(OptimaRestoreValidationResultUpSell.FirstOrDefault().cross_Check.Value, 2)
            //                          : (decimal?)null,
            //            or_total_Premium = OptimaRestoreValidationResult.FirstOrDefault()?.or_total_Premium.HasValue == true
            //                          ? (decimal?)Math.Round(OptimaRestoreValidationResult.FirstOrDefault().or_total_Premium.Value, 2)
            //                          : (decimal?)null,
            //            or_netpremium = OptimaRestoreValidationResult.FirstOrDefault()?.or_netpremium.HasValue == true
            //                          ? (decimal?)Math.Round(OptimaRestoreValidationResult.FirstOrDefault().or_netpremium.Value, 2)
            //                          : (decimal?)null,
            //            or_GST = OptimaRestoreValidationResult.FirstOrDefault()?.or_netpremium.HasValue == true
            //                          ? (decimal?)Math.Round(OptimaRestoreValidationResult.FirstOrDefault().or_netpremium.Value, 2)
            //                          : (decimal?)null,

            //            upsell_sum_insured1 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.sum_insured1,
            //            upsell_sum_insured2 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.sum_insured2,
            //            upsell_sum_insured3 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.sum_insured3,
            //            upsell_sum_insured4 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.sum_insured4,
            //            upsell_sum_insured5 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.sum_insured5,
            //            upsell_sum_insured6 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.sum_insured6,

            //            base_upsell_Premium1 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.base_Premium1,
            //            base_upsell_Premium2 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.base_Premium2,
            //            base_upsell_Premium3 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.base_Premium3,
            //            base_upsell_Premium4 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.base_Premium4,
            //            base_upsell_Premium5 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.base_Premium5,
            //            base_upsell_Premium6 = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.base_Premium6,
            //            final_Premium_upsell = OptimaRestoreValidationResultUpSell.FirstOrDefault()?.final_Premium_upsell,
            //        };
            //        decimal? crosscheck1 = objORPremiumValidationUpSell.cross_Check1;
            //        decimal? crosscheck2 = objORPremiumValidationUpSell.cross_Check2;
            //        decimal? netPremium = objORPremiumValidationUpSell.net_premium;
            //        decimal? finalPremium = objORPremiumValidationUpSell.final_Premium;
            //        decimal? gst = objORPremiumValidationUpSell.gst;
            //        if (objORPremiumValidationUpSell?.policy_number == null)
            //        {
            //            Console.WriteLine("Policy number not found.");
            //        }
            //        var record_idst = await dbContext.idst_renewal_data_rgs.AsNoTracking().FirstOrDefaultAsync(item => item.certificate_no == policyNo.ToString());
            //        if (record_idst != null)
            //        {
            //            if (objORPremiumValidationUpSell.insured_cb1 == string.Empty && objORPremiumValidationUpSell.insured_cb1 == null)
            //            {
            //                record_idst.rn_generation_status = "IT Issue - No CB";
            //                record_idst.error_description = "CB SI cannot be zero";
            //                dbContext.idst_renewal_data_rgs.UpdateRange();
            //                await dbContext.SaveChangesAsync();
            //            }
            //            else
            //            {
            //                await HandleCrosschecksAndUpdateStatus(orRNEData.FirstOrDefault(), crosscheck1, crosscheck2, netPremium, finalPremium, gst);
            //            }
            //        }
            //        var record = await dbContext.rne_calculated_cover_rg.AsNoTracking().FirstOrDefaultAsync(item => item.policy_number == policyNo.ToString());
            //        if (objORPremiumValidationUpSell != null)
            //        {
            //            var no_of_members = objORPremiumValidationUpSell.no_of_members;
            //            var ridercount = 5;
            //            var policy_number = objORPremiumValidationUpSell.policy_number;
            //            var reference_number = objORPremiumValidationUpSell.reference_num;
            //            var newRecord = new List<rne_calculated_cover_rg>();
            //            for (int i = 1; i <= no_of_members; i++)
            //            {
            //                var sumInsured = Convert.ToDecimal(objORPremiumValidationUpSell.GetType().GetProperty($"sum_insured{i}")?.GetValue(objORPremiumValidationUpSell));
            //                var basePremium = Convert.ToDecimal(objORPremiumValidationUpSell.GetType().GetProperty($"base_Premium{i}")?.GetValue(objORPremiumValidationUpSell));
            //                var sumInsuredupsell = Convert.ToDecimal(objORPremiumValidationUpSell.GetType().GetProperty($"upsell_sum_insured{i}")?.GetValue(objORPremiumValidationUpSell));
            //                var basePremiumupsell = Convert.ToDecimal(objORPremiumValidationUpSell.GetType().GetProperty($"base_upsell_Premium{i}")?.GetValue(objORPremiumValidationUpSell));
            //                var finalPremiumupsell = Convert.ToDecimal(objORPremiumValidationUpSell.GetType().GetProperty($"final_Premium_upsell")?.GetValue(objORPremiumValidationUpSell));
            //                if (no_of_members > 1 && i >= 2 && i <= 6)
            //                {
            //                    basePremium *= 0.45m;
            //                    basePremiumupsell *= 0.45m;
            //                }
            //                var newRecord1 = new rne_calculated_cover_rg
            //                {
            //                    policy_number = policy_number,
            //                    referencenum = reference_number,
            //                    suminsured = sumInsured,
            //                    premium = basePremium,
            //                    riskname = objORPremiumValidationUpSell.GetType().GetProperty($"insuredname_{i}")?.GetValue(objORPremiumValidationUpSell)?.ToString(),
            //                    covername = "Basic Optima Restore Cover"
            //                };
            //                var newRecord2 = new rne_calculated_cover_rg
            //                {
            //                    isupsell = 1,
            //                    policy_number = policy_number,
            //                    referencenum = reference_number,
            //                    suminsured = sumInsuredupsell,
            //                    premium = basePremiumupsell,
            //                    totalpremium = finalPremiumupsell,//total premium column in rne_calculated_cover_rg will store the finalpremiumupsell from premium computation
            //                    riskname = objORPremiumValidationUpSell.GetType().GetProperty($"insuredname_{i}")?.GetValue(objORPremiumValidationUpSell)?.ToString(),
            //                    covername = "Upsell Cover"
            //                };
            //                newRecord.Add(newRecord1);
            //                newRecord.Add(newRecord2);
            //            }
            //            for (int j = 1; j <= ridercount; j++)
            //            {
            //                var riderPremium = Convert.ToDecimal(objORPremiumValidationUpSell.GetType().GetProperty($"criticalAdvantageRider_Insured_{j}")?.GetValue(objORPremiumValidationUpSell));
            //                var riderPremiumpr = Convert.ToDecimal(objORPremiumValidationUpSell.GetType().GetProperty($"pr_insured_{j}")?.GetValue(objORPremiumValidationUpSell));
            //                var riderPremiumipa = Convert.ToDecimal(objORPremiumValidationUpSell.GetType().GetProperty($"individual_Personal_AccidentRiderPremium")?.GetValue(objORPremiumValidationUpSell));
            //                var riderPremiumhdc = Convert.ToDecimal(objORPremiumValidationUpSell.GetType().GetProperty($"hdc_Rider_Premium{j}")?.GetValue(objORPremiumValidationUpSell));
            //                var riderPremiumur = Convert.ToDecimal(objORPremiumValidationUpSell.GetType().GetProperty($"unlimited_restore")?.GetValue(objORPremiumValidationUpSell));
            //                if (riderPremium > 0)
            //                {
            //                    var riderRecord = new rne_calculated_cover_rg
            //                    {
            //                        policy_number = policy_number,
            //                        referencenum = reference_number,
            //                        riskname = objORPremiumValidationUpSell.GetType().GetProperty($"insuredname_{j}")?.GetValue(objORPremiumValidationUpSell)?.ToString(),
            //                        premium = riderPremium,
            //                        covername = "Critical Advantage Rider"
            //                    };
            //                    newRecord.Add(riderRecord);
            //                }
            //                if (riderPremiumpr > 0)
            //                {
            //                    var riderRecordpr = new rne_calculated_cover_rg
            //                    {
            //                        policy_number = policy_number,
            //                        referencenum = reference_number,
            //                        riskname = objORPremiumValidationUpSell.GetType().GetProperty($"insuredname_{j}")?.GetValue(objORPremiumValidationUpSell)?.ToString(),
            //                        premium = riderPremiumpr,
            //                        covername = "Protector Rider"
            //                    };
            //                    newRecord.Add(riderRecordpr);
            //                }
            //                if (riderPremiumipa > 0)
            //                {
            //                    var riderRecordipa = new rne_calculated_cover_rg
            //                    {
            //                        policy_number = policy_number,
            //                        referencenum = reference_number,
            //                        riskname = objORPremiumValidationUpSell.GetType().GetProperty($"insuredname_{j}")?.GetValue(objORPremiumValidationUpSell)?.ToString(),
            //                        premium = riderPremiumipa,
            //                        covername = "Individual Personal Accident Rider"
            //                    };
            //                    newRecord.Add(riderRecordipa);
            //                }
            //                if (riderPremiumhdc > 0)
            //                {
            //                    var riderRecordhdc = new rne_calculated_cover_rg
            //                    {
            //                        policy_number = policy_number,
            //                        referencenum = reference_number,
            //                        riskname = objORPremiumValidationUpSell.GetType().GetProperty($"insuredname_{j}")?.GetValue(objORPremiumValidationUpSell)?.ToString(),
            //                        premium = riderPremiumhdc,
            //                        covername = "Hospital Daily Cash Rider"
            //                    };
            //                    newRecord.Add(riderRecordhdc);
            //                }
            //                if (riderPremiumur > 0)
            //                {
            //                    var riderRecordur = new rne_calculated_cover_rg
            //                    {
            //                        policy_number = policy_number,
            //                        referencenum = reference_number,
            //                        riskname = objORPremiumValidationUpSell.GetType().GetProperty($"insuredname_{j}")?.GetValue(objORPremiumValidationUpSell)?.ToString(),
            //                        premium = riderPremiumur,
            //                        covername = "Unlimited Restore"
            //                    };
            //                    newRecord.Add(riderRecordur);
            //                }
            //            }
            //            dbContext.rne_calculated_cover_rg.AddRange(newRecord);
            //            await dbContext.SaveChangesAsync();
            //        }
            //return objORPremiumValidationUpSell;
        }

        async Task HandleCrosschecksAndUpdateStatus(OptimaRestoreRNE orRNEData, decimal? crosscheck1, decimal? crosscheck2, decimal? netPremium, decimal? finalPremium, decimal? gst)
        {
            var record = dbContext.idst_renewal_data_rgs.FirstOrDefault(item => item.certificate_no == orRNEData.policy_number.ToString());
            if (record != null && record.rn_generation_status == "Reconciliation Successful")
            {
                decimal crosscheck1Value = crosscheck1.HasValue ? crosscheck1.Value : 0;
                decimal crosscheck2Value = crosscheck2.HasValue ? crosscheck2.Value : 0;
                if (crosscheck1.HasValue)
                {
                    if ((Math.Abs(crosscheck1.Value) <= 10) || ((Math.Abs(crosscheck1.Value) <= 10 && Math.Abs(crosscheck2Value) <= 10)))
                    {
                        record.rn_generation_status = "RN Generation Awaited";
                        record.final_remarks = "RN Generation Awaited";
                        record.dispatch_status = "PDF Gen Under Process With CLICK PSS Team";
                    }
                    else if ((Math.Abs(crosscheck1.Value) > 10) || (Math.Abs(crosscheck1.Value) > 10 && Math.Abs(crosscheck2Value) <= 10))
                    {
                        record.rn_generation_status = "IT Issue - QC Failed";
                        record.final_remarks = "IT Issues";
                        record.dispatch_status = "Revised Extraction REQ From IT Team QC Failed Cases";
                        record.error_description = "Premium verification failed due to premium difference of more than 10 rupees";
                    }
                    else if (Math.Abs(crosscheck1.Value) <= 10 && Math.Abs(crosscheck2Value) > 10)
                    {
                        record.rn_generation_status = "IT Issue - Upsell QC Failed";
                    }
                    else if (Math.Abs(crosscheck1.Value) > 10 && Math.Abs(crosscheck2Value) > 10)
                    {
                        record.rn_generation_status = "IT Issue - QC Failed";
                    }
                }
                record.verified_prem = netPremium;
                record.verified_gst = gst;
                record.verified_total_prem = finalPremium;
                dbContext.idst_renewal_data_rgs.Update(record);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OptimaRestoreRNE>> GetGCOptimaRestoreCalculationAsync(string policyNo, HDFCDbContext dbContext, int pageNumber, int pageSize)
        {
            var orRNEData = await (
                 from or in dbContext.rne_healthtab
                 join oridst in dbContext.idst_renewal_data_rgs on or.policy_number equals oridst.certificate_no
                 where (or.policy_number == policyNo)
                 select new OptimaRestoreRNE
                 {
                     prod_code = or.prod_code,
                     prod_name = or.prod_name,
                     reference_num = or.reference_num,
                     policy_number = or.policy_number,
                     policy_start_date = or.policy_start_date,
                     policy_expiry_date = or.policy_expiry_date,
                     policy_period = or.policy_period,
                     tier_type = or.tier_type,
                     policyplan = or.policyplan,
                     policy_type = or.policy_type,
                     txt_family = or.txt_family,
                     num_tot_premium = or.num_tot_premium,
                     num_net_premium = or.num_net_premium,
                     num_service_tax = or.num_service_tax,

                     pollddesc1 = or.pollddesc1,
                     pollddesc2 = or.pollddesc2,
                     pollddesc3 = or.pollddesc3,
                     pollddesc4 = or.pollddesc4,
                     pollddesc5 = or.pollddesc5,

                     upselltype1 = or.upselltype1,
                     upselltype2 = or.upselltype2,
                     upselltype3 = or.upselltype3,
                     upselltype4 = or.upselltype4,
                     upselltype5 = or.upselltype5,

                     upsellvalue1 = or.upsellvalue1,
                     upsellvalue2 = or.upsellvalue2,
                     upsellvalue3 = or.upsellvalue3,
                     upsellvalue4 = or.upsellvalue4,
                     upsellvalue5 = or.upsellvalue5,

                     upsellpremium1 = or.upsellpremium1,
                     upsellpremium2 = or.upsellpremium2,
                     upsellpremium3 = or.upsellpremium3,
                     upsellpremium4 = or.upsellpremium4,
                     upsellpremium5 = or.upsellpremium5,

                     txt_insuredname1 = or.txt_insuredname1,
                     txt_insuredname2 = or.txt_insuredname2,
                     txt_insuredname3 = or.txt_insuredname3,
                     txt_insuredname4 = or.txt_insuredname4,
                     txt_insuredname5 = or.txt_insuredname5,
                     txt_insuredname6 = or.txt_insuredname6,

                     txt_insured_dob1 = or.txt_insured_dob1,
                     txt_insured_dob2 = or.txt_insured_dob2,
                     txt_insured_dob3 = or.txt_insured_dob3,
                     txt_insured_dob4 = or.txt_insured_dob4,
                     txt_insured_dob5 = or.txt_insured_dob5,
                     txt_insured_dob6 = or.txt_insured_dob6,

                     txt_insured_age1 = or.txt_insured_age1,
                     txt_insured_age2 = or.txt_insured_age2,
                     txt_insured_age3 = or.txt_insured_age3,
                     txt_insured_age4 = or.txt_insured_age4,
                     txt_insured_age5 = or.txt_insured_age5,
                     txt_insured_age6 = or.txt_insured_age6,

                     txt_insured_gender1 = or.txt_insured_gender1,
                     txt_insured_gender2 = or.txt_insured_gender2,
                     txt_insured_gender3 = or.txt_insured_gender3,
                     txt_insured_gender4 = or.txt_insured_gender4,
                     txt_insured_gender5 = or.txt_insured_gender5,
                     txt_insured_gender6 = or.txt_insured_gender6,

                     sum_insured1 = or.sum_insured1,
                     sum_insured2 = or.sum_insured2,
                     sum_insured3 = or.sum_insured3,
                     sum_insured4 = or.sum_insured4,
                     sum_insured5 = or.sum_insured5,
                     sum_insured6 = or.sum_insured6,

                     insured_cb1 = or.insured_cb1,
                     insured_cb2 = or.insured_cb2,
                     insured_cb3 = or.insured_cb3,
                     insured_cb4 = or.insured_cb4,
                     insured_cb5 = or.insured_cb5,
                     insured_cb6 = or.insured_cb6,

                     premium_insured1 = or.premium_insured1,
                     premium_insured2 = or.premium_insured2,
                     premium_insured3 = or.premium_insured3,
                     premium_insured4 = or.premium_insured4,
                     premium_insured5 = or.premium_insured5,
                     premium_insured6 = or.premium_insured6,

                     coverbaseloadingrate1 = or.coverbaseloadingrate1,
                     coverbaseloadingrate2 = or.coverbaseloadingrate2,
                     coverbaseloadingrate3 = or.coverbaseloadingrate3,
                     coverbaseloadingrate4 = or.coverbaseloadingrate4,
                     coverbaseloadingrate5 = or.coverbaseloadingrate5,
                     coverbaseloadingrate6 = or.coverbaseloadingrate6,

                     covername11 = or.covername11,
                     covername12 = or.covername12,
                     covername13 = or.covername13,
                     covername14 = or.covername14,
                     covername15 = or.covername15,
                     covername16 = or.covername16,
                     covername17 = or.covername17,
                     covername18 = or.covername18,
                     covername19 = or.covername19,
                     covername21 = or.covername21,
                     covername22 = or.covername22,
                     covername23 = or.covername23,
                     covername24 = or.covername24,
                     covername25 = or.covername25,
                     covername26 = or.covername26,
                     covername27 = or.covername27,
                     covername28 = or.covername28,
                     covername29 = or.covername29,
                     covername31 = or.covername31,
                     covername32 = or.covername32,
                     covername33 = or.covername33,
                     covername34 = or.covername34,
                     covername35 = or.covername35,
                     covername36 = or.covername36,
                     covername37 = or.covername37,
                     covername38 = or.covername38,
                     covername39 = or.covername39,
                     covername41 = or.covername41,
                     covername42 = or.covername42,
                     covername43 = or.covername43,
                     covername44 = or.covername44,
                     covername45 = or.covername45,
                     covername46 = or.covername46,
                     covername47 = or.covername47,
                     covername48 = or.covername48,
                     covername49 = or.covername49,
                     covername51 = or.covername51,
                     covername52 = or.covername52,
                     covername53 = or.covername53,
                     covername54 = or.covername54,
                     covername55 = or.covername55,
                     covername56 = or.covername56,
                     covername57 = or.covername57,
                     covername58 = or.covername58,
                     covername59 = or.covername59,
                     covername61 = or.covername61,
                     covername62 = or.covername62,
                     covername63 = or.covername63,
                     covername64 = or.covername64,
                     covername65 = or.covername65,
                     covername66 = or.covername66,
                     covername67 = or.covername67,
                     covername68 = or.covername68,
                     covername69 = or.covername69,
                     covername71 = or.covername71,
                     covername72 = or.covername72,
                     covername73 = or.covername73,
                     covername74 = or.covername74,
                     covername75 = or.covername75,
                     covername76 = or.covername76,
                     covername77 = or.covername77,
                     covername78 = or.covername78,
                     covername79 = or.covername79,
                     covername81 = or.covername81,
                     covername82 = or.covername82,
                     covername83 = or.covername83,
                     covername84 = or.covername84,
                     covername85 = or.covername85,
                     covername86 = or.covername86,
                     covername87 = or.covername87,
                     covername88 = or.covername88,
                     covername89 = or.covername89,
                     covername91 = or.covername91,
                     covername92 = or.covername92,
                     covername93 = or.covername93,
                     covername94 = or.covername94,
                     covername95 = or.covername95,
                     covername96 = or.covername96,
                     covername97 = or.covername97,
                     covername98 = or.covername98,
                     covername99 = or.covername99,
                     covername101 = or.covername101,
                     covername102 = or.covername102,
                     covername103 = or.covername103,
                     covername104 = or.covername104,
                     covername105 = or.covername105,
                     covername106 = or.covername106,
                     covername107 = or.covername107,
                     covername108 = or.covername108,
                     covername109 = or.covername109,
                     covername110 = or.covername110,
                     covername210 = or.covername210,
                     covername310 = or.covername310,
                     covername410 = or.covername410,
                     covername510 = or.covername510,
                     covername610 = or.covername610,
                     covername710 = or.covername710,
                     covername810 = or.covername810,
                     covername910 = or.covername910,
                     covername1010 = or.covername1010,

                     coversi11 = or.coversi11,
                     coversi12 = or.coversi12,
                     coversi13 = or.coversi13,
                     coversi14 = or.coversi14,
                     coversi15 = or.coversi15,
                     coversi16 = or.coversi16,
                     coversi17 = or.coversi17,
                     coversi18 = or.coversi18,
                     coversi19 = or.coversi19,
                     coversi21 = or.coversi21,
                     coversi22 = or.coversi22,
                     coversi23 = or.coversi23,
                     coversi24 = or.coversi24,
                     coversi25 = or.coversi25,
                     coversi26 = or.coversi26,
                     coversi27 = or.coversi27,
                     coversi28 = or.coversi28,
                     coversi29 = or.coversi29,
                     coversi31 = or.coversi31,
                     coversi32 = or.coversi32,
                     coversi33 = or.coversi33,
                     coversi34 = or.coversi34,
                     coversi35 = or.coversi35,
                     coversi36 = or.coversi36,
                     coversi37 = or.coversi37,
                     coversi38 = or.coversi38,
                     coversi39 = or.coversi39,
                     coversi41 = or.coversi41,
                     coversi42 = or.coversi42,
                     coversi43 = or.coversi43,
                     coversi44 = or.coversi44,
                     coversi45 = or.coversi46,
                     coversi47 = or.coversi47,
                     coversi48 = or.coversi48,
                     coversi49 = or.coversi49,
                     coversi51 = or.coversi51,
                     coversi52 = or.coversi52,
                     coversi53 = or.coversi53,
                     coversi54 = or.coversi54,
                     coversi55 = or.coversi55,
                     coversi56 = or.coversi56,
                     coversi57 = or.coversi57,
                     coversi58 = or.coversi58,
                     coversi59 = or.coversi59,
                     coversi61 = or.coversi61,
                     coversi62 = or.coversi62,
                     coversi63 = or.coversi63,
                     coversi64 = or.coversi64,
                     coversi65 = or.coversi65,
                     coversi66 = or.coversi66,
                     coversi67 = or.coversi67,
                     coversi68 = or.coversi68,
                     coversi69 = or.coversi69,
                     coversi71 = or.coversi71,
                     coversi72 = or.coversi72,
                     coversi73 = or.coversi73,
                     coversi74 = or.coversi74,
                     coversi75 = or.coversi75,
                     coversi76 = or.coversi76,
                     coversi77 = or.coversi77,
                     coversi78 = or.coversi78,
                     coversi79 = or.coversi79,
                     coversi81 = or.coversi81,
                     coversi82 = or.coversi82,
                     coversi83 = or.coversi83,
                     coversi84 = or.coversi84,
                     coversi85 = or.coversi85,
                     coversi86 = or.coversi86,
                     coversi87 = or.coversi87,
                     coversi88 = or.coversi88,
                     coversi89 = or.coversi89,
                     coversi91 = or.coversi91,
                     coversi92 = or.coversi92,
                     coversi93 = or.coversi93,
                     coversi94 = or.coversi94,
                     coversi95 = or.coversi95,
                     coversi96 = or.coversi96,
                     coversi97 = or.coversi97,
                     coversi98 = or.coversi98,
                     coversi99 = or.coversi99,
                     coversi101 = or.coversi101,
                     coversi102 = or.coversi102,
                     coversi103 = or.coversi103,
                     coversi104 = or.coversi104,
                     coversi105 = or.coversi105,
                     coversi106 = or.coversi106,
                     coversi107 = or.coversi107,
                     coversi108 = or.coversi108,
                     coversi109 = or.coversi109,
                     coversi210 = or.coversi210,
                     coversi310 = or.coversi310,
                     coversi410 = or.coversi410,
                     coversi510 = or.coversi510,
                     coversi610 = or.coversi610,
                     coversi810 = or.coversi810,
                     coversi910 = or.coversi910,
                     coversi1010 = or.coversi1010,

                     coverprem11 = or.coverprem11,
                     coverprem12 = or.coverprem12,
                     coverprem13 = or.coverprem13,
                     coverprem14 = or.coverprem14,
                     coverprem15 = or.coverprem15,
                     coverprem16 = or.coverprem16,
                     coverprem17 = or.coverprem17,
                     coverprem18 = or.coverprem18,
                     coverprem19 = or.coverprem19,
                     coverprem21 = or.coverprem21,
                     coverprem22 = or.coverprem22,
                     coverprem23 = or.coverprem23,
                     coverprem24 = or.coverprem24,
                     coverprem25 = or.coverprem25,
                     coverprem26 = or.coverprem26,
                     coverprem27 = or.coverprem27,
                     coverprem28 = or.coverprem28,
                     coverprem29 = or.coverprem29,
                     coverprem31 = or.coverprem31,
                     coverprem32 = or.coverprem32,
                     coverprem33 = or.coverprem33,
                     coverprem34 = or.coverprem34,
                     coverprem35 = or.coverprem35,
                     coverprem36 = or.coverprem36,
                     coverprem37 = or.coverprem37,
                     coverprem38 = or.coverprem38,
                     coverprem39 = or.coverprem39,
                     coverprem41 = or.coverprem41,
                     coverprem42 = or.coverprem42,
                     coverprem43 = or.coverprem43,
                     coverprem44 = or.coverprem44,
                     coverprem45 = or.coverprem46,
                     coverprem47 = or.coverprem47,
                     coverprem48 = or.coverprem48,
                     coverprem49 = or.coverprem49,
                     coverprem51 = or.coverprem51,
                     coverprem52 = or.coverprem52,
                     coverprem53 = or.coverprem53,
                     coverprem54 = or.coverprem54,
                     coverprem55 = or.coverprem55,
                     coverprem56 = or.coverprem56,
                     coverprem57 = or.coverprem57,
                     coverprem58 = or.coverprem58,
                     coverprem59 = or.coverprem59,
                     coverprem61 = or.coverprem61,
                     coverprem62 = or.coverprem62,
                     coverprem63 = or.coverprem63,
                     coverprem64 = or.coverprem64,
                     coverprem65 = or.coverprem65,
                     coverprem66 = or.coverprem66,
                     coverprem67 = or.coverprem67,
                     coverprem68 = or.coverprem68,
                     coverprem69 = or.coverprem69,
                     coverprem71 = or.coverprem71,
                     coverprem72 = or.coverprem72,
                     coverprem73 = or.coverprem73,
                     coverprem74 = or.coverprem74,
                     coverprem75 = or.coverprem75,
                     coverprem76 = or.coverprem76,
                     coverprem77 = or.coverprem77,
                     coverprem78 = or.coverprem78,
                     coverprem79 = or.coverprem79,
                     coverprem81 = or.coverprem81,
                     coverprem82 = or.coverprem82,
                     coverprem83 = or.coverprem83,
                     coverprem84 = or.coverprem84,
                     coverprem85 = or.coverprem85,
                     coverprem86 = or.coverprem86,
                     coverprem87 = or.coverprem87,
                     coverprem88 = or.coverprem88,
                     coverprem89 = or.coverprem89,
                     coverprem91 = or.coverprem91,
                     coverprem92 = or.coverprem92,
                     coverprem93 = or.coverprem93,
                     coverprem94 = or.coverprem94,
                     coverprem95 = or.coverprem95,
                     coverprem96 = or.coverprem96,
                     coverprem97 = or.coverprem97,
                     coverprem98 = or.coverprem98,
                     coverprem99 = or.coverprem99,
                     coverprem101 = or.coverprem101,
                     coverprem102 = or.coverprem102,
                     coverprem103 = or.coverprem103,
                     coverprem104 = or.coverprem104,
                     coverprem105 = or.coverprem105,
                     coverprem106 = or.coverprem106,
                     coverprem107 = or.coverprem107,
                     coverprem108 = or.coverprem108,
                     coverprem109 = or.coverprem109,
                     coverprem210 = or.coverprem210,
                     coverprem310 = or.coverprem310,
                     coverprem410 = or.coverprem410,
                     coverprem510 = or.coverprem510,
                     coverprem610 = or.coverprem610,
                     coverprem810 = or.coverprem810,
                     coverprem910 = or.coverprem910,
                     coverprem1010 = or.coverprem1010,

                     coverloadingrate11 = or.coverloadingrate11,
                     coverloadingrate12 = or.coverloadingrate12,
                     coverloadingrate13 = or.coverloadingrate13,
                     coverloadingrate14 = or.coverloadingrate14,
                     coverloadingrate15 = or.coverloadingrate15,
                     coverloadingrate16 = or.coverloadingrate16,
                     coverloadingrate17 = or.coverloadingrate17,
                     coverloadingrate18 = or.coverloadingrate18,
                     coverloadingrate19 = or.coverloadingrate19,
                     coverloadingrate21 = or.coverloadingrate21,
                     coverloadingrate22 = or.coverloadingrate22,
                     coverloadingrate23 = or.coverloadingrate23,
                     coverloadingrate24 = or.coverloadingrate24,
                     coverloadingrate25 = or.coverloadingrate25,
                     coverloadingrate26 = or.coverloadingrate26,
                     coverloadingrate27 = or.coverloadingrate27,
                     coverloadingrate28 = or.coverloadingrate28,
                     coverloadingrate29 = or.coverloadingrate29,
                     coverloadingrate31 = or.coverloadingrate31,
                     coverloadingrate32 = or.coverloadingrate32,
                     coverloadingrate33 = or.coverloadingrate33,
                     coverloadingrate34 = or.coverloadingrate34,
                     coverloadingrate35 = or.coverloadingrate35,
                     coverloadingrate36 = or.coverloadingrate36,
                     coverloadingrate37 = or.coverloadingrate37,
                     coverloadingrate38 = or.coverloadingrate38,
                     coverloadingrate39 = or.coverloadingrate39,
                     coverloadingrate41 = or.coverloadingrate41,
                     coverloadingrate42 = or.coverloadingrate42,
                     coverloadingrate43 = or.coverloadingrate43,
                     coverloadingrate44 = or.coverloadingrate44,
                     coverloadingrate45 = or.coverloadingrate46,
                     coverloadingrate47 = or.coverloadingrate47,
                     coverloadingrate48 = or.coverloadingrate48,
                     coverloadingrate49 = or.coverloadingrate49,
                     coverloadingrate51 = or.coverloadingrate51,
                     coverloadingrate52 = or.coverloadingrate52,
                     coverloadingrate53 = or.coverloadingrate53,
                     coverloadingrate54 = or.coverloadingrate54,
                     coverloadingrate55 = or.coverloadingrate55,
                     coverloadingrate56 = or.coverloadingrate56,
                     coverloadingrate57 = or.coverloadingrate57,
                     coverloadingrate58 = or.coverloadingrate58,
                     coverloadingrate59 = or.coverloadingrate59,
                     coverloadingrate61 = or.coverloadingrate61,
                     coverloadingrate62 = or.coverloadingrate62,
                     coverloadingrate63 = or.coverloadingrate63,
                     coverloadingrate64 = or.coverloadingrate64,
                     coverloadingrate65 = or.coverloadingrate65,
                     coverloadingrate66 = or.coverloadingrate66,
                     coverloadingrate67 = or.coverloadingrate67,
                     coverloadingrate68 = or.coverloadingrate68,
                     coverloadingrate69 = or.coverloadingrate69,
                     coverloadingrate71 = or.coverloadingrate71,
                     coverloadingrate72 = or.coverloadingrate72,
                     coverloadingrate73 = or.coverloadingrate73,
                     coverloadingrate74 = or.coverloadingrate74,
                     coverloadingrate75 = or.coverloadingrate75,
                     coverloadingrate76 = or.coverloadingrate76,
                     coverloadingrate77 = or.coverloadingrate77,
                     coverloadingrate78 = or.coverloadingrate78,
                     coverloadingrate79 = or.coverloadingrate79,
                     coverloadingrate81 = or.coverloadingrate81,
                     coverloadingrate82 = or.coverloadingrate82,
                     coverloadingrate83 = or.coverloadingrate83,
                     coverloadingrate84 = or.coverloadingrate84,
                     coverloadingrate85 = or.coverloadingrate85,
                     coverloadingrate86 = or.coverloadingrate86,
                     coverloadingrate87 = or.coverloadingrate87,
                     coverloadingrate88 = or.coverloadingrate88,
                     coverloadingrate89 = or.coverloadingrate89,
                     coverloadingrate91 = or.coverloadingrate91,
                     coverloadingrate92 = or.coverloadingrate92,
                     coverloadingrate93 = or.coverloadingrate93,
                     coverloadingrate94 = or.coverloadingrate94,
                     coverloadingrate95 = or.coverloadingrate95,
                     coverloadingrate96 = or.coverloadingrate96,
                     coverloadingrate97 = or.coverloadingrate97,
                     coverloadingrate98 = or.coverloadingrate98,
                     coverloadingrate99 = or.coverloadingrate99,
                     coverloadingrate101 = or.coverloadingrate101,
                     coverloadingrate102 = or.coverloadingrate102,
                     coverloadingrate103 = or.coverloadingrate103,
                     coverloadingrate104 = or.coverloadingrate104,
                     coverloadingrate105 = or.coverloadingrate105,
                     coverloadingrate106 = or.coverloadingrate106,
                     coverloadingrate107 = or.coverloadingrate107,
                     coverloadingrate108 = or.coverloadingrate108,
                     coverloadingrate109 = or.coverloadingrate109,
                     coverloadingrate210 = or.coverloadingrate210,
                     coverloadingrate310 = or.coverloadingrate310,
                     coverloadingrate410 = or.coverloadingrate410,
                     coverloadingrate510 = or.coverloadingrate510,
                     coverloadingrate610 = or.coverloadingrate610,
                     coverloadingrate810 = or.coverloadingrate810,
                     coverloadingrate910 = or.coverloadingrate910,
                     coverloadingrate1010 = or.coverloadingrate1010,

                     insured_loadingper1 = oridst.loading_per_insured1,
                     insured_loadingper2 = oridst.loading_per_insured2,
                     insured_loadingper3 = oridst.loading_per_insured3,
                     insured_loadingper4 = oridst.loading_per_insured4,
                     insured_loadingper5 = oridst.loading_per_insured5,
                     insured_loadingper6 = oridst.loading_per_insured6,

                 }
               ).Skip((pageNumber - 1) * pageSize) // Skip the appropriate number of records
            .Take(pageSize) // Take only the specified page size
        .ToListAsync();
            return new List<OptimaRestoreRNE>(orRNEData);
        }

        static Dictionary<string, string> DataRowToDictionary(DataRow row)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (DataColumn column in row.Table.Columns)
            {
                dictionary[column.ColumnName] = row[column].ToString();
            }
            return dictionary;
        }

        public static Dictionary<string, object> ExtractData(RenewalGovernancePremiumValidation.Models.Domain.OptimaRestoreRNE optimaRS)
        {
            var data = new Dictionary<string, object>();
            var properties = typeof(RenewalGovernancePremiumValidation.Models.Domain.OptimaRestoreRNE).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                for (int i = 11; i <= 1010; i += 1)  // Adjust the range if necessary
                {
                    if (property.Name.StartsWith($"covername{i}") ||
                         property.Name.StartsWith($"coversi{i}") ||
                         property.Name.StartsWith($"coverprem{i}") ||
                         property.Name.StartsWith($"coverloadingrate{i}")
                         )
                    {
                        data[property.Name] = property.GetValue(optimaRS);
                    }
                }
            }
            return data;
        }

        public async Task OptimaRestoreValidation(IEnumerable<OptimaRestoreRNE> orRNEData)
        {
            OptimaRestoreRNE or = null;
            var finalPremiums = new List<decimal?>();
            var idstData = await (
        from table in dbContext.idst_renewal_data_rgs
        select new
        {
            table.certificate_no,
            table.loading_per_insured1,
            table.loading_per_insured2,
            table.loading_per_insured3,
            table.loading_per_insured4,
            table.loading_per_insured5,
            table.loading_per_insured6,
        }
    ).ToListAsync();
            foreach (var row in orRNEData)
            {
                var policyNo16 = row.policy_number;
                var iDSTData = idstData.FirstOrDefault(x => x.certificate_no == policyNo16);
                DataTable table = new DataTable();
                for (int i = 11; i <= 66; i += 1)  // Adjust the range if necessary
                {
                    table.Columns.Add($"covername{i}", typeof(string));
                    table.Columns.Add($"coversi{i}", typeof(string));
                    table.Columns.Add($"coverprem{i}", typeof(string));
                    table.Columns.Add($"coverloadingrate{i}", typeof(string));
                }
                var data = ExtractData(row);
                DataRow newRow = table.NewRow();
                foreach (var column in data)
                {
                    if (table.Columns.Contains(column.Key))
                    {
                        newRow[column.Key] = column.Value;
                    }
                }
                table.Rows.Add(newRow);
                string searchRider1 = "Hospital Daily Cash Rider";
                string searchRider2 = "Protector Rider";
                string searchRider3 = "Individual Personal Accident Rider";
                string searchRider4 = "Critical Advantage Rider";
                string searchRider5 = "Unlimited Restore";

                DataTable siRiderOneDataTable = GetRiderSI(table, searchRider1);
                DataTable siRiderTwoDataTable = GetRiderSI(table, searchRider2);
                DataTable siRiderThreeDataTable = GetRiderSI(table, searchRider3);
                DataTable siRiderFourDataTable = GetRiderSI(table, searchRider4);
                DataTable siRiderFiveDataTable = GetRiderSI(table, searchRider5);
                string? policyLdDesc1 = row.pollddesc1;
                string? policyLdDesc2 = row.pollddesc2;
                string? policyLdDesc3 = row.pollddesc3;
                string? policyLdDesc4 = row.pollddesc4;
                string? policyLdDesc5 = row.pollddesc5;

                List<string?> policyLdDescValues = new List<string?>();
                policyLdDescValues.Add(policyLdDesc1);
                policyLdDescValues.Add(policyLdDesc2);
                policyLdDescValues.Add(policyLdDesc3);
                policyLdDescValues.Add(policyLdDesc4);
                policyLdDescValues.Add(policyLdDesc5);

                string? upsellType1 = row.upselltype1;
                string? upsellType2 = row.upselltype2;
                string? upsellType3 = row.upselltype3;
                string? upsellType4 = row.upselltype4;
                string? upsellType5 = row.upselltype5;

                string? upsellValue1 = row.upsellvalue1;
                string? upsellValue2 = row.upsellvalue2;
                string? upsellValue3 = row.upsellvalue3;
                string? upsellValue4 = row.upsellvalue4;
                string? upsellValue5 = row.upsellvalue5;

                List<string?> upsellvalueValues = new List<string?>()
                {
                    upsellValue1,
                    upsellValue2,
                    upsellValue3,
                    upsellValue4,
                    upsellValue5
                };

                string searchLoyaltyDescText = "Loyalty Discount";
                bool containsLoyaltyDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchLoyaltyDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultLoyaltyDescText = containsLoyaltyDescText ? 1 : 0;
                decimal? loyaltyDiscountValue = containsLoyaltyDescText ? 2.5m : 0.0m;
                string searchEmployeeDescText = "Employee Discount";
                bool containsEmployeeDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchEmployeeDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultSearchEmployeeDescText = containsEmployeeDescText ? 1 : 0;
                decimal? employeeDiscountValue = containsEmployeeDescText ? 5.0m : 0.0m;
                string searchOnlineDescText = "Online Discount";
                bool containsOnlineDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchOnlineDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultSearchOnlineDescText = containsOnlineDescText ? 1 : 0;
                decimal? onlineDiscountValue = containsOnlineDescText ? 5.0m : 0.0m;
                var policyType = row.policy_type;
                var numberOfMembers = 1;
                string searcFamilyDescText = "Family Discount";
                bool containsFamilyDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searcFamilyDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultSearchFamilyDescText = containsFamilyDescText ? 1 : 0;
                decimal? familyDiscountValue = GetFamilyDiscount(policyType, numberOfMembers);
                var policyperiod = row.policy_period;
                decimal longTermDiscount = GetLongTermDiscount(policyperiod);
                var columnName = GetColumnNameForPolicyPeriod(policyperiod);
                if (columnName == null)
                {
                    throw new ArgumentException($"Invalid policy period: {policyperiod}");
                }
                decimal? siOne = row.sum_insured1;//c26
                decimal? siTwo = row.sum_insured2;//c27
                decimal? siThree = row.sum_insured3;//c28
                decimal? siFour = row.sum_insured4;//c29
                decimal? siFive = row.sum_insured5;//c30
                decimal? siSix = row.sum_insured6;//c31
                decimal? totalsuminsured = (siOne ?? 0) + (siTwo ?? 0) + (siThree ?? 0) + (siFour ?? 0) + (siFive ?? 0) + (siSix ?? 0);

                decimal? cbOne = Convert.ToDecimal(row.insured_cb1);
                decimal? cbTwo = Convert.ToDecimal(row.insured_cb2);
                decimal? cbThree = Convert.ToDecimal(row.insured_cb3);
                decimal? cbFour = Convert.ToDecimal(row.insured_cb4);
                decimal? cbFive = Convert.ToDecimal(row.insured_cb5);
                decimal? cbSix = Convert.ToDecimal(row.insured_cb6);
                decimal? cumulativebonus = (cbOne ?? 0) + (cbTwo ?? 0) + (cbThree ?? 0) + (cbFour ?? 0) + (cbFive ?? 0) + (cbSix ?? 0);

                var insured_name1 = row.txt_insuredname1;
                var insured_name2 = row.txt_insuredname2;
                var insured_name3 = row.txt_insuredname3;
                var insured_name4 = row.txt_insuredname4;
                var insured_name5 = row.txt_insuredname5;
                var insured_name6 = row.txt_insuredname6;

                var insuredAgeOne = Convert.ToInt32(row.txt_insured_age1);//c18
                var insuredAgeTwo = Convert.ToInt32(row.txt_insured_age2);//c19            
                var insuredAgeThree = Convert.ToInt32(row.txt_insured_age3);//c20
                var insuredAgeFour = Convert.ToInt32(row.txt_insured_age4);//c21
                var insuredAgeFive = Convert.ToInt32(row.txt_insured_age5);//c22
                var insuredAgeSix = Convert.ToInt32(row.txt_insured_age6);//c23

                var zone = row.tier_type;//c11
                List<int?> ageValues = new List<int?>()
                {
                    insuredAgeOne,
                    insuredAgeTwo,
                    insuredAgeThree,
                    insuredAgeFour,
                    insuredAgeFive,
                    insuredAgeSix
                };
                var noOfMembers = ageValues.Count(age => age > 0);
                int? eldestMember = ageValues.Max();
                int? count = noOfMembers;
                var numberOfMemberss = noOfMembers;

                decimal? basicLoadingRateOne = iDSTData.loading_per_insured1 ?? 0;//c38
                decimal? basicLoadingRateTwo = iDSTData.loading_per_insured2 ?? 0;//c39
                decimal? basicLoadingRateThree = iDSTData.loading_per_insured3 ?? 0;//c40
                decimal? basicLoadingRateFour = iDSTData.loading_per_insured4 ?? 0;//c41
                decimal? basicLoadingRateFive = iDSTData.loading_per_insured5 ?? 0;//c42
                decimal? basicLoadingRateSix = iDSTData.loading_per_insured6 ?? 0;//c43
                var sql = $@"
                SELECT {columnName}
                FROM baserate_optimarestore
                WHERE si = @p0 AND age = @p1 AND zone = @p2";
                var basePremium1 = await dbContext.baserate_optimarestore
                .FromSqlRaw(sql, siOne, insuredAgeOne, zone)
                    .Select(r => EF.Property<decimal?>(r, columnName))
                    .FirstOrDefaultAsync();
                var basePremium2 = await dbContext.baserate_optimarestore
                .FromSqlRaw(sql, siTwo, insuredAgeTwo, zone)
                    .Select(r => EF.Property<decimal?>(r, columnName))
                    .FirstOrDefaultAsync();
                var basePremium3 = await dbContext.baserate_optimarestore
                    .FromSqlRaw(sql, siThree, insuredAgeThree, zone)
                    .Select(r => EF.Property<decimal?>(r, columnName))
                    .FirstOrDefaultAsync();
                var basePremium4 = await dbContext.baserate_optimarestore
                   .FromSqlRaw(sql, siFour, insuredAgeFour, zone)
                   .Select(r => EF.Property<decimal?>(r, columnName))
                   .FirstOrDefaultAsync();
                var basePremium5 = await dbContext.baserate_optimarestore
                    .FromSqlRaw(sql, siFive, insuredAgeFive, zone)
                    .Select(r => EF.Property<decimal?>(r, columnName))
                    .FirstOrDefaultAsync();
                var basePremium6 = await dbContext.baserate_optimarestore
                   .FromSqlRaw(sql, siSix, insuredAgeSix, zone)
                   .Select(r => EF.Property<decimal?>(r, columnName))
                   .FirstOrDefaultAsync();
                var basePremiumvalues = new List<decimal?> { basePremium1, basePremium2, basePremium3, basePremium4, basePremium5, basePremium6 };
                string condition = policyType;
                decimal? basePremium = GetBasePremium(condition, basePremiumvalues);//c55

                decimal? basePremLoadingInsured1 = GetBasePremLoadingInsured1(basePremium1, basicLoadingRateOne);
                decimal? basePremLoadingInsured2 = GetBasePremLoadingInsured2(basePremium2, basicLoadingRateTwo);
                decimal? basePremLoadingInsured3 = GetBasePremLoadingInsured3(basePremium3, basicLoadingRateThree);
                decimal? basePremLoadingInsured4 = GetBasePremLoadingInsured4(basePremium4, basicLoadingRateFour);
                decimal? basePremLoadingInsured5 = GetBasePremLoadingInsured5(basePremium5, basicLoadingRateFive);
                decimal? basePremLoadingInsured6 = GetBasePremLoadingInsured6(basePremium6, basicLoadingRateSix);
                var loadingPremvalues = new List<decimal?> { basePremLoadingInsured1 + basePremLoadingInsured2 + basePremLoadingInsured3 + basePremLoadingInsured4 + basePremLoadingInsured5 + basePremLoadingInsured6 }; //c62
                decimal? basePremLoading = loadingPremvalues.Sum();
                decimal? orbaseLoadingPremium = basePremium + basePremLoading;//c55+c62
                decimal? orloyaltyDiscount = orbaseLoadingPremium * (loyaltyDiscountValue / 100);//c63*c44
                decimal? oremployeeDiscount = orbaseLoadingPremium * (employeeDiscountValue / 100);//c63*c45
                decimal? oronlineDiscount = orbaseLoadingPremium * (onlineDiscountValue / 100);//c63*c46
                decimal? orfamilyDiscount = orbaseLoadingPremium * (familyDiscountValue / 100);//c63*c47
                decimal? orlongTermDiscount = (orbaseLoadingPremium - (orloyaltyDiscount + oremployeeDiscount + oronlineDiscount + orfamilyDiscount)) * longTermDiscount;
                decimal? orbasePremium = orbaseLoadingPremium - (orloyaltyDiscount + oremployeeDiscount + oronlineDiscount + orfamilyDiscount + orlongTermDiscount);

                decimal? unlimitedRestoreValue = 0;
                if (siRiderFiveDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderFiveDataTable.Rows)
                        unlimitedRestoreValue = 1;
                }
                decimal? unlimitedRestore = unlimitedRestoreValue > 0 ? orbasePremium * 0.005m : 0;
                decimal? finalBasePremium = orbasePremium + unlimitedRestore;

                string Opt = "N"; //need to calculate 
                decimal? hdcSI = 0;//rider_si_11 
                if (siRiderOneDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderOneDataTable.Rows)
                    {
                        Opt = "Y";
                        object siValueObject = itemRow["SIValue"];
                        hdcSI = Convert.ToDecimal(siValueObject);
                    }
                }
                string familysize = row.txt_family;
                var sqlhdc = $@"
                SELECT {columnName}
                FROM hdcrates_optimarestore
                WHERE age = @p0  AND suminsured = @p1 AND plan = @p2";
                var hdcOpt = await dbContext.hdcrates_optimarestore
                .FromSqlRaw(sqlhdc, insuredAgeOne, siOne, familysize)
                    .Select(r => EF.Property<decimal?>(r, columnName))
                    .FirstOrDefaultAsync();

                decimal? hdcRiderPremium = await GetHDCRiderPremium(policyperiod, eldestMember, hdcSI, familysize);
                decimal? hdcFamilyDiscount = hdcRiderPremium * familyDiscountValue;
                decimal? hdcLongTermDiscount = (hdcRiderPremium - hdcFamilyDiscount) * (longTermDiscount);
                decimal? hdcFinalPremium = hdcRiderPremium - hdcFamilyDiscount - hdcLongTermDiscount;
                hdcFinalPremium = hdcFinalPremium ?? 0;
                string propt = "N";
                if (siRiderTwoDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderTwoDataTable.Rows)
                    {
                        propt = "Y";
                        object prsiValueObject = itemRow["SIValue"];
                    }
                }
                decimal? prInsured1 = GetProtectorRiderInsured1(propt, siOne, basePremium1);
                decimal? prInsured2 = GetProtectorRiderInsured2(propt, siTwo, basePremium2);
                decimal? prInsured3 = GetProtectorRiderInsured3(propt, siThree, basePremium3);
                decimal? prInsured4 = GetProtectorRiderInsured4(propt, siFour, basePremium4);
                decimal? prInsured5 = GetProtectorRiderInsured5(propt, siFive, basePremium5);
                decimal? prInsured6 = GetProtectorRiderInsured6(propt, siSix, basePremium6);
                decimal? prProtectorRiderPremium = GetProtectorRiderPremium(policyType, prInsured1, prInsured2, prInsured3, prInsured4, prInsured5, prInsured6);

                decimal? prLoadingInsured1 = GetProtectorRiderLoadingInsured1(prInsured1, basicLoadingRateOne / 100);
                decimal? prLoadingInsured2 = GetProtectorRiderLoadingInsured2(prInsured2, basicLoadingRateTwo / 100);
                decimal? prLoadingInsured3 = GetProtectorRiderLoadingInsured3(prInsured3, basicLoadingRateThree / 100);
                decimal? prLoadingInsured4 = GetProtectorRiderLoadingInsured4(prInsured4, basicLoadingRateFour / 100);
                decimal? prLoadingInsured5 = GetProtectorRiderLoadingInsured5(prInsured5, basicLoadingRateFive / 100);
                decimal? prLoadingInsured6 = GetProtectorRiderLoadingInsured6(prInsured6, basicLoadingRateSix / 100);
                decimal? ProtectorRiderLoadingPremium = prLoadingInsured1 + prLoadingInsured2 + prLoadingInsured3 + prLoadingInsured4 + prLoadingInsured5 + prLoadingInsured6;
                decimal? prBaseLoadingPremium = prProtectorRiderPremium + ProtectorRiderLoadingPremium;
                decimal? prFamilyDiscount = prProtectorRiderPremium * familyDiscountValue;
                decimal? prLongTermDiscount = (prBaseLoadingPremium - prFamilyDiscount) * (longTermDiscount);
                decimal? prpremiumProtectorRiderPremium = prBaseLoadingPremium - prFamilyDiscount - prLongTermDiscount;
                string individualpersonalARopt = "N"; //need to calculate and add the logic hardcoded as of now
                if (siRiderThreeDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderThreeDataTable.Rows)
                    {
                        individualpersonalARopt = "Y";
                    }
                }
                decimal? individualpersonalARSI = GetIndividualPersonalARSI(individualpersonalARopt, siOne);
                var policystartdate = row.policy_start_date;
                var policyenddate = row.policy_expiry_date;
                decimal? individualpersonalARAmt = GetIndividualPersonalARAmt(individualpersonalARopt, individualpersonalARSI, policystartdate, policyenddate);
                decimal? individualpersonalARLongTermDiscount = individualpersonalARAmt * longTermDiscount;
                decimal? individualPersonalAccidentRiderPremium = individualpersonalARAmt - individualpersonalARLongTermDiscount;

                string criticalAdvantageRideropt = "N";
                decimal? caSI = row.coversi11;
                decimal? ca_si_1 = 0;
                decimal? ca_si_2 = 0;
                decimal? ca_si_3 = 0;
                decimal? ca_si_4 = 0;
                decimal? ca_si_5 = 0;
                decimal? ca_si_6 = 0;
                int index = 1;
                if (siRiderFourDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderFourDataTable.Rows)
                    {
                        criticalAdvantageRideropt = "Y";
                        decimal siValue = (decimal)itemRow["SIValue"];
                        caSI = Convert.ToDecimal(siValue);
                        switch (index)
                        {
                            case 1:
                                ca_si_1 += siValue;
                                break;
                            case 2:
                                ca_si_2 += siValue;
                                break;
                            case 3:
                                ca_si_3 += siValue;
                                break;
                            case 4:
                                ca_si_4 += siValue;
                                break;
                            case 5:
                                ca_si_5 += siValue;
                                break;
                            case 6:
                                ca_si_6 += siValue;
                                break;
                        }
                        index++;
                    }
                }
                var sqlca = $@"
                SELECT {columnName}
                FROM carates
                WHERE age = @p0  AND si = @p1";
                decimal criticalAdvantageRiderInsured1 = await dbContext.carates
                .FromSqlRaw(sqlca, insuredAgeOne, ca_si_1)
                    .Select(r => EF.Property<decimal?>(r, columnName))
                    .FirstOrDefaultAsync() ?? 0;
                decimal criticalAdvantageRiderInsured2 = await dbContext.carates
               .FromSqlRaw(sqlca, insuredAgeTwo, ca_si_2)
                   .Select(r => EF.Property<decimal?>(r, columnName))
                   .FirstOrDefaultAsync() ?? 0;
                decimal criticalAdvantageRiderInsured3 = await dbContext.carates
               .FromSqlRaw(sqlca, insuredAgeThree, ca_si_3)
                   .Select(r => EF.Property<decimal?>(r, columnName))
                   .FirstOrDefaultAsync() ?? 0;
                decimal criticalAdvantageRiderInsured4 = await dbContext.carates
               .FromSqlRaw(sqlca, insuredAgeFour, ca_si_4)
                 .Select(r => EF.Property<decimal?>(r, columnName))
                 .FirstOrDefaultAsync() ?? 0;
                decimal criticalAdvantageRiderInsured5 = await dbContext.carates
               .FromSqlRaw(sqlca, insuredAgeFive, ca_si_5)
                   .Select(r => EF.Property<decimal?>(r, columnName))
                   .FirstOrDefaultAsync() ?? 0;
                decimal criticalAdvantageRiderInsured6 = await dbContext.carates
              .FromSqlRaw(sqlca, insuredAgeSix, ca_si_6)
                  .Select(r => EF.Property<decimal?>(r, columnName))
                  .FirstOrDefaultAsync() ?? 0;
                decimal? criticalAdvantageRiderBasePremium = criticalAdvantageRiderInsured1 + criticalAdvantageRiderInsured2 + criticalAdvantageRiderInsured3 + criticalAdvantageRiderInsured4 + criticalAdvantageRiderInsured5 + criticalAdvantageRiderInsured6;
                decimal? criticalAdvantageRiderLoadingInsured1 = GetCARRiderLoadingInsured1(criticalAdvantageRiderInsured1, basicLoadingRateOne / 100);
                decimal? criticalAdvantageRiderLoadingInsured2 = GetCARRiderLoadingInsured2(criticalAdvantageRiderInsured2, basicLoadingRateTwo / 100);
                decimal? criticalAdvantageRiderLoadingInsured3 = GetCARRiderLoadingInsured3(criticalAdvantageRiderInsured3, basicLoadingRateThree / 100);
                decimal? criticalAdvantageRiderLoadingInsured4 = GetCARRiderLoadingInsured4(criticalAdvantageRiderInsured4, basicLoadingRateFour / 100);
                decimal? criticalAdvantageRiderLoadingInsured5 = GetCARRiderLoadingInsured5(criticalAdvantageRiderInsured5, basicLoadingRateFive / 100);
                decimal? criticalAdvantageRiderLoadingInsured6 = GetCARRiderLoadingInsured6(criticalAdvantageRiderInsured6, basicLoadingRateSix / 100);
                decimal? criticalAdvantageRiderLoading = GetCriticalAdvantageRiderLoading(policyType, criticalAdvantageRiderLoadingInsured1, criticalAdvantageRiderLoadingInsured2, criticalAdvantageRiderLoadingInsured3, criticalAdvantageRiderLoadingInsured4, criticalAdvantageRiderLoadingInsured5, criticalAdvantageRiderLoadingInsured6);
                decimal? criticalAdvRiderBaseLoadingPremium = criticalAdvantageRiderBasePremium + criticalAdvantageRiderLoading;
                decimal? criticalAdvRiderPremiumFamilyDiscount = criticalAdvRiderBaseLoadingPremium * familyDiscountValue;
                decimal? criticalAdvRiderPremiumLongTermDiscount = (criticalAdvRiderBaseLoadingPremium - criticalAdvRiderPremiumFamilyDiscount) * (longTermDiscount);
                decimal? criticalAdvRiderPremium = criticalAdvRiderBaseLoadingPremium - criticalAdvRiderPremiumFamilyDiscount - criticalAdvRiderPremiumLongTermDiscount;
                decimal? netPremium = (finalBasePremium + hdcFinalPremium + prpremiumProtectorRiderPremium + individualPersonalAccidentRiderPremium + criticalAdvRiderPremium);
                decimal? gST = (netPremium) * (0.18m);
                decimal? finalPremium = netPremium + gST;
                var totalpremium = row.num_tot_premium ?? 0;
                decimal? Crosscheck1 = totalpremium - finalPremium;
                var crosscheck1 = Crosscheck1;

                or = new OptimaRestoreRNE
                {
                    prod_code = row.prod_code,
                    prod_name = row.prod_name,
                    policy_number = row.policy_number,
                    reference_num = row.reference_num,
                    batchid = row.batchid,
                    policy_start_date = row.policy_start_date,
                    policy_expiry_date = row.policy_expiry_date,

                    txt_family = row.txt_family,
                    policyplan = row.policyplan,
                    policy_type = row.policy_type,
                    policy_period = row.policy_period,
                    no_of_members = noOfMembers,
                    eldest_member = eldestMember,
                    insuredname_1 = insured_name1,
                    insuredname_2 = insured_name2,
                    insuredname_3 = insured_name3,
                    insuredname_4 = insured_name4,
                    insuredname_5 = insured_name5,
                    insuredname_6 = insured_name6,
                    sum_insured1 = siOne,
                    sum_insured2 = siTwo,
                    sum_insured3 = siThree,
                    sum_insured4 = siFour,
                    sum_insured5 = siFive,
                    sum_insured6 = siSix,
                    loading_per_insured_1 = basicLoadingRateOne,
                    loading_per_insured_2 = basicLoadingRateTwo,
                    loading_per_insured_3 = basicLoadingRateThree,
                    loading_per_insured_4 = basicLoadingRateFour,
                    loading_per_insured_5 = basicLoadingRateFive,
                    loading_per_insured_6 = basicLoadingRateSix,
                    base_Premium1 = basePremium1,
                    base_Premium2 = basePremium2,
                    base_Premium3 = basePremium3,
                    base_Premium4 = basePremium4,
                    base_Premium5 = basePremium5,
                    base_Premium6 = basePremium6,
                    base_Premium = basePremium,

                    orbase_Premium = orbasePremium.HasValue ? Math.Round(orbasePremium.Value, 2) : (decimal?)null,
                    net_premium = netPremium.HasValue ? Math.Round(netPremium.Value, 2) : (decimal?)null,
                    final_Premium = finalPremium.HasValue ? Math.Round(finalPremium.Value, 2) : (decimal?)null,
                    gst = gST.HasValue ? Math.Round(gST.Value, 2) : (decimal?)null,
                    cross_Check = crosscheck1.HasValue ? Math.Round(crosscheck1.Value, 2) : (decimal?)null,
                    or_total_Premium = row.num_tot_premium.HasValue ? Math.Round(row.num_tot_premium.Value, 2) : (decimal?)null,
                    or_netpremium = row.num_net_premium.HasValue ? Math.Round(row.num_net_premium.Value, 2) : (decimal?)null,
                    or_GST = row.num_service_tax.HasValue ? Math.Round(row.num_service_tax.Value, 2) : (decimal?)null
                };

                //Insert to db.

                var record = dbContext.idst_renewal_data_rgs.FirstOrDefault(item => item.certificate_no == policyNo16);
                if (record != null)
                {
                    record.verified_prem = netPremium;
                    record.verified_gst = gST;
                    record.verified_total_prem = finalPremium;
                }
                dbContext.idst_renewal_data_rgs.UpdateRange();
                try
                {
                    // Perform your update operation
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Handle the concurrency issue (e.g., reload data, prompt user for action)
                    // Log or process the exception as needed
                }
            }
            //  return new List<OptimaRestoreRNE> { or };
        }

        public async Task<IEnumerable<OptimaRestoreRNE>> OptimaRestoreValidationupsell(IEnumerable<OptimaRestoreRNE> orRNEData)
        {
            OptimaRestoreRNE or = null;
            var finalPremiums = new List<decimal?>();
            var idstData = await (
      from table in dbContext.idst_renewal_data_rgs
      select new
      {
          table.certificate_no,
          table.loading_per_insured1,
          table.loading_per_insured2,
          table.loading_per_insured3,
          table.loading_per_insured4,
          table.loading_per_insured5,
          table.loading_per_insured6,
      }
  ).ToListAsync();
            foreach (var row in orRNEData)
            {
                var policyNo16 = row.policy_number;
                var iDSTData = idstData.FirstOrDefault(x => x.certificate_no == policyNo16);
                DataTable table = new DataTable();
                for (int i = 11; i <= 66; i += 1)  // Adjust the range if necessary
                {
                    table.Columns.Add($"covername{i}", typeof(string));
                    table.Columns.Add($"coversi{i}", typeof(string));
                    table.Columns.Add($"coverprem{i}", typeof(string));
                    table.Columns.Add($"coverloadingrate{i}", typeof(string));
                }
                var data = ExtractData(row);
                DataRow newRow = table.NewRow();
                foreach (var column in data)
                {
                    if (table.Columns.Contains(column.Key))
                    {
                        newRow[column.Key] = column.Value;
                    }
                }
                table.Rows.Add(newRow);
                string searchRider1 = "Hospital Daily Cash Rider";
                string searchRider2 = "Protector Rider";
                string searchRider3 = "Individual Personal Accident Rider";
                string searchRider4 = "Critical Advantage Rider";
                string searchRider5 = "Unlimited Restore";

                DataTable siRiderOneDataTable = GetRiderSI(table, searchRider1);
                DataTable siRiderTwoDataTable = GetRiderSI(table, searchRider2);
                DataTable siRiderThreeDataTable = GetRiderSI(table, searchRider3);
                DataTable siRiderFourDataTable = GetRiderSI(table, searchRider4);
                DataTable siRiderFiveDataTable = GetRiderSI(table, searchRider5);
                var insuredAgeOne = Convert.ToInt32(row.txt_insured_age1);//c18
                var insuredAgeTwo = Convert.ToInt32(row.txt_insured_age2);//c19            
                var insuredAgeThree = Convert.ToInt32(row.txt_insured_age3);//c20
                var insuredAgeFour = Convert.ToInt32(row.txt_insured_age4);//c21
                var insuredAgeFive = Convert.ToInt32(row.txt_insured_age5);//c22
                var insuredAgeSix = Convert.ToInt32(row.txt_insured_age6);//c23
                List<int?> ageValues = new List<int?>()
                {
                    insuredAgeOne,
                    insuredAgeTwo,
                    insuredAgeThree,
                    insuredAgeFour,
                    insuredAgeFive,
                    insuredAgeSix
                };
                var noOfMembers = ageValues.Count(age => age > 0);
                int? eldestMember = ageValues.Max();
                int? count = noOfMembers;
                var numberOfMemberss = noOfMembers;
                string? policyLdDesc1 = row.pollddesc1;
                string? policyLdDesc2 = row.pollddesc2;
                string? policyLdDesc3 = row.pollddesc3;
                string? policyLdDesc4 = row.pollddesc4;
                string? policyLdDesc5 = row.pollddesc5;
                List<string?> policyLdDescValues = new List<string?>();
                policyLdDescValues.Add(policyLdDesc1);
                policyLdDescValues.Add(policyLdDesc2);
                policyLdDescValues.Add(policyLdDesc3);
                policyLdDescValues.Add(policyLdDesc4);
                policyLdDescValues.Add(policyLdDesc5);
                string? upsellType1 = row.upselltype1;
                string? upsellType2 = row.upselltype2;
                string? upsellType3 = row.upselltype3;
                string? upsellType4 = row.upselltype4;
                string? upsellType5 = row.upselltype5;
                List<string?> upselltypeValues = new List<string?>()
                        {
                            upsellType1,
                            upsellType2,
                            upsellType3,
                            upsellType4,
                            upsellType5
                        };
                string? upsellValue1 = row.upsellvalue1;
                string? upsellValue2 = row.upsellvalue2;
                string? upsellValue3 = row.upsellvalue3;
                string? upsellValue4 = row.upsellvalue4;
                string? upsellValue5 = row.upsellvalue5;

                List<string?> upsellvalueValues = new List<string?>()
                        {
                            upsellValue1,
                            upsellValue2,
                            upsellValue3,
                            upsellValue4,
                            upsellValue5
                        };

                decimal? siOne = row.sum_insured1; ;//c26
                decimal? siTwo = row.sum_insured2;//c27
                decimal? siThree = row.sum_insured3;//c28
                decimal? siFour = row.sum_insured4;//c29
                decimal? siFive = row.sum_insured5;//c30
                decimal? siSix = row.sum_insured6;//c31
                decimal? totalsuminsured = (siOne ?? 0) + (siTwo ?? 0) + (siThree ?? 0) + (siFour ?? 0) + (siFive ?? 0) + (siSix ?? 0);

                decimal? cbOne = Convert.ToDecimal(row.insured_cb1);
                decimal? cbTwo = Convert.ToDecimal(row.insured_cb2);
                decimal? cbThree = Convert.ToDecimal(row.insured_cb3);
                decimal? cbFour = Convert.ToDecimal(row.insured_cb4);
                decimal? cbFive = Convert.ToDecimal(row.insured_cb5);
                decimal? cbSix = Convert.ToDecimal(row.insured_cb6);
                decimal? cumulativebonus = (cbOne ?? 0) + (cbTwo ?? 0) + (cbThree ?? 0) + (cbFour ?? 0) + (cbFive ?? 0) + (cbSix ?? 0);

                var insured_name1 = row.txt_insuredname1;
                var insured_name2 = row.txt_insuredname2;
                var insured_name3 = row.txt_insuredname3;
                var insured_name4 = row.txt_insuredname4;
                var insured_name5 = row.txt_insuredname5;
                var insured_name6 = row.txt_insuredname6;

                string searchUpsellType1 = "SI_UPSELL";
                string searchUpsellType2 = "UPSELLBASESI_1";
                bool containsUpsellType = upselltypeValues.Any(upsell => upsell != null &&
                    (upsell.Contains(searchUpsellType1, StringComparison.OrdinalIgnoreCase) ||
                     upsell.Contains(searchUpsellType2, StringComparison.OrdinalIgnoreCase)));
                if (containsUpsellType)
                {
                    if (noOfMembers > 0)
                    {
                        if (upselltypeValues.Contains(searchUpsellType1, StringComparer.OrdinalIgnoreCase))
                        {
                            if (decimal.TryParse(upsellValue1, out decimal parsedValue1))
                            {
                                if (noOfMembers >= 1)
                                {
                                    siOne = parsedValue1;
                                }
                                if (noOfMembers >= 2)
                                {
                                    siTwo = parsedValue1;
                                }
                                if (noOfMembers >= 3)
                                {
                                    siThree = parsedValue1;
                                }
                                if (noOfMembers >= 4)
                                {
                                    siFour = parsedValue1;
                                }
                                if (noOfMembers >= 5)
                                {
                                    siFive = parsedValue1;
                                }
                                if (noOfMembers >= 6)
                                {
                                    siSix = parsedValue1;
                                }
                            }
                        }
                        else if (upselltypeValues.Contains(searchUpsellType2, StringComparer.OrdinalIgnoreCase))
                        {
                            if (decimal.TryParse(upsellValue2, out decimal parsedValue2))
                            {
                                if (noOfMembers >= 1)
                                {
                                    siOne = parsedValue2;
                                }
                                if (noOfMembers >= 2)
                                {
                                    siTwo = parsedValue2;
                                }
                                if (noOfMembers >= 3)
                                {
                                    siThree = parsedValue2;
                                }
                                if (noOfMembers >= 4)
                                {
                                    siFour = parsedValue2;
                                }
                                if (noOfMembers >= 5)
                                {
                                    siFive = parsedValue2;
                                }
                                if (noOfMembers >= 6)
                                {
                                    siSix = parsedValue2;
                                }
                            }
                        }
                        else
                        {
                            if (decimal.TryParse(upsellValue1, out decimal parsedValue1))
                            {
                                siOne = parsedValue1;
                            }
                            if (noOfMembers > 1 && decimal.TryParse(upsellValue2, out decimal parsedValue2))
                            {
                                siTwo = parsedValue2;
                            }
                            if (noOfMembers > 2 && decimal.TryParse(upsellValue3, out decimal parsedValue3))
                            {
                                siThree = parsedValue3;
                            }
                            if (noOfMembers > 3 && decimal.TryParse(upsellValue4, out decimal parsedValue4))
                            {
                                siFour = parsedValue4;
                            }
                            if (noOfMembers > 4 && decimal.TryParse(upsellValue5, out decimal parsedValue5))
                            {
                                siFive = parsedValue5;
                            }
                            if (noOfMembers > 5 && decimal.TryParse(upsellValue5, out decimal parsedValue6)) // Assuming same value for siSix
                            {
                                siSix = parsedValue6;
                            }
                        }
                    }
                }

                string searchLoyaltyDescText = "Loyalty Discount";
                bool containsLoyaltyDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchLoyaltyDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultLoyaltyDescText = containsLoyaltyDescText ? 1 : 0;
                decimal? loyaltyDiscountValue = containsLoyaltyDescText ? 2.5m : 0.0m;
                string searchEmployeeDescText = "Employee Discount";
                bool containsEmployeeDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchEmployeeDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultSearchEmployeeDescText = containsEmployeeDescText ? 1 : 0;
                decimal? employeeDiscountValue = containsEmployeeDescText ? 5.0m : 0.0m;
                string searchOnlineDescText = "Online Discount";
                bool containsOnlineDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchOnlineDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultSearchOnlineDescText = containsOnlineDescText ? 1 : 0;
                decimal? onlineDiscountValue = containsOnlineDescText ? 5.0m : 0.0m;
                var policyType = row.policy_type;
                var numberOfMembers = 1;
                string searcFamilyDescText = "Family Discount";
                bool containsFamilyDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searcFamilyDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultSearchFamilyDescText = containsFamilyDescText ? 1 : 0;
                decimal? familyDiscountValue = GetFamilyDiscount(policyType, numberOfMembers);
                var policyperiod = row.policy_period;
                decimal longTermDiscount = GetLongTermDiscount(policyperiod);
                var columnName = GetColumnNameForPolicyPeriod(policyperiod);
                if (columnName == null)
                {
                    throw new ArgumentException($"Invalid policy period: {policyperiod}");
                }
                var zone = row.tier_type;//c11
                decimal? basicLoadingRateOne = iDSTData.loading_per_insured1 ?? 0;//c38
                decimal? basicLoadingRateTwo = iDSTData.loading_per_insured2 ?? 0;//c39
                decimal? basicLoadingRateThree = iDSTData.loading_per_insured3 ?? 0;//c40
                decimal? basicLoadingRateFour = iDSTData.loading_per_insured4 ?? 0;//c41
                decimal? basicLoadingRateFive = iDSTData.loading_per_insured5 ?? 0;//c42
                decimal? basicLoadingRateSix = iDSTData.loading_per_insured6 ?? 0;//c43
                var sql = $@"
                        SELECT {columnName}
                        FROM baserate_optimarestore
                        WHERE si = @p0 AND age = @p1 AND zone = @p2";
                var basePremium1 = await dbContext.baserate_optimarestore
                .FromSqlRaw(sql, siOne, insuredAgeOne, zone)
                    .Select(r => EF.Property<decimal?>(r, columnName))
                    .FirstOrDefaultAsync();
                var basePremium2 = await dbContext.baserate_optimarestore
                .FromSqlRaw(sql, siTwo, insuredAgeTwo, zone)
                    .Select(r => EF.Property<decimal?>(r, columnName))
                    .FirstOrDefaultAsync();
                var basePremium3 = await dbContext.baserate_optimarestore
                    .FromSqlRaw(sql, siThree, insuredAgeThree, zone)
                    .Select(r => EF.Property<decimal?>(r, columnName))
                    .FirstOrDefaultAsync();
                var basePremium4 = await dbContext.baserate_optimarestore
                   .FromSqlRaw(sql, siFour, insuredAgeFour, zone)
                   .Select(r => EF.Property<decimal?>(r, columnName))
                   .FirstOrDefaultAsync();
                var basePremium5 = await dbContext.baserate_optimarestore
                    .FromSqlRaw(sql, siFive, insuredAgeFive, zone)
                    .Select(r => EF.Property<decimal?>(r, columnName))
                    .FirstOrDefaultAsync();
                var basePremium6 = await dbContext.baserate_optimarestore
                   .FromSqlRaw(sql, siSix, insuredAgeSix, zone)
                   .Select(r => EF.Property<decimal?>(r, columnName))
                   .FirstOrDefaultAsync();
                var basePremiumvalues = new List<decimal?> { basePremium1, basePremium2, basePremium3, basePremium4, basePremium5, basePremium6 };
                string condition = policyType;
                decimal? basePremium = GetBasePremium(condition, basePremiumvalues);//c55

                decimal? basePremLoadingInsured1 = GetBasePremLoadingInsured1(basePremium1, basicLoadingRateOne);
                decimal? basePremLoadingInsured2 = GetBasePremLoadingInsured2(basePremium2, basicLoadingRateTwo);
                decimal? basePremLoadingInsured3 = GetBasePremLoadingInsured3(basePremium3, basicLoadingRateThree);
                decimal? basePremLoadingInsured4 = GetBasePremLoadingInsured4(basePremium4, basicLoadingRateFour);
                decimal? basePremLoadingInsured5 = GetBasePremLoadingInsured5(basePremium5, basicLoadingRateFive);
                decimal? basePremLoadingInsured6 = GetBasePremLoadingInsured6(basePremium6, basicLoadingRateSix);
                var loadingPremvalues = new List<decimal?> { basePremLoadingInsured1 + basePremLoadingInsured2 + basePremLoadingInsured3 + basePremLoadingInsured4 + basePremLoadingInsured5 + basePremLoadingInsured6 }; //c62
                decimal? basePremLoading = loadingPremvalues.Sum();

                decimal? orbaseLoadingPremium = basePremium + basePremLoading;//c55+c62
                decimal? orloyaltyDiscount = orbaseLoadingPremium * (loyaltyDiscountValue / 100);//c63*c44
                decimal? oremployeeDiscount = orbaseLoadingPremium * (employeeDiscountValue / 100);//c63*c45
                decimal? oronlineDiscount = orbaseLoadingPremium * (onlineDiscountValue / 100);//c63*c46
                decimal? orfamilyDiscount = orbaseLoadingPremium * (familyDiscountValue / 100);//c63*c47
                decimal? orlongTermDiscount = (orbaseLoadingPremium - (orloyaltyDiscount + oremployeeDiscount + oronlineDiscount + orfamilyDiscount)) * longTermDiscount;
                decimal? orbasePremium = orbaseLoadingPremium - (orloyaltyDiscount + oremployeeDiscount + oronlineDiscount + orfamilyDiscount + orlongTermDiscount);

                decimal? unlimitedRestoreValue = 0;
                if (siRiderFiveDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderFiveDataTable.Rows)

                        unlimitedRestoreValue = 1;
                }
                decimal? unlimitedRestore = unlimitedRestoreValue > 0 ? orbasePremium * 0.005m : 0;
                decimal? finalBasePremium = orbasePremium + unlimitedRestore;

                string Opt = "N"; //need to calculate 
                decimal? hdcSI = 0;//rider_si_11 
                if (siRiderOneDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderOneDataTable.Rows)
                    {
                        Opt = "Y";
                        object siValueObject = itemRow["SIValue"];
                        hdcSI = Convert.ToDecimal(siValueObject);
                    }
                }
                string familysize = row.txt_family;
                var sqlhdc = $@"
                        SELECT {columnName}
                        FROM hdcrates_optimarestore
                        WHERE age = @p0  AND suminsured = @p1 AND plan = @p2";

                var hdcOpt = await dbContext.hdcrates_optimarestore
                .FromSqlRaw(sqlhdc, insuredAgeOne, siOne, familysize)
                    .Select(r => EF.Property<decimal?>(r, columnName))
                    .FirstOrDefaultAsync();

                decimal? hdcRiderPremium = await GetHDCRiderPremium(policyperiod, eldestMember, hdcSI, familysize);
                decimal? hdcFamilyDiscount = hdcRiderPremium * familyDiscountValue;
                decimal? hdcLongTermDiscount = (hdcRiderPremium - hdcFamilyDiscount) * (longTermDiscount);
                decimal? hdcFinalPremium = hdcRiderPremium - hdcFamilyDiscount - hdcLongTermDiscount;
                hdcFinalPremium = hdcFinalPremium ?? 0;
                string propt = "N";
                if (siRiderTwoDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderTwoDataTable.Rows)
                    {
                        propt = "Y";
                        object prsiValueObject = itemRow["SIValue"];
                    }
                }
                decimal? prInsured1 = GetProtectorRiderInsured1(propt, siOne, basePremium1);
                decimal? prInsured2 = GetProtectorRiderInsured2(propt, siTwo, basePremium2);
                decimal? prInsured3 = GetProtectorRiderInsured3(propt, siThree, basePremium3);
                decimal? prInsured4 = GetProtectorRiderInsured4(propt, siFour, basePremium4);
                decimal? prInsured5 = GetProtectorRiderInsured5(propt, siFive, basePremium5);
                decimal? prInsured6 = GetProtectorRiderInsured6(propt, siSix, basePremium6);
                decimal? prProtectorRiderPremium = GetProtectorRiderPremium(policyType, prInsured1, prInsured2, prInsured3, prInsured4, prInsured5, prInsured6);
                decimal? prLoadingInsured1 = GetProtectorRiderLoadingInsured1(prInsured1, basicLoadingRateOne / 100);
                decimal? prLoadingInsured2 = GetProtectorRiderLoadingInsured2(prInsured2, basicLoadingRateTwo / 100);
                decimal? prLoadingInsured3 = GetProtectorRiderLoadingInsured3(prInsured3, basicLoadingRateThree / 100);
                decimal? prLoadingInsured4 = GetProtectorRiderLoadingInsured4(prInsured4, basicLoadingRateFour / 100);
                decimal? prLoadingInsured5 = GetProtectorRiderLoadingInsured5(prInsured5, basicLoadingRateFive / 100);
                decimal? prLoadingInsured6 = GetProtectorRiderLoadingInsured6(prInsured6, basicLoadingRateSix / 100);
                decimal? ProtectorRiderLoadingPremium = prLoadingInsured1 + prLoadingInsured2 + prLoadingInsured3 + prLoadingInsured4 + prLoadingInsured5 + prLoadingInsured6;
                decimal? prBaseLoadingPremium = prProtectorRiderPremium + ProtectorRiderLoadingPremium;
                decimal? prFamilyDiscount = prProtectorRiderPremium * familyDiscountValue;
                decimal? prLongTermDiscount = (prBaseLoadingPremium - prFamilyDiscount) * (longTermDiscount);
                decimal? prpremiumProtectorRiderPremium = prBaseLoadingPremium - prFamilyDiscount - prLongTermDiscount;

                string individualpersonalARopt = "N"; //need to calculate and add the logic hardcoded as of now
                if (siRiderThreeDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderThreeDataTable.Rows)
                    {
                        individualpersonalARopt = "Y";
                    }
                }
                decimal? individualpersonalARSI = GetIndividualPersonalARSI(individualpersonalARopt, siOne);
                var policystartdate = row.policy_start_date;
                var policyenddate = row.policy_expiry_date;
                decimal? individualpersonalARAmt = GetIndividualPersonalARAmt(individualpersonalARopt, individualpersonalARSI, policystartdate, policyenddate);
                decimal? individualpersonalARLongTermDiscount = individualpersonalARAmt * longTermDiscount;
                decimal? individualPersonalAccidentRiderPremium = individualpersonalARAmt - individualpersonalARLongTermDiscount;
                string criticalAdvantageRideropt = "N";
                decimal? caSI = row.coversi11;
                decimal? ca_si_1 = 0;
                decimal? ca_si_2 = 0;
                decimal? ca_si_3 = 0;
                decimal? ca_si_4 = 0;
                decimal? ca_si_5 = 0;
                decimal? ca_si_6 = 0;
                int index = 1;
                if (siRiderFourDataTable.Rows.Count >= 1)
                {
                    foreach (DataRow itemRow in siRiderFourDataTable.Rows)
                    {
                        criticalAdvantageRideropt = "Y";
                        decimal siValue = (decimal)itemRow["SIValue"];
                        caSI = Convert.ToDecimal(siValue);
                        switch (index)
                        {
                            case 1:
                                ca_si_1 += siValue;
                                break;
                            case 2:
                                ca_si_2 += siValue;
                                break;
                            case 3:
                                ca_si_3 += siValue;
                                break;
                            case 4:
                                ca_si_4 += siValue;
                                break;
                            case 5:
                                ca_si_5 += siValue;
                                break;
                            case 6:
                                ca_si_6 += siValue;
                                break;
                        }
                        index++;
                    }
                }

                var sqlca = $@"
                        SELECT {columnName}
                        FROM carates
                        WHERE age = @p0  AND si = @p1";
                decimal criticalAdvantageRiderInsured1 = await dbContext.carates
                .FromSqlRaw(sqlca, insuredAgeOne, ca_si_1)
                    .Select(r => EF.Property<decimal?>(r, columnName))
                    .FirstOrDefaultAsync() ?? 0;
                decimal criticalAdvantageRiderInsured2 = await dbContext.carates
               .FromSqlRaw(sqlca, insuredAgeTwo, ca_si_2)
                   .Select(r => EF.Property<decimal?>(r, columnName))
                   .FirstOrDefaultAsync() ?? 0;
                decimal criticalAdvantageRiderInsured3 = await dbContext.carates
               .FromSqlRaw(sqlca, insuredAgeThree, ca_si_3)
                   .Select(r => EF.Property<decimal?>(r, columnName))
                   .FirstOrDefaultAsync() ?? 0;
                decimal criticalAdvantageRiderInsured4 = await dbContext.carates
               .FromSqlRaw(sqlca, insuredAgeFour, ca_si_4)
                 .Select(r => EF.Property<decimal?>(r, columnName))
                 .FirstOrDefaultAsync() ?? 0;
                decimal criticalAdvantageRiderInsured5 = await dbContext.carates
               .FromSqlRaw(sqlca, insuredAgeFive, ca_si_5)
                   .Select(r => EF.Property<decimal?>(r, columnName))
                   .FirstOrDefaultAsync() ?? 0;
                decimal criticalAdvantageRiderInsured6 = await dbContext.carates
              .FromSqlRaw(sqlca, insuredAgeSix, ca_si_6)
                  .Select(r => EF.Property<decimal?>(r, columnName))
                  .FirstOrDefaultAsync() ?? 0;
                decimal? criticalAdvantageRiderBasePremium = criticalAdvantageRiderInsured1 + criticalAdvantageRiderInsured2 + criticalAdvantageRiderInsured3 + criticalAdvantageRiderInsured4 + criticalAdvantageRiderInsured5 + criticalAdvantageRiderInsured6;

                decimal? criticalAdvantageRiderLoadingInsured1 = GetCARRiderLoadingInsured1(criticalAdvantageRiderInsured1, basicLoadingRateOne / 100);
                decimal? criticalAdvantageRiderLoadingInsured2 = GetCARRiderLoadingInsured2(criticalAdvantageRiderInsured2, basicLoadingRateTwo / 100);
                decimal? criticalAdvantageRiderLoadingInsured3 = GetCARRiderLoadingInsured3(criticalAdvantageRiderInsured3, basicLoadingRateThree / 100);
                decimal? criticalAdvantageRiderLoadingInsured4 = GetCARRiderLoadingInsured4(criticalAdvantageRiderInsured4, basicLoadingRateFour / 100);
                decimal? criticalAdvantageRiderLoadingInsured5 = GetCARRiderLoadingInsured5(criticalAdvantageRiderInsured5, basicLoadingRateFive / 100);
                decimal? criticalAdvantageRiderLoadingInsured6 = GetCARRiderLoadingInsured6(criticalAdvantageRiderInsured6, basicLoadingRateSix / 100);
                decimal? criticalAdvantageRiderLoading = GetCriticalAdvantageRiderLoading(policyType, criticalAdvantageRiderLoadingInsured1, criticalAdvantageRiderLoadingInsured2, criticalAdvantageRiderLoadingInsured3, criticalAdvantageRiderLoadingInsured4, criticalAdvantageRiderLoadingInsured5, criticalAdvantageRiderLoadingInsured6);

                decimal? criticalAdvRiderBaseLoadingPremium = criticalAdvantageRiderBasePremium + criticalAdvantageRiderLoading;
                decimal? criticalAdvRiderPremiumFamilyDiscount = criticalAdvRiderBaseLoadingPremium * familyDiscountValue;
                decimal? criticalAdvRiderPremiumLongTermDiscount = (criticalAdvRiderBaseLoadingPremium - criticalAdvRiderPremiumFamilyDiscount) * (longTermDiscount);
                decimal? criticalAdvRiderPremium = criticalAdvRiderBaseLoadingPremium - criticalAdvRiderPremiumFamilyDiscount - criticalAdvRiderPremiumLongTermDiscount;

                decimal? netPremium = (finalBasePremium + hdcFinalPremium + prpremiumProtectorRiderPremium + individualPersonalAccidentRiderPremium + criticalAdvRiderPremium);
                decimal? gST = (netPremium) * (0.18m);
                decimal? finalPremium = netPremium + gST;
                var totalpremium = row.num_tot_premium ?? 0;
                decimal? Crosscheck2 = totalpremium - finalPremium;
                var crosscheck2 = Crosscheck2;

                or = new OptimaRestoreRNE
                {
                    prod_code = row.prod_code,
                    prod_name = row.prod_name,
                    policy_number = row.policy_number,
                    batchid = row.batchid,
                    policy_start_date = row.policy_start_date,
                    policy_expiry_date = row.policy_expiry_date,
                    txt_salutation = row.txt_salutation,
                    location_code = row.location_code,
                    txt_family = row.txt_family,
                    policyplan = row.policyplan,
                    policy_type = row.policy_type,
                    policy_period = row.policy_period,
                    insuredname_1 = insured_name1,
                    insuredname_2 = insured_name2,
                    insuredname_3 = insured_name3,
                    insuredname_4 = insured_name4,
                    insuredname_5 = insured_name5,
                    insuredname_6 = insured_name6,
                    sum_insured1 = siOne,
                    sum_insured2 = siTwo,
                    sum_insured3 = siThree,
                    sum_insured4 = siFour,
                    sum_insured5 = siFive,
                    sum_insured6 = siSix,

                    upselltype1 = row.upselltype1,
                    upselltype2 = row.upselltype2,
                    upselltype3 = row.upselltype3,
                    upselltype4 = row.upselltype4,
                    upselltype5 = row.upselltype5,

                    upsellvalue1 = row.upsellvalue1,
                    upsellvalue2 = row.upsellvalue2,
                    upsellvalue3 = row.upsellvalue3,
                    upsellvalue4 = row.upsellvalue4,
                    upsellvalue5 = row.upsellvalue5,

                    upsellpremium1 = row.upsellpremium1,
                    upsellpremium2 = row.upsellpremium2,
                    upsellpremium3 = row.upsellpremium3,
                    upsellpremium4 = row.upsellpremium4,
                    upsellpremium5 = row.upsellpremium5,

                    no_of_members = noOfMembers,
                    eldest_member = eldestMember,
                    loading_per_insured_1 = basicLoadingRateOne,
                    loading_per_insured_2 = basicLoadingRateTwo,
                    loading_per_insured_3 = basicLoadingRateThree,
                    loading_per_insured_4 = basicLoadingRateFour,
                    loading_per_insured_5 = basicLoadingRateFive,
                    loading_per_insured_6 = basicLoadingRateSix,

                    base_Premium1 = basePremium1,
                    base_Premium2 = basePremium2,
                    base_Premium3 = basePremium3,
                    base_Premium4 = basePremium4,
                    base_Premium5 = basePremium5,
                    base_Premium6 = basePremium6,
                    base_Premium = basePremium,

                    basePremLoading_Insured1 = basePremLoadingInsured1,
                    basePremLoading_Insured2 = basePremLoadingInsured2,
                    basePremLoading_Insured3 = basePremLoadingInsured3,
                    basePremLoading_Insured4 = basePremLoadingInsured4,
                    basePremLoading_Insured5 = basePremLoadingInsured5,
                    basePremLoading_Insured6 = basePremLoadingInsured6,
                    basePrem_Loading = basePremLoading,


                    orbaseLoading_Premium = orbaseLoadingPremium,
                    orloyalty_Discount = orloyaltyDiscount,
                    oremployee_Discount = oremployeeDiscount,
                    oronline_Discount = oronlineDiscount,
                    orfamily_Discount = orfamilyDiscount,
                    orlongTerm_Discount = orlongTermDiscount,
                    orbase_Premium = orbasePremium.HasValue ? Math.Round(orbasePremium.Value, 2) : (decimal?)null,
                    unlimited_restore = unlimitedRestore,
                    finalbase_premium = finalBasePremium,
                    hdc_opt = Opt,
                    hdc_si = hdcSI,
                    hdc_rider_premium = hdcRiderPremium,
                    hdc_family_discount = hdcFamilyDiscount,
                    hdc_longterm_discount = hdcLongTermDiscount,
                    hdc_final_premium = hdcFinalPremium,
                    pr_opt = propt,
                    pr_insured_1 = prInsured1,
                    pr_insured_2 = prInsured2,
                    pr_insured_3 = prInsured3,
                    pr_insured_4 = prInsured4,
                    pr_insured_5 = prInsured5,
                    pr_insured_6 = prInsured6,
                    pr_ProtectorRider_Premium = prProtectorRiderPremium,
                    pr_loading_insured1 = prLoadingInsured1,
                    pr_loading_insured2 = prLoadingInsured2,
                    pr_loading_insured3 = prLoadingInsured3,
                    pr_loading_insured4 = prLoadingInsured4,
                    pr_loading_insured5 = prLoadingInsured5,
                    pr_loading_insured6 = prLoadingInsured6,
                    pr_protectorriderloading_premium = ProtectorRiderLoadingPremium,
                    pr_BaseLoading_Premium = prBaseLoadingPremium,
                    pr_Family_Discount = prFamilyDiscount,
                    pr_LongTerm_Discount = prLongTermDiscount,
                    prpremium_Protector_Rider_Premium = prpremiumProtectorRiderPremium,

                    individual_personalAR_opt = individualpersonalARopt,
                    individual_personalAR_SI = individualpersonalARSI,
                    individual_personalAR_Amt = individualpersonalARAmt,
                    individual_personalAR_LongTermDiscount = individualpersonalARLongTermDiscount,
                    individual_Personal_AccidentRiderPremium = individualPersonalAccidentRiderPremium,

                    criticalAdvantage_Rider_opt = criticalAdvantageRideropt,
                    criticalAdvantageRider_SumInsured_1 = ca_si_1,
                    criticalAdvantageRider_SumInsured_2 = ca_si_2,
                    criticalAdvantageRider_SumInsured_3 = ca_si_3,
                    criticalAdvantageRider_SumInsured_4 = ca_si_4,
                    criticalAdvantageRider_SumInsured_5 = ca_si_5,
                    criticalAdvantageRider_SumInsured_6 = ca_si_6,
                    criticalAdvantageRider_Insured_1 = criticalAdvantageRiderInsured1,
                    criticalAdvantageRider_Insured_2 = criticalAdvantageRiderInsured2,
                    criticalAdvantageRider_Insured_3 = criticalAdvantageRiderInsured3,
                    criticalAdvantageRider_Insured_4 = criticalAdvantageRiderInsured4,
                    criticalAdvantageRider_Insured_5 = criticalAdvantageRiderInsured5,
                    criticalAdvantageRider_Insured_6 = criticalAdvantageRiderInsured6,
                    criticalAdvantage_RiderBase_Premium = criticalAdvantageRiderBasePremium,

                    criticalAdvrider_loadinginsured1 = criticalAdvantageRiderLoadingInsured1,
                    criticalAdvrider_loadinginsured2 = criticalAdvantageRiderLoadingInsured2,
                    criticalAdvrider_loadinginsured3 = criticalAdvantageRiderLoadingInsured3,
                    criticalAdvrider_loadinginsured4 = criticalAdvantageRiderLoadingInsured4,
                    criticalAdvrider_loadinginsured5 = criticalAdvantageRiderLoadingInsured5,
                    criticalAdvrider_loadinginsured6 = criticalAdvantageRiderLoadingInsured6,
                    criticalAdvriderloading = criticalAdvantageRiderLoading,
                    criticalAdvriderbase_loading_premium = criticalAdvRiderBaseLoadingPremium,
                    criticalAdvRiderPremium_Family_Discount = criticalAdvRiderPremiumFamilyDiscount,
                    criticalAdvRiderPremium_LongTerm_Discount = criticalAdvRiderPremiumLongTermDiscount,
                    criticalAdv_Rider_Premium = criticalAdvRiderPremium,

                    net_premium = netPremium.HasValue ? Math.Round(netPremium.Value, 2) : (decimal?)null,
                    final_Premium_upsell = finalPremium.HasValue ? Math.Round(finalPremium.Value, 2) : (decimal?)null,
                    gst = gST.HasValue ? Math.Round(gST.Value, 2) : (decimal?)null,
                    cross_Check = crosscheck2.HasValue ? Math.Round(crosscheck2.Value, 2) : (decimal?)null,
                    or_total_Premium = row.num_tot_premium.HasValue ? Math.Round(row.num_tot_premium.Value, 2) : (decimal?)null,
                    or_netpremium = row.num_net_premium.HasValue ? Math.Round(row.num_net_premium.Value, 2) : (decimal?)null,
                    or_GST = row.num_service_tax.HasValue ? Math.Round(row.num_service_tax.Value, 2) : (decimal?)null
                };
            }
            return new List<OptimaRestoreRNE> { or };
        }

        static DataTable GetRiderSI(DataTable table, string riderName)
        {
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("RiderName", typeof(string));
            resultTable.Columns.Add("SIValue", typeof(object));
            foreach (DataColumn column in table.Columns)
            {
                if (column.ColumnName.StartsWith("covername"))
                {
                    string siColumnName = column.ColumnName.Replace("name", "si");
                    foreach (DataRow row in table.Rows)
                    {
                        if (row[column].ToString() == riderName)
                        {
                            DataRow newRow = resultTable.NewRow();
                            newRow["RiderName"] = riderName;
                            newRow["SIValue"] = Convert.ToDecimal(row[siColumnName]);
                            resultTable.Rows.Add(newRow);
                        }
                    }
                }
            }
            return resultTable;
        }

        public static decimal GetFamilyDiscount(string policyType, int numberOfMembers)
        {
            if (policyType == "Individual" && numberOfMembers > 1)
            {
                return 010m; // 10% discount
            }
            else
            {
                return 0.00m; // 0% discount
            }
        }

        public static decimal GetLongTermDiscount(string policyPeriod)
        {
            if (policyPeriod == "2 Years")
            {
                return 0.075m; // 7.5%
            }
            else if (policyPeriod == "3 Years")
            {
                return 0.10m; // 10%
            }
            else
            {
                return 0.00m; // 0%
            }
        }

        public static string GetColumnNameForPolicyPeriod(string policyPeriod)
        {
            return policyPeriod switch
            {
                "1 Year" => "one_year",
                "2 Years" => "two_years",
                "3 Years" => "three_years",
                _ => null,
            };
        }
        public decimal? GetBasePremium(string condition, List<decimal?> values)
        {
            decimal? sum = values.Sum();
            decimal? max = values.Max();
            decimal? difference = sum - max;
            decimal? percentageAdjustment = difference * 0.45m;

            if (condition == "INDIVIDUAL")
            {
                return sum;
            }
            else
            {
                return max + percentageAdjustment;
            }
        }

        static decimal? GetBasePremLoadingInsured1(decimal? basepremium1, decimal? base_loading_insured_1)
        {
            if (base_loading_insured_1.HasValue && base_loading_insured_1 != 0)
            {
                return basepremium1 * base_loading_insured_1 / 100;

            }
            else
            {
                return 0;
            }
        }

        static decimal? GetBasePremLoadingInsured2(decimal? basepremium2, decimal? base_loading_insured_2)
        {
            if (base_loading_insured_2.HasValue && base_loading_insured_2 != 0)
            {
                return basepremium2 * base_loading_insured_2 / 100;
            }
            else
            {
                return 0;
            }
        }

        static decimal? GetBasePremLoadingInsured3(decimal? basepremium3, decimal? base_loading_insured_3)
        {
            if (base_loading_insured_3.HasValue && base_loading_insured_3 != 0)
            {
                return basepremium3 * base_loading_insured_3 / 100;
            }
            else
            {
                return 0;
            }
        }

        static decimal? GetBasePremLoadingInsured4(decimal? basepremium4, decimal? base_loading_insured_4)
        {
            if (base_loading_insured_4.HasValue && base_loading_insured_4 != 0)
            {
                return basepremium4 * base_loading_insured_4 / 100;
            }
            else
            {
                return 0;
            }
        }

        static decimal? GetBasePremLoadingInsured5(decimal? basepremium5, decimal? base_loading_insured_5)
        {
            if (base_loading_insured_5.HasValue && base_loading_insured_5 != 0)
            {
                return basepremium5 * base_loading_insured_5 / 100;
            }
            else
            {
                return 0;
            }
        }

        static decimal? GetBasePremLoadingInsured6(decimal? basepremium6, decimal? base_loading_insured_6)
        {
            if (base_loading_insured_6.HasValue && base_loading_insured_6 != 0)
            {
                return basepremium6 * base_loading_insured_6 / 100;
            }
            else
            {
                return 0;
            }
        }


        public async Task<decimal?> GetHDCRiderPremium(string policyperiod, int? eldestMember, decimal? hdcSI, string familysize)
        {
            if (eldestMember == null || hdcSI == null)
            {
                throw new ArgumentException("Eldest member and HDCSI cannot be null.");
            }
            var column = GetColumnNameForPolicyPeriod(policyperiod);
            var sql = $@"
                SELECT {column}
                FROM hdcrates_optimarestore
                WHERE age = @p0
                AND suminsured = @p1
                AND plan = @p2";

            var rate = await dbContext.hdcrates_optimarestore
                    .FromSqlRaw(sql, eldestMember, hdcSI, familysize)
                    .Select(r => EF.Property<decimal?>(r, column))
                .FirstOrDefaultAsync();
            return rate;

        }

        static decimal? GetProtectorRiderInsured1(string propt, decimal? si_1, decimal? basePremiumInsured1)
        {
            try
            {
                if (propt == "Y")
                {
                    if (si_1 > 500000)
                    {
                        return basePremiumInsured1 * 0.075m;
                    }
                    else
                    {
                        return basePremiumInsured1 * 0.10m;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static decimal? GetProtectorRiderInsured2(string propt, decimal? si_2, decimal? basePremiumInsured2)
        {
            try
            {
                if (propt == "Y")
                {
                    if (si_2 > 500000)
                    {
                        return basePremiumInsured2 * 0.075m;
                    }
                    else
                    {
                        return basePremiumInsured2 * 0.10m;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static decimal? GetProtectorRiderInsured3(string propt, decimal? si_3, decimal? basePremiumInsured3)
        {
            try
            {
                if (propt == "Y")
                {
                    if (si_3 > 500000)
                    {
                        return basePremiumInsured3 * 0.075m;
                    }
                    else
                    {
                        return basePremiumInsured3 * 0.10m;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static decimal? GetProtectorRiderInsured4(string propt, decimal? si_4, decimal? basePremiumInsured4)
        {
            try
            {
                if (propt == "Y")
                {
                    if (si_4 > 500000)
                    {
                        return basePremiumInsured4 * 0.075m;
                    }
                    else
                    {
                        return basePremiumInsured4 * 0.10m;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static decimal? GetProtectorRiderInsured5(string propt, decimal? si_5, decimal? basePremiumInsured5)
        {
            try
            {
                if (propt == "Y")
                {
                    if (si_5 > 500000)
                    {
                        return basePremiumInsured5 * 0.075m;
                    }
                    else
                    {
                        return basePremiumInsured5 * 0.10m;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static decimal? GetProtectorRiderInsured6(string propt, decimal? si_6, decimal? basePremiumInsured6)
        {
            try
            {
                if (propt == "Y")
                {
                    if (si_6 > 500000)
                    {
                        return basePremiumInsured6 * 0.075m;
                    }
                    else
                    {
                        return basePremiumInsured6 * 0.10m;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static decimal? GetProtectorRiderPremium(string policyType, decimal? prInsured1, decimal? prInsured2, decimal? prInsured3, decimal? prInsured4, decimal? prInsured5, decimal? prInsured6)
        {
            var premiums = new List<decimal?> { prInsured1, prInsured2, prInsured3, prInsured4, prInsured5, prInsured6 };
            decimal? totalPremium = premiums.Sum();
            decimal? maxPremium = premiums.Max();
            if (policyType == "Individual")
            {
                return totalPremium;
            }
            else
            {
                return maxPremium + (totalPremium - maxPremium) * 0.45m;
            }
        }


        static decimal? GetProtectorRiderLoadingInsured1(decimal? prInsured1, decimal? loadingRateForInsured1)
        {
            try
            {
                if (prInsured1.HasValue && loadingRateForInsured1.HasValue)
                {
                    decimal? result = prInsured1.Value * loadingRateForInsured1.Value;
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        static decimal? GetProtectorRiderLoadingInsured2(decimal? prInsured2, decimal? loadingRateForInsured2)
        {
            try
            {
                if (prInsured2.HasValue && loadingRateForInsured2.HasValue)
                {
                    decimal? result = prInsured2.Value * loadingRateForInsured2.Value;
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        static decimal? GetProtectorRiderLoadingInsured3(decimal? prInsured3, decimal? loadingRateForInsured3)
        {
            try
            {
                if (prInsured3.HasValue && loadingRateForInsured3.HasValue)
                {
                    decimal? result = prInsured3.Value * loadingRateForInsured3.Value;
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        static decimal? GetProtectorRiderLoadingInsured4(decimal? prInsured4, decimal? loadingRateForInsured4)
        {
            try
            {
                if (prInsured4.HasValue && loadingRateForInsured4.HasValue)
                {
                    decimal? result = prInsured4.Value * loadingRateForInsured4.Value;
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        static decimal? GetProtectorRiderLoadingInsured5(decimal? prInsured5, decimal? loadingRateForInsured5)
        {
            try
            {
                if (prInsured5.HasValue && loadingRateForInsured5.HasValue)
                {
                    decimal? result = prInsured5.Value * loadingRateForInsured5.Value;
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        static decimal? GetProtectorRiderLoadingInsured6(decimal? prInsured6, decimal? loadingRateForInsured6)
        {
            try
            {
                if (prInsured6.HasValue && loadingRateForInsured6.HasValue)
                {
                    decimal result = prInsured6.Value * loadingRateForInsured6.Value;
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        static decimal? GetIndividualPersonalARSI(string? individualpersonalARopt, decimal? si_1)
        {
            if (individualpersonalARopt == null || si_1 == null) return 0;
            var isApplicable = individualpersonalARopt.Equals("Y", StringComparison.OrdinalIgnoreCase);
            var sumInsured = si_1 * 5 < 10000000 ? si_1 * 5 : 10000000;
            return isApplicable ? sumInsured : 0;
        }

        static decimal? GetIndividualPersonalARAmt(string? individualpersonalARopt, decimal? individualpersonalARSI, DateTime? policy_start_date, DateTime? policy_end_date)
        {
            if (individualpersonalARopt == null || individualpersonalARSI == null || policy_start_date == null || policy_end_date == null) return 0;
            var dailyRate = individualpersonalARopt.Equals("Y", StringComparison.OrdinalIgnoreCase) ? individualpersonalARSI.Value * 0.99m / 1000 : 0;
            var years = (policy_end_date.Value.Year - policy_start_date.Value.Year) -
                        ((policy_end_date.Value.DayOfYear < policy_start_date.Value.DayOfYear) ? 1 : 0);
            return dailyRate * years;
        }

        public async Task<decimal?> GetCriticalAdvantageRiderInsured1(string policyperiod, int? insuredAgeOne, decimal? caSI)
        {
            if (insuredAgeOne == null || caSI == null)
            {
                throw new ArgumentException("Eldest member and HDCSI cannot be null.");
            }
            var column = GetColumnNameForPolicyPeriod(policyperiod);
            if (column == null)
            {
                throw new ArgumentException("Invalid policy period provided.");
            }
            var sql = $@"
        SELECT {column}
        FROM carates
        WHERE age = @p0
        AND si = @p1";
            var rate = await dbContext.carates
                .FromSqlRaw(sql, insuredAgeOne, caSI)
                .Select(r => EF.Property<decimal?>(r, column))
                .FirstOrDefaultAsync();
            if (rate == null)
            {
                throw new InvalidOperationException("Rate not found for the given parameters.");
            }
            return rate;
        }

        static decimal? GetCARRiderLoadingInsured1(decimal? si_1, decimal? loading_per_insured_1)
        {
            try
            {
                if (si_1.HasValue && loading_per_insured_1.HasValue)
                {
                    decimal loadingRate = loading_per_insured_1.Value;
                    decimal loadingAmount = si_1.Value * loadingRate;
                    return loadingAmount;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating loading amount: {ex.Message}");
                return 0;
            }
        }

        static decimal? GetCARRiderLoadingInsured2(decimal? si_2, decimal? loading_per_insured_2)
        {
            try
            {
                if (si_2.HasValue && loading_per_insured_2.HasValue)
                {
                    decimal loadingRate = loading_per_insured_2.Value;
                    decimal loadingAmount = si_2.Value * loadingRate;
                    return loadingAmount;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating loading amount: {ex.Message}");
                return 0;
            }
        }


        static decimal? GetCARRiderLoadingInsured3(decimal? si_3, decimal? loading_per_insured_3)
        {
            try
            {
                if (si_3.HasValue && loading_per_insured_3.HasValue)
                {
                    decimal loadingRate = loading_per_insured_3.Value;
                    decimal loadingAmount = si_3.Value * loadingRate;
                    return loadingAmount;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating loading amount: {ex.Message}");
                return 0;
            }
        }


        static decimal? GetCARRiderLoadingInsured4(decimal? si_4, decimal? loading_per_insured_4)
        {
            try
            {
                if (si_4.HasValue && loading_per_insured_4.HasValue)
                {
                    decimal loadingRate = loading_per_insured_4.Value;
                    decimal loadingAmount = si_4.Value * loadingRate;
                    return loadingAmount;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating loading amount: {ex.Message}");
                return 0;
            }
        }


        static decimal? GetCARRiderLoadingInsured5(decimal? si_5, decimal? loading_per_insured_5)
        {
            try
            {
                if (si_5.HasValue && loading_per_insured_5.HasValue)
                {
                    decimal loadingRate = loading_per_insured_5.Value;
                    decimal loadingAmount = si_5.Value * loadingRate;
                    return loadingAmount;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating loading amount: {ex.Message}");
                return 0;
            }
        }

        static decimal? GetCARRiderLoadingInsured6(decimal? si_6, decimal? loading_per_insured_6)
        {
            try
            {
                if (si_6.HasValue && loading_per_insured_6.HasValue)
                {
                    decimal loadingRate = loading_per_insured_6.Value;
                    decimal loadingAmount = si_6.Value * loadingRate;
                    return loadingAmount;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating loading amount: {ex.Message}");
                return 0;
            }
        }

        static decimal? GetCriticalAdvantageRiderLoading(string policyType, decimal? criticalAdvantageRiderLoadingInsured1, decimal? criticalAdvantageRiderLoadingInsured2, decimal? criticalAdvantageRiderLoadingInsured3, decimal? criticalAdvantageRiderLoadingInsured4, decimal? criticalAdvantageRiderLoadingInsured5, decimal? criticalAdvantageRiderLoadingInsured6)
        {
            var premiums = new List<decimal?> { criticalAdvantageRiderLoadingInsured1, criticalAdvantageRiderLoadingInsured2, criticalAdvantageRiderLoadingInsured3, criticalAdvantageRiderLoadingInsured4, criticalAdvantageRiderLoadingInsured5, criticalAdvantageRiderLoadingInsured6 };
            decimal? totalPremium = premiums.Sum();
            decimal? maxPremium = premiums.Max();
            if (policyType == "Individual")
            {
                return totalPremium;
            }
            else
            {
                return maxPremium + (totalPremium - maxPremium) * 0.45m;
            }

        }

    }
}
