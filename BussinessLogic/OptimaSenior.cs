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
using Serilog;
using System.Configuration;
using System.Collections;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RenewalGovernancePremiumValidation.BussinessLogic
{

    public class OptimaSenior
    {
        private readonly HDFCDbContext dbContext;
        private readonly ILogger<OptimaSenior> _logger;
        public OptimaSenior(HDFCDbContext hDFCDbContext, ILogger<OptimaSenior> logger)
        {
            _logger = logger;
        }
        public async Task<List<OptimaSeniorRNE>> GetOptimaSeniorDataAsync(string policyNo)
        {
            string? connectionString = ConfigurationManager.ConnectionStrings["PostgresDb"].ConnectionString;

            string sqlQuery = @"
SELECT
        os.prod_code,
               os.batchid,
                os.prod_name,               
               os.policy_number,
                 os.reference_num,
                os.split_flag,
                 os.customer_id,
                 os.customername,
                os.verticale_name,
               os.verticalname,
                 os.policy_start_date,
                 os.policy_expiry_date,
                os.policy_period,
                 os.tier_type,
                 os.policyplan,
               os.policy_type,
                os.txt_family,
              os.claimcount,
                 os.num_tot_premium,
                os.num_net_premium,
                os.num_service_tax,
                 os.coverbaseloadingrate1,
                 os.coverbaseloadingrate2,
                os.coverbaseloadingrate3,
               os.coverbaseloadingrate4,
                 os.coverbaseloadingrate5,
                os.coverbaseloadingrate6,
               osidst.loading_per_insured1,
                osidst.loading_per_insured2,
                osidst.loading_per_insured3,
                osidst.loading_per_insured4,
                osidst.loading_per_insured5,
                 osidst.loading_per_insured6,
              os.txt_insuredname1,
            os.txt_insuredname2,
                os.txt_insuredname3,
                os.txt_insuredname4,
                  os.txt_insuredname5,
               os.txt_insuredname6,
                 os.txt_insured_dob1,
                os.txt_insured_dob2,
             os.txt_insured_dob3,
                os.txt_insured_dob4,
               os.txt_insured_dob5,
              os.txt_insured_dob6,
                os.txt_insured_relation1,
           os.txt_insured_relation2,
                os.txt_insured_relation3,
                os.txt_insured_relation4,
                 os.txt_insured_relation5,
                os.txt_insured_relation6,
                 os.txt_insured_age1,
                 os.txt_insured_age2,
                 os.txt_insured_age3,
               os.txt_insured_age4,
              os.txt_insured_age5,
               os.txt_insured_age6,
               os.txt_insured_gender1,
                os.txt_insured_gender2,
                os.txt_insured_gender3,
              os.txt_insured_gender4,
                os.txt_insured_gender5,
                os.txt_insured_gender6,
              os.member_id1,
                os.member_id2,
             os.member_id3,
                os.member_id4,
             os.member_id5,
               os.member_id6,
                os.nominee_name1,
            os.nominee_name2,
                os.nominee_name3,
              os.nominee_name4,
               os.nominee_name5,
                os.nominee_name6,
                 os.nominee_relationship1,
              os.nominee_relationship2,
                os.nominee_relationship3,
                 os.nominee_relationship4,
                 os.nominee_relationship5,
               os.nominee_relationship6,
                os.pollddesc1,
                os.pollddesc2,
               os.pollddesc3,
              os.pollddesc4,
              os.pollddesc5,
             os.sum_insured1,
                 os.sum_insured2,
                os.sum_insured3,
              os.sum_insured4,
               os.sum_insured5,
                os.sum_insured6,
              os.insured_cb1,
                os.insured_cb2,
                 os.insured_cb3,
                os.insured_cb4,
               os.insured_cb5,
               os.insured_cb6,
               os.premium_insured1,
                os.premium_insured2,
                os.premium_insured3,
                os.premium_insured4,
               os.premium_insured5,
           os.premium_insured6,
              os.insured_deductable1,
                os.insured_deductable2,
              os.insured_deductable3,
               os.insured_deductable4,
               os.insured_deductable5,
               os.insured_deductable6,
          os.covername11,
                os.covername12,
               os.covername13,
              os.covername14,
               os.covername15,
              os.covername16,
                os.covername17,
               os.covername18,
                 os.covername19,
                 os.covername21,
               os.covername22,
             os.covername23,
                 os.covername24,
               os.covername25,
                os.covername26,
                os.covername27,
                os.covername28,
                os.covername29,
              os.covername31,
                 os.covername32,
                 os.covername33,
            os.covername34,
               os.covername35,
              os.covername36,
               os.covername37,
             os.covername38,
                os.covername39,
              os.covername41,
               os.covername42,
               os.covername43,
             os.covername44,
                os.covername45,
                 os.covername46,
            os.covername47,
               os.covername48,
               os.covername49,
                os.covername51,
                os.covername52,
               os.covername53,
                os.covername54,
                os.covername55,
              os.covername56,
              os.covername57,
                 os.covername58,
                os.covername59,
              os.covername61,
                os.covername62,
             os.covername63,
              os.covername64,
              os.covername65,
                 os.covername66,
                 os.covername67,
             os.covername68,
                os.covername69,
             os.covername101,
                os.covername102,
              os.covername103,
                os.covername104,
               os.covername105,
               os.covername106,
                os.covername107,
             os.covername108,
                os.covername109,
                os.covername110,
               os.covername210,
               os.covername310,
              os.covername410,
                 os.covername510,
              os.covername610,
                os.covername710,
               os.covername810,
               os.covername910,
             os.covername1010,
               os.coversi11,
              os.coversi12,
               os.coversi13,
             os.coversi14,
                 os.coversi15,
               os.coversi16,
               os.coversi17,
               os.coversi18,
                os.coversi19,
               os.coversi21,
             os.coversi22,
               os.coversi23,
                 os.coversi24,
                os.coversi25,
                os.coversi26,
                 os.coversi27,
                os.coversi28,
                os.coversi29,
                os.coversi31,
                os.coversi32,
                os.coversi33,
                 os.coversi34,
                os.coversi35,
                os.coversi36,
                os.coversi37,
                 os.coversi38,
                 os.coversi39,
                 os.coversi41,
                 os.coversi42,
                 os.coversi43,
                os.coversi44,
                os.coversi46,
                 os.coversi47,
                 os.coversi48,
                 os.coversi49,
               os.coversi51,
                 os.coversi52,
                os.coversi53,
                 os.coversi54,
                os.coversi55,
                os.coversi56,
              os.coversi57,
               os.coversi58,
                os.coversi59,
                os.coversi61,
                 os.coversi62,
               os.coversi63,
                os.coversi64,
                os.coversi65,
                os.coversi66,
                os.coversi67,
              os.coversi68,
               os.coversi69,
               os.coversi101,
               os.coversi102,
               os.coversi103,
             os.coversi104,
               os.coversi105,
               os.coversi106,
               os.coversi107,
               os.coversi108,
              os.coversi109,
             os.coversi210,
               os.coversi310,
             os.coversi410,
             os.coversi510,
              os.coversi610,
                 os.coversi810,
                 os.coversi910,
               os.coversi1010,
              os.coverprem11,
                os.coverprem12,
                os.coverprem13,
            os.coverprem14,
                os.coverprem15,
              os.coverprem16,
            os.coverprem17,
             os.coverprem18,
            os.coverprem19,
               os.coverprem21,
            os.coverprem22,
               os.coverprem23,
                os.coverprem24,
               os.coverprem25,
             os.coverprem26,
              os.coverprem27,
                 os.coverprem28,
               os.coverprem29,
              os.coverprem31,
               os.coverprem32,
               os.coverprem33,
                os.coverprem34,
                os.coverprem35,
                os.coverprem36,
                 os.coverprem37,
                os.coverprem38,
                 os.coverprem39,
                os.coverprem41,
               os.coverprem42,
                os.coverprem43,
                 os.coverprem44,
                os.coverprem46,
                 os.coverprem47,
                  os.coverprem48,
                  os.coverprem49,
                 os.coverprem51,
               os.coverprem52,
                os.coverprem53,
               os.coverprem54,
                 os.coverprem55,
                  os.coverprem56,
                  os.coverprem57,
                 os.coverprem58,
                  os.coverprem59,
                 os.coverprem61,
                  os.coverprem62,
                 os.coverprem63,
                 os.coverprem64,
                os.coverprem65,
                 os.coverprem66,
                  os.coverprem67,
             os.coverprem68,
              os.coverprem69,
                   os.coverprem101,
                    os.coverprem102,
                    os.coverprem103,
                    os.coverprem104,
                   os.coverprem105,
                   os.coverprem106,
                    os.coverprem107,
                   os.coverprem108,
                    os.coverprem109,
                   os.coverprem210,
                  os.coverprem310,
                 os.coverprem410,
                    os.coverprem510,
                  os.coverprem610,
                  os.coverprem810,
                os.coverprem910,
                  os.coverprem1010,
                 os.insured_loadingamt1,
                os.insured_loadingamt2,
                os.insured_loadingamt3,
                 os.insured_loadingamt4,
                os.insured_loadingamt5,
                 os.insured_loadingamt6
        FROM 
            ins.rne_healthtab os
        INNER JOIN 
            ins.idst_renewal_data_rgs osidst ON os.policy_number = osidst.certificate_no
        WHERE 
            os.policy_number = @PolicyNo";
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var optimaseniorData = await connection.QueryAsync<OptimaSeniorRNE>(sqlQuery, new { PolicyNo = policyNo }).ConfigureAwait(false);
                    return new List<OptimaSeniorRNE>(optimaseniorData);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while fetching GC data for policy: {PolicyNo}", policyNo);
                return new List<OptimaSeniorRNE>();
            }
        }
        public async Task GetOptimaSeniorValidation(string policyNo, Dictionary<string, Hashtable> baseRateHashTable)
        {
            List<OptimaSeniorRNE> osRNEData;
            IEnumerable<OptimaSeniorRNE> optimaseniorRNE = Enumerable.Empty<OptimaSeniorRNE>();

            osRNEData = await GetOptimaSeniorDataAsync(policyNo);

            optimaseniorRNE = await CalculateOptimaSeniorPremium(policyNo, osRNEData, baseRateHashTable);

            OptimaSeniorValidationResult objOptimaSeniorValidation = new OptimaSeniorValidationResult
            {
                prod_code = osRNEData.FirstOrDefault()?.prod_code,
                prod_name = osRNEData.FirstOrDefault()?.prod_name,
                reference_num = osRNEData.FirstOrDefault()?.reference_num,
                policy_number = osRNEData.FirstOrDefault()?.policy_number,
                batchid = osRNEData.FirstOrDefault()?.batchid,
                txt_family = osRNEData.FirstOrDefault()?.txt_family,
                no_of_members = osRNEData.FirstOrDefault()?.no_of_members,
                insured_loadingper1 = osRNEData.FirstOrDefault()?.insured_loadingper1,
                insured_loadingper2 = osRNEData.FirstOrDefault()?.insured_loadingper2,
                insured_loadingper3 = osRNEData.FirstOrDefault()?.insured_loadingper3,
                insured_loadingper4 = osRNEData.FirstOrDefault()?.insured_loadingper4,
                insured_loadingper5 = osRNEData.FirstOrDefault()?.insured_loadingper5,
                insured_loadingper6 = osRNEData.FirstOrDefault()?.insured_loadingper6,
                insured_loadingamt1 = osRNEData.FirstOrDefault()?.insured_loadingamt1,
                insured_loadingamt2 = osRNEData.FirstOrDefault()?.insured_loadingamt2,
                insured_loadingamt3 = osRNEData.FirstOrDefault()?.insured_loadingamt3,
                insured_loadingamt4 = osRNEData.FirstOrDefault()?.insured_loadingamt4,
                insured_loadingamt5 = osRNEData.FirstOrDefault()?.insured_loadingamt5,
                insured_loadingamt6 = osRNEData.FirstOrDefault()?.insured_loadingamt6,
                txt_insuredname1 = osRNEData.FirstOrDefault()?.txt_insuredname1,
                txt_insuredname2 = osRNEData.FirstOrDefault()?.txt_insuredname2,
                txt_insuredname3 = osRNEData.FirstOrDefault()?.txt_insuredname3,
                txt_insuredname4 = osRNEData.FirstOrDefault()?.txt_insuredname4,
                txt_insuredname5 = osRNEData.FirstOrDefault()?.txt_insuredname5,
                txt_insuredname6 = osRNEData.FirstOrDefault()?.txt_insuredname6,
                txt_insured_dob1 = osRNEData.FirstOrDefault()?.txt_insured_dob1,
                txt_insured_dob2 = osRNEData.FirstOrDefault()?.txt_insured_dob2,
                txt_insured_dob3 = osRNEData.FirstOrDefault()?.txt_insured_dob3,
                txt_insured_dob4 = osRNEData.FirstOrDefault()?.txt_insured_dob4,
                txt_insured_dob5 = osRNEData.FirstOrDefault()?.txt_insured_dob5,
                txt_insured_dob6 = osRNEData.FirstOrDefault()?.txt_insured_dob6,
                txt_insured_age1 = osRNEData.FirstOrDefault()?.txt_insured_age1,
                txt_insured_age2 = osRNEData.FirstOrDefault()?.txt_insured_age2,
                txt_insured_age3 = osRNEData.FirstOrDefault()?.txt_insured_age3,
                txt_insured_age4 = osRNEData.FirstOrDefault()?.txt_insured_age4,
                txt_insured_age5 = osRNEData.FirstOrDefault()?.txt_insured_age5,
                txt_insured_age6 = osRNEData.FirstOrDefault()?.txt_insured_age6,
                txt_insured_relation1 = osRNEData.FirstOrDefault()?.txt_insured_relation1,//coming as "string"
                txt_insured_relation2 = osRNEData.FirstOrDefault()?.txt_insured_relation2,
                txt_insured_relation3 = osRNEData.FirstOrDefault()?.txt_insured_relation3,
                txt_insured_relation4 = osRNEData.FirstOrDefault()?.txt_insured_relation4,
                txt_insured_relation5 = osRNEData.FirstOrDefault()?.txt_insured_relation5,
                txt_insured_relation6 = osRNEData.FirstOrDefault()?.txt_insured_relation6,
                insured_cb1 = osRNEData.FirstOrDefault()?.insured_cb1,
                insured_cb2 = osRNEData.FirstOrDefault()?.insured_cb2,
                insured_cb3 = osRNEData.FirstOrDefault()?.insured_cb3,
                insured_cb4 = osRNEData.FirstOrDefault()?.insured_cb4,
                insured_cb5 = osRNEData.FirstOrDefault()?.insured_cb5,
                insured_cb6 = osRNEData.FirstOrDefault()?.insured_cb6,
                sum_insured1 = osRNEData.FirstOrDefault()?.sum_insured1,
                sum_insured2 = osRNEData.FirstOrDefault()?.sum_insured2,
                sum_insured3 = osRNEData.FirstOrDefault()?.sum_insured3,
                sum_insured4 = osRNEData.FirstOrDefault()?.sum_insured4,
                sum_insured5 = osRNEData.FirstOrDefault()?.sum_insured5,
                sum_insured6 = osRNEData.FirstOrDefault()?.sum_insured6,
                coverbaseloadingrate1 = osRNEData.FirstOrDefault()?.coverbaseloadingrate1,//Basic Loading Rate1 in gc
                coverbaseloadingrate2 = osRNEData.FirstOrDefault()?.coverbaseloadingrate2,
                coverbaseloadingrate3 = osRNEData.FirstOrDefault()?.coverbaseloadingrate3,
                coverbaseloadingrate4 = osRNEData.FirstOrDefault()?.coverbaseloadingrate4,
                coverbaseloadingrate5 = osRNEData.FirstOrDefault()?.coverbaseloadingrate5,
                coverbaseloadingrate6 = osRNEData.FirstOrDefault()?.coverbaseloadingrate6,
                policy_start_date = osRNEData.FirstOrDefault()?.policy_start_date,
                policy_expiry_date = osRNEData.FirstOrDefault()?.policy_expiry_date,
                policy_type = osRNEData.FirstOrDefault()?.policy_type,
                policy_period = osRNEData.FirstOrDefault()?.policy_period,
                policyplan = osRNEData.FirstOrDefault()?.policyplan,
                tier_type = osRNEData.FirstOrDefault()?.tier_type,
                no_claim_discount = osRNEData.FirstOrDefault()?.no_claim_discount,
                family_discount = osRNEData.FirstOrDefault()?.family_discount,
                longterm_discount = osRNEData.FirstOrDefault()?.longterm_discount,
                insured_Premium1 = osRNEData.FirstOrDefault()?.insured_Premium1,
                insured_Premium2 = osRNEData.FirstOrDefault()?.insured_Premium2,
                insured_Premium3 = osRNEData.FirstOrDefault()?.insured_Premium3,
                insured_Premium4 = osRNEData.FirstOrDefault()?.insured_Premium4,
                insured_Premium5 = osRNEData.FirstOrDefault()?.insured_Premium5,
                insured_Premium6 = osRNEData.FirstOrDefault()?.insured_Premium6,
                base_Premium = osRNEData.FirstOrDefault()?.base_Premium,
                loading_Premium1 = osRNEData.FirstOrDefault()?.loading_Premium1,
                loading_Premium2 = osRNEData.FirstOrDefault()?.loading_Premium2,
                loading_Premium3 = osRNEData.FirstOrDefault()?.loading_Premium3,
                loading_Premium4 = osRNEData.FirstOrDefault()?.loading_Premium4,
                loading_Premium5 = osRNEData.FirstOrDefault()?.loading_Premium5,
                loading_Premium6 = osRNEData.FirstOrDefault()?.loading_Premium6,
                loading_Premium = osRNEData.FirstOrDefault()?.loading_Premium,
                optimaSenior_BasePremium_Loading = osRNEData.FirstOrDefault()?.optimaSenior_BasePremium_Loading,
                optimaSenior_NoClaim_Discount = osRNEData.FirstOrDefault()?.optimaSenior_NoClaim_Discount,
                optimaSenior_Family_Discount = osRNEData.FirstOrDefault()?.optimaSenior_Family_Discount,
                optimaSenior_LongTerm_Discount = osRNEData.FirstOrDefault()?.optimaSenior_LongTerm_Discount,
                optimaSeniorFinalBase_Premium = osRNEData.FirstOrDefault()?.optimaSeniorFinalBase_Premium.HasValue == true
              ? (decimal?)Math.Round(osRNEData.FirstOrDefault().optimaSeniorFinalBase_Premium.Value, 2)
              : (decimal?)0,

                net_premium = osRNEData.FirstOrDefault()?.net_premium.HasValue == true
              ? (decimal?)Math.Round(osRNEData.FirstOrDefault().net_premium.Value, 2)
              : (decimal?)0,

                final_Premium = osRNEData.FirstOrDefault()?.final_Premium.HasValue == true
              ? (decimal?)Math.Round(osRNEData.FirstOrDefault().final_Premium.Value, 2)
              : (decimal?)0,
                gst = osRNEData.FirstOrDefault()?.gst.HasValue == true
              ? (decimal?)Math.Round(osRNEData.FirstOrDefault().gst.Value, 2)
              : (decimal?)0,
                cross_Check = osRNEData.FirstOrDefault()?.cross_Check.HasValue == true
              ? (decimal?)Math.Round(osRNEData.FirstOrDefault().cross_Check.Value, 2)
              : (decimal?)0,


                optimasenior_total_Premium = osRNEData.FirstOrDefault()?.optimasenior_total_Premium.HasValue == true
              ? (decimal?)Math.Round(osRNEData.FirstOrDefault().optimasenior_total_Premium.Value, 2)
              : (decimal?)0,
                optimasenior_netpremium = osRNEData.FirstOrDefault()?.optimasenior_netpremium.HasValue == true
              ? (decimal?)Math.Round(osRNEData.FirstOrDefault().optimasenior_netpremium.Value, 2)
              : (decimal?)0,
                optimasenior_gst = osRNEData.FirstOrDefault()?.optimasenior_gst.HasValue == true
              ? (decimal?)Math.Round(osRNEData.FirstOrDefault().optimasenior_gst.Value, 2)
              : (decimal?)0,

            };
            decimal? crosscheck1 = optimaseniorRNE.FirstOrDefault()?.cross_Check;
            decimal? netPremium = optimaseniorRNE.FirstOrDefault()?.net_premium ?? 0;
            decimal? finalPremium = optimaseniorRNE.FirstOrDefault()?.final_Premium ?? 0;
            decimal? gst = optimaseniorRNE.FirstOrDefault()?.gst ?? 0;

            if (objOptimaSeniorValidation?.policy_number == null)
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
                    if (osRNEData.FirstOrDefault()?.insured_cb1 == string.Empty && osRNEData.FirstOrDefault()?.insured_cb1 == null)
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
                            await HandleCrosschecksAndUpdateStatus(policyNo, osRNEData.FirstOrDefault(), crosscheck1Value, netPremium, finalPremium, gst);
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

                if (objOptimaSeniorValidation != null)
                {
                    var no_of_members = optimaseniorRNE.FirstOrDefault()?.no_of_members;
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
                    var insertQuery = @"
                                INSERT INTO ins.rne_calculated_cover_rg (policy_number, referencenum, suminsured, premium, totalpremium, riskname, covername)
                                VALUES (@policy_number, @referencenum, @suminsured, @premium, @totalpremium, @riskname, @covername);
                                ";

                    await dbConnection.ExecuteAsync(insertQuery, newRecord).ConfigureAwait(false);
                }
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


        private async Task<IEnumerable<OptimaSeniorRNE>> CalculateOptimaSeniorPremium(string policyNo, IEnumerable<OptimaSeniorRNE> osRNEData, Dictionary<string, Hashtable> baseRateHashTable)
        {
            OptimaSeniorRNE os = null;
            var columnNames = new List<string>();
            var finalPremiumValues = new List<decimal?>();
            IEnumerable<IdstData> idstData = await GetIdstRenewalData(policyNo);
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

                var insuredAgeOne = Convert.ToInt32(row.txt_insured_age1);
                var insuredAgeTwo = Convert.ToInt32(row.txt_insured_age2);            
                var insuredAgeThree = Convert.ToInt32(row.txt_insured_age3);
                var insuredAgeFour = Convert.ToInt32(row.txt_insured_age4);
                var insuredAgeFive = Convert.ToInt32(row.txt_insured_age5);
                var insuredAgeSix = Convert.ToInt32(row.txt_insured_age6);
                var zone = row.tier_type;

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
                    ageValues.Add(age); 
                }

                var noOfMembers = ageValues.Count(age => age > 0);
                int? eldestMember = ageValues.Max();
                int? count = noOfMembers;
                var numberOfMemberss = noOfMembers;
                string searchNoClaimDescText = "No Claim Discount";
                bool containsNoClaimDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searchNoClaimDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultLoyaltyDescText = containsNoClaimDescText ? 1 : 0;
                decimal? noClaimDiscountValue = containsNoClaimDescText ? 0.05m : 0.0m;
                var policyType = row.policy_type;
                string searcFamilyDescText = "Family Discount";
                bool containsFamilyDescText = policyLdDescValues.Any(desc => desc != null && desc.Contains(searcFamilyDescText, StringComparison.OrdinalIgnoreCase));
                decimal? resultSearchFamilyDescText = containsFamilyDescText ? 1 : 0;
                decimal? familyDiscountValue = GetFamilyDiscount(policyType, numberOfMemberss);
                var policyPeriod = row.policy_period;
                decimal longTermDiscount = GetLongTermDiscount(policyPeriod);
                var columnName = GetColumnNameForPolicyPeriod(policyPeriod);
                if (columnName == null)
                {
                    throw new ArgumentException($"Invalid policy period: {policyPeriod}");
                }
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
                decimal? insuredPremium1 = await GetInsuredPremium1(policyPeriod,0, sumInsuredList, ageValues, baseRateHashTable);
                decimal? insuredPremium2 = await GetInsuredPremium1(policyPeriod,1, sumInsuredList, ageValues, baseRateHashTable);
                decimal? insuredPremium3 = await GetInsuredPremium1(policyPeriod,2, sumInsuredList, ageValues, baseRateHashTable);
                decimal? insuredPremium4 = await GetInsuredPremium1(policyPeriod,3, sumInsuredList, ageValues, baseRateHashTable);
                decimal? insuredPremium5 = await GetInsuredPremium1(policyPeriod,4, sumInsuredList, ageValues, baseRateHashTable);
                decimal? insuredPremium6 = await GetInsuredPremium1(policyPeriod,5, sumInsuredList, ageValues, baseRateHashTable);
                var insuredPremiumvalues = new List<decimal?> { insuredPremium1, insuredPremium2, insuredPremium3, insuredPremium4, insuredPremium5, insuredPremium6 };
                string condition = policyType;
                decimal? basePremium = GetBasePremium(condition, insuredPremiumvalues);
                decimal? loadingPremInsured1 = GetBasePremLoadingInsured1(0, insuredPremiumvalues, basicLoadingRates);
                decimal? loadingPremInsured2 = GetBasePremLoadingInsured1(1, insuredPremiumvalues, basicLoadingRates);
                decimal? loadingPremInsured3 = GetBasePremLoadingInsured1(2, insuredPremiumvalues, basicLoadingRates);
                decimal? loadingPremInsured4 = GetBasePremLoadingInsured1(3, insuredPremiumvalues, basicLoadingRates);
                decimal? loadingPremInsured5 = GetBasePremLoadingInsured1(4, insuredPremiumvalues, basicLoadingRates);
                decimal? loadingPremInsured6 = GetBasePremLoadingInsured1(5, insuredPremiumvalues, basicLoadingRates);
                var loadingPremvalues = new List<decimal?> { loadingPremInsured1 + loadingPremInsured2 + loadingPremInsured3 + loadingPremInsured4 + loadingPremInsured5 + loadingPremInsured6 };
                decimal? loadingPremium = (loadingPremvalues.Sum()) ?? 0;
                decimal? optimaseniorBasePremiumwithLoading = (basePremium + loadingPremium) ?? 0;
                decimal? optimaseniorNoClaimDiscount = (basePremium * noClaimDiscountValue) ?? 0;
                decimal? optimaseniorFamilyDiscount = (basePremium * familyDiscountValue) ?? 0;
                decimal? optimaseniorLongTermDiscount = (basePremium * longTermDiscount) ?? 0;
                decimal? optimaseniorFinalBasePremium = (optimaseniorBasePremiumwithLoading - (optimaseniorNoClaimDiscount + optimaseniorFamilyDiscount + optimaseniorLongTermDiscount)) ?? 0;
                decimal? netPremium = optimaseniorFinalBasePremium;
                decimal? GST = (netPremium * 0.18m) ?? 0;
                decimal? finalPremium = (netPremium + GST) ?? 0;
                decimal? Crosscheck = finalPremium - row.num_tot_premium;

                os = new OptimaSeniorRNE
                {
                    prod_code = row.prod_code,
                    prod_name = row.prod_name,
                    reference_num = row.reference_num,
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
                    txt_dealer_cd = row.txt_dealer_cd,
                    imdname = row.imdname,
                    verticalname = row.verticalname,
                    txt_family = row.txt_family,
                    isrnflag = row.isrnflag,
                    split_flag = row.split_flag,
                    isvipflag = row.isvipflag,
                    no_of_members = noOfMembers,
                    txt_insured_entrydate1 = row.txt_insured_entrydate1,
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
                    insured_loadingper1 = row.insured_loadingper1,
                    insured_loadingper2 = row.insured_loadingper2,
                    insured_loadingper3 = row.insured_loadingper3,
                    insured_loadingper4 = row.insured_loadingper4,
                    insured_loadingper5 = row.insured_loadingper5,
                    insured_loadingper6 = row.insured_loadingper6,
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
                    txt_insured_relation1 = row.txt_insured_relation1,
                    txt_insured_relation2 = row.txt_insured_relation2,
                    txt_insured_relation3 = row.txt_insured_relation3,
                    txt_insured_relation4 = row.txt_insured_relation4,
                    txt_insured_relation5 = row.txt_insured_relation5,
                    txt_insured_relation6 = row.txt_insured_relation6,
                    pre_existing_disease1 = row.pre_existing_disease1,
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
                    wellness_discount1 = row.wellness_discount1,
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
                    coverbaseloadingrate1 = row.coverbaseloadingrate1,
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
                return 0.05m; 
            }
            else
            {
                return 0.00m; 
            }
        }

        public static decimal GetLongTermDiscount(string policyPeriod)
        {
            if (policyPeriod == "2 Years")
            {
                return 0.075m;
            }
            else if (policyPeriod == "3 Years")
            {
                return 0.10m; 
            }
            else
            {
                return 0.00m; 
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

        public async Task<decimal?> GetInsuredPremium1(string policyperiod, int i, List<decimal?> sumInsuredList, List<int?> ageValues, Dictionary<string, Hashtable> baseRateHashTable)
        {
            if (sumInsuredList == null || ageValues == null || sumInsuredList.Count == 0 || ageValues.Count == 0)
            {
                return 0;
            }
            var column = GetColumnNameForPolicyPeriod(policyperiod);
            decimal? basePremium = 0;
            if (i >= 0 && i < sumInsuredList.Count && i < ageValues.Count)
            {
                var insuredPrem = baseRateHashTable
                    .Where(row =>
                        row.Value is Hashtable rateDetails &&
                        (int)rateDetails["suminsured"] == sumInsuredList[i] &&
                        (int)rateDetails["age"] == ageValues[i])
                    .Select(row =>
                        row.Value is Hashtable details &&
                        details[column] != null
                            ? Convert.ToDecimal(details[column])
                            : (decimal?)null)
                    .FirstOrDefault();

                if (insuredPrem.HasValue)
                {
                    basePremium += insuredPrem.Value;
                }
            }
            return basePremium ?? 0;
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

        static decimal? GetBasePremLoadingInsured1(int i, List<decimal?> basePremiumList, List<decimal?> baseLoadingRates)
        {
            if (basePremiumList != null && baseLoadingRates != null &&
                    i >= 0 && i < basePremiumList.Count && i < baseLoadingRates.Count)
            {
                return basePremiumList[i] * baseLoadingRates[i] / 100;
            }
            else
            {
                return 0;
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

        public List<List<string>> FetchNewBatchIds(NpgsqlConnection postgresConnection)
        {
            string? status = ConfigurationManager.AppSettings["Status"];
            var sqlSource = $"SELECT distinct ir.certificate_no, ir.product_code FROM ins.idst_renewal_data_rgs ir " +
                $"INNER JOIN ins.rne_healthtab ht" +
                $" ON ir.certificate_no = ht.policy_number " +
                $"LEFT JOIN ins.premium_validation pt3 ON ir.certificate_no = pt3.certificate_no " +
                $"WHERE ir.rn_generation_status = @Status AND  ht.prod_code in (2804, 2861, 2813)" +
                $" AND pt3.rn_generation_status IS NULL ";
            var sourceResults = postgresConnection.Query(sqlSource, new { Status = status });
            var sourceBatchIds = new List<List<string>>();
            foreach (var result in sourceResults)
            {
                var batchInfo = new List<string> { result.certificate_no, result.product_code.ToString() };
                sourceBatchIds.Add(batchInfo);
            }
            return sourceBatchIds;
        }

        public class IdstData
        {
            public string certificate_no { get; set; }
            public decimal? loading_per_insured1 { get; set; }
            public decimal? loading_per_insured2 { get; set; }
            public decimal? loading_per_insured3 { get; set; }
            public decimal? loading_per_insured4 { get; set; }
            public decimal? loading_per_insured5 { get; set; }
            public decimal? loading_per_insured6 { get; set; }
        }
        async Task HandleCrosschecksAndUpdateStatus(string policyNo, OptimaSeniorRNE osRNEData, decimal? crosscheck1, decimal? netPremium, decimal? finalPremium, decimal? gst)
        {
            string? connectionString = ConfigurationManager.ConnectionStrings["PostgresDb"].ConnectionString;
            using (IDbConnection dbConnection = new NpgsqlConnection(connectionString))
            {
                dbConnection.Open();
                var record = dbConnection.QueryFirstOrDefault<premium_validation>(
                    "SELECT certificate_no FROM ins.premium_validation WHERE certificate_no = @CertificateNo",
                    new { CertificateNo = osRNEData.policy_number.ToString() });
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
        public async Task<Dictionary<string, Hashtable>> GetRatesAsync(HDFCDbContext dbContext)
        {
            var ratesTable = new Dictionary<string, Hashtable>();
            var rates = await dbContext.optimasenior_baserates.ToListAsync().ConfigureAwait(false);
            foreach (var rate in rates)
            {
                var compositeKey = $"{rate.suminsured}-{rate.age}";
                var rateDetails = new Hashtable
                {
                    { "suminsured", rate.suminsured },
                    { "age", rate.age },
                    { "age_band", rate.age_band },
                    { "one_year", rate.one_year },
                    { "two_years", rate.two_years },
                    { "three_years", rate.three_years }
                };
                ratesTable[compositeKey] = rateDetails;
            }
            return ratesTable;
        }
    }
}
