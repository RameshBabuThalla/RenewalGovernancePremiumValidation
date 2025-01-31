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
using System.Collections;
using static RenewalGovernancePremiumValidation.BussinessLogic.OptimaSenior;
using Serilog;
using System.Configuration;
using System.Data.Common;
namespace RenewalGovernancePremiumValidation.BussinessLogic
{
    public class AarogyaSanjeevani
    {
        private readonly HDFCDbContext dbContext;
        private readonly ILogger<AarogyaSanjeevani> _logger;
        public AarogyaSanjeevani(HDFCDbContext hDFCDbContext, ILogger<AarogyaSanjeevani> logger)
        {
           // this.dbContext = hDFCDbContext;
            _logger = logger;
        }

        public async Task<AarogyaSanjeevaniValidationResult> GetAarogyaSanjeevaniValidation(string policyNo, Dictionary<string, Hashtable> baseRateHashTable, Dictionary<string, Hashtable> relations)
        {
            List<AarogyaSanjeevaniRNE> asRNEData;
            IEnumerable<AarogyaSanjeevaniRNE> arogyasanjeevaniRNE = Enumerable.Empty<AarogyaSanjeevaniRNE>();
            IEnumerable<AarogyaSanjeevaniRNE> AarogyaSanjeevaniValidationResult;
            asRNEData = await GetGCAarogyaSanjeevaniDataAsync(policyNo);
            arogyasanjeevaniRNE = await CalculateAarogyaSanjeevaniPremium(policyNo,asRNEData, baseRateHashTable, relations);

            AarogyaSanjeevaniValidationResult objAarogyaSanjeevaniValidation = new AarogyaSanjeevaniValidationResult
            {
                prod_code = asRNEData.FirstOrDefault()?.prod_code,
                prod_name = asRNEData.FirstOrDefault()?.prod_name,
                policy_number = asRNEData.FirstOrDefault()?.policy_number,
                txt_family = asRNEData.FirstOrDefault()?.txt_family,
                insured_loadingper1 = asRNEData.FirstOrDefault()?.loading_per_insured_1,
                insured_loadingper2 = asRNEData.FirstOrDefault()?.loading_per_insured_2,
                insured_loadingper3 = asRNEData.FirstOrDefault()?.loading_per_insured_3,
                insured_loadingper4 = asRNEData.FirstOrDefault()?.loading_per_insured_4,
                insured_loadingper5 = asRNEData.FirstOrDefault()?.loading_per_insured_5,
                insured_loadingper6 = asRNEData.FirstOrDefault()?.loading_per_insured_6,
                insured_loadingper7 = asRNEData.FirstOrDefault()?.loading_per_insured_7,
                insured_loadingper8 = asRNEData.FirstOrDefault()?.loading_per_insured_8,
                insured_loadingper9 = asRNEData.FirstOrDefault()?.loading_per_insured_9,
                insured_loadingper10 = asRNEData.FirstOrDefault()?.loading_per_insured_10,
                insured_loadingper11 = asRNEData.FirstOrDefault()?.loading_per_insured_11,
                insured_loadingper12 = asRNEData.FirstOrDefault()?.loading_per_insured_12,
                txt_insured_dob1 = asRNEData.FirstOrDefault()?.txt_insured_dob1,
                txt_insured_dob2 = asRNEData.FirstOrDefault()?.txt_insured_dob2,
                txt_insured_dob3 = asRNEData.FirstOrDefault()?.txt_insured_dob3,
                txt_insured_dob4 = asRNEData.FirstOrDefault()?.txt_insured_dob4,
                txt_insured_dob5 = asRNEData.FirstOrDefault()?.txt_insured_dob5,
                txt_insured_dob6 = asRNEData.FirstOrDefault()?.txt_insured_dob6,
                txt_insured_dob7 = asRNEData.FirstOrDefault()?.txt_insured_dob7,
                txt_insured_dob8 = asRNEData.FirstOrDefault()?.txt_insured_dob8,
                txt_insured_dob9 = asRNEData.FirstOrDefault()?.txt_insured_dob9,
                txt_insured_dob10 = asRNEData.FirstOrDefault()?.txt_insured_dob10,
                txt_insured_dob11 = asRNEData.FirstOrDefault()?.txt_insured_dob11,
                txt_insured_dob12 = asRNEData.FirstOrDefault()?.txt_insured_dob12,
                txt_insured_age1 = asRNEData.FirstOrDefault()?.txt_insured_age1,
                txt_insured_age2 = asRNEData.FirstOrDefault()?.txt_insured_age2,
                txt_insured_age3 = asRNEData.FirstOrDefault()?.txt_insured_age3,
                txt_insured_age4 = asRNEData.FirstOrDefault()?.txt_insured_age4,
                txt_insured_age5 = asRNEData.FirstOrDefault()?.txt_insured_age5,
                txt_insured_age6 = asRNEData.FirstOrDefault()?.txt_insured_age6,
                txt_insured_age7 = asRNEData.FirstOrDefault()?.txt_insured_age7,
                txt_insured_age8 = asRNEData.FirstOrDefault()?.txt_insured_age8,
                txt_insured_age9 = asRNEData.FirstOrDefault()?.txt_insured_age9,
                txt_insured_age10 = asRNEData.FirstOrDefault()?.txt_insured_age10,
                txt_insured_age11 = asRNEData.FirstOrDefault()?.txt_insured_age11,
                txt_insured_age12 = asRNEData.FirstOrDefault()?.txt_insured_age12,
                txt_insured_relation1 = asRNEData.FirstOrDefault()?.txt_insured_relation1,
                txt_insured_relation2 = asRNEData.FirstOrDefault()?.txt_insured_relation2,
                txt_insured_relation3 = asRNEData.FirstOrDefault()?.txt_insured_relation3,
                txt_insured_relation4 = asRNEData.FirstOrDefault()?.txt_insured_relation4,
                txt_insured_relation5 = asRNEData.FirstOrDefault()?.txt_insured_relation5,
                txt_insured_relation6 = asRNEData.FirstOrDefault()?.txt_insured_relation6,
                txt_insured_relation7 = asRNEData.FirstOrDefault()?.txt_insured_relation7,
                txt_insured_relation8 = asRNEData.FirstOrDefault()?.txt_insured_relation8,
                txt_insured_relation9 = asRNEData.FirstOrDefault()?.txt_insured_relation9,
                txt_insured_relation10 = asRNEData.FirstOrDefault()?.txt_insured_relation10,
                txt_insured_relation11 = asRNEData.FirstOrDefault()?.txt_insured_relation11,
                txt_insured_relation12 = asRNEData.FirstOrDefault()?.txt_insured_relation12,
                insured_relation_tag_1 = asRNEData.FirstOrDefault()?.insured_relation_tag_1,
                insured_relation_tag_2 = asRNEData.FirstOrDefault()?.insured_relation_tag_2,
                insured_relation_tag_3 = asRNEData.FirstOrDefault()?.insured_relation_tag_3,
                insured_relation_tag_4 = asRNEData.FirstOrDefault()?.insured_relation_tag_4,
                insured_relation_tag_5 = asRNEData.FirstOrDefault()?.insured_relation_tag_5,
                insured_relation_tag_6 = asRNEData.FirstOrDefault()?.insured_relation_tag_6,
                insured_relation_tag_7 = asRNEData.FirstOrDefault()?.insured_relation_tag_7,
                insured_relation_tag_8 = asRNEData.FirstOrDefault()?.insured_relation_tag_8,
                insured_relation_tag_9 = asRNEData.FirstOrDefault()?.insured_relation_tag_9,
                insured_relation_tag_10 = asRNEData.FirstOrDefault()?.insured_relation_tag_10,
                insured_relation_tag_11 = asRNEData.FirstOrDefault()?.insured_relation_tag_11,
                insured_relation_tag_12 = asRNEData.FirstOrDefault()?.insured_relation_tag_12,
                insured_cb1 = asRNEData.FirstOrDefault()?.insured_cb1,
                insured_cb2 = asRNEData.FirstOrDefault()?.insured_cb2,
                insured_cb3 = asRNEData.FirstOrDefault()?.insured_cb3,
                insured_cb4 = asRNEData.FirstOrDefault()?.insured_cb4,
                insured_cb5 = asRNEData.FirstOrDefault()?.insured_cb5,
                insured_cb6 = asRNEData.FirstOrDefault()?.insured_cb6,
                insured_cb7 = asRNEData.FirstOrDefault()?.insured_cb7,
                insured_cb8 = asRNEData.FirstOrDefault()?.insured_cb8,
                insured_cb9 = asRNEData.FirstOrDefault()?.insured_cb9,
                insured_cb10 = asRNEData.FirstOrDefault()?.insured_cb10,
                insured_cb11 = asRNEData.FirstOrDefault()?.insured_cb11,
                insured_cb12 = asRNEData.FirstOrDefault()?.insured_cb12,
                sum_insured1 = asRNEData.FirstOrDefault()?.sum_insured1,
                sum_insured2 = asRNEData.FirstOrDefault()?.sum_insured2,
                sum_insured3 = asRNEData.FirstOrDefault()?.sum_insured3,
                sum_insured4 = asRNEData.FirstOrDefault()?.sum_insured4,
                sum_insured5 = asRNEData.FirstOrDefault()?.sum_insured5,
                sum_insured6 = asRNEData.FirstOrDefault()?.sum_insured6,
                sum_insured7 = asRNEData.FirstOrDefault()?.sum_insured7,
                sum_insured8 = asRNEData.FirstOrDefault()?.sum_insured8,
                sum_insured9 = asRNEData.FirstOrDefault()?.sum_insured9,
                sum_insured10 = asRNEData.FirstOrDefault()?.sum_insured10,
                sum_insured11 = asRNEData.FirstOrDefault()?.sum_insured11,
                sum_insured12 = asRNEData.FirstOrDefault()?.sum_insured12,
                coverbaseloadingrate1 = asRNEData.FirstOrDefault()?.coverbaseloadingrate1,
                coverbaseloadingrate2 = asRNEData.FirstOrDefault()?.coverbaseloadingrate2,
                coverbaseloadingrate3 = asRNEData.FirstOrDefault()?.coverbaseloadingrate3,
                coverbaseloadingrate4 = asRNEData.FirstOrDefault()?.coverbaseloadingrate4,
                coverbaseloadingrate5 = asRNEData.FirstOrDefault()?.coverbaseloadingrate5,
                coverbaseloadingrate6 = asRNEData.FirstOrDefault()?.coverbaseloadingrate6,
                coverbaseloadingrate7 = asRNEData.FirstOrDefault()?.coverbaseloadingrate7,
                coverbaseloadingrate8 = asRNEData.FirstOrDefault()?.coverbaseloadingrate8,
                coverbaseloadingrate9 = asRNEData.FirstOrDefault()?.coverbaseloadingrate9,
                coverbaseloadingrate10 = asRNEData.FirstOrDefault()?.coverbaseloadingrate10,
                coverbaseloadingrate11 = asRNEData.FirstOrDefault()?.coverbaseloadingrate11,
                coverbaseloadingrate12 = asRNEData.FirstOrDefault()?.coverbaseloadingrate12,
                policy_start_date = asRNEData.FirstOrDefault()?.policy_start_date,
                policy_expiry_date = asRNEData.FirstOrDefault()?.policy_expiry_date,
                policy_type = asRNEData.FirstOrDefault()?.policy_type,
                policy_period = asRNEData.FirstOrDefault()?.policy_period,
                policyplan = asRNEData.FirstOrDefault()?.policyplan,
                claimcount = asRNEData.FirstOrDefault()?.claimcount,
                no_of_members = asRNEData.FirstOrDefault()?.no_of_members,
                eldest_member = asRNEData.FirstOrDefault()?.eldest_member,
                tier_type = asRNEData.FirstOrDefault()?.tier_type,
                employee_discount = asRNEData.FirstOrDefault()?.employee_discount,
                online_discount = asRNEData.FirstOrDefault()?.online_discount,
                loyalty_discount = asRNEData.FirstOrDefault()?.loyalty_discount,
                family_discount = asRNEData.FirstOrDefault()?.family_discount,
                rural_discount = asRNEData.FirstOrDefault()?.rural_discount,
                capping_discount = asRNEData.FirstOrDefault()?.capping_discount,
                base_premium_1 = asRNEData.FirstOrDefault()?.base_premium_1,
                base_premium_2 = asRNEData.FirstOrDefault()?.base_premium_2,
                base_premium_3 = asRNEData.FirstOrDefault()?.base_premium_3,
                base_premium_4 = asRNEData.FirstOrDefault()?.base_premium_4,
                base_premium_5 = asRNEData.FirstOrDefault()?.base_premium_5,
                base_premium_6 = asRNEData.FirstOrDefault()?.base_premium_6,
                base_premium_7 = asRNEData.FirstOrDefault()?.base_premium_7,
                base_premium_8 = asRNEData.FirstOrDefault()?.base_premium_8,
                base_premium_9 = asRNEData.FirstOrDefault()?.base_premium_9,
                base_premium_10 = asRNEData.FirstOrDefault()?.base_premium_10,
                base_premium_11 = asRNEData.FirstOrDefault()?.base_premium_11,
                base_premium_12 = asRNEData.FirstOrDefault()?.base_premium_12,
                base_premium = asRNEData.FirstOrDefault()?.base_premium,
                loading_prem_1 = asRNEData.FirstOrDefault()?.loading_prem1,
                loading_prem_2 = asRNEData.FirstOrDefault()?.loading_prem2,
                loading_prem_3 = asRNEData.FirstOrDefault()?.loading_prem3,
                loading_prem_4 = asRNEData.FirstOrDefault()?.loading_prem4,
                loading_prem_5 = asRNEData.FirstOrDefault()?.loading_prem5,
                loading_prem_6 = asRNEData.FirstOrDefault()?.loading_prem6,
                loading_prem_7 = asRNEData.FirstOrDefault()?.loading_prem7,
                loading_prem_8 = asRNEData.FirstOrDefault()?.loading_prem8,
                loading_prem_9 = asRNEData.FirstOrDefault()?.loading_prem9,
                loading_prem_10 = asRNEData.FirstOrDefault()?.loading_prem10,
                loading_prem_11 = asRNEData.FirstOrDefault()?.loading_prem11,
                loading_prem_12 = asRNEData.FirstOrDefault()?.loading_prem12,
                loading_premium = asRNEData.FirstOrDefault()?.loading_prem,
                basepremiumLoading = asRNEData.FirstOrDefault()?.basepremiumLoading,
                baseAndLoading_LoyaltyDiscount = asRNEData.FirstOrDefault()?.baseAndLoading_LoyaltyDiscount,
                baseAndLoading_EmployeeDiscount = asRNEData.FirstOrDefault()?.baseAndLoading_EmployeeDiscount,
                baseAndLoading_OnlineDiscount = asRNEData.FirstOrDefault()?.baseAndLoading_OnlineDiscount,
                baseAndLoading_FamilyDiscount = asRNEData.FirstOrDefault()?.baseAndLoading_FamilyDiscount,
                baseAndLoading_RuralDiscount = asRNEData.FirstOrDefault()?.baseAndLoading_RuralDiscount,
                baseAndLoading_CapppingDiscount = asRNEData.FirstOrDefault()?.baseAndLoading_CapppingDiscount,
                netPremium = asRNEData.FirstOrDefault()?.netPremium.HasValue == true
             ? (decimal?)Math.Round(asRNEData.FirstOrDefault().netPremium.Value, 2)
             : (decimal?)0,
                finalPremium = asRNEData.FirstOrDefault()?.finalPremium.HasValue == true
             ? (decimal?)Math.Round(asRNEData.FirstOrDefault().finalPremium.Value, 2)
             : (decimal?)0,
                GST = asRNEData.FirstOrDefault()?.GST.HasValue == true
             ? (decimal?)Math.Round(asRNEData.FirstOrDefault().GST.Value, 2)
             : (decimal?)0,
                crossCheck = asRNEData.FirstOrDefault()?.crossCheck.HasValue == true
             ? (decimal?)Math.Round(asRNEData.FirstOrDefault().crossCheck.Value, 2)
             : (decimal?)0,
                aarogyasanjeevani_total_Premium = asRNEData.FirstOrDefault()?.aarogyasanjeevani_total_Premium.HasValue == true
             ? (decimal?)Math.Round(asRNEData.FirstOrDefault().aarogyasanjeevani_total_Premium.Value, 2)
             : (decimal?)0,
                aarogyasanjeevani_netpremium = asRNEData.FirstOrDefault()?.aarogyasanjeevani_netpremium.HasValue == true
             ? (decimal?)Math.Round(asRNEData.FirstOrDefault().aarogyasanjeevani_netpremium.Value, 2)
             : (decimal?)0,
                aarogyasanjeevani_GST = asRNEData.FirstOrDefault()?.aarogyasanjeevani_GST.HasValue == true
             ? (decimal?)Math.Round(asRNEData.FirstOrDefault().aarogyasanjeevani_GST.Value, 2)
             : (decimal?)0,

            };
            decimal? crosscheck1 = arogyasanjeevaniRNE.FirstOrDefault()?.crossCheck;
            decimal? netPremium = arogyasanjeevaniRNE.FirstOrDefault()?.netPremium ?? 0;
            decimal? finalPremium = arogyasanjeevaniRNE.FirstOrDefault()?.finalPremium ?? 0;
            decimal? gst = arogyasanjeevaniRNE.FirstOrDefault()?.GST ?? 0;
            if (objAarogyaSanjeevaniValidation?.policy_number == null)
            {
                Console.WriteLine("Policy number not found.");
            }
            string? connectionString = ConfigurationManager.ConnectionStrings["PostgresDb"].ConnectionString;
            using (IDbConnection dbConnection = new NpgsqlConnection(connectionString))
            {
                dbConnection.Open();
                var record = dbConnection.QueryFirstOrDefault<premium_validation>(
                    "SELECT certificate_no FROM ins.premium_validation WHERE certificate_no = @CertificateNo",
                    new { CertificateNo = policyNo.ToString() });

                if (record == null)
                {
                    decimal? crosscheck1Value = crosscheck1.GetValueOrDefault(0);
                    if (asRNEData.FirstOrDefault()?.insured_cb1 == string.Empty && asRNEData.FirstOrDefault()?.insured_cb1 == null)
                    {
                        var insertQuery = @"
                                    INSERT INTO ins.premium_validation (certificate_no, verified_prem, verified_gst, verified_total_prem, rn_generation_status, final_remarks, dispatch_status)
                                    VALUES (@CertificateNo, @VerifiedPrem, @VerifiedGst, @VerifiedTotalPrem, 'IT Issue - No CB', 'CB SI cannot be zero')";

                        dbConnection.Execute(insertQuery, new
                        {
                            CertificateNo = policyNo,
                            VerifiedPrem = netPremium,
                            VerifiedGst = gst,
                            VerifiedTotalPrem = finalPremium
                        });
                    }
                    else
                    {
                        try
                        {
                            await HandleCrosschecksAndUpdateStatus(policyNo, asRNEData.FirstOrDefault(), crosscheck1Value, netPremium, finalPremium, gst);
                        }
                        catch (DbUpdateConcurrencyException ex)
                        {
                            var entry = ex.Entries.Single();
                            await entry.ReloadAsync();
                        }
                        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "40P01")
                        {

                        }
                    }

                }

                if (objAarogyaSanjeevaniValidation != null)
                {
                    var no_of_members = arogyasanjeevaniRNE.FirstOrDefault()?.no_of_members;
                    var policy_number = objAarogyaSanjeevaniValidation.policy_number;
                    var reference_number = objAarogyaSanjeevaniValidation.reference_num;
                    var newRecord = new List<rne_calculated_cover_rg>();
                    for (int i = 1; i <= no_of_members; i++)
                    {
                        var sumInsured = Convert.ToDecimal(objAarogyaSanjeevaniValidation.GetType().GetProperty($"sum_insured{i}")?.GetValue(objAarogyaSanjeevaniValidation));
                        var basePremium = Convert.ToDecimal(objAarogyaSanjeevaniValidation.GetType().GetProperty($"base_premium_{i}")?.GetValue(objAarogyaSanjeevaniValidation));
                        if (no_of_members > 1 && i >= 2 && i <= 12)
                        {
                            basePremium *= 0.45m;
                        }
                        var newRecords = new rne_calculated_cover_rg
                        {
                            policy_number = policy_number,
                            referencenum = reference_number,
                            suminsured = sumInsured,
                            premium = basePremium,
                            riskname = objAarogyaSanjeevaniValidation.GetType().GetProperty($"insuredname_{i}")?.GetValue(objAarogyaSanjeevaniValidation)?.ToString(),
                            covername = "Baic AarogyaSanjeevani"
                        };
                        newRecord.Add(newRecords);
                    }
                    var insertQuery = @"
                                INSERT INTO ins.rne_calculated_cover_rg (policy_number, referencenum, suminsured, premium, totalpremium, riskname, covername)
                                VALUES (@policy_number, @referencenum, @suminsured, @premium, @totalpremium, @riskname, @covername);
                                ";

                    await dbConnection.ExecuteAsync(insertQuery, newRecord).ConfigureAwait(false);
                }
            }
            return objAarogyaSanjeevaniValidation;
        }
        private async Task<List<AarogyaSanjeevaniRNE>> GetGCAarogyaSanjeevaniDataAsync(string policyNo)
        {
            string? connectionString = ConfigurationManager.ConnectionStrings["PostgresDb"].ConnectionString;
            string sqlQuery = @"
        SELECT 
 aas.prod_code,
 aas.batchid,
 aas.prod_name,
aas.reference_num,
aas.policy_number,
aas.split_flag,
aas.customer_id,
aas.customername,
aas.verticalname, 
aas.policy_start_date,
aas.policy_expiry_date,
aas.policy_period,
aas.tier_type, 
aas.policyplan,
aas.policy_type,
aas.txt_family, 
aas.claimcount,
aas.num_tot_premium,
aas.num_net_premium,
aas.num_service_tax,
aas.coverbaseloadingrate1,
aas.coverbaseloadingrate2,
aas.coverbaseloadingrate3,
aas.coverbaseloadingrate4,
aas.coverbaseloadingrate5,
aas.coverbaseloadingrate6,
aas.coverbaseloadingrate7,
aas.coverbaseloadingrate8,
aas.coverbaseloadingrate9,
aas.coverbaseloadingrate10,
aas.coverbaseloadingrate11,
aas.coverbaseloadingrate12,

asidst.loading_per_insured1,
asidst.loading_per_insured2,
asidst.loading_per_insured3,
asidst.loading_per_insured4,
asidst.loading_per_insured5,
asidst.loading_per_insured6,
asidst.loading_per_insured7,
asidst.loading_per_insured8,
asidst.loading_per_insured9,
asidst.loading_per_insured10,
asidst.loading_per_insured11,
asidst.loading_per_insured12,

 aas.insured_loadingamt1,
aas.insured_loadingamt2,
aas.insured_loadingamt3,
aas.insured_loadingamt4,
aas.insured_loadingamt5,
aas.insured_loadingamt6,
aas.insured_loadingamt7,
aas.insured_loadingamt8,
aas.insured_loadingamt9,
aas.insured_loadingamt10,
aas.insured_loadingamt11,
aas.insured_loadingamt12,

 aas.txt_insured_relation1,
aas.txt_insured_relation2,
aas.txt_insured_relation3,
aas.txt_insured_relation4,
aas.txt_insured_relation5,
aas.txt_insured_relation6,
aas.txt_insured_relation7,
aas.txt_insured_relation8,
aas.txt_insured_relation9,
aas.txt_insured_relation10,
aas.txt_insured_relation11,
aas.txt_insured_relation12,

aas.txt_insured_age1,
aas.txt_insured_age2,
aas.txt_insured_age3,
aas.txt_insured_age4,
aas.txt_insured_age5,
aas.txt_insured_age6,
aas.txt_insured_age7,
aas.txt_insured_age8,
aas.txt_insured_age9,
aas.txt_insured_age10,
aas.txt_insured_age11,
aas.txt_insured_age12,

 aas.pollddesc1,
aas.pollddesc2,
aas.pollddesc3,
aas.pollddesc4,
aas.pollddesc5,

aas.sum_insured1,
aas.sum_insured2,
aas.sum_insured3,
aas.sum_insured4,
aas.sum_insured5,
aas.sum_insured6,
aas.sum_insured7,
aas.sum_insured8,
aas.sum_insured9,
aas.sum_insured10,
aas.sum_insured11,
aas.sum_insured12,

aas.insured_cb1,
aas.insured_cb2,
aas.insured_cb3,
aas.insured_cb4,
aas.insured_cb5,
aas.insured_cb6,
aas.insured_cb7,
aas.insured_cb8,
aas.insured_cb9,
aas.insured_cb10,
aas.insured_cb11,
aas.insured_cb12,

aas.covername11,
aas.covername12,
aas.covername13,
aas.covername14,
aas.covername15,
aas.covername16,
aas.covername17,
aas.covername18,
aas.covername19,
aas.covername21,
aas.covername22,
aas.covername23,
aas.covername24,
aas.covername25,
aas.covername26,
aas.covername27,
aas.covername28,
aas.covername29,
aas.covername31,
aas.covername32,
aas.covername33,
aas.covername34,
aas.covername35,
aas.covername36,
aas.covername37,
aas.covername38,
aas.covername39,
aas.covername41,
aas.covername42,
aas.covername43,
aas.covername44,
aas.covername45,
aas.covername46,
aas.covername47,
aas.covername48,
aas.covername49,
aas.covername51,
aas.covername52,
aas.covername53,
aas.covername54,
aas.covername55,
aas.covername56,
aas.covername57,
aas.covername58,
aas.covername59,
aas.covername61,
aas.covername62,
aas.covername63,
aas.covername64,
aas.covername65,
aas.covername66,
aas.covername67,
aas.covername68,
aas.covername69,
aas.covername71,
aas.covername72,
aas.covername73,
aas.covername74,
aas.covername75,
aas.covername76,
aas.covername77,
aas.covername78,
aas.covername79,
aas.covername81,
aas.covername82,
aas.covername83,
aas.covername84,
aas.covername85,
aas.covername86,
aas.covername87,
aas.covername88,
aas.covername89,
aas.covername91,
aas.covername92,
aas.covername93,
aas.covername94,
aas.covername95,
aas.covername96,
aas.covername97,
aas.covername98,
aas.covername99,
aas.covername101,
aas.covername102,
aas.covername103,
aas.covername104,
aas.covername105,
aas.covername106,
aas.covername107,
aas.covername108,
aas.covername109,
aas.covername110,
aas.covername210,
aas.covername310,
aas.covername410,
aas.covername510,
aas.covername610,
aas.covername710,
aas.covername810,
aas.covername910,
aas.covername1010,

aas.coversi11,
aas.coversi12,
aas.coversi13,
aas.coversi14,
aas.coversi15,
aas.coversi16,
aas.coversi17,
aas.coversi18,
aas.coversi19,
aas.coversi21,
aas.coversi22,
aas.coversi23,
aas.coversi24,
aas.coversi25,
aas.coversi26,
aas.coversi27,
aas.coversi28,
aas.coversi29,
aas.coversi31,
aas.coversi32,
aas.coversi33,
aas.coversi34,
aas.coversi35,
aas.coversi36,
aas.coversi37,
aas.coversi38,
aas.coversi39,
aas.coversi41,
aas.coversi42,
aas.coversi43,
aas.coversi44,
aas.coversi45,
aas.coversi46,
aas.coversi47,
aas.coversi48,
aas.coversi49,
aas.coversi51,
aas.coversi52,
aas.coversi53,
aas.coversi54,
aas.coversi55,
aas.coversi56,
aas.coversi57,
aas.coversi58,
aas.coversi59,
aas.coversi61,
aas.coversi62,
aas.coversi63,
aas.coversi64,
aas.coversi65,
aas.coversi66,
aas.coversi67,
aas.coversi68,
aas.coversi69,
aas.coversi71,
aas.coversi72,
aas.coversi73,
aas.coversi74,
aas.coversi75,
aas.coversi76,
aas.coversi77,
aas.coversi78,
aas.coversi79,
aas.coversi81,
aas.coversi82,
aas.coversi83,
aas.coversi84,
aas.coversi85,
aas.coversi86,
aas.coversi87,
aas.coversi88,
aas.coversi89,
aas.coversi91,
aas.coversi92,
aas.coversi93,
aas.coversi94,
aas.coversi95,
aas.coversi96,
aas.coversi97,
aas.coversi98,
aas.coversi99,
aas.coversi101,
aas.coversi102,
aas.coversi103,
aas.coversi104,
aas.coversi105,
aas.coversi106,
aas.coversi107,
aas.coversi108,
aas.coversi109,
aas.coversi210,
aas.coversi310,
aas.coversi410,
aas.coversi510,
aas.coversi610,
aas.coversi810,
aas.coversi910,
aas.coversi1010,

aas.coverprem11,
aas.coverprem12,
aas.coverprem13,
aas.coverprem14,
aas.coverprem15,
aas.coverprem16,
aas.coverprem17,
aas.coverprem18,
aas.coverprem19,
aas.coverprem21,
aas.coverprem22,
aas.coverprem23,
aas.coverprem24,
aas.coverprem25,
aas.coverprem26,
aas.coverprem27,
aas.coverprem28,
aas.coverprem29,
aas.coverprem31,
aas.coverprem32,
aas.coverprem33,
aas.coverprem34,
aas.coverprem35,
aas.coverprem36,
aas.coverprem37,
aas.coverprem38,
aas.coverprem39,
aas.coverprem41,
aas.coverprem42,
aas.coverprem43,
aas.coverprem44,
aas.coverprem46,
aas.coverprem47,
aas.coverprem48,
aas.coverprem49,
aas.coverprem51,
aas.coverprem52,
aas.coverprem53,
aas.coverprem54,
aas.coverprem55,
aas.coverprem56,
aas.coverprem57,
aas.coverprem58,
aas.coverprem59,
aas.coverprem61,
aas.coverprem62,
aas.coverprem63,
aas.coverprem64,
aas.coverprem65,
aas.coverprem66,
aas.coverprem67,
aas.coverprem68,
aas.coverprem69,
aas.coverprem71,
aas.coverprem72,
aas.coverprem73,
aas.coverprem74,
aas.coverprem75,
aas.coverprem76,
aas.coverprem77,
aas.coverprem78,
aas.coverprem79,
aas.coverprem81,
aas.coverprem82,
aas.coverprem83,
aas.coverprem84,
aas.coverprem85,
aas.coverprem86,
aas.coverprem87,
aas.coverprem88,
aas.coverprem89,
aas.coverprem91,
aas.coverprem92,
aas.coverprem93,
aas.coverprem94,
aas.coverprem95,
aas.coverprem96,
aas.coverprem97,
aas.coverprem98,
aas.coverprem99,
aas.coverprem101,
aas.coverprem102,
aas.coverprem103,
aas.coverprem104,
aas.coverprem105,
aas.coverprem106,
aas.coverprem107,
aas.coverprem108,
aas.coverprem109,
aas.coverprem210,
aas.coverprem310,
aas.coverprem410,
aas.coverprem510,
aas.coverprem610,
aas.coverprem810,
aas.coverprem910,
aas.coverprem1010,


aas.coverloadingrate11,
aas.coverloadingrate12,
aas.coverloadingrate13,
aas.coverloadingrate14,
aas.coverloadingrate15,
aas.coverloadingrate16,
aas.coverloadingrate17,
aas.coverloadingrate18,
aas.coverloadingrate19,
aas.coverloadingrate21,
aas.coverloadingrate22,
aas.coverloadingrate23,
aas.coverloadingrate24,
aas.coverloadingrate25,
aas.coverloadingrate26,
aas.coverloadingrate27,
aas.coverloadingrate28,
aas.coverloadingrate29,
aas.coverloadingrate31,
aas.coverloadingrate32,
aas.coverloadingrate33,
aas.coverloadingrate34,
aas.coverloadingrate35,
aas.coverloadingrate36,
aas.coverloadingrate37,
aas.coverloadingrate38,
aas.coverloadingrate39,
aas.coverloadingrate41,
aas.coverloadingrate42,
aas.coverloadingrate43,
aas.coverloadingrate44,
aas.coverloadingrate46,
aas.coverloadingrate47,
aas.coverloadingrate48,
aas.coverloadingrate49,
aas.coverloadingrate51,
aas.coverloadingrate52,
aas.coverloadingrate53,
aas.coverloadingrate54,
aas.coverloadingrate55,
aas.coverloadingrate56,
aas.coverloadingrate57,
aas.coverloadingrate58,
aas.coverloadingrate59,
aas.coverloadingrate61,
aas.coverloadingrate62,
aas.coverloadingrate63,
aas.coverloadingrate64,
aas.coverloadingrate65,
aas.coverloadingrate66,
aas.coverloadingrate67,
aas.coverloadingrate68,
aas.coverloadingrate69,
aas.coverloadingrate71,
aas.coverloadingrate72,
aas.coverloadingrate73,
aas.coverloadingrate74,
aas.coverloadingrate75,
aas.coverloadingrate76,
aas.coverloadingrate77,
aas.coverloadingrate78,
aas.coverloadingrate79,
aas.coverloadingrate81,
aas.coverloadingrate82,
aas.coverloadingrate83,
aas.coverloadingrate84,
aas.coverloadingrate85,
aas.coverloadingrate86,
aas.coverloadingrate87,
aas.coverloadingrate88,
aas.coverloadingrate89,
aas.coverloadingrate91,
aas.coverloadingrate92,
aas.coverloadingrate93,
aas.coverloadingrate94,
aas.coverloadingrate95,
aas.coverloadingrate96,
aas.coverloadingrate97,
aas.coverloadingrate98,
aas.coverloadingrate99,
aas.coverloadingrate101,
aas.coverloadingrate102,
aas.coverloadingrate103,
aas.coverloadingrate104,
aas.coverloadingrate105,
aas.coverloadingrate106,
aas.coverloadingrate107,
aas.coverloadingrate108,
aas.coverloadingrate109,
aas.coverloadingrate210,
aas.coverloadingrate310,
aas.coverloadingrate410,
aas.coverloadingrate510,
aas.coverloadingrate610,
aas.coverloadingrate810,
aas.coverloadingrate910,
aas.coverloadingrate1010
 FROM 
            ins.rne_healthtab aas
        INNER JOIN 
            ins.idst_renewal_data_rgs asidst ON aas.policy_number = asidst.certificate_no
        WHERE 
            aas.policy_number = @PolicyNo";
             try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var aarogyasanjeevaniData = await connection.QueryAsync<AarogyaSanjeevaniRNE>(sqlQuery, new { PolicyNo = policyNo }).ConfigureAwait(false);
                    return new List<AarogyaSanjeevaniRNE>(aarogyasanjeevaniData);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while fetching GC data for policy: {PolicyNo}", policyNo);
                return new List<AarogyaSanjeevaniRNE>();
            }
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
        private async Task<IEnumerable<AarogyaSanjeevaniRNE>> CalculateAarogyaSanjeevaniPremium(string policyNo,IEnumerable<AarogyaSanjeevaniRNE> asRNEData, Dictionary<string, Hashtable> baseRateHashTable, Dictionary<string, Hashtable> relations)
        {
            AarogyaSanjeevaniRNE os = null;
            var columnNames = new List<string>();
            var finalPremiumValues = new List<decimal?>();
            IEnumerable<IdstData> idstData = await GetIdstRenewalData(policyNo);
            foreach (var row in asRNEData)
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
                string searcFamilyDescText = "FAMILY_DISCOUNT";
                bool containsFamilyDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searcFamilyDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultSearchFamilyDescText = containsFamilyDescText ? 1 : 0;
                string searchOnlineDescText = "Online Discount";
                bool containsOnlineDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchOnlineDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultSearchOnlineDescText = containsOnlineDescText ? 1 : 0;
                string searchEmployeeDescText = "Employee Discount";
                bool containsEmployeeDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchEmployeeDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultSearchEmployeeDescText = containsEmployeeDescText ? 1 : 0;
                string searchLoyaltyDescText = "Loyalty discount";
                bool containsLoyaltyDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchLoyaltyDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultLoyaltyDescText = containsLoyaltyDescText ? 1 : 0;
                string searchRuralDescText = "Rural Discount";
                bool containsRuralDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchRuralDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultRuralDescText = containsRuralDescText ? 1 : 0;
                decimal? deductablesInsured1 = row.insured_deductable1;
                decimal? loyaltyDiscount = resultLoyaltyDescText;
                decimal? employeeDiscount = resultSearchEmployeeDescText;
                decimal? onlineDiscount = resultSearchOnlineDescText;

                decimal? ruralDiscount = resultRuralDescText;
                var policyType = row.policy_type;
                var policyPeriod = row.policy_period;
          

                List<string> insuredRelations = new List<string>();
                for (int i = 1; i <= 12; i++)
                {
                    var insuredRelation = row.GetType().GetProperty($"txt_insured_relation{i}")?.GetValue(row)?.ToString();
                    insuredRelations.Add(insuredRelation);
                }

                var insuredRelationTAGValues = new List<string?>();
                for (int i = 0; i < insuredRelations.Count; i++)
                {
                    var insuredRelation = insuredRelations[i];
                    var insuredRelationTAG = relations
                        .Where(roww =>
                            roww.Value is Hashtable rateDetails &&
                            rateDetails["insured_relation"]?.ToString() == insuredRelation)
                        .Select(roww =>
                            roww.Value is Hashtable details && details["relation_tag"] != null
                            ? Convert.ToString(details["relation_tag"])
                            : (string?)null)
                        .FirstOrDefault();

                    insuredRelationTAGValues.Add(insuredRelationTAG);
                }

                var insuredAgeOne = Convert.ToInt32(row.txt_insured_age1);
                var insuredAgeTwo = Convert.ToInt32(row.txt_insured_age2);               
                var insuredAgeThree = Convert.ToInt32(row.txt_insured_age3); 
                var insuredAgeFour = Convert.ToInt32(row.txt_insured_age4);
                var insuredAgeFive = Convert.ToInt32(row.txt_insured_age5); 
                var insuredAgeSix = Convert.ToInt32(row.txt_insured_age6);
                var insuredAgeSeven = Convert.ToInt32(row.txt_insured_age7); 
                var insuredAgeEight = Convert.ToInt32(row.txt_insured_age8); 
                var insuredAgeNine = Convert.ToInt32(row.txt_insured_age9); 
                var insuredAgeTen = Convert.ToInt32(row.txt_insured_age10); 
                var insuredAgeEleven = Convert.ToInt32(row.txt_insured_age11); 
                var insuredAgeTwelve = Convert.ToInt32(row.txt_insured_age12); 
                List<int?> ageValues = new List<int?>();
                void AddAge(string ageStr)
                {
                    if (int.TryParse(ageStr, out int age) && age != 0)
                    {
                        ageValues.Add(age);
                    }
                    else
                    {
                        ageValues.Add(null);
                    }
                }
                AddAge(row.txt_insured_age1);
                AddAge(row.txt_insured_age2);
                AddAge(row.txt_insured_age3);
                AddAge(row.txt_insured_age4);
                AddAge(row.txt_insured_age5);
                AddAge(row.txt_insured_age6);
                AddAge(row.txt_insured_age7);
                AddAge(row.txt_insured_age8);
                AddAge(row.txt_insured_age9);
                AddAge(row.txt_insured_age10);
                AddAge(row.txt_insured_age11);
                AddAge(row.txt_insured_age12);

                var nonNullAges = ageValues.Where(age => age.HasValue && age.Value != 0).ToList();
                var noOfMembers = nonNullAges.Count();
                var eldestMember = ageValues.Max();
                var numberOfMembers = noOfMembers;//calculate this field
                int? count = noOfMembers;
                List<decimal?> sumInsuredList = new List<decimal?>();
                for (int i = 1; i <= noOfMembers; i++)
                {
                    decimal? sumInsured = (decimal?)row.GetType().GetProperty($"sum_insured{i}").GetValue(row);
                    sumInsuredList.Add(sumInsured);
                }
                decimal? totalsuminsured = sumInsuredList.Sum(si => si ?? 0);
                List<decimal?> cumulativeBonusList = new List<decimal?>();
                for (int i = 1; i <= noOfMembers; i++)
                {
                    decimal? bonusValue = Convert.ToDecimal(row.GetType().GetProperty($"insured_cb{i}")?.GetValue(row));
                    cumulativeBonusList.Add(bonusValue);
                }
                decimal? cumulativeBonus = cumulativeBonusList.Sum(cb => cb ?? 0);
                List<decimal?> basicLoadingRates = new List<decimal?>();
                for (int i = 1; i <= noOfMembers; i++)
                {
                    decimal? loadingRate = (decimal?)iDSTData.GetType().GetProperty($"loading_per_insured{i}")?.GetValue(iDSTData);
                    basicLoadingRates.Add(loadingRate ?? 0);
                }
                decimal? loyaltyDiscountValue = loyaltyDiscount;
                loyaltyDiscountValue = loyaltyDiscount.HasValue && loyaltyDiscount.Value > 0 ? 2.5m : 0.0m;
                decimal? employeeDiscountValue = employeeDiscount;
                employeeDiscountValue = employeeDiscount.HasValue && employeeDiscount.Value > 0 ? 5.0m : 0.0m;
                decimal? onlineDiscountValue = onlineDiscount;
                onlineDiscountValue = onlineDiscount.HasValue && onlineDiscount.Value > 0 ? 5.0m : 0.0m;
                decimal? familyDiscountValue = CalculateFamilyDiscount(policyType, numberOfMembers);
                decimal? ruralDiscountValue = CalculateRuralDiscount(ruralDiscount);
                decimal? cappingDiscount = CalculateCappingDiscount(familyDiscountValue, onlineDiscountValue, employeeDiscountValue, loyaltyDiscountValue, ruralDiscountValue);
                var columnName = GetColumnNameForPolicyPeriod(policyPeriod);
                if (columnName == null)
                {
                    throw new ArgumentException($"Invalid policy period: {policyPeriod}");
                }
                var productName = row.prod_name;
                string grouporretail = GetGroupOrRetail(productName);
                string grpcolumnName = (grouporretail == "Group") ? "group" : "retail";

                var sql = $@"
                    SELECT {grpcolumnName}
                    FROM aarogya_sanjeevani_baserates
                    WHERE suminsured = @p0 AND age = @p1";
                List<decimal?> basePremiumsList = new List<decimal?>();
                decimal? basePrem = 0;
                for (int i = 0; i < noOfMembers; i++)
                {
                    if (i >= 0 && i < sumInsuredList.Count && i < ageValues.Count)
                    {
                        basePrem = baseRateHashTable
                        .Where(row =>
                            row.Value is Hashtable rateDetails &&
                            (int)rateDetails["suminsured"] == sumInsuredList[i] &&
                            (int)rateDetails["age"] == ageValues[i])
                        .Select(row =>
                            row.Value is Hashtable details &&
                            details[grpcolumnName] != null
                                ? Convert.ToDecimal(details[grpcolumnName])
                                : (decimal?)null)
                        .FirstOrDefault();

                        basePremiumsList.Add(basePrem);
                    }
                }
                string condition = policyType;
                decimal? basePremium = CalculateResult(condition, basePremiumsList);
                decimal? loadingPrem_1 = CalculateLoadingPrem(0,basePremiumsList, basicLoadingRates);
                decimal? loadingPrem_2 = CalculateLoadingPrem(1,basePremiumsList, basicLoadingRates);
                decimal? loadingPrem_3 = CalculateLoadingPrem(2,basePremiumsList, basicLoadingRates);
                decimal? loadingPrem_4 = CalculateLoadingPrem(3,basePremiumsList, basicLoadingRates);
                decimal? loadingPrem_5 = CalculateLoadingPrem(4,basePremiumsList, basicLoadingRates);
                decimal? loadingPrem_6 = CalculateLoadingPrem(5,basePremiumsList, basicLoadingRates);
                decimal? loadingPrem_7 = CalculateLoadingPrem(6,basePremiumsList, basicLoadingRates);
                decimal? loadingPrem_8 = CalculateLoadingPrem(7,basePremiumsList, basicLoadingRates);
                decimal? loadingPrem_9 = CalculateLoadingPrem(8,basePremiumsList, basicLoadingRates);
                decimal? loadingPrem_10 = CalculateLoadingPrem(9,basePremiumsList, basicLoadingRates);
                decimal? loadingPrem_11 = CalculateLoadingPrem(10,basePremiumsList, basicLoadingRates);
                decimal? loadingPrem_12 = CalculateLoadingPrem(11,basePremiumsList, basicLoadingRates);
                var loadingPremvalues = new List<decimal?> { loadingPrem_1, loadingPrem_2, loadingPrem_3, loadingPrem_4,
                    loadingPrem_5, loadingPrem_6, loadingPrem_7, loadingPrem_8, loadingPrem_9, loadingPrem_10, loadingPrem_11, loadingPrem_12 };
                decimal? loadingPrem = (loadingPremvalues.Sum()) ?? 0;
                decimal? BasePremiumLoading = (basePremium + loadingPrem) ?? 0;
                decimal? BaseAndLoadingLoyaltyDiscount = ((loyaltyDiscountValue / 100) * BasePremiumLoading) ?? 0;
                decimal? BaseAndLoadingEmployeeDiscount = ((employeeDiscountValue / 100) * BasePremiumLoading) ?? 0;
                decimal? BaseAndLoadingOnlineDiscount = ((onlineDiscountValue / 100) * BasePremiumLoading) ?? 0;
                decimal? BaseAndLoadingFamilyDiscount = (familyDiscountValue * BasePremiumLoading) ?? 0;
                decimal? BaseAndLoadingRuralDiscount = (ruralDiscountValue * BasePremiumLoading) ?? 0;
                decimal? BaseAndLoadingCappingDiscount = (cappingDiscount * BasePremiumLoading) ?? 0;
                decimal? netPremium = (BasePremiumLoading - BaseAndLoadingCappingDiscount) ?? 0;
                decimal? GST = (netPremium * 0.18m) ?? 0;
                decimal? finalPremium = (netPremium + GST) ?? 0;
                decimal? Crosscheck = finalPremium - row.num_tot_premium;
                os = new AarogyaSanjeevaniRNE
                {
                    prod_code = row.prod_code,
                    prod_name = row.prod_name,
                    policy_number = row.policy_number,
                    txt_family = row.txt_family,
                    txt_insured_entrydate1 = row.txt_insured_entrydate1,
                    txt_insured_entrydate2 = row.txt_insured_entrydate2,
                    txt_insured_entrydate3 = row.txt_insured_entrydate3,
                    txt_insured_entrydate4 = row.txt_insured_entrydate4,
                    txt_insured_entrydate5 = row.txt_insured_entrydate5,
                    txt_insured_entrydate6 = row.txt_insured_entrydate6,
                    txt_insured_entrydate7 = row.txt_insured_entrydate7,
                    txt_insured_entrydate8 = row.txt_insured_entrydate8,
                    txt_insured_entrydate9 = row.txt_insured_entrydate9,
                    txt_insured_entrydate10 = row.txt_insured_entrydate10,
                    txt_insured_entrydate11 = row.txt_insured_entrydate11,
                    txt_insured_entrydate12 = row.txt_insured_entrydate12,
                    insured_loadingper1 = row.insured_loadingper1,
                    insured_loadingper2 = row.insured_loadingper2,
                    insured_loadingper3 = row.insured_loadingper3,
                    insured_loadingper4 = row.insured_loadingper4,
                    insured_loadingper5 = row.insured_loadingper5,
                    insured_loadingper6 = row.insured_loadingper6,
                    insured_loadingper7 = row.insured_loadingper7,
                    insured_loadingper8 = row.insured_loadingper8,
                    insured_loadingper9 = row.insured_loadingper9,
                    insured_loadingper10 = row.insured_loadingper10,
                    insured_loadingper11 = row.insured_loadingper11,
                    insured_loadingper12 = row.insured_loadingper12,

                    insured_loadingamt1 = row.insured_loadingamt1,
                    insured_loadingamt2 = row.insured_loadingamt2,
                    insured_loadingamt3 = row.insured_loadingamt3,
                    insured_loadingamt4 = row.insured_loadingamt4,
                    insured_loadingamt5 = row.insured_loadingamt5,
                    insured_loadingamt6 = row.insured_loadingamt6,
                    insured_loadingamt7 = row.insured_loadingamt7,
                    insured_loadingamt8 = row.insured_loadingamt8,
                    insured_loadingamt9 = row.insured_loadingamt9,
                    insured_loadingamt10 = row.insured_loadingamt10,
                    insured_loadingamt11 = row.insured_loadingamt11,
                    insured_loadingamt12 = row.insured_loadingamt12,

                    txt_insured_dob1 = row.txt_insured_dob1,
                    txt_insured_dob2 = row.txt_insured_dob2,
                    txt_insured_dob3 = row.txt_insured_dob3,
                    txt_insured_dob4 = row.txt_insured_dob4,
                    txt_insured_dob5 = row.txt_insured_dob5,
                    txt_insured_dob6 = row.txt_insured_dob6,
                    txt_insured_dob7 = row.txt_insured_dob7,
                    txt_insured_dob8 = row.txt_insured_dob8,
                    txt_insured_dob9 = row.txt_insured_dob9,
                    txt_insured_dob10 = row.txt_insured_dob10,
                    txt_insured_dob11 = row.txt_insured_dob11,
                    txt_insured_dob12 = row.txt_insured_dob12,
                    txt_insured_age1 = row.txt_insured_age1,
                    txt_insured_age2 = row.txt_insured_age2,
                    txt_insured_age3 = row.txt_insured_age3,
                    txt_insured_age4 = row.txt_insured_age4,
                    txt_insured_age5 = row.txt_insured_age5,
                    txt_insured_age6 = row.txt_insured_age6,
                    txt_insured_age7 = row.txt_insured_age7,
                    txt_insured_age8 = row.txt_insured_age8,
                    txt_insured_age9 = row.txt_insured_age9,
                    txt_insured_age10 = row.txt_insured_age10,
                    txt_insured_age11 = row.txt_insured_age11,
                    txt_insured_age12 = row.txt_insured_age12,
                    txt_insured_relation1 = row.txt_insured_relation1,
                    txt_insured_relation2 = row.txt_insured_relation2,
                    txt_insured_relation3 = row.txt_insured_relation3,
                    txt_insured_relation4 = row.txt_insured_relation4,
                    txt_insured_relation5 = row.txt_insured_relation5,
                    txt_insured_relation6 = row.txt_insured_relation6,
                    txt_insured_relation7 = row.txt_insured_relation7,
                    txt_insured_relation8 = row.txt_insured_relation8,
                    txt_insured_relation9 = row.txt_insured_relation9,
                    txt_insured_relation10 = row.txt_insured_relation10,
                    txt_insured_relation11 = row.txt_insured_relation11,
                    txt_insured_relation12 = row.txt_insured_relation12,
                    insured_cb1 = row.insured_cb1,
                    insured_cb2 = row.insured_cb2,
                    insured_cb3 = row.insured_cb3,
                    insured_cb4 = row.insured_cb4,
                    insured_cb5 = row.insured_cb5,
                    insured_cb6 = row.insured_cb6,
                    insured_cb7 = row.insured_cb7,
                    insured_cb8 = row.insured_cb8,
                    insured_cb9 = row.insured_cb9,
                    insured_cb10 = row.insured_cb10,
                    insured_cb11 = row.insured_cb11,
                    insured_cb12 = row.insured_cb12,
                    sum_insured1 = row.sum_insured1,
                    sum_insured2 = row.sum_insured2,
                    sum_insured3 = row.sum_insured3,
                    sum_insured4 = row.sum_insured4,
                    sum_insured5 = row.sum_insured5,
                    sum_insured6 = row.sum_insured6,
                    sum_insured7 = row.sum_insured7,
                    sum_insured8 = row.sum_insured8,
                    sum_insured9 = row.sum_insured9,
                    sum_insured10 = row.sum_insured10,
                    sum_insured11 = row.sum_insured11,
                    sum_insured12 = row.sum_insured12,
                    coverbaseloadingrate1 = row.coverbaseloadingrate1,
                    coverbaseloadingrate2 = row.coverbaseloadingrate2,
                    coverbaseloadingrate3 = row.coverbaseloadingrate3,
                    coverbaseloadingrate4 = row.coverbaseloadingrate4,
                    coverbaseloadingrate5 = row.coverbaseloadingrate5,
                    coverbaseloadingrate6 = row.coverbaseloadingrate6,
                    coverbaseloadingrate7 = row.coverbaseloadingrate7,
                    coverbaseloadingrate8 = row.coverbaseloadingrate8,
                    coverbaseloadingrate9 = row.coverbaseloadingrate9,
                    coverbaseloadingrate10 = row.coverbaseloadingrate10,
                    coverbaseloadingrate11 = row.coverbaseloadingrate11,
                    coverbaseloadingrate12 = row.coverbaseloadingrate12,
                    policy_start_date = row.policy_start_date,
                    policy_expiry_date = row.policy_expiry_date,
                    policy_type = row.policy_type,
                    policy_period = row.policy_period,
                    policyplan = row.policyplan,
                    claimcount = row.claimcount,
                    no_of_members = noOfMembers,
                    eldest_member = eldestMember,
                    tier_type = row.tier_type,
                    employee_discount = employeeDiscountValue,
                    online_discount = (onlineDiscountValue * 100),
                    loyalty_discount = loyaltyDiscountValue,
                    family_discount = (familyDiscountValue * 100),
                    rural_discount = ruralDiscountValue,
                    capping_discount = cappingDiscount,
                  
                    base_premium = basePremium,
                    loading_prem_1 = loadingPrem_1,
                    loading_prem_2 = loadingPrem_2,
                    loading_prem_3 = loadingPrem_3,
                    loading_prem_4 = loadingPrem_4,
                    loading_prem_5 = loadingPrem_5,
                    loading_prem_6 = loadingPrem_6,
                    loading_prem_7 = loadingPrem_7,
                    loading_prem_8 = loadingPrem_8,
                    loading_prem_9 = loadingPrem_9,
                    loading_prem_10 = loadingPrem_10,
                    loading_prem_11 = loadingPrem_11,
                    loading_prem_12 = loadingPrem_12,
                    loading_premium = loadingPrem,
                    basepremiumLoading = BasePremiumLoading,
                    baseAndLoading_LoyaltyDiscount = BaseAndLoadingLoyaltyDiscount,
                    baseAndLoading_EmployeeDiscount = BaseAndLoadingEmployeeDiscount,
                    baseAndLoading_OnlineDiscount = BaseAndLoadingOnlineDiscount,
                    baseAndLoading_FamilyDiscount = BaseAndLoadingFamilyDiscount,
                    baseAndLoading_RuralDiscount = BaseAndLoadingRuralDiscount,
                    baseAndLoading_CapppingDiscount = BaseAndLoadingCappingDiscount,
                    netPremium = netPremium.HasValue ? Math.Round(netPremium.Value, 2) : (decimal?)0,
                    finalPremium = finalPremium.HasValue ? Math.Round(finalPremium.Value, 2) : (decimal?)0,
                    GST = GST.HasValue ? Math.Round(GST.Value, 2) : (decimal?)0,
                    crossCheck = Crosscheck.HasValue ? Math.Round(Crosscheck.Value, 2) : (decimal?)0,
                    aarogyasanjeevani_total_Premium = row.num_tot_premium.HasValue ? Math.Round(row.num_tot_premium.Value, 2) : (decimal?)0,
                    aarogyasanjeevani_netpremium = row.num_net_premium.HasValue ? Math.Round(row.num_net_premium.Value, 2) : (decimal?)0,
                    aarogyasanjeevani_GST = row.num_service_tax.HasValue ? Math.Round(row.num_service_tax.Value, 2) : (decimal?)0
                };
                for (int i = 0; i < basePremiumsList.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            os.base_premium_1 = basePremiumsList[i];
                            break;
                        case 1:
                            os.base_premium_2 = basePremiumsList[i];
                            break;
                        case 2:
                            os.base_premium_3 = basePremiumsList[i];
                            break;
                        case 3:
                            os.base_premium_4 = basePremiumsList[i];
                            break;
                        case 4:
                            os.base_premium_5 = basePremiumsList[i];
                            break;
                        case 5:
                            os.base_premium_6 = basePremiumsList[i];
                            break;
                        case 6:
                            os.base_premium_7 = basePremiumsList[i];
                            break;
                        case 7:
                            os.base_premium_8 = basePremiumsList[i];
                            break;
                        case 8:
                            os.base_premium_9 = basePremiumsList[i];
                            break;
                        case 9:
                            os.base_premium_10 = basePremiumsList[i];
                            break;
                        case 10:
                            os.base_premium_11 = basePremiumsList[i];
                            break;
                        case 11:
                            os.base_premium_12 = basePremiumsList[i];
                            break;
                        default:
                            break;
                    }
                }
            }
            return new List<AarogyaSanjeevaniRNE> { os };
        }
        async Task HandleCrosschecksAndUpdateStatus(string policyNo, AarogyaSanjeevaniRNE asRNEData, decimal? crosscheck1, decimal? netPremium, decimal? finalPremium, decimal? gst)
        {
            string? connectionString = ConfigurationManager.ConnectionStrings["PostgresDb"].ConnectionString;
            using (IDbConnection dbConnection = new NpgsqlConnection(connectionString))
            {
                dbConnection.Open();
                var record = dbConnection.QueryFirstOrDefault<premium_validation>(
                    "SELECT certificate_no FROM ins.premium_validation WHERE certificate_no = @CertificateNo",
                    new { CertificateNo = asRNEData.policy_number.ToString() });
                if ((Math.Abs(crosscheck1.GetValueOrDefault()) <= 10))
                {
                    var insertQuery = @"
                    INSERT INTO ins.premium_validation (certificate_no, verified_prem, verified_gst, verified_total_prem, rn_generation_status, final_remarks, dispatch_status)
                    VALUES (@CertificateNo, @VerifiedPrem, @VerifiedGst, @VerifiedTotalPrem, 'RN Generation Awaited', 'RN Generation Awaited', 'PDF Gen Under Process With CLICK PSS Team')";

                    dbConnection.Execute(insertQuery, new
                    {
                        CertificateNo = policyNo.ToString(),
                        VerifiedPrem = netPremium,
                        VerifiedGst = gst,
                        VerifiedTotalPrem = finalPremium
                    });
                }
                else if (Math.Abs(crosscheck1.GetValueOrDefault()) > 10)
                {
                    var insertQuery = @"
                            INSERT INTO ins.premium_validation (certificate_no, verified_prem, verified_gst, verified_total_prem, rn_generation_status, final_remarks, dispatch_status, error_description)
                            VALUES (@CertificateNo, @VerifiedPrem, @VerifiedGst, @VerifiedTotalPrem, 'IT Issue - QC Failed', 'IT Issues', 'Revised Extraction REQ From IT Team QC Failed Cases', 'Premium verification failed due to premium difference of more than 10 rupees')";

                    dbConnection.Execute(insertQuery, new
                    {
                        CertificateNo = policyNo.ToString(),
                        VerifiedPrem = netPremium,
                        VerifiedGst = gst,
                        VerifiedTotalPrem = finalPremium,
                    });
                }

            }
        }


        private static string GetGroupOrRetail(string productName)
        {
            try
            {
                if (productName.IndexOf("Group", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return "Group";
                }
                else
                {
                    return "Retail";
                }
            }
            catch
            {
                return "Retail";
            }
        }

        private static decimal? GetEmployeeDiscount(decimal? noOfMembers)
        {
            decimal? cIFamilyDiscountValue = 0m;
            if (noOfMembers > 1)
            {
                cIFamilyDiscountValue = 0.1m; 
            }

            return cIFamilyDiscountValue;
        }

        private static decimal? GetFamilyDiscount(int? noOfMembers)
        {
            decimal? cIFamilyDiscountValue = 0m;
            if (noOfMembers > 1)
            {
                cIFamilyDiscountValue = 0.1m; 
            }

            return cIFamilyDiscountValue;
        }

        private static int GetOnlineDiscount(int? noOfMembers)
        {
            var cIFamilyDiscountValue = 0;
            if (noOfMembers > 0)
            {
                cIFamilyDiscountValue = 10; // 2.5%
            }
            return cIFamilyDiscountValue;
        }

        private static void GetLoyaltyDiscount(decimal? loyaltyDiscount, out decimal? cIloyaltyDiscountValue, out decimal? cIloyaltyDiscount)
        {
            cIloyaltyDiscountValue = loyaltyDiscount;
            cIloyaltyDiscount = 0m;
            // Apply the discount logic
            if (cIloyaltyDiscountValue > 0)
            {
                cIloyaltyDiscount = 0.1m; // 10%
            }
        }

        static decimal? CalculateLoadingPrem(int i,List<decimal?> basePremiumList, List<decimal?> baseLoadingRates)
        {
            if (basePremiumList != null && baseLoadingRates != null &&
                   i >= 0 && i < basePremiumList.Count && i < baseLoadingRates.Count)
            {
                return basePremiumList[i] * baseLoadingRates[i] / 100;
            }
            return null;
        }

        private static string GetColumnNameForPolicyPeriod(string policyPeriod)
        {
            return policyPeriod switch
            {
                "1 Year" => "one_year",
                "2 Years" => "two_years",
                "3 Years" => "three_years",
                _ => null,
            };
        }

        private decimal? CalculateResult(string condition, List<decimal?> values)
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

        private static decimal CalculateFamilyDiscount(string policyType, int numberOfMembers)
        {
            if (policyType == "Individual" && numberOfMembers > 1)
            {
                return 0.1m; 
            }
            else
            {
                return 0.00m; 
            }
        }

        private static decimal CalculateRuralDiscount(decimal? ruralDiscount)
        {
            if (ruralDiscount > 0)
            {
                return 0.15m; 
            }
            else
            {
                return 0.00m; 
            }
        }
        public async Task<IEnumerable<IdstData>> GetIdstRenewalData(string policyNo)
        {
            string? connectionString = ConfigurationManager.ConnectionStrings["PostgresDb"].ConnectionString;
            string sqlQuery = @"
        SELECT
            certificate_no,
            loading_per_insured1,
            loading_per_insured2,
            loading_per_insured3,
            loading_per_insured4,
            loading_per_insured5,
            loading_per_insured6,
            loading_per_insured7,
            loading_per_insured8,
            loading_per_insured9,
            loading_per_insured10,
            loading_per_insured11,
            loading_per_insured12
        FROM
            ins.idst_renewal_data_rgs
        WHERE
            certificate_no = @PolicyNo";
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<IdstData>(sqlQuery, new { PolicyNo = policyNo }).ConfigureAwait(false);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while fetching renewal data for policy: {PolicyNo}", policyNo);
                return Enumerable.Empty<IdstData>();
            }
        }
        private static decimal? CalculateCappingDiscount(decimal? familyDiscountValue, decimal? onlineDiscountValue, decimal? employeeDiscountValue, decimal? loyaltyDiscountValue, decimal? ruralDiscountValue)
        {
            decimal totalDiscount = (familyDiscountValue ?? 0) + (onlineDiscountValue ?? 0) +
                                     (employeeDiscountValue ?? 0) + (loyaltyDiscountValue ?? 0) +
                                     (ruralDiscountValue ?? 0);

            if (totalDiscount > 0.20m)
            {
                return 0.20m;
            }

            return totalDiscount;
        }
        public async Task<Dictionary<string, Hashtable>> GetBaseRatesAsync(HDFCDbContext dbContext)
        {
            var ratesTable = new Dictionary<string, Hashtable>();
            var rates = await dbContext.aarogya_sanjeevani_baserates.ToListAsync().ConfigureAwait(false);
            foreach (var rate in rates)
            {
                var compositeKey = $"{rate.suminsured}-{rate.age}";
                var rateDetails = new Hashtable
                {
                    { "suminsured", rate.suminsured },
                    { "age", rate.age },
                    { "age_band", rate.age_band },
                    { "retail", rate.retail },
                    { "group", rate.group }
                };
                ratesTable[compositeKey] = rateDetails;
            }
            return ratesTable;
        }
        public async Task<Dictionary<string, Hashtable>> GetRelationTagsAsync(HDFCDbContext dbContext)
        {
            var relationTagsTable = new Dictionary<string, Hashtable>();

            var relations = await dbContext.relations.ToListAsync().ConfigureAwait(false);
            foreach (var relation in relations)
            {
                var compositeKey = $"{relation.insured_relation}-{relation.relation_tag}";// In this case, just using insured_relation as key

                var relationDetails = new Hashtable
                {
                    { "insured_relation", relation.insured_relation },
                    { "relation_tag", relation.relation_tag }
        };

                relationTagsTable[compositeKey] = relationDetails;
            }
            return relationTagsTable;
        }
    }
}



