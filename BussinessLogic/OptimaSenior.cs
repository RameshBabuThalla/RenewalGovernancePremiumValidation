using RenewalGovernancePremiumValidation.Data;
using RenewalGovernancePremiumValidation.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Npgsql;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Logging;

namespace RenewalGovernancePremiumValidation.BussinessLogic
{

    public class OptimaSenior
    {
        private readonly HDFCDbContext dbContext;
        private readonly ILogger<OptimaSenior> _logger;
        public OptimaSenior(HDFCDbContext hDFCDbContext, ILogger<OptimaSenior> logger)
        {
            this.dbContext = hDFCDbContext;
            _logger = logger;
        }


        public async Task<OptimaSeniorValidationResult> GetOptimaSeniorValidation(string policyNo)
        {
            _logger.LogInformation("GetOptimaSeniorValidation is Started!");
            //step 1 get data from gc
            IEnumerable<OptimaSeniorRNE> OptimaSeniorValidationResult;
            var optimaseniorData = await GetOptimaSeniorDataAsync(policyNo, 1, 1000);
            //step 2 calculation
            _logger.LogInformation("CalculateOptimaSeniorPremium is Started!");
            OptimaSeniorValidationResult = await CalculateOptimaSeniorPremium(optimaseniorData);

            OptimaSeniorValidationResult objOptimaSeniorValidation = new OptimaSeniorValidationResult
            {
                prod_code = OptimaSeniorValidationResult.FirstOrDefault()?.prod_code,
                prod_name = OptimaSeniorValidationResult.FirstOrDefault()?.prod_name,
                reference_num = OptimaSeniorValidationResult.FirstOrDefault()?.reference_num,
                policy_number = OptimaSeniorValidationResult.FirstOrDefault()?.policy_number,
                batchid = OptimaSeniorValidationResult.FirstOrDefault()?.batchid,
                customer_id = OptimaSeniorValidationResult.FirstOrDefault()?.customer_id,
                customername = OptimaSeniorValidationResult.FirstOrDefault()?.customername,
                txt_salutation = OptimaSeniorValidationResult.FirstOrDefault()?.txt_salutation,
                location_code = OptimaSeniorValidationResult.FirstOrDefault()?.location_code,
                txt_apartment = OptimaSeniorValidationResult.FirstOrDefault()?.txt_apartment,
                txt_street = OptimaSeniorValidationResult.FirstOrDefault()?.txt_street,
                txt_areavillage = OptimaSeniorValidationResult.FirstOrDefault()?.txt_areavillage,
                txt_citydistrict = OptimaSeniorValidationResult.FirstOrDefault()?.txt_citydistrict,
                txt_state = OptimaSeniorValidationResult.FirstOrDefault()?.txt_state,
                state_code = OptimaSeniorValidationResult.FirstOrDefault()?.state_code,
                state_regis = OptimaSeniorValidationResult.FirstOrDefault()?.state_regis,
                txt_pincode = OptimaSeniorValidationResult.FirstOrDefault()?.txt_pincode,
                txt_nationality = OptimaSeniorValidationResult.FirstOrDefault()?.txt_nationality,
                imdmobile = OptimaSeniorValidationResult.FirstOrDefault()?.imdmobile,
                txt_telephone = OptimaSeniorValidationResult.FirstOrDefault()?.txt_telephone,
                txt_email = OptimaSeniorValidationResult.FirstOrDefault()?.txt_email,
                txt_dealer_cd = OptimaSeniorValidationResult.FirstOrDefault()?.txt_dealer_cd,//intermediary code
                imdname = OptimaSeniorValidationResult.FirstOrDefault()?.imdname,//intermediary name
                verticalname = OptimaSeniorValidationResult.FirstOrDefault()?.verticalname,
                //ssm_name = OptimaSeniorValidationResult.FirstOrDefault()?.ssm_name,
                txt_family = OptimaSeniorValidationResult.FirstOrDefault()?.txt_family,
                isrnflag = OptimaSeniorValidationResult.FirstOrDefault()?.isrnflag,
                split_flag = OptimaSeniorValidationResult.FirstOrDefault()?.split_flag,
                isvipflag = OptimaSeniorValidationResult.FirstOrDefault()?.isvipflag,
                no_of_members = OptimaSeniorValidationResult.FirstOrDefault()?.no_of_members,

                txt_insured_entrydate1 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_entrydate1,// inceptiondate in gc
                txt_insured_entrydate2 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_entrydate2,
                txt_insured_entrydate3 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_entrydate3,
                txt_insured_entrydate4 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_entrydate4,
                txt_insured_entrydate5 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_entrydate5,
                txt_insured_entrydate6 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_entrydate6,

                member_id1 = OptimaSeniorValidationResult.FirstOrDefault()?.member_id1,
                member_id2 = OptimaSeniorValidationResult.FirstOrDefault()?.member_id2,
                member_id3 = OptimaSeniorValidationResult.FirstOrDefault()?.member_id3,
                member_id4 = OptimaSeniorValidationResult.FirstOrDefault()?.member_id4,
                member_id5 = OptimaSeniorValidationResult.FirstOrDefault()?.member_id5,
                member_id6 = OptimaSeniorValidationResult.FirstOrDefault()?.member_id6,

                insured_loadingper1 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_loadingper1,
                insured_loadingper2 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_loadingper2,
                insured_loadingper3 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_loadingper3,
                insured_loadingper4 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_loadingper4,
                insured_loadingper5 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_loadingper5,
                insured_loadingper6 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_loadingper6,

                insured_loadingamt1 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_loadingamt1,
                insured_loadingamt2 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_loadingamt2,
                insured_loadingamt3 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_loadingamt3,
                insured_loadingamt4 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_loadingamt4,
                insured_loadingamt5 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_loadingamt5,
                insured_loadingamt6 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_loadingamt6,

                txt_insuredname1 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insuredname1,
                txt_insuredname2 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insuredname2,
                txt_insuredname3 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insuredname3,
                txt_insuredname4 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insuredname4,
                txt_insuredname5 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insuredname5,
                txt_insuredname6 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insuredname6,

                txt_insured_dob1 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_dob1,
                txt_insured_dob2 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_dob2,
                txt_insured_dob3 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_dob3,
                txt_insured_dob4 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_dob4,
                txt_insured_dob5 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_dob5,
                txt_insured_dob6 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_dob6,

                txt_insured_age1 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_age1,
                txt_insured_age2 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_age2,
                txt_insured_age3 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_age3,
                txt_insured_age4 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_age4,
                txt_insured_age5 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_age5,
                txt_insured_age6 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_age6,


                txt_insured_relation1 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_relation1,//coming as "string"
                txt_insured_relation2 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_relation2,
                txt_insured_relation3 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_relation3,
                txt_insured_relation4 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_relation4,
                txt_insured_relation5 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_relation5,
                txt_insured_relation6 = OptimaSeniorValidationResult.FirstOrDefault()?.txt_insured_relation6,

                pre_existing_disease1 = OptimaSeniorValidationResult.FirstOrDefault()?.pre_existing_disease1,//ped in  gc
                pre_existing_disease2 = OptimaSeniorValidationResult.FirstOrDefault()?.pre_existing_disease2,
                pre_existing_disease3 = OptimaSeniorValidationResult.FirstOrDefault()?.pre_existing_disease3,
                pre_existing_disease4 = OptimaSeniorValidationResult.FirstOrDefault()?.pre_existing_disease4,
                pre_existing_disease5 = OptimaSeniorValidationResult.FirstOrDefault()?.pre_existing_disease5,
                pre_existing_disease6 = OptimaSeniorValidationResult.FirstOrDefault()?.pre_existing_disease6,


                insured_cb1 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_cb1,
                insured_cb2 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_cb2,
                insured_cb3 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_cb3,
                insured_cb4 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_cb4,
                insured_cb5 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_cb5,
                insured_cb6 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_cb6,

                sum_insured1 = OptimaSeniorValidationResult.FirstOrDefault()?.sum_insured1,
                sum_insured2 = OptimaSeniorValidationResult.FirstOrDefault()?.sum_insured2,
                sum_insured3 = OptimaSeniorValidationResult.FirstOrDefault()?.sum_insured3,
                sum_insured4 = OptimaSeniorValidationResult.FirstOrDefault()?.sum_insured4,
                sum_insured5 = OptimaSeniorValidationResult.FirstOrDefault()?.sum_insured5,
                sum_insured6 = OptimaSeniorValidationResult.FirstOrDefault()?.sum_insured6,

                insured_deductable1 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_deductable1,
                insured_deductable2 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_deductable2,
                insured_deductable3 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_deductable3,
                insured_deductable4 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_deductable4,
                insured_deductable5 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_deductable5,
                insured_deductable6 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_deductable6,


                wellness_discount1 = OptimaSeniorValidationResult.FirstOrDefault()?.wellness_discount1, //wellness in gc
                wellness_discount2 = OptimaSeniorValidationResult.FirstOrDefault()?.wellness_discount2,
                wellness_discount3 = OptimaSeniorValidationResult.FirstOrDefault()?.wellness_discount3,
                wellness_discount4 = OptimaSeniorValidationResult.FirstOrDefault()?.wellness_discount4,
                wellness_discount5 = OptimaSeniorValidationResult.FirstOrDefault()?.wellness_discount5,
                wellness_discount6 = OptimaSeniorValidationResult.FirstOrDefault()?.wellness_discount6,


                stayactive1 = OptimaSeniorValidationResult.FirstOrDefault()?.stayactive1,
                stayactive2 = OptimaSeniorValidationResult.FirstOrDefault()?.stayactive2,
                stayactive3 = OptimaSeniorValidationResult.FirstOrDefault()?.stayactive3,
                stayactive4 = OptimaSeniorValidationResult.FirstOrDefault()?.stayactive4,
                stayactive5 = OptimaSeniorValidationResult.FirstOrDefault()?.stayactive5,
                stayactive6 = OptimaSeniorValidationResult.FirstOrDefault()?.stayactive6,


                coverbaseloadingrate1 = OptimaSeniorValidationResult.FirstOrDefault()?.coverbaseloadingrate1,//Basic Loading Rate1 in gc
                coverbaseloadingrate2 = OptimaSeniorValidationResult.FirstOrDefault()?.coverbaseloadingrate2,
                coverbaseloadingrate3 = OptimaSeniorValidationResult.FirstOrDefault()?.coverbaseloadingrate3,
                coverbaseloadingrate4 = OptimaSeniorValidationResult.FirstOrDefault()?.coverbaseloadingrate4,
                coverbaseloadingrate5 = OptimaSeniorValidationResult.FirstOrDefault()?.coverbaseloadingrate5,
                coverbaseloadingrate6 = OptimaSeniorValidationResult.FirstOrDefault()?.coverbaseloadingrate6,

                health_incentive1 = OptimaSeniorValidationResult.FirstOrDefault()?.health_incentive1,
                health_incentive2 = OptimaSeniorValidationResult.FirstOrDefault()?.health_incentive2,
                health_incentive3 = OptimaSeniorValidationResult.FirstOrDefault()?.health_incentive3,
                health_incentive4 = OptimaSeniorValidationResult.FirstOrDefault()?.health_incentive4,
                health_incentive5 = OptimaSeniorValidationResult.FirstOrDefault()?.health_incentive5,
                health_incentive6 = OptimaSeniorValidationResult.FirstOrDefault()?.health_incentive6,


                fitness_discount1 = OptimaSeniorValidationResult.FirstOrDefault()?.fitness_discount1,
                fitness_discount2 = OptimaSeniorValidationResult.FirstOrDefault()?.fitness_discount2,
                fitness_discount3 = OptimaSeniorValidationResult.FirstOrDefault()?.fitness_discount3,
                fitness_discount4 = OptimaSeniorValidationResult.FirstOrDefault()?.fitness_discount4,
                fitness_discount5 = OptimaSeniorValidationResult.FirstOrDefault()?.fitness_discount5,
                fitness_discount6 = OptimaSeniorValidationResult.FirstOrDefault()?.fitness_discount6,


                reservbenefis1 = OptimaSeniorValidationResult.FirstOrDefault()?.reservbenefis1,
                reservbenefis2 = OptimaSeniorValidationResult.FirstOrDefault()?.reservbenefis2,
                reservbenefis3 = OptimaSeniorValidationResult.FirstOrDefault()?.reservbenefis3,
                reservbenefis4 = OptimaSeniorValidationResult.FirstOrDefault()?.reservbenefis4,
                reservbenefis5 = OptimaSeniorValidationResult.FirstOrDefault()?.reservbenefis5,
                reservbenefis6 = OptimaSeniorValidationResult.FirstOrDefault()?.reservbenefis6,

                insured_rb_claimamt1 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_rb_claimamt1,
                insured_rb_claimamt2 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_rb_claimamt2,
                insured_rb_claimamt3 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_rb_claimamt3,
                insured_rb_claimamt4 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_rb_claimamt4,
                insured_rb_claimamt5 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_rb_claimamt5,
                insured_rb_claimamt6 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_rb_claimamt6,

                preventive_hc = OptimaSeniorValidationResult.FirstOrDefault()?.preventive_hc,
                policy_start_date = OptimaSeniorValidationResult.FirstOrDefault()?.policy_start_date,
                policy_expiry_date = OptimaSeniorValidationResult.FirstOrDefault()?.policy_expiry_date,
                policy_type = OptimaSeniorValidationResult.FirstOrDefault()?.policy_type,
                policy_period = OptimaSeniorValidationResult.FirstOrDefault()?.policy_period,
                policyplan = OptimaSeniorValidationResult.FirstOrDefault()?.policyplan,
                claimcount = OptimaSeniorValidationResult.FirstOrDefault()?.claimcount,
                //num_tot_premium = OptimaSeniorValidationResult.FirstOrDefault()?.num_tot_premium,

                tier_type = OptimaSeniorValidationResult.FirstOrDefault()?.tier_type,

                no_claim_discount = OptimaSeniorValidationResult.FirstOrDefault()?.no_claim_discount,
                family_discount = OptimaSeniorValidationResult.FirstOrDefault()?.family_discount,
                longterm_discount = OptimaSeniorValidationResult.FirstOrDefault()?.longterm_discount,

                insured_Premium1 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_Premium1,
                insured_Premium2 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_Premium2,
                insured_Premium3 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_Premium3,
                insured_Premium4 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_Premium4,
                insured_Premium5 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_Premium5,
                insured_Premium6 = OptimaSeniorValidationResult.FirstOrDefault()?.insured_Premium6,
                base_Premium = OptimaSeniorValidationResult.FirstOrDefault()?.base_Premium,

                loading_Premium1 = OptimaSeniorValidationResult.FirstOrDefault()?.loading_Premium1,
                loading_Premium2 = OptimaSeniorValidationResult.FirstOrDefault()?.loading_Premium2,
                loading_Premium3 = OptimaSeniorValidationResult.FirstOrDefault()?.loading_Premium3,
                loading_Premium4 = OptimaSeniorValidationResult.FirstOrDefault()?.loading_Premium4,
                loading_Premium5 = OptimaSeniorValidationResult.FirstOrDefault()?.loading_Premium5,
                loading_Premium6 = OptimaSeniorValidationResult.FirstOrDefault()?.loading_Premium6,
                loading_Premium = OptimaSeniorValidationResult.FirstOrDefault()?.loading_Premium,

                optimaSenior_BasePremium_Loading = OptimaSeniorValidationResult.FirstOrDefault()?.optimaSenior_BasePremium_Loading,
                optimaSenior_NoClaim_Discount = OptimaSeniorValidationResult.FirstOrDefault()?.optimaSenior_NoClaim_Discount,
                optimaSenior_Family_Discount = OptimaSeniorValidationResult.FirstOrDefault()?.optimaSenior_Family_Discount,
                optimaSenior_LongTerm_Discount = OptimaSeniorValidationResult.FirstOrDefault()?.optimaSenior_LongTerm_Discount,
                optimaSeniorFinalBase_Premium = OptimaSeniorValidationResult.FirstOrDefault()?.optimaSeniorFinalBase_Premium.HasValue == true
                             ? (decimal?)Math.Round(OptimaSeniorValidationResult.FirstOrDefault().optimaSeniorFinalBase_Premium.Value, 2)
                             : (decimal?)null,

                net_premium = OptimaSeniorValidationResult.FirstOrDefault()?.net_premium.HasValue == true
                             ? (decimal?)Math.Round(OptimaSeniorValidationResult.FirstOrDefault().net_premium.Value, 2)
                             : (decimal?)null,

                final_Premium = OptimaSeniorValidationResult.FirstOrDefault()?.final_Premium.HasValue == true
                             ? (decimal?)Math.Round(OptimaSeniorValidationResult.FirstOrDefault().final_Premium.Value, 2)
                             : (decimal?)null,
                gst = OptimaSeniorValidationResult.FirstOrDefault()?.gst.HasValue == true
                             ? (decimal?)Math.Round(OptimaSeniorValidationResult.FirstOrDefault().gst.Value, 2)
                             : (decimal?)null,
                cross_Check = OptimaSeniorValidationResult.FirstOrDefault()?.cross_Check.HasValue == true
                             ? (decimal?)Math.Round(OptimaSeniorValidationResult.FirstOrDefault().cross_Check.Value, 2)
                             : (decimal?)null,


                optimasenior_total_Premium = OptimaSeniorValidationResult.FirstOrDefault()?.optimasenior_total_Premium.HasValue == true
                             ? (decimal?)Math.Round(OptimaSeniorValidationResult.FirstOrDefault().optimasenior_total_Premium.Value, 2)
                             : (decimal?)null,
                optimasenior_netpremium = OptimaSeniorValidationResult.FirstOrDefault()?.optimasenior_netpremium.HasValue == true
                             ? (decimal?)Math.Round(OptimaSeniorValidationResult.FirstOrDefault().optimasenior_netpremium.Value, 2)
                             : (decimal?)null,
                optimasenior_gst = OptimaSeniorValidationResult.FirstOrDefault()?.optimasenior_gst.HasValue == true
                             ? (decimal?)Math.Round(OptimaSeniorValidationResult.FirstOrDefault().optimasenior_gst.Value, 2)
                             : (decimal?)null,

            };
            if (objOptimaSeniorValidation?.policy_number == null)
            {
                Console.WriteLine("Policy number not found.");
            }
            var record = await dbContext.rne_calculated_cover_rg.AsNoTracking().FirstOrDefaultAsync(item => item.policy_number == policyNo.ToString());

            if (objOptimaSeniorValidation != null)
            {
                var no_of_members = objOptimaSeniorValidation.no_of_members;
                var policy_number = objOptimaSeniorValidation.policy_number;
                var reference_number = objOptimaSeniorValidation.reference_num;
                var newRecord = new List<rne_calculated_cover_rg>();
                for (int i = 1; i <= no_of_members; i++)
                {
                    var sumInsured = Convert.ToDecimal(objOptimaSeniorValidation.GetType().GetProperty($"sum_insured{i}")?.GetValue(objOptimaSeniorValidation));
                    var basePremium = Convert.ToDecimal(objOptimaSeniorValidation.GetType().GetProperty($"insured_Premium{i}")?.GetValue(objOptimaSeniorValidation));
                    if (no_of_members > 1 && i >= 2 && i <= 6)
                    {
                        basePremium *= 0.45m;
                    }
                    var newRecords = new rne_calculated_cover_rg
                    {
                        policy_number = policy_number,
                        referencenum = reference_number,
                        suminsured = sumInsured,
                        premium = basePremium,
                        riskname = objOptimaSeniorValidation.GetType().GetProperty($"insuredname_{i}")?.GetValue(objOptimaSeniorValidation)?.ToString(),
                        covername = "Baic OptimaSenior"
                    };
                    newRecord.Add(newRecords);
                }
                dbContext.rne_calculated_cover_rg.AddRange(newRecord);
                await dbContext.SaveChangesAsync();
            }
            return objOptimaSeniorValidation;
        }
        private async Task<IEnumerable<OptimaSeniorRNE>> GetOptimaSeniorDataAsync(string policyNo, int pageNumber, int pageSize)
        {
            // Query to get data from GC to calculate optima senior premium
            var optimaseniorData = await (
            from os in dbContext.rne_healthtab
            join osidst in dbContext.idst_renewal_data_rgs on os.policy_number equals osidst.certificate_no
            where (os.policy_number == policyNo)
            select new OptimaSeniorRNE
            {
                prod_code = os.prod_code,
                batchid = os.batchid,
                prod_name = os.prod_name,
                //proposal_no = os.proposal_no,
                //policy_no = gc.policy_no,
                policy_number = os.policy_number,
                reference_num = os.reference_num,
                split_flag = os.split_flag,
                customer_id = os.customer_id,
                customername = os.customername,
                vertical_name = os.verticale_name,//psm code in old gc mapping
                verticalname = os.verticalname,//psm name in old gc mapping
                policy_start_date = os.policy_start_date,
                policy_expiry_date = os.policy_expiry_date,
                policy_period = os.policy_period,
                tier_type = os.tier_type,//tier in old gc mapping
                policyplan = os.policyplan,
                policy_type = os.policy_type,
                txt_family = os.txt_family,//family size in old gc mapping
                claimcount = os.claimcount,
                num_tot_premium = os.num_tot_premium,
                num_net_premium = os.num_net_premium,
                num_service_tax = os.num_service_tax,
                //gst in old gc mapping                                    
                // rn_flag = gc.rn_flag,
                //vip_flag = gc.vip_flag,
                //first_inception_date_1 = gc.first_inception_date_1,
                //first_inception_date_2 = gc.first_inception_date_2,
                //first_inception_date_3 = gc.first_inception_date_3,
                //first_inception_date_4 = gc.first_inception_date_4,
                //first_inception_date_5 = gc.first_inception_date_5,
                //first_inception_date_6 = gc.first_inception_date_6,

                coverbaseloadingrate1 = os.coverbaseloadingrate1,
                coverbaseloadingrate2 = os.coverbaseloadingrate2,
                coverbaseloadingrate3 = os.coverbaseloadingrate3,
                coverbaseloadingrate4 = os.coverbaseloadingrate4,
                coverbaseloadingrate5 = os.coverbaseloadingrate5,
                coverbaseloadingrate6 = os.coverbaseloadingrate6,


                insured_loadingper1 = osidst.loading_per_insured1,
                insured_loadingper2 = osidst.loading_per_insured2,
                insured_loadingper3 = osidst.loading_per_insured3,
                insured_loadingper4 = osidst.loading_per_insured4,
                insured_loadingper5 = osidst.loading_per_insured5,
                insured_loadingper6 = osidst.loading_per_insured6,

                txt_insuredname1 = os.txt_insuredname1,
                txt_insuredname2 = os.txt_insuredname2,
                txt_insuredname3 = os.txt_insuredname3,
                txt_insuredname4 = os.txt_insuredname4,
                txt_insuredname5 = os.txt_insuredname5,
                txt_insuredname6 = os.txt_insuredname6,

                txt_insured_dob1 = os.txt_insured_dob1,
                txt_insured_dob2 = os.txt_insured_dob2,
                txt_insured_dob3 = os.txt_insured_dob3,
                txt_insured_dob4 = os.txt_insured_dob4,
                txt_insured_dob5 = os.txt_insured_dob5,
                txt_insured_dob6 = os.txt_insured_dob6,

                txt_insured_relation1 = os.txt_insured_relation1,
                txt_insured_relation2 = os.txt_insured_relation2,
                txt_insured_relation3 = os.txt_insured_relation3,
                txt_insured_relation4 = os.txt_insured_relation4,
                txt_insured_relation5 = os.txt_insured_relation5,
                txt_insured_relation6 = os.txt_insured_relation6,

                txt_insured_age1 = os.txt_insured_age1,
                txt_insured_age2 = os.txt_insured_age2,
                txt_insured_age3 = os.txt_insured_age3,
                txt_insured_age4 = os.txt_insured_age4,
                txt_insured_age5 = os.txt_insured_age5,
                txt_insured_age6 = os.txt_insured_age6,

                txt_insured_gender1 = os.txt_insured_gender1,
                txt_insured_gender2 = os.txt_insured_gender2,
                txt_insured_gender3 = os.txt_insured_gender3,
                txt_insured_gender4 = os.txt_insured_gender4,
                txt_insured_gender5 = os.txt_insured_gender5,
                txt_insured_gender6 = os.txt_insured_gender6,

                member_id1 = os.member_id1,
                member_id2 = os.member_id2,
                member_id3 = os.member_id3,
                member_id4 = os.member_id4,
                member_id5 = os.member_id5,
                member_id6 = os.member_id6,

                nominee_name1 = os.nominee_name1,
                nominee_name2 = os.nominee_name2,
                nominee_name3 = os.nominee_name3,
                nominee_name4 = os.nominee_name4,
                nominee_name5 = os.nominee_name5,
                nominee_name6 = os.nominee_name6,

                nominee_relationship1 = os.nominee_relationship1,
                nominee_relationship2 = os.nominee_relationship2,
                nominee_relationship3 = os.nominee_relationship3,
                nominee_relationship4 = os.nominee_relationship4,
                nominee_relationship5 = os.nominee_relationship5,
                nominee_relationship6 = os.nominee_relationship6,

                pollddesc1 = os.pollddesc1,
                pollddesc2 = os.pollddesc2,
                pollddesc3 = os.pollddesc3,
                pollddesc4 = os.pollddesc4,
                pollddesc5 = os.pollddesc5,

                sum_insured1 = os.sum_insured1,
                sum_insured2 = os.sum_insured2,
                sum_insured3 = os.sum_insured3,
                sum_insured4 = os.sum_insured4,
                sum_insured5 = os.sum_insured5,
                sum_insured6 = os.sum_insured6,

                insured_cb1 = os.insured_cb1,
                insured_cb2 = os.insured_cb2,
                insured_cb3 = os.insured_cb3,
                insured_cb4 = os.insured_cb4,
                insured_cb5 = os.insured_cb5,
                insured_cb6 = os.insured_cb6,

                premium_insured1 = os.premium_insured1,
                premium_insured2 = os.premium_insured2,
                premium_insured3 = os.premium_insured3,
                premium_insured4 = os.premium_insured4,
                premium_insured5 = os.premium_insured5,
                premium_insured6 = os.premium_insured6,

                insured_deductable1 = os.insured_deductable1,//deductable insured in old gc mapping
                insured_deductable2 = os.insured_deductable2,
                insured_deductable3 = os.insured_deductable3,
                insured_deductable4 = os.insured_deductable4,
                insured_deductable5 = os.insured_deductable5,
                insured_deductable6 = os.insured_deductable6,

                covername11 = os.covername11,
                covername12 = os.covername12,
                covername13 = os.covername13,
                covername14 = os.covername14,
                covername15 = os.covername15,
                covername16 = os.covername16,
                covername17 = os.covername17,
                covername18 = os.covername18,
                covername19 = os.covername19,
                covername21 = os.covername21,
                covername22 = os.covername22,
                covername23 = os.covername23,
                covername24 = os.covername24,
                covername25 = os.covername25,
                covername26 = os.covername26,
                covername27 = os.covername27,
                covername28 = os.covername28,
                covername29 = os.covername29,
                covername31 = os.covername31,
                covername32 = os.covername32,
                covername33 = os.covername33,
                covername34 = os.covername34,
                covername35 = os.covername35,
                covername36 = os.covername36,
                covername37 = os.covername37,
                covername38 = os.covername38,
                covername39 = os.covername39,
                covername41 = os.covername41,
                covername42 = os.covername42,
                covername43 = os.covername43,
                covername44 = os.covername44,
                covername45 = os.covername45,
                covername46 = os.covername46,
                covername47 = os.covername47,
                covername48 = os.covername48,
                covername49 = os.covername49,
                covername51 = os.covername51,
                covername52 = os.covername52,
                covername53 = os.covername53,
                covername54 = os.covername54,
                covername55 = os.covername55,
                covername56 = os.covername56,
                covername57 = os.covername57,
                covername58 = os.covername58,
                covername59 = os.covername59,
                covername61 = os.covername61,
                covername62 = os.covername62,
                covername63 = os.covername63,
                covername64 = os.covername64,
                covername65 = os.covername65,
                covername66 = os.covername66,
                covername67 = os.covername67,
                covername68 = os.covername68,
                covername69 = os.covername69,
                covername71 = os.covername71,
                covername72 = os.covername72,
                covername73 = os.covername73,
                covername74 = os.covername74,
                covername75 = os.covername75,
                covername76 = os.covername76,
                covername77 = os.covername77,
                covername78 = os.covername78,
                covername79 = os.covername79,
                covername81 = os.covername81,
                covername82 = os.covername82,
                covername83 = os.covername83,
                covername84 = os.covername84,
                covername85 = os.covername85,
                covername86 = os.covername86,
                covername87 = os.covername87,
                covername88 = os.covername88,
                covername89 = os.covername89,
                covername91 = os.covername91,
                covername92 = os.covername92,
                covername93 = os.covername93,
                covername94 = os.covername94,
                covername95 = os.covername95,
                covername96 = os.covername96,
                covername97 = os.covername97,
                covername98 = os.covername98,
                covername99 = os.covername99,
                covername101 = os.covername101,
                covername102 = os.covername102,
                covername103 = os.covername103,
                covername104 = os.covername104,
                covername105 = os.covername105,
                covername106 = os.covername106,
                covername107 = os.covername107,
                covername108 = os.covername108,
                covername109 = os.covername109,
                covername110 = os.covername110,
                covername210 = os.covername210,
                covername310 = os.covername310,
                covername410 = os.covername410,
                covername510 = os.covername510,
                covername610 = os.covername610,
                covername710 = os.covername710,
                covername810 = os.covername810,
                covername910 = os.covername910,
                covername1010 = os.covername1010,

                coversi11 = os.coversi11,
                coversi12 = os.coversi12,
                coversi13 = os.coversi13,
                coversi14 = os.coversi14,
                coversi15 = os.coversi15,
                coversi16 = os.coversi16,
                coversi17 = os.coversi17,
                coversi18 = os.coversi18,
                coversi19 = os.coversi19,
                coversi21 = os.coversi21,
                coversi22 = os.coversi22,
                coversi23 = os.coversi23,
                coversi24 = os.coversi24,
                coversi25 = os.coversi25,
                coversi26 = os.coversi26,
                coversi27 = os.coversi27,
                coversi28 = os.coversi28,
                coversi29 = os.coversi29,
                coversi31 = os.coversi31,
                coversi32 = os.coversi32,
                coversi33 = os.coversi33,
                coversi34 = os.coversi34,
                coversi35 = os.coversi35,
                coversi36 = os.coversi36,
                coversi37 = os.coversi37,
                coversi38 = os.coversi38,
                coversi39 = os.coversi39,
                coversi41 = os.coversi41,
                coversi42 = os.coversi42,
                coversi43 = os.coversi43,
                coversi44 = os.coversi44,
                coversi45 = os.coversi46,
                coversi47 = os.coversi47,
                coversi48 = os.coversi48,
                coversi49 = os.coversi49,
                coversi51 = os.coversi51,
                coversi52 = os.coversi52,
                coversi53 = os.coversi53,
                coversi54 = os.coversi54,
                coversi55 = os.coversi55,
                coversi56 = os.coversi56,
                coversi57 = os.coversi57,
                coversi58 = os.coversi58,
                coversi59 = os.coversi59,
                coversi61 = os.coversi61,
                coversi62 = os.coversi62,
                coversi63 = os.coversi63,
                coversi64 = os.coversi64,
                coversi65 = os.coversi65,
                coversi66 = os.coversi66,
                coversi67 = os.coversi67,
                coversi68 = os.coversi68,
                coversi69 = os.coversi69,
                coversi71 = os.coversi71,
                coversi72 = os.coversi72,
                coversi73 = os.coversi73,
                coversi74 = os.coversi74,
                coversi75 = os.coversi75,
                coversi76 = os.coversi76,
                coversi77 = os.coversi77,
                coversi78 = os.coversi78,
                coversi79 = os.coversi79,
                coversi81 = os.coversi81,
                coversi82 = os.coversi82,
                coversi83 = os.coversi83,
                coversi84 = os.coversi84,
                coversi85 = os.coversi85,
                coversi86 = os.coversi86,
                coversi87 = os.coversi87,
                coversi88 = os.coversi88,
                coversi89 = os.coversi89,
                coversi91 = os.coversi91,
                coversi92 = os.coversi92,
                coversi93 = os.coversi93,
                coversi94 = os.coversi94,
                coversi95 = os.coversi95,
                coversi96 = os.coversi96,
                coversi97 = os.coversi97,
                coversi98 = os.coversi98,
                coversi99 = os.coversi99,
                coversi101 = os.coversi101,
                coversi102 = os.coversi102,
                coversi103 = os.coversi103,
                coversi104 = os.coversi104,
                coversi105 = os.coversi105,
                coversi106 = os.coversi106,
                coversi107 = os.coversi107,
                coversi108 = os.coversi108,
                coversi109 = os.coversi109,
                coversi210 = os.coversi210,
                coversi310 = os.coversi310,
                coversi410 = os.coversi410,
                coversi510 = os.coversi510,
                coversi610 = os.coversi610,
                coversi810 = os.coversi810,
                coversi910 = os.coversi910,
                coversi1010 = os.coversi1010,

                coverprem11 = os.coverprem11,
                coverprem12 = os.coverprem12,
                coverprem13 = os.coverprem13,
                coverprem14 = os.coverprem14,
                coverprem15 = os.coverprem15,
                coverprem16 = os.coverprem16,
                coverprem17 = os.coverprem17,
                coverprem18 = os.coverprem18,
                coverprem19 = os.coverprem19,
                coverprem21 = os.coverprem21,
                coverprem22 = os.coverprem22,
                coverprem23 = os.coverprem23,
                coverprem24 = os.coverprem24,
                coverprem25 = os.coverprem25,
                coverprem26 = os.coverprem26,
                coverprem27 = os.coverprem27,
                coverprem28 = os.coverprem28,
                coverprem29 = os.coverprem29,
                coverprem31 = os.coverprem31,
                coverprem32 = os.coverprem32,
                coverprem33 = os.coverprem33,
                coverprem34 = os.coverprem34,
                coverprem35 = os.coverprem35,
                coverprem36 = os.coverprem36,
                coverprem37 = os.coverprem37,
                coverprem38 = os.coverprem38,
                coverprem39 = os.coverprem39,
                coverprem41 = os.coverprem41,
                coverprem42 = os.coverprem42,
                coverprem43 = os.coverprem43,
                coverprem44 = os.coverprem44,
                coverprem45 = os.coverprem46,
                coverprem47 = os.coverprem47,
                coverprem48 = os.coverprem48,
                coverprem49 = os.coverprem49,
                coverprem51 = os.coverprem51,
                coverprem52 = os.coverprem52,
                coverprem53 = os.coverprem53,
                coverprem54 = os.coverprem54,
                coverprem55 = os.coverprem55,
                coverprem56 = os.coverprem56,
                coverprem57 = os.coverprem57,
                coverprem58 = os.coverprem58,
                coverprem59 = os.coverprem59,
                coverprem61 = os.coverprem61,
                coverprem62 = os.coverprem62,
                coverprem63 = os.coverprem63,
                coverprem64 = os.coverprem64,
                coverprem65 = os.coverprem65,
                coverprem66 = os.coverprem66,
                coverprem67 = os.coverprem67,
                coverprem68 = os.coverprem68,
                coverprem69 = os.coverprem69,
                coverprem71 = os.coverprem71,
                coverprem72 = os.coverprem72,
                coverprem73 = os.coverprem73,
                coverprem74 = os.coverprem74,
                coverprem75 = os.coverprem75,
                coverprem76 = os.coverprem76,
                coverprem77 = os.coverprem77,
                coverprem78 = os.coverprem78,
                coverprem79 = os.coverprem79,
                coverprem81 = os.coverprem81,
                coverprem82 = os.coverprem82,
                coverprem83 = os.coverprem83,
                coverprem84 = os.coverprem84,
                coverprem85 = os.coverprem85,
                coverprem86 = os.coverprem86,
                coverprem87 = os.coverprem87,
                coverprem88 = os.coverprem88,
                coverprem89 = os.coverprem89,
                coverprem91 = os.coverprem91,
                coverprem92 = os.coverprem92,
                coverprem93 = os.coverprem93,
                coverprem94 = os.coverprem94,
                coverprem95 = os.coverprem95,
                coverprem96 = os.coverprem96,
                coverprem97 = os.coverprem97,
                coverprem98 = os.coverprem98,
                coverprem99 = os.coverprem99,
                coverprem101 = os.coverprem101,
                coverprem102 = os.coverprem102,
                coverprem103 = os.coverprem103,
                coverprem104 = os.coverprem104,
                coverprem105 = os.coverprem105,
                coverprem106 = os.coverprem106,
                coverprem107 = os.coverprem107,
                coverprem108 = os.coverprem108,
                coverprem109 = os.coverprem109,
                coverprem210 = os.coverprem210,
                coverprem310 = os.coverprem310,
                coverprem410 = os.coverprem410,
                coverprem510 = os.coverprem510,
                coverprem610 = os.coverprem610,
                coverprem810 = os.coverprem810,
                coverprem910 = os.coverprem910,
                coverprem1010 = os.coverprem1010,


                insured_loadingamt1 = os.insured_loadingamt1,
                insured_loadingamt2 = os.insured_loadingamt2,
                insured_loadingamt3 = os.insured_loadingamt3,
                insured_loadingamt4 = os.insured_loadingamt4,
                insured_loadingamt5 = os.insured_loadingamt5,
                insured_loadingamt6 = os.insured_loadingamt6,

            }
           ).Skip((pageNumber - 1) * pageSize) // Skip the appropriate number of records
            .Take(pageSize) // Take only the specified page size
        .ToListAsync();
            return new List<OptimaSeniorRNE>(optimaseniorData);
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


        private async Task<IEnumerable<OptimaSeniorRNE>> CalculateOptimaSeniorPremium(IEnumerable<OptimaSeniorRNE> osRNEData)
        {
            OptimaSeniorRNE os = null;

            var columnNames = new List<string>();
            var finalPremiumValues = new List<decimal?>();
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
            foreach (var row in osRNEData)
            {
                var policNo16 = row.policy_number;
                var iDSTData = idstData.FirstOrDefault(x => x.certificate_no == policNo16);
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

                var insuredAgeOne = Convert.ToInt32(row.txt_insured_age1);//c18
                var insuredAgeTwo = Convert.ToInt32(row.txt_insured_age2);//c19            
                var insuredAgeThree = Convert.ToInt32(row.txt_insured_age3);//c20
                var insuredAgeFour = Convert.ToInt32(row.txt_insured_age4);//c21
                var insuredAgeFive = Convert.ToInt32(row.txt_insured_age5);//c22
                var insuredAgeSix = Convert.ToInt32(row.txt_insured_age6);//c23

                var zone = row.tier_type;//c11


                List<int?> ageValues = new List<int?>();
                int?[] ageStrings = {
            insuredAgeOne,
            insuredAgeTwo,
            insuredAgeThree,
            insuredAgeFour,
            insuredAgeFive,
            insuredAgeSix
        };
                foreach (var age in ageStrings)
                {
                    ageValues.Add(age); // Directly add the nullable integer
                }

                var noOfMembers = ageValues.Count(age => age > 0);
                int? eldestMember = ageValues.Max();

                // Output results
                int? count = noOfMembers;
                var numberOfMemberss = noOfMembers;

                // Discount Types

                // No Claim Discount
                string searchNoClaimDescText = "No Claim Discount";
                bool containsNoClaimDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchNoClaimDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultLoyaltyDescText = containsNoClaimDescText ? 1 : 0;
                decimal? noClaimDiscountValue = containsNoClaimDescText ? 0.05m : 0.0m;


                // Family Discount
                var policyType = row.policy_type;
                string searcFamilyDescText = "Family Discount";
                bool containsFamilyDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searcFamilyDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultSearchFamilyDescText = containsFamilyDescText ? 1 : 0;
                decimal? familyDiscountValue = GetFamilyDiscount(policyType, numberOfMemberss);

                // Long Term Discount
                var policyPeriod = row.policy_period;
                decimal longTermDiscount = GetLongTermDiscount(policyPeriod);
                var columnName = GetColumnNameForPolicyPeriod(policyPeriod);
                if (columnName == null)
                {
                    throw new ArgumentException($"Invalid policy period: {policyPeriod}");
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

                decimal? basicLoadingRateOne = iDSTData.loading_per_insured1 ?? 0;//c38
                decimal? basicLoadingRateTwo = iDSTData.loading_per_insured2 ?? 0;//c39
                decimal? basicLoadingRateThree = iDSTData.loading_per_insured3 ?? 0;//c40
                decimal? basicLoadingRateFour = iDSTData.loading_per_insured4 ?? 0;//c41
                decimal? basicLoadingRateFive = iDSTData.loading_per_insured5 ?? 0;//c42
                decimal? basicLoadingRateSix = iDSTData.loading_per_insured6 ?? 0;//c43

                // Insured Premiums
                decimal? insuredPremium1 = await GetInsuredPremium1(policyPeriod, siOne, insuredAgeOne);
                decimal? insuredPremium2 = await GetInsuredPremium2(policyPeriod, siTwo, insuredAgeTwo);
                decimal? insuredPremium3 = await GetInsuredPremium3(policyPeriod, siThree, insuredAgeThree);
                decimal? insuredPremium4 = await GetInsuredPremium4(policyPeriod, siFour, insuredAgeFour);
                decimal? insuredPremium5 = await GetInsuredPremium5(policyPeriod, siFive, insuredAgeFive);
                decimal? insuredPremium6 = await GetInsuredPremium6(policyPeriod, siSix, insuredAgeSix);

                var insuredPremiumvalues = new List<decimal?> { insuredPremium1, insuredPremium2, insuredPremium3, insuredPremium4, insuredPremium5, insuredPremium6 };

                string condition = policyType;

                decimal? basePremium = GetBasePremium(condition, insuredPremiumvalues);

                // Base Premium Loading
                // basicloadingrt1
                decimal? loadingPremInsured1 = GetBasePremLoadingInsured1(insuredPremium1, basicLoadingRateOne);
                decimal? loadingPremInsured2 = GetBasePremLoadingInsured2(insuredPremium2, basicLoadingRateTwo);
                decimal? loadingPremInsured3 = GetBasePremLoadingInsured3(insuredPremium3, basicLoadingRateThree);
                decimal? loadingPremInsured4 = GetBasePremLoadingInsured4(insuredPremium4, basicLoadingRateFour);
                decimal? loadingPremInsured5 = GetBasePremLoadingInsured5(insuredPremium5, basicLoadingRateFive);
                decimal? loadingPremInsured6 = GetBasePremLoadingInsured6(insuredPremium6, basicLoadingRateSix);
                var loadingPremvalues = new List<decimal?> { loadingPremInsured1 + loadingPremInsured2 + loadingPremInsured3 + loadingPremInsured4 + loadingPremInsured5 + loadingPremInsured6 };
                decimal? loadingPremium = loadingPremvalues.Sum();

                decimal? optimaseniorBasePremiumwithLoading = basePremium + loadingPremium;
                decimal? optimaseniorNoClaimDiscount = basePremium * noClaimDiscountValue;
                decimal? optimaseniorFamilyDiscount = basePremium * familyDiscountValue;
                decimal? optimaseniorLongTermDiscount = basePremium * longTermDiscount;
                decimal? optimaseniorFinalBasePremium = optimaseniorBasePremiumwithLoading - (optimaseniorNoClaimDiscount + optimaseniorFamilyDiscount + optimaseniorLongTermDiscount);

                decimal? netPremium = optimaseniorFinalBasePremium;
                decimal? GST = netPremium * 0.18m;
                decimal? finalPremium = netPremium + GST;
                decimal? Crosscheck = finalPremium - row.num_tot_premium;

                var record = dbContext.idst_renewal_data_rgs.FirstOrDefault(item => item.certificate_no == policNo16.ToString());

                if (Crosscheck.HasValue && record.rn_generation_status == "Reconciliation Successful")
                {
                    if (Math.Abs(Crosscheck.Value) <= 10)
                    {
                        if (record != null)
                        {
                            record.final_remarks = "RN Generation Awaited";
                            record.dispatch_status = "PDF Gen Under Process With CLICK PSS Team";
                            record.rn_generation_status = "RN Generation Awaited";
                            dbContext.idst_renewal_data_rgs.Attach(record);
                        }
                    }
                    else
                    {
                        record.final_remarks = "IT Issues";
                        record.dispatch_status = "Revised Extraction REQ From IT Team QC Failed Cases";
                        record.rn_generation_status = "IT Issue - QC Failed";
                        record.error_description = "Premium verification failed due to premium difference of more than 10 rupees";
                    }

                }
                if ((!totalsuminsured.HasValue || totalsuminsured.Value == 0) || (!cumulativebonus.HasValue || cumulativebonus.Value == 0))
                {
                    record.rn_generation_status = "IT Issue - No CB";
                    record.error_description = "CB SI cannot be zero";
                }
                record.verified_prem = netPremium;
                record.verified_gst = GST;
                record.verified_total_prem = finalPremium;

                dbContext.idst_renewal_data_rgs.Update(record);
                await dbContext.SaveChangesAsync();

                os = new OptimaSeniorRNE
                {
                    prod_code = row.prod_code,
                    prod_name = row.prod_name,
                    reference_num = row.reference_num,//proposal no in gc
                    policy_number = row.policy_number,
                    batchid = row.batchid,
                    customer_id = row.customer_id,
                    customername = row.customername,
                    txt_salutation = row.txt_salutation,
                    location_code = row.location_code,
                    txt_apartment = row.txt_apartment,
                    txt_street = row.txt_street,
                    txt_areavillage = row.txt_areavillage,
                    txt_citydistrict = row.txt_citydistrict,
                    txt_state = row.txt_state,
                    state_code = row.state_code,
                    state_regis = row.state_regis,
                    txt_pincode = row.txt_pincode,
                    txt_nationality = row.txt_nationality,
                    imdmobile = row.imdmobile,
                    txt_telephone = row.txt_telephone,
                    txt_email = row.txt_email,
                    txt_dealer_cd = row.txt_dealer_cd,//intermediary code
                    imdname = row.imdname,//intermediary name
                    verticalname = row.verticalname,
                    //ssm_name = row.ssm_name,
                    txt_family = row.txt_family,
                    isrnflag = row.isrnflag,
                    split_flag = row.split_flag,
                    isvipflag = row.isvipflag,
                    no_of_members = noOfMembers,
                    txt_insured_entrydate1 = row.txt_insured_entrydate1,// inceptiondate in gc
                    txt_insured_entrydate2 = row.txt_insured_entrydate2,
                    txt_insured_entrydate3 = row.txt_insured_entrydate3,
                    txt_insured_entrydate4 = row.txt_insured_entrydate4,
                    txt_insured_entrydate5 = row.txt_insured_entrydate5,
                    txt_insured_entrydate6 = row.txt_insured_entrydate6,

                    member_id1 = row.member_id1,
                    member_id2 = row.member_id2,
                    member_id3 = row.member_id3,
                    member_id4 = row.member_id4,
                    member_id5 = row.member_id5,
                    member_id6 = row.member_id6,

                    insured_loadingper1 = basicLoadingRateOne,
                    insured_loadingper2 = basicLoadingRateTwo,
                    insured_loadingper3 = basicLoadingRateThree,
                    insured_loadingper4 = basicLoadingRateFour,
                    insured_loadingper5 = basicLoadingRateFive,
                    insured_loadingper6 = basicLoadingRateSix,

                    insured_loadingamt1 = row.insured_loadingamt1,
                    insured_loadingamt2 = row.insured_loadingamt2,
                    insured_loadingamt3 = row.insured_loadingamt3,
                    insured_loadingamt4 = row.insured_loadingamt4,
                    insured_loadingamt5 = row.insured_loadingamt5,
                    insured_loadingamt6 = row.insured_loadingamt6,

                    txt_insuredname1 = row.txt_insuredname1,
                    txt_insuredname2 = row.txt_insuredname2,
                    txt_insuredname3 = row.txt_insuredname3,
                    txt_insuredname4 = row.txt_insuredname4,
                    txt_insuredname5 = row.txt_insuredname5,
                    txt_insuredname6 = row.txt_insuredname6,

                    txt_insured_dob1 = row.txt_insured_dob1,
                    txt_insured_dob2 = row.txt_insured_dob2,
                    txt_insured_dob3 = row.txt_insured_dob3,
                    txt_insured_dob4 = row.txt_insured_dob4,
                    txt_insured_dob5 = row.txt_insured_dob5,
                    txt_insured_dob6 = row.txt_insured_dob6,

                    txt_insured_age1 = row.txt_insured_age1,
                    txt_insured_age2 = row.txt_insured_age2,
                    txt_insured_age3 = row.txt_insured_age3,
                    txt_insured_age4 = row.txt_insured_age4,
                    txt_insured_age5 = row.txt_insured_age5,
                    txt_insured_age6 = row.txt_insured_age6,


                    txt_insured_relation1 = row.txt_insured_relation1,//coming as "string"
                    txt_insured_relation2 = row.txt_insured_relation2,
                    txt_insured_relation3 = row.txt_insured_relation3,
                    txt_insured_relation4 = row.txt_insured_relation4,
                    txt_insured_relation5 = row.txt_insured_relation5,
                    txt_insured_relation6 = row.txt_insured_relation6,

                    pre_existing_disease1 = row.pre_existing_disease1,//ped in  gc
                    pre_existing_disease2 = row.pre_existing_disease2,
                    pre_existing_disease3 = row.pre_existing_disease3,
                    pre_existing_disease4 = row.pre_existing_disease4,
                    pre_existing_disease5 = row.pre_existing_disease5,
                    pre_existing_disease6 = row.pre_existing_disease6,


                    insured_cb1 = row.insured_cb1,
                    insured_cb2 = row.insured_cb2,
                    insured_cb3 = row.insured_cb3,
                    insured_cb4 = row.insured_cb4,
                    insured_cb5 = row.insured_cb5,
                    insured_cb6 = row.insured_cb6,

                    sum_insured1 = row.sum_insured1,
                    sum_insured2 = row.sum_insured2,
                    sum_insured3 = row.sum_insured3,
                    sum_insured4 = row.sum_insured4,
                    sum_insured5 = row.sum_insured5,
                    sum_insured6 = row.sum_insured6,

                    insured_deductable1 = row.insured_deductable1,
                    insured_deductable2 = row.insured_deductable2,
                    insured_deductable3 = row.insured_deductable3,
                    insured_deductable4 = row.insured_deductable4,
                    insured_deductable5 = row.insured_deductable5,
                    insured_deductable6 = row.insured_deductable6,


                    wellness_discount1 = row.wellness_discount1, //wellness in gc
                    wellness_discount2 = row.wellness_discount2,
                    wellness_discount3 = row.wellness_discount3,
                    wellness_discount4 = row.wellness_discount4,
                    wellness_discount5 = row.wellness_discount5,
                    wellness_discount6 = row.wellness_discount6,


                    stayactive1 = row.stayactive1,
                    stayactive2 = row.stayactive2,
                    stayactive3 = row.stayactive3,
                    stayactive4 = row.stayactive4,
                    stayactive5 = row.stayactive5,
                    stayactive6 = row.stayactive6,


                    coverbaseloadingrate1 = row.coverbaseloadingrate1,//Basic Loading Rate1 in gc
                    coverbaseloadingrate2 = row.coverbaseloadingrate2,
                    coverbaseloadingrate3 = row.coverbaseloadingrate3,
                    coverbaseloadingrate4 = row.coverbaseloadingrate4,
                    coverbaseloadingrate5 = row.coverbaseloadingrate5,
                    coverbaseloadingrate6 = row.coverbaseloadingrate6,

                    health_incentive1 = row.health_incentive1,
                    health_incentive2 = row.health_incentive2,
                    health_incentive3 = row.health_incentive3,
                    health_incentive4 = row.health_incentive4,
                    health_incentive5 = row.health_incentive5,
                    health_incentive6 = row.health_incentive6,


                    fitness_discount1 = row.fitness_discount1,
                    fitness_discount2 = row.fitness_discount2,
                    fitness_discount3 = row.fitness_discount3,
                    fitness_discount4 = row.fitness_discount4,
                    fitness_discount5 = row.fitness_discount5,
                    fitness_discount6 = row.fitness_discount6,


                    reservbenefis1 = row.reservbenefis1,
                    reservbenefis2 = row.reservbenefis2,
                    reservbenefis3 = row.reservbenefis3,
                    reservbenefis4 = row.reservbenefis4,
                    reservbenefis5 = row.reservbenefis5,
                    reservbenefis6 = row.reservbenefis6,

                    insured_rb_claimamt1 = row.insured_rb_claimamt1,
                    insured_rb_claimamt2 = row.insured_rb_claimamt2,
                    insured_rb_claimamt3 = row.insured_rb_claimamt3,
                    insured_rb_claimamt4 = row.insured_rb_claimamt4,
                    insured_rb_claimamt5 = row.insured_rb_claimamt5,
                    insured_rb_claimamt6 = row.insured_rb_claimamt6,

                    preventive_hc = row.preventive_hc,
                    policy_start_date = row.policy_start_date,
                    policy_expiry_date = row.policy_expiry_date,
                    policy_type = row.policy_type,
                    policy_period = row.policy_period,
                    policyplan = row.policyplan,
                    claimcount = row.claimcount,
                    //num_tot_premium = row.num_tot_premium,

                    tier_type = row.tier_type,

                    no_claim_discount = noClaimDiscountValue,
                    family_discount = (familyDiscountValue * 100),
                    longterm_discount = (longTermDiscount * 100),

                    insured_Premium1 = insuredPremium1,
                    insured_Premium2 = insuredPremium2,
                    insured_Premium3 = insuredPremium3,
                    insured_Premium4 = insuredPremium4,
                    insured_Premium5 = insuredPremium5,
                    insured_Premium6 = insuredPremium6,
                    base_Premium = basePremium,

                    loading_Premium1 = loadingPremInsured1,
                    loading_Premium2 = loadingPremInsured2,
                    loading_Premium3 = loadingPremInsured3,
                    loading_Premium4 = loadingPremInsured4,
                    loading_Premium5 = loadingPremInsured5,
                    loading_Premium6 = loadingPremInsured6,
                    loading_Premium = loadingPremium,

                    optimaSenior_BasePremium_Loading = optimaseniorBasePremiumwithLoading,
                    optimaSenior_NoClaim_Discount = optimaseniorNoClaimDiscount,
                    optimaSenior_Family_Discount = optimaseniorFamilyDiscount,
                    optimaSenior_LongTerm_Discount = optimaseniorLongTermDiscount,
                    optimaSeniorFinalBase_Premium = optimaseniorFinalBasePremium.HasValue ? Math.Round(optimaseniorFinalBasePremium.Value, 2) : (decimal?)null,



                    net_premium = netPremium.HasValue ? Math.Round(netPremium.Value, 2) : (decimal?)null,
                    final_Premium = finalPremium.HasValue ? Math.Round(finalPremium.Value, 2) : (decimal?)null,
                    gst = GST.HasValue ? Math.Round(GST.Value, 2) : (decimal?)null,
                    cross_Check = Crosscheck.HasValue ? Math.Round(Crosscheck.Value, 2) : (decimal?)null,


                    optimasenior_total_Premium = row.num_tot_premium.HasValue ? Math.Round(row.num_tot_premium.Value, 2) : (decimal?)null,
                    optimasenior_netpremium = row.num_net_premium.HasValue ? Math.Round(row.num_net_premium.Value, 2) : (decimal?)null,
                    optimasenior_gst = row.num_service_tax.HasValue ? Math.Round(row.num_service_tax.Value, 2) : (decimal?)null

                };
            }
            return new List<OptimaSeniorRNE> { os };
        }

        public static decimal GetFamilyDiscount(string policyType, int numberOfMembers)
        {
            if (policyType == "Individual" && numberOfMembers > 1)
            {
                return 0.05m; // 10% discount
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
                "2 Year" => "two_years",
                "3 Year" => "three_years",
                _ => null,
            };
        }

        public async Task<decimal?> GetInsuredPremium1(string policyperiod, decimal? siOne, int? insuredAgeOne)
        {

            var column = GetColumnNameForPolicyPeriod(policyperiod);
            var sql = $@"
                SELECT {column}
                FROM optimasenior_baserates
                WHERE suminsured = @p0
                AND age = @p1";


            var rate = await dbContext.optimasenior_baserates
                    .FromSqlRaw(sql, siOne, insuredAgeOne)
                    .Select(r => EF.Property<decimal?>(r, column))
                .FirstOrDefaultAsync();

            return rate;

        }

        public async Task<decimal?> GetInsuredPremium2(string policyperiod, decimal? siTwo, int? insuredAgeTwo)
        {

            var column = GetColumnNameForPolicyPeriod(policyperiod);
            var sql = $@"
                SELECT {column}
                FROM optimasenior_baserates
                WHERE suminsured = @p0
                AND age = @p1";


            var rate = await dbContext.optimasenior_baserates
                    .FromSqlRaw(sql, siTwo, insuredAgeTwo)
                    .Select(r => EF.Property<decimal?>(r, column))
                .FirstOrDefaultAsync();

            return rate;

        }

        public async Task<decimal?> GetInsuredPremium3(string policyperiod, decimal? siThree, int? insuredAgeThree)
        {

            var column = GetColumnNameForPolicyPeriod(policyperiod);
            var sql = $@"
                SELECT {column}
                FROM optimasenior_baserates
                WHERE suminsured = @p0
                AND age = @p1";


            var rate = await dbContext.optimasenior_baserates
                    .FromSqlRaw(sql, siThree, insuredAgeThree)
                    .Select(r => EF.Property<decimal?>(r, column))
                .FirstOrDefaultAsync();

            return rate;

        }

        public async Task<decimal?> GetInsuredPremium4(string policyperiod, decimal? siFour, int? insuredAgeFour)
        {

            var column = GetColumnNameForPolicyPeriod(policyperiod);
            var sql = $@"
                SELECT {column}
                FROM optimasenior_baserates
                WHERE suminsured = @p0
                AND age = @p1";


            var rate = await dbContext.optimasenior_baserates
                    .FromSqlRaw(sql, siFour, insuredAgeFour)
                    .Select(r => EF.Property<decimal?>(r, column))
                .FirstOrDefaultAsync();

            return rate;

        }

        public async Task<decimal?> GetInsuredPremium5(string policyperiod, decimal? siFive, int? insuredAgeFive)
        {

            var column = GetColumnNameForPolicyPeriod(policyperiod);
            var sql = $@"
                SELECT {column}
                FROM optimasenior_baserates
                WHERE suminsured = @p0
                AND age = @p1";


            var rate = await dbContext.optimasenior_baserates
                    .FromSqlRaw(sql, siFive, insuredAgeFive)
                    .Select(r => EF.Property<decimal?>(r, column))
                .FirstOrDefaultAsync();

            return rate;

        }

        public async Task<decimal?> GetInsuredPremium6(string policyperiod, decimal? siSix, int? insuredAgeSix)
        {

            var column = GetColumnNameForPolicyPeriod(policyperiod);
            var sql = $@"
                SELECT {column}
                FROM optimasenior_baserates
                WHERE suminsured = @p0
                AND age = @p1";


            var rate = await dbContext.optimasenior_baserates
                    .FromSqlRaw(sql, siSix, insuredAgeSix)
                    .Select(r => EF.Property<decimal?>(r, column))
                .FirstOrDefaultAsync();

            return rate;

        }

        public decimal? GetBasePremium(string condition, List<decimal?> values)
        {
            decimal? sum = values.Sum();
            decimal? max = values.Max();
            decimal? difference = sum - max;
            decimal? percentageAdjustment = difference * 0.45m;

            if (condition == "Individual")
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
    }
}
