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

    public class OptimaVital
    {
        private readonly HDFCDbContext dbContext;
        private readonly ILogger<OptimaVital> _logger;
        public OptimaVital(HDFCDbContext hDFCDbContext, ILogger<OptimaVital> logger)
        {
            this.dbContext = hDFCDbContext;
            _logger = logger;
        }


        public async Task<OptimaVitalValidationResult> GetOptimaVitalValidation(string policyNo)
        {
            _logger.LogInformation("GetOptimaVitalValidation is Started!");
            //step 1 get data from gc
            IEnumerable<OptimaVitalRNE> OptimaVitalValidationResult;
            var optimavitalData = await GetOptimaVitalDataAsync(policyNo, 1, 1000);
            _logger.LogInformation("CalculateOptimaVitalPremium is Started!");
            //step 2 calculation           
            OptimaVitalValidationResult = await CalculateOptimaVitalPremium(optimavitalData);
            OptimaVitalValidationResult objOptimaVitalValidation = new OptimaVitalValidationResult
            {
                prod_code = OptimaVitalValidationResult.FirstOrDefault()?.prod_code,
                prod_name = OptimaVitalValidationResult.FirstOrDefault()?.prod_name,
                reference_num = OptimaVitalValidationResult.FirstOrDefault()?.reference_num,
                policy_number = OptimaVitalValidationResult.FirstOrDefault()?.policy_number,
                batchid = OptimaVitalValidationResult.FirstOrDefault()?.batchid,
                customer_id = OptimaVitalValidationResult.FirstOrDefault()?.customer_id,
                customername = OptimaVitalValidationResult.FirstOrDefault()?.customername,
                txt_salutation = OptimaVitalValidationResult.FirstOrDefault()?.txt_salutation,
                location_code = OptimaVitalValidationResult.FirstOrDefault()?.location_code,
                txt_apartment = OptimaVitalValidationResult.FirstOrDefault()?.txt_apartment,
                txt_street = OptimaVitalValidationResult.FirstOrDefault()?.txt_street,
                txt_areavillage = OptimaVitalValidationResult.FirstOrDefault()?.txt_areavillage,
                txt_citydistrict = OptimaVitalValidationResult.FirstOrDefault()?.txt_citydistrict,
                txt_state = OptimaVitalValidationResult.FirstOrDefault()?.txt_state,
                state_code = OptimaVitalValidationResult.FirstOrDefault()?.state_code,
                state_regis = OptimaVitalValidationResult.FirstOrDefault()?.state_regis,
                txt_pincode = OptimaVitalValidationResult.FirstOrDefault()?.txt_pincode,
                txt_nationality = OptimaVitalValidationResult.FirstOrDefault()?.txt_nationality,
                imdmobile = OptimaVitalValidationResult.FirstOrDefault()?.imdmobile,
                txt_telephone = OptimaVitalValidationResult.FirstOrDefault()?.txt_telephone,
                txt_email = OptimaVitalValidationResult.FirstOrDefault()?.txt_email,
                txt_dealer_cd = OptimaVitalValidationResult.FirstOrDefault()?.txt_dealer_cd,//intermediary code
                imdname = OptimaVitalValidationResult.FirstOrDefault()?.imdname,//intermediary name
                verticalname = OptimaVitalValidationResult.FirstOrDefault()?.verticalname,
                //ssm_name = OptimaVitalValidationResult.FirstOrDefault()?.ssm_name,
                txt_family = OptimaVitalValidationResult.FirstOrDefault()?.txt_family,
                isrnflag = OptimaVitalValidationResult.FirstOrDefault()?.isrnflag,//chk
                split_flag = OptimaVitalValidationResult.FirstOrDefault()?.split_flag,
                isvipflag = OptimaVitalValidationResult.FirstOrDefault()?.isvipflag,//chk
                no_of_members = OptimaVitalValidationResult.FirstOrDefault()?.no_of_members,
                txt_insured_entrydate1 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_entrydate1,// inceptiondate in gc
                txt_insured_entrydate2 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_entrydate2,
                txt_insured_entrydate3 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_entrydate3,
                txt_insured_entrydate4 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_entrydate4,
                txt_insured_entrydate5 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_entrydate5,
                txt_insured_entrydate6 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_entrydate6,

                member_id1 = OptimaVitalValidationResult.FirstOrDefault()?.member_id1,
                member_id2 = OptimaVitalValidationResult.FirstOrDefault()?.member_id2,
                member_id3 = OptimaVitalValidationResult.FirstOrDefault()?.member_id3,
                member_id4 = OptimaVitalValidationResult.FirstOrDefault()?.member_id4,
                member_id5 = OptimaVitalValidationResult.FirstOrDefault()?.member_id5,
                member_id6 = OptimaVitalValidationResult.FirstOrDefault()?.member_id6,

                insured_loadingper1 = OptimaVitalValidationResult.FirstOrDefault()?.insured_loadingper1,
                insured_loadingper2 = OptimaVitalValidationResult.FirstOrDefault()?.insured_loadingper2,
                insured_loadingper3 = OptimaVitalValidationResult.FirstOrDefault()?.insured_loadingper3,
                insured_loadingper4 = OptimaVitalValidationResult.FirstOrDefault()?.insured_loadingper4,
                insured_loadingper5 = OptimaVitalValidationResult.FirstOrDefault()?.insured_loadingper5,
                insured_loadingper6 = OptimaVitalValidationResult.FirstOrDefault()?.insured_loadingper6,

                insured_loadingamt1 = OptimaVitalValidationResult.FirstOrDefault()?.insured_loadingamt1,
                insured_loadingamt2 = OptimaVitalValidationResult.FirstOrDefault()?.insured_loadingamt2,
                insured_loadingamt3 = OptimaVitalValidationResult.FirstOrDefault()?.insured_loadingamt3,
                insured_loadingamt4 = OptimaVitalValidationResult.FirstOrDefault()?.insured_loadingamt4,
                insured_loadingamt5 = OptimaVitalValidationResult.FirstOrDefault()?.insured_loadingamt5,
                insured_loadingamt6 = OptimaVitalValidationResult.FirstOrDefault()?.insured_loadingamt6,

                txt_insuredname1 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insuredname1,
                txt_insuredname2 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insuredname2,
                txt_insuredname3 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insuredname3,
                txt_insuredname4 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insuredname4,
                txt_insuredname5 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insuredname5,
                txt_insuredname6 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insuredname6,

                txt_insured_dob1 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_dob1,
                txt_insured_dob2 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_dob2,
                txt_insured_dob3 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_dob3,
                txt_insured_dob4 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_dob4,
                txt_insured_dob5 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_dob5,
                txt_insured_dob6 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_dob6,

                txt_insured_age1 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_age1,
                txt_insured_age2 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_age2,
                txt_insured_age3 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_age3,
                txt_insured_age4 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_age4,
                txt_insured_age5 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_age5,
                txt_insured_age6 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_age6,


                txt_insured_relation1 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_relation1,//coming as "string"
                txt_insured_relation2 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_relation2,
                txt_insured_relation3 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_relation3,
                txt_insured_relation4 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_relation4,
                txt_insured_relation5 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_relation5,
                txt_insured_relation6 = OptimaVitalValidationResult.FirstOrDefault()?.txt_insured_relation6,

                pre_existing_disease1 = OptimaVitalValidationResult.FirstOrDefault()?.pre_existing_disease1,//ped in  gc
                pre_existing_disease2 = OptimaVitalValidationResult.FirstOrDefault()?.pre_existing_disease2,
                pre_existing_disease3 = OptimaVitalValidationResult.FirstOrDefault()?.pre_existing_disease3,
                pre_existing_disease4 = OptimaVitalValidationResult.FirstOrDefault()?.pre_existing_disease4,
                pre_existing_disease5 = OptimaVitalValidationResult.FirstOrDefault()?.pre_existing_disease5,
                pre_existing_disease6 = OptimaVitalValidationResult.FirstOrDefault()?.pre_existing_disease6,


                insured_cb1 = OptimaVitalValidationResult.FirstOrDefault()?.insured_cb1,
                insured_cb2 = OptimaVitalValidationResult.FirstOrDefault()?.insured_cb2,
                insured_cb3 = OptimaVitalValidationResult.FirstOrDefault()?.insured_cb3,
                insured_cb4 = OptimaVitalValidationResult.FirstOrDefault()?.insured_cb4,
                insured_cb5 = OptimaVitalValidationResult.FirstOrDefault()?.insured_cb5,
                insured_cb6 = OptimaVitalValidationResult.FirstOrDefault()?.insured_cb6,

                sum_insured1 = OptimaVitalValidationResult.FirstOrDefault()?.sum_insured1,
                sum_insured2 = OptimaVitalValidationResult.FirstOrDefault()?.sum_insured2,
                sum_insured3 = OptimaVitalValidationResult.FirstOrDefault()?.sum_insured3,
                sum_insured4 = OptimaVitalValidationResult.FirstOrDefault()?.sum_insured4,
                sum_insured5 = OptimaVitalValidationResult.FirstOrDefault()?.sum_insured5,
                sum_insured6 = OptimaVitalValidationResult.FirstOrDefault()?.sum_insured6,

                insured_deductable1 = OptimaVitalValidationResult.FirstOrDefault()?.insured_deductable1,
                insured_deductable2 = OptimaVitalValidationResult.FirstOrDefault()?.insured_deductable2,
                insured_deductable3 = OptimaVitalValidationResult.FirstOrDefault()?.insured_deductable3,
                insured_deductable4 = OptimaVitalValidationResult.FirstOrDefault()?.insured_deductable4,
                insured_deductable5 = OptimaVitalValidationResult.FirstOrDefault()?.insured_deductable5,
                insured_deductable6 = OptimaVitalValidationResult.FirstOrDefault()?.insured_deductable6,


                wellness_discount1 = OptimaVitalValidationResult.FirstOrDefault()?.wellness_discount1, //wellness in gc
                wellness_discount2 = OptimaVitalValidationResult.FirstOrDefault()?.wellness_discount2,
                wellness_discount3 = OptimaVitalValidationResult.FirstOrDefault()?.wellness_discount3,
                wellness_discount4 = OptimaVitalValidationResult.FirstOrDefault()?.wellness_discount4,
                wellness_discount5 = OptimaVitalValidationResult.FirstOrDefault()?.wellness_discount5,
                wellness_discount6 = OptimaVitalValidationResult.FirstOrDefault()?.wellness_discount6,


                stayactive1 = OptimaVitalValidationResult.FirstOrDefault()?.stayactive1,
                stayactive2 = OptimaVitalValidationResult.FirstOrDefault()?.stayactive2,
                stayactive3 = OptimaVitalValidationResult.FirstOrDefault()?.stayactive3,
                stayactive4 = OptimaVitalValidationResult.FirstOrDefault()?.stayactive4,
                stayactive5 = OptimaVitalValidationResult.FirstOrDefault()?.stayactive5,
                stayactive6 = OptimaVitalValidationResult.FirstOrDefault()?.stayactive6,


                coverbaseloadingrate1 = OptimaVitalValidationResult.FirstOrDefault()?.coverbaseloadingrate1,//Basic Loading Rate1 in gc
                coverbaseloadingrate2 = OptimaVitalValidationResult.FirstOrDefault()?.coverbaseloadingrate2,
                coverbaseloadingrate3 = OptimaVitalValidationResult.FirstOrDefault()?.coverbaseloadingrate3,
                coverbaseloadingrate4 = OptimaVitalValidationResult.FirstOrDefault()?.coverbaseloadingrate4,
                coverbaseloadingrate5 = OptimaVitalValidationResult.FirstOrDefault()?.coverbaseloadingrate5,
                coverbaseloadingrate6 = OptimaVitalValidationResult.FirstOrDefault()?.coverbaseloadingrate6,

                health_incentive1 = OptimaVitalValidationResult.FirstOrDefault()?.health_incentive1,
                health_incentive2 = OptimaVitalValidationResult.FirstOrDefault()?.health_incentive2,
                health_incentive3 = OptimaVitalValidationResult.FirstOrDefault()?.health_incentive3,
                health_incentive4 = OptimaVitalValidationResult.FirstOrDefault()?.health_incentive4,
                health_incentive5 = OptimaVitalValidationResult.FirstOrDefault()?.health_incentive5,
                health_incentive6 = OptimaVitalValidationResult.FirstOrDefault()?.health_incentive6,


                fitness_discount1 = OptimaVitalValidationResult.FirstOrDefault()?.fitness_discount1,
                fitness_discount2 = OptimaVitalValidationResult.FirstOrDefault()?.fitness_discount2,
                fitness_discount3 = OptimaVitalValidationResult.FirstOrDefault()?.fitness_discount3,
                fitness_discount4 = OptimaVitalValidationResult.FirstOrDefault()?.fitness_discount4,
                fitness_discount5 = OptimaVitalValidationResult.FirstOrDefault()?.fitness_discount5,
                fitness_discount6 = OptimaVitalValidationResult.FirstOrDefault()?.fitness_discount6,


                reservbenefis1 = OptimaVitalValidationResult.FirstOrDefault()?.reservbenefis1,
                reservbenefis2 = OptimaVitalValidationResult.FirstOrDefault()?.reservbenefis2,
                reservbenefis3 = OptimaVitalValidationResult.FirstOrDefault()?.reservbenefis3,
                reservbenefis4 = OptimaVitalValidationResult.FirstOrDefault()?.reservbenefis4,
                reservbenefis5 = OptimaVitalValidationResult.FirstOrDefault()?.reservbenefis5,
                reservbenefis6 = OptimaVitalValidationResult.FirstOrDefault()?.reservbenefis6,

                insured_rb_claimamt1 = OptimaVitalValidationResult.FirstOrDefault()?.insured_rb_claimamt1,
                insured_rb_claimamt2 = OptimaVitalValidationResult.FirstOrDefault()?.insured_rb_claimamt2,
                insured_rb_claimamt3 = OptimaVitalValidationResult.FirstOrDefault()?.insured_rb_claimamt3,
                insured_rb_claimamt4 = OptimaVitalValidationResult.FirstOrDefault()?.insured_rb_claimamt4,
                insured_rb_claimamt5 = OptimaVitalValidationResult.FirstOrDefault()?.insured_rb_claimamt5,
                insured_rb_claimamt6 = OptimaVitalValidationResult.FirstOrDefault()?.insured_rb_claimamt6,

                preventive_hc = OptimaVitalValidationResult.FirstOrDefault()?.preventive_hc,
                policy_start_date = OptimaVitalValidationResult.FirstOrDefault()?.policy_start_date,
                policy_expiry_date = OptimaVitalValidationResult.FirstOrDefault()?.policy_expiry_date,
                policy_type = OptimaVitalValidationResult.FirstOrDefault()?.policy_type,
                policy_period = OptimaVitalValidationResult.FirstOrDefault()?.policy_period,
                policyplan = OptimaVitalValidationResult.FirstOrDefault()?.policyplan,
                claimcount = OptimaVitalValidationResult.FirstOrDefault()?.claimcount,
                //num_tot_premium = OptimaVitalValidationResult.FirstOrDefault()?.num_tot_premium,

                tier_type = OptimaVitalValidationResult.FirstOrDefault()?.tier_type,

                longterm_discount = OptimaVitalValidationResult.FirstOrDefault()?.longterm_discount,

                insured_Premium1 = OptimaVitalValidationResult.FirstOrDefault()?.insured_Premium1,
                insured_Premium2 = OptimaVitalValidationResult.FirstOrDefault()?.insured_Premium2,
                insured_Premium3 = OptimaVitalValidationResult.FirstOrDefault()?.insured_Premium3,
                insured_Premium4 = OptimaVitalValidationResult.FirstOrDefault()?.insured_Premium4,
                insured_Premium5 = OptimaVitalValidationResult.FirstOrDefault()?.insured_Premium5,
                insured_Premium6 = OptimaVitalValidationResult.FirstOrDefault()?.insured_Premium6,
                base_Premium = OptimaVitalValidationResult.FirstOrDefault()?.base_Premium,

                loading_Premium1 = OptimaVitalValidationResult.FirstOrDefault()?.loading_Premium1,
                loading_Premium2 = OptimaVitalValidationResult.FirstOrDefault()?.loading_Premium2,
                loading_Premium3 = OptimaVitalValidationResult.FirstOrDefault()?.loading_Premium3,
                loading_Premium4 = OptimaVitalValidationResult.FirstOrDefault()?.loading_Premium4,
                loading_Premium5 = OptimaVitalValidationResult.FirstOrDefault()?.loading_Premium5,
                loading_Premium6 = OptimaVitalValidationResult.FirstOrDefault()?.loading_Premium6,
                base_loading_Premium = OptimaVitalValidationResult.FirstOrDefault()?.base_loading_Premium,

                optimaVital_FinalBasePremium_Loading = OptimaVitalValidationResult.FirstOrDefault()?.optimaVital_FinalBasePremium_Loading,
                optimaVital_LongTerm_Discount = OptimaVitalValidationResult.FirstOrDefault()?.optimaVital_LongTerm_Discount,
                optimaVital_LongTerm_Amount = OptimaVitalValidationResult.FirstOrDefault()?.optimaVital_LongTerm_Amount,


                net_premium = OptimaVitalValidationResult.FirstOrDefault()?.net_premium.HasValue == true
                             ? (decimal?)Math.Round(OptimaVitalValidationResult.FirstOrDefault().net_premium.Value, 2)
                             : (decimal?)null,
                final_Premium = OptimaVitalValidationResult.FirstOrDefault()?.final_Premium.HasValue == true
                             ? (decimal?)Math.Round(OptimaVitalValidationResult.FirstOrDefault().final_Premium.Value, 2)
                             : (decimal?)null,
                gst = OptimaVitalValidationResult.FirstOrDefault()?.gst.HasValue == true
                             ? (decimal?)Math.Round(OptimaVitalValidationResult.FirstOrDefault().gst.Value, 2)
                             : (decimal?)null,
                cross_Check = OptimaVitalValidationResult.FirstOrDefault()?.cross_Check.HasValue == true
                             ? (decimal?)Math.Round(OptimaVitalValidationResult.FirstOrDefault().cross_Check.Value, 2)
                             : (decimal?)null,


                optimavital_total_Premium = OptimaVitalValidationResult.FirstOrDefault()?.optimavital_total_Premium.HasValue == true
                             ? (decimal?)Math.Round(OptimaVitalValidationResult.FirstOrDefault().optimavital_total_Premium.Value, 2)
                             : (decimal?)null,
                optimavital_netpremium = OptimaVitalValidationResult.FirstOrDefault()?.optimavital_netpremium.HasValue == true
                             ? (decimal?)Math.Round(OptimaVitalValidationResult.FirstOrDefault().optimavital_netpremium.Value, 2)
                             : (decimal?)null,
                optimavital_gst = OptimaVitalValidationResult.FirstOrDefault()?.optimavital_gst.HasValue == true
                             ? (decimal?)Math.Round(OptimaVitalValidationResult.FirstOrDefault().optimavital_gst.Value, 2)
                             : (decimal?)null,

            };

            if (objOptimaVitalValidation?.policy_number == null)
            {
                Console.WriteLine("Policy number not found.");
            }
            var record = await dbContext.rne_calculated_cover_rg.AsNoTracking().FirstOrDefaultAsync(item => item.policy_number == policyNo.ToString());

            if (objOptimaVitalValidation != null)
            {
                var no_of_members = objOptimaVitalValidation.no_of_members;
                var policy_number = objOptimaVitalValidation.policy_number;
                var reference_number = objOptimaVitalValidation.reference_num;
                var newRecord = new List<rne_calculated_cover_rg>();
                for (int i = 1; i <= no_of_members; i++)
                {
                    var sumInsured = Convert.ToDecimal(objOptimaVitalValidation.GetType().GetProperty($"sum_insured{i}")?.GetValue(objOptimaVitalValidation));
                    var basePremium = Convert.ToDecimal(objOptimaVitalValidation.GetType().GetProperty($"insured_Premium{i}")?.GetValue(objOptimaVitalValidation));
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
                        riskname = objOptimaVitalValidation.GetType().GetProperty($"insuredname_{i}")?.GetValue(objOptimaVitalValidation)?.ToString(),
                        covername = "Baic OptimaVital"
                    };
                    newRecord.Add(newRecords);
                }
                dbContext.rne_calculated_cover_rg.AddRange(newRecord);
                await dbContext.SaveChangesAsync();
            }

            return objOptimaVitalValidation;
        }

        private async Task<IEnumerable<OptimaVitalRNE>> GetOptimaVitalDataAsync(string policyNo, int pageNumber, int pageSize)
        {
            // Query to get data from GC to calculate optima vital premium
            var optimavitalData = await (
             from ov in dbContext.rne_healthtab
             join ovidst in dbContext.idst_renewal_data_rgs on ov.policy_number equals ovidst.certificate_no
             where (ov.policy_number == policyNo)
             select new OptimaVitalRNE
             {
                 prod_code = ov.prod_code,
                 batchid = ov.batchid,
                 prod_name = ov.prod_name,
                 reference_num = ov.reference_num,
                 //proposal_no = ov.proposal_no,
                 //policy_no = gc.policy_no,
                 policy_number = ov.policy_number,
                 split_flag = ov.split_flag,
                 customer_id = ov.customer_id,
                 customername = ov.customername,
                 vertical_name = ov.verticale_name,//psm code in old gc mapping
                 verticalname = ov.verticalname,//psm name in old gc mapping
                 policy_start_date = ov.policy_start_date,
                 policy_expiry_date = ov.policy_expiry_date,
                 policy_period = ov.policy_period,
                 tier_type = ov.tier_type,//tier in old gc mapping
                 policyplan = ov.policyplan,
                 policy_type = ov.policy_type,
                 txt_family = ov.txt_family,//family size in old gc mapping
                 claimcount = ov.claimcount,
                 num_tot_premium = ov.num_tot_premium,
                 num_net_premium = ov.num_net_premium,
                 num_service_tax = ov.num_service_tax,
                 //gst in old gc mapping                                    
                 isrnflag = ov.isrnflag,
                 isvipflag = ov.isvipflag,
                 //first_inception_date_1 = gc.first_inception_date_1,
                 //first_inception_date_2 = gc.first_inception_date_2,
                 //first_inception_date_3 = gc.first_inception_date_3,
                 //first_inception_date_4 = gc.first_inception_date_4,
                 //first_inception_date_5 = gc.first_inception_date_5,
                 //first_inception_date_6 = gc.first_inception_date_6,

                 coverbaseloadingrate1 = ov.coverbaseloadingrate1,
                 coverbaseloadingrate2 = ov.coverbaseloadingrate2,
                 coverbaseloadingrate3 = ov.coverbaseloadingrate3,
                 coverbaseloadingrate4 = ov.coverbaseloadingrate4,
                 coverbaseloadingrate5 = ov.coverbaseloadingrate5,
                 coverbaseloadingrate6 = ov.coverbaseloadingrate6,


                 insured_loadingper1 = ovidst.loading_per_insured1,
                 insured_loadingper2 = ovidst.loading_per_insured2,
                 insured_loadingper3 = ovidst.loading_per_insured3,
                 insured_loadingper4 = ovidst.loading_per_insured4,
                 insured_loadingper5 = ovidst.loading_per_insured5,
                 insured_loadingper6 = ovidst.loading_per_insured6,

                 txt_insuredname1 = ov.txt_insuredname1,
                 txt_insuredname2 = ov.txt_insuredname2,
                 txt_insuredname3 = ov.txt_insuredname3,
                 txt_insuredname4 = ov.txt_insuredname4,
                 txt_insuredname5 = ov.txt_insuredname5,
                 txt_insuredname6 = ov.txt_insuredname6,

                 txt_insured_dob1 = ov.txt_insured_dob1,
                 txt_insured_dob2 = ov.txt_insured_dob2,
                 txt_insured_dob3 = ov.txt_insured_dob3,
                 txt_insured_dob4 = ov.txt_insured_dob4,
                 txt_insured_dob5 = ov.txt_insured_dob5,
                 txt_insured_dob6 = ov.txt_insured_dob6,

                 txt_insured_relation1 = ov.txt_insured_relation1,
                 txt_insured_relation2 = ov.txt_insured_relation2,
                 txt_insured_relation3 = ov.txt_insured_relation3,
                 txt_insured_relation4 = ov.txt_insured_relation4,
                 txt_insured_relation5 = ov.txt_insured_relation5,
                 txt_insured_relation6 = ov.txt_insured_relation6,

                 txt_insured_age1 = ov.txt_insured_age1,
                 txt_insured_age2 = ov.txt_insured_age2,
                 txt_insured_age3 = ov.txt_insured_age3,
                 txt_insured_age4 = ov.txt_insured_age4,
                 txt_insured_age5 = ov.txt_insured_age5,
                 txt_insured_age6 = ov.txt_insured_age6,

                 txt_insured_gender1 = ov.txt_insured_gender1,
                 txt_insured_gender2 = ov.txt_insured_gender2,
                 txt_insured_gender3 = ov.txt_insured_gender3,
                 txt_insured_gender4 = ov.txt_insured_gender4,
                 txt_insured_gender5 = ov.txt_insured_gender5,
                 txt_insured_gender6 = ov.txt_insured_gender6,

                 member_id1 = ov.member_id1,
                 member_id2 = ov.member_id2,
                 member_id3 = ov.member_id3,
                 member_id4 = ov.member_id4,
                 member_id5 = ov.member_id5,
                 member_id6 = ov.member_id6,

                 nominee_name1 = ov.nominee_name1,
                 nominee_name2 = ov.nominee_name2,
                 nominee_name3 = ov.nominee_name3,
                 nominee_name4 = ov.nominee_name4,
                 nominee_name5 = ov.nominee_name5,
                 nominee_name6 = ov.nominee_name6,

                 nominee_relationship1 = ov.nominee_relationship1,
                 nominee_relationship2 = ov.nominee_relationship2,
                 nominee_relationship3 = ov.nominee_relationship3,
                 nominee_relationship4 = ov.nominee_relationship4,
                 nominee_relationship5 = ov.nominee_relationship5,
                 nominee_relationship6 = ov.nominee_relationship6,

                 pollddesc1 = ov.pollddesc1,
                 pollddesc2 = ov.pollddesc2,
                 pollddesc3 = ov.pollddesc3,
                 pollddesc4 = ov.pollddesc4,
                 pollddesc5 = ov.pollddesc5,

                 sum_insured1 = ov.sum_insured1,
                 sum_insured2 = ov.sum_insured2,
                 sum_insured3 = ov.sum_insured3,
                 sum_insured4 = ov.sum_insured4,
                 sum_insured5 = ov.sum_insured5,
                 sum_insured6 = ov.sum_insured6,

                 insured_cb1 = ov.insured_cb1,
                 insured_cb2 = ov.insured_cb2,
                 insured_cb3 = ov.insured_cb3,
                 insured_cb4 = ov.insured_cb4,
                 insured_cb5 = ov.insured_cb5,
                 insured_cb6 = ov.insured_cb6,

                 premium_insured1 = ov.premium_insured1,
                 premium_insured2 = ov.premium_insured2,
                 premium_insured3 = ov.premium_insured3,
                 premium_insured4 = ov.premium_insured4,
                 premium_insured5 = ov.premium_insured5,
                 premium_insured6 = ov.premium_insured6,

                 insured_deductable1 = ov.insured_deductable1,//deductable insured in old gc mapping
                 insured_deductable2 = ov.insured_deductable2,
                 insured_deductable3 = ov.insured_deductable3,
                 insured_deductable4 = ov.insured_deductable4,
                 insured_deductable5 = ov.insured_deductable5,
                 insured_deductable6 = ov.insured_deductable6,

                 covername11 = ov.covername11,
                 covername12 = ov.covername12,
                 covername13 = ov.covername13,
                 covername14 = ov.covername14,
                 covername15 = ov.covername15,
                 covername16 = ov.covername16,
                 covername17 = ov.covername17,
                 covername18 = ov.covername18,
                 covername19 = ov.covername19,
                 covername21 = ov.covername21,
                 covername22 = ov.covername22,
                 covername23 = ov.covername23,
                 covername24 = ov.covername24,
                 covername25 = ov.covername25,
                 covername26 = ov.covername26,
                 covername27 = ov.covername27,
                 covername28 = ov.covername28,
                 covername29 = ov.covername29,
                 covername31 = ov.covername31,
                 covername32 = ov.covername32,
                 covername33 = ov.covername33,
                 covername34 = ov.covername34,
                 covername35 = ov.covername35,
                 covername36 = ov.covername36,
                 covername37 = ov.covername37,
                 covername38 = ov.covername38,
                 covername39 = ov.covername39,
                 covername41 = ov.covername41,
                 covername42 = ov.covername42,
                 covername43 = ov.covername43,
                 covername44 = ov.covername44,
                 covername45 = ov.covername45,
                 covername46 = ov.covername46,
                 covername47 = ov.covername47,
                 covername48 = ov.covername48,
                 covername49 = ov.covername49,
                 covername51 = ov.covername51,
                 covername52 = ov.covername52,
                 covername53 = ov.covername53,
                 covername54 = ov.covername54,
                 covername55 = ov.covername55,
                 covername56 = ov.covername56,
                 covername57 = ov.covername57,
                 covername58 = ov.covername58,
                 covername59 = ov.covername59,
                 covername61 = ov.covername61,
                 covername62 = ov.covername62,
                 covername63 = ov.covername63,
                 covername64 = ov.covername64,
                 covername65 = ov.covername65,
                 covername66 = ov.covername66,
                 covername67 = ov.covername67,
                 covername68 = ov.covername68,
                 covername69 = ov.covername69,
                 covername71 = ov.covername71,
                 covername72 = ov.covername72,
                 covername73 = ov.covername73,
                 covername74 = ov.covername74,
                 covername75 = ov.covername75,
                 covername76 = ov.covername76,
                 covername77 = ov.covername77,
                 covername78 = ov.covername78,
                 covername79 = ov.covername79,
                 covername81 = ov.covername81,
                 covername82 = ov.covername82,
                 covername83 = ov.covername83,
                 covername84 = ov.covername84,
                 covername85 = ov.covername85,
                 covername86 = ov.covername86,
                 covername87 = ov.covername87,
                 covername88 = ov.covername88,
                 covername89 = ov.covername89,
                 covername91 = ov.covername91,
                 covername92 = ov.covername92,
                 covername93 = ov.covername93,
                 covername94 = ov.covername94,
                 covername95 = ov.covername95,
                 covername96 = ov.covername96,
                 covername97 = ov.covername97,
                 covername98 = ov.covername98,
                 covername99 = ov.covername99,
                 covername101 = ov.covername101,
                 covername102 = ov.covername102,
                 covername103 = ov.covername103,
                 covername104 = ov.covername104,
                 covername105 = ov.covername105,
                 covername106 = ov.covername106,
                 covername107 = ov.covername107,
                 covername108 = ov.covername108,
                 covername109 = ov.covername109,
                 covername110 = ov.covername110,
                 covername210 = ov.covername210,
                 covername310 = ov.covername310,
                 covername410 = ov.covername410,
                 covername510 = ov.covername510,
                 covername610 = ov.covername610,
                 covername710 = ov.covername710,
                 covername810 = ov.covername810,
                 covername910 = ov.covername910,
                 covername1010 = ov.covername1010,

                 coversi11 = ov.coversi11,
                 coversi12 = ov.coversi12,
                 coversi13 = ov.coversi13,
                 coversi14 = ov.coversi14,
                 coversi15 = ov.coversi15,
                 coversi16 = ov.coversi16,
                 coversi17 = ov.coversi17,
                 coversi18 = ov.coversi18,
                 coversi19 = ov.coversi19,
                 coversi21 = ov.coversi21,
                 coversi22 = ov.coversi22,
                 coversi23 = ov.coversi23,
                 coversi24 = ov.coversi24,
                 coversi25 = ov.coversi25,
                 coversi26 = ov.coversi26,
                 coversi27 = ov.coversi27,
                 coversi28 = ov.coversi28,
                 coversi29 = ov.coversi29,
                 coversi31 = ov.coversi31,
                 coversi32 = ov.coversi32,
                 coversi33 = ov.coversi33,
                 coversi34 = ov.coversi34,
                 coversi35 = ov.coversi35,
                 coversi36 = ov.coversi36,
                 coversi37 = ov.coversi37,
                 coversi38 = ov.coversi38,
                 coversi39 = ov.coversi39,
                 coversi41 = ov.coversi41,
                 coversi42 = ov.coversi42,
                 coversi43 = ov.coversi43,
                 coversi44 = ov.coversi44,
                 coversi45 = ov.coversi46,
                 coversi47 = ov.coversi47,
                 coversi48 = ov.coversi48,
                 coversi49 = ov.coversi49,
                 coversi51 = ov.coversi51,
                 coversi52 = ov.coversi52,
                 coversi53 = ov.coversi53,
                 coversi54 = ov.coversi54,
                 coversi55 = ov.coversi55,
                 coversi56 = ov.coversi56,
                 coversi57 = ov.coversi57,
                 coversi58 = ov.coversi58,
                 coversi59 = ov.coversi59,
                 coversi61 = ov.coversi61,
                 coversi62 = ov.coversi62,
                 coversi63 = ov.coversi63,
                 coversi64 = ov.coversi64,
                 coversi65 = ov.coversi65,
                 coversi66 = ov.coversi66,
                 coversi67 = ov.coversi67,
                 coversi68 = ov.coversi68,
                 coversi69 = ov.coversi69,
                 coversi71 = ov.coversi71,
                 coversi72 = ov.coversi72,
                 coversi73 = ov.coversi73,
                 coversi74 = ov.coversi74,
                 coversi75 = ov.coversi75,
                 coversi76 = ov.coversi76,
                 coversi77 = ov.coversi77,
                 coversi78 = ov.coversi78,
                 coversi79 = ov.coversi79,
                 coversi81 = ov.coversi81,
                 coversi82 = ov.coversi82,
                 coversi83 = ov.coversi83,
                 coversi84 = ov.coversi84,
                 coversi85 = ov.coversi85,
                 coversi86 = ov.coversi86,
                 coversi87 = ov.coversi87,
                 coversi88 = ov.coversi88,
                 coversi89 = ov.coversi89,
                 coversi91 = ov.coversi91,
                 coversi92 = ov.coversi92,
                 coversi93 = ov.coversi93,
                 coversi94 = ov.coversi94,
                 coversi95 = ov.coversi95,
                 coversi96 = ov.coversi96,
                 coversi97 = ov.coversi97,
                 coversi98 = ov.coversi98,
                 coversi99 = ov.coversi99,
                 coversi101 = ov.coversi101,
                 coversi102 = ov.coversi102,
                 coversi103 = ov.coversi103,
                 coversi104 = ov.coversi104,
                 coversi105 = ov.coversi105,
                 coversi106 = ov.coversi106,
                 coversi107 = ov.coversi107,
                 coversi108 = ov.coversi108,
                 coversi109 = ov.coversi109,
                 coversi210 = ov.coversi210,
                 coversi310 = ov.coversi310,
                 coversi410 = ov.coversi410,
                 coversi510 = ov.coversi510,
                 coversi610 = ov.coversi610,
                 coversi810 = ov.coversi810,
                 coversi910 = ov.coversi910,
                 coversi1010 = ov.coversi1010,

                 coverprem11 = ov.coverprem11,
                 coverprem12 = ov.coverprem12,
                 coverprem13 = ov.coverprem13,
                 coverprem14 = ov.coverprem14,
                 coverprem15 = ov.coverprem15,
                 coverprem16 = ov.coverprem16,
                 coverprem17 = ov.coverprem17,
                 coverprem18 = ov.coverprem18,
                 coverprem19 = ov.coverprem19,
                 coverprem21 = ov.coverprem21,
                 coverprem22 = ov.coverprem22,
                 coverprem23 = ov.coverprem23,
                 coverprem24 = ov.coverprem24,
                 coverprem25 = ov.coverprem25,
                 coverprem26 = ov.coverprem26,
                 coverprem27 = ov.coverprem27,
                 coverprem28 = ov.coverprem28,
                 coverprem29 = ov.coverprem29,
                 coverprem31 = ov.coverprem31,
                 coverprem32 = ov.coverprem32,
                 coverprem33 = ov.coverprem33,
                 coverprem34 = ov.coverprem34,
                 coverprem35 = ov.coverprem35,
                 coverprem36 = ov.coverprem36,
                 coverprem37 = ov.coverprem37,
                 coverprem38 = ov.coverprem38,
                 coverprem39 = ov.coverprem39,
                 coverprem41 = ov.coverprem41,
                 coverprem42 = ov.coverprem42,
                 coverprem43 = ov.coverprem43,
                 coverprem44 = ov.coverprem44,
                 coverprem45 = ov.coverprem46,
                 coverprem47 = ov.coverprem47,
                 coverprem48 = ov.coverprem48,
                 coverprem49 = ov.coverprem49,
                 coverprem51 = ov.coverprem51,
                 coverprem52 = ov.coverprem52,
                 coverprem53 = ov.coverprem53,
                 coverprem54 = ov.coverprem54,
                 coverprem55 = ov.coverprem55,
                 coverprem56 = ov.coverprem56,
                 coverprem57 = ov.coverprem57,
                 coverprem58 = ov.coverprem58,
                 coverprem59 = ov.coverprem59,
                 coverprem61 = ov.coverprem61,
                 coverprem62 = ov.coverprem62,
                 coverprem63 = ov.coverprem63,
                 coverprem64 = ov.coverprem64,
                 coverprem65 = ov.coverprem65,
                 coverprem66 = ov.coverprem66,
                 coverprem67 = ov.coverprem67,
                 coverprem68 = ov.coverprem68,
                 coverprem69 = ov.coverprem69,
                 coverprem71 = ov.coverprem71,
                 coverprem72 = ov.coverprem72,
                 coverprem73 = ov.coverprem73,
                 coverprem74 = ov.coverprem74,
                 coverprem75 = ov.coverprem75,
                 coverprem76 = ov.coverprem76,
                 coverprem77 = ov.coverprem77,
                 coverprem78 = ov.coverprem78,
                 coverprem79 = ov.coverprem79,
                 coverprem81 = ov.coverprem81,
                 coverprem82 = ov.coverprem82,
                 coverprem83 = ov.coverprem83,
                 coverprem84 = ov.coverprem84,
                 coverprem85 = ov.coverprem85,
                 coverprem86 = ov.coverprem86,
                 coverprem87 = ov.coverprem87,
                 coverprem88 = ov.coverprem88,
                 coverprem89 = ov.coverprem89,
                 coverprem91 = ov.coverprem91,
                 coverprem92 = ov.coverprem92,
                 coverprem93 = ov.coverprem93,
                 coverprem94 = ov.coverprem94,
                 coverprem95 = ov.coverprem95,
                 coverprem96 = ov.coverprem96,
                 coverprem97 = ov.coverprem97,
                 coverprem98 = ov.coverprem98,
                 coverprem99 = ov.coverprem99,
                 coverprem101 = ov.coverprem101,
                 coverprem102 = ov.coverprem102,
                 coverprem103 = ov.coverprem103,
                 coverprem104 = ov.coverprem104,
                 coverprem105 = ov.coverprem105,
                 coverprem106 = ov.coverprem106,
                 coverprem107 = ov.coverprem107,
                 coverprem108 = ov.coverprem108,
                 coverprem109 = ov.coverprem109,
                 coverprem210 = ov.coverprem210,
                 coverprem310 = ov.coverprem310,
                 coverprem410 = ov.coverprem410,
                 coverprem510 = ov.coverprem510,
                 coverprem610 = ov.coverprem610,
                 coverprem810 = ov.coverprem810,
                 coverprem910 = ov.coverprem910,
                 coverprem1010 = ov.coverprem1010,


                 insured_loadingamt1 = ov.insured_loadingamt1,
                 insured_loadingamt2 = ov.insured_loadingamt2,
                 insured_loadingamt3 = ov.insured_loadingamt3,
                 insured_loadingamt4 = ov.insured_loadingamt4,
                 insured_loadingamt5 = ov.insured_loadingamt5,
                 insured_loadingamt6 = ov.insured_loadingamt6,

             }
             ).Skip((pageNumber - 1) * pageSize) // Skip the appropriate number of records
            .Take(pageSize) // Take only the specified page size
        .ToListAsync();
            return new List<OptimaVitalRNE>(optimavitalData);
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


        private async Task<IEnumerable<OptimaVitalRNE>> CalculateOptimaVitalPremium(IEnumerable<OptimaVitalRNE> ovRNEData)
        {
            OptimaVitalRNE ov = null;

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
            foreach (var row in ovRNEData)
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
                var policyType = row.policy_type;
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
                decimal? baseLoadingPremium = loadingPremvalues.Sum();

                decimal? optimavitalFinalBasePremiumLoading = basePremium + baseLoadingPremium;
                decimal? optimavitalLongTermDiscount = GetOVLongTermDiscount(policyPeriod);
                decimal? optimavitalLongTermDiscountAmount = optimavitalFinalBasePremiumLoading * optimavitalLongTermDiscount;

                decimal? netPremium = optimavitalFinalBasePremiumLoading - optimavitalLongTermDiscountAmount;
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

                ov = new OptimaVitalRNE
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
                    isrnflag = row.isrnflag,//chk
                    split_flag = row.split_flag,
                    isvipflag = row.isvipflag,//chk
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
                    base_loading_Premium = baseLoadingPremium,

                    optimaVital_FinalBasePremium_Loading = optimavitalFinalBasePremiumLoading,
                    optimaVital_LongTerm_Discount = optimavitalLongTermDiscount,
                    optimaVital_LongTerm_Amount = optimavitalLongTermDiscountAmount,


                    net_premium = netPremium.HasValue ? Math.Round(netPremium.Value, 2) : (decimal?)null,
                    final_Premium = finalPremium.HasValue ? Math.Round(finalPremium.Value, 2) : (decimal?)null,
                    gst = GST.HasValue ? Math.Round(GST.Value, 2) : (decimal?)null,
                    cross_Check = Crosscheck.HasValue ? Math.Round(Crosscheck.Value, 2) : (decimal?)null,


                    optimavital_total_Premium = row.num_tot_premium.HasValue ? Math.Round(row.num_tot_premium.Value, 2) : (decimal?)null,
                    optimavital_netpremium = row.num_net_premium.HasValue ? Math.Round(row.num_net_premium.Value, 2) : (decimal?)null,
                    optimavital_gst = row.num_service_tax.HasValue ? Math.Round(row.num_service_tax.Value, 2) : (decimal?)null

                };
            }
            return new List<OptimaVitalRNE> { ov };
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
                FROM optimavital_baserates
                WHERE si = @p0
                AND age = @p1";


            var rate = await dbContext.optimavital_baserates
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
                FROM optimavital_baserates
                WHERE si = @p0
                AND age = @p1";


            var rate = await dbContext.optimavital_baserates
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
                FROM optimavital_baserates
                WHERE si = @p0
                AND age = @p1";


            var rate = await dbContext.optimavital_baserates
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
                FROM optimavital_baserates
                WHERE si = @p0
                AND age = @p1";


            var rate = await dbContext.optimavital_baserates
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
                FROM optimavital_baserates
                WHERE si = @p0
                AND age = @p1";


            var rate = await dbContext.optimavital_baserates
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
                FROM optimavital_baserates
                WHERE si = @p0
                AND age = @p1";


            var rate = await dbContext.optimavital_baserates
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

        public static decimal GetOVLongTermDiscount(string policyPeriod)
        {
            if (policyPeriod == "2 Years")
            {
                return 0.075m; // 7.5%
            }
            else
            {
                return 0.00m; // 0%
            }
        }
    }
}
