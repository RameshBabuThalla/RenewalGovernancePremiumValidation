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
using static RenewalGovernancePremiumValidation.BussinessLogic.OptimaSenior;
using System.Collections;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.Common;
namespace RenewalGovernancePremiumValidation.BussinessLogic
{

    public class OptimaVital
    {
        private readonly HDFCDbContext dbContext;
        private readonly ILogger<OptimaVital> _logger;
        public OptimaVital(HDFCDbContext hDFCDbContext, ILogger<OptimaVital> logger)
        {
            //this.dbContext = hDFCDbContext;
            _logger = logger;
        }
        public async Task<List<OptimaVitalRNE>> GetOptimaVitalDataAsync(string policyNo)
        {
            string? connectionString = ConfigurationManager.ConnectionStrings["PostgresDb"].ConnectionString;

            string sqlQuery = @"
        SELECT 
      ov.prod_code,
       ov.batchid,
        ov.prod_name,
        ov.reference_num,
        ov.policy_number,
                ov.policy_start_date,
                 ov.policy_expiry_date,
                 ov.policy_period,
                 ov.tier_type,
                 ov.policyplan,
                 ov.policy_type,
                ov.txt_family,
               ov.num_tot_premium,
               ov.num_net_premium,
              ov.num_service_tax,
               ov.coverbaseloadingrate1,
              ov.coverbaseloadingrate2,
               ov.coverbaseloadingrate3,
              ov.coverbaseloadingrate4,
               ov.coverbaseloadingrate5,
               ov.coverbaseloadingrate6,
              ovidst.loading_per_insured1,
               ovidst.loading_per_insured2,
             ovidst.loading_per_insured3,
            ovidst.loading_per_insured4,
             ovidst.loading_per_insured5,
           ovidst.loading_per_insured6,
             ov.txt_insuredname1,
                ov.txt_insuredname2,
ov.txt_insuredname3,
ov.txt_insuredname4,
ov.txt_insuredname5,
ov.txt_insuredname6,
ov.txt_insured_dob1,
ov.txt_insured_dob2,
ov.txt_insured_dob3,
ov.txt_insured_dob4,
ov.txt_insured_dob5,
ov.txt_insured_dob6,
ov.txt_insured_relation1,
ov.txt_insured_relation2,
ov.txt_insured_relation3,
ov.txt_insured_relation4,
ov.txt_insured_relation5,
ov.txt_insured_relation6,
ov.txt_insured_age1,
ov.txt_insured_age2,
ov.txt_insured_age3,
ov.txt_insured_age4,
ov.txt_insured_age5,
ov.txt_insured_age6,
ov.txt_insured_gender1,
ov.txt_insured_gender2,
ov.txt_insured_gender3,
ov.txt_insured_gender4,
ov.txt_insured_gender5,
ov.txt_insured_gender6,
ov.pollddesc1,
ov.pollddesc2,
ov.pollddesc3,
ov.pollddesc4,
ov.pollddesc5,
ov.sum_insured1,
ov.sum_insured2,
ov.sum_insured3,
ov.sum_insured4,
ov.sum_insured5,
ov.sum_insured6,
ov.insured_cb1,
ov.insured_cb2,
ov.insured_cb3,
ov.insured_cb4,
ov.insured_cb5,
ov.insured_cb6,
ov.premium_insured1,
ov.premium_insured2,
ov.premium_insured3,
ov.premium_insured4,
ov.premium_insured5,
ov.premium_insured6,
ov.covername11,
ov.covername12,
ov.covername13,
ov.covername14,
ov.covername15,
ov.covername16,
ov.covername17,
ov.covername18,
ov.covername19,
ov.covername21,
ov.covername22,
ov.covername23,
ov.covername24,
ov.covername25,
ov.covername26,
ov.covername27,
ov.covername28,
ov.covername29,
ov.covername31,
ov.covername32,
ov.covername33,
ov.covername34,
ov.covername35,
ov.covername36,
ov.covername37,
ov.covername38,
ov.covername39,
ov.covername41,
ov.covername42,
ov.covername43,
ov.covername44,
ov.covername45,
ov.covername46,
ov.covername47,
ov.covername48,
ov.covername49,
ov.covername51,
ov.covername52,
ov.covername53,
ov.covername54,
ov.covername55,
ov.covername56,
ov.covername57,
ov.covername58,
ov.covername59,
ov.covername61,
ov.covername62,
ov.covername63,
ov.covername64,
ov.covername65,
ov.covername66,
ov.covername67,
ov.covername68,
ov.covername69,
ov.covername101,
ov.covername102,
ov.covername103,
ov.covername104,
ov.covername105,
ov.covername106,
ov.covername107,
ov.covername108,
ov.covername109,
ov.covername110,
ov.covername210,
ov.covername310,
ov.covername410,
ov.covername510,
ov.covername610,
ov.covername710,
ov.covername810,
ov.covername910,
ov.covername1010,
ov.coversi11,
ov.coversi12,
ov.coversi13,
ov.coversi14,
ov.coversi15,
ov.coversi16,
ov.coversi17,
ov.coversi18,
ov.coversi19,
ov.coversi21,
ov.coversi22,
ov.coversi23,
ov.coversi24,
ov.coversi25,
ov.coversi26,
ov.coversi27,
ov.coversi28,
ov.coversi29,
ov.coversi31,
ov.coversi32,
ov.coversi33,
ov.coversi34,
ov.coversi35,
ov.coversi36,
ov.coversi37,
ov.coversi38,
ov.coversi39,
ov.coversi41,
ov.coversi42,
ov.coversi43,
ov.coversi44,
ov.coversi45,
ov.coversi46,
ov.coversi47,
ov.coversi48,
ov.coversi49,
ov.coversi51,
ov.coversi52,
ov.coversi53,
ov.coversi54,
ov.coversi55,
ov.coversi56,
ov.coversi57,
ov.coversi58,
ov.coversi59,
ov.coversi61,
ov.coversi62,
ov.coversi63,
ov.coversi64,
ov.coversi65,
ov.coversi66,
ov.coversi67,
ov.coversi68,
ov.coversi69,
ov.coversi101,
ov.coversi102,
ov.coversi103,
ov.coversi104,
ov.coversi105,
ov.coversi106,
ov.coversi107,
ov.coversi108,
ov.coversi109,
ov.coversi210,
ov.coversi310,
ov.coversi410,
ov.coversi510,
ov.coversi610,
ov.coversi810,
ov.coversi910,
ov.coversi1010,
ov.coverprem11,
ov.coverprem12,
ov.coverprem13,
ov.coverprem14,
ov.coverprem15,
ov.coverprem16,
ov.coverprem17,
ov.coverprem18,
ov.coverprem19,
ov.coverprem21,
ov.coverprem22,
ov.coverprem23,
ov.coverprem24,
ov.coverprem25,
ov.coverprem26,
ov.coverprem27,
ov.coverprem28,
ov.coverprem29,
ov.coverprem31,
ov.coverprem32,
ov.coverprem33,
ov.coverprem34,
ov.coverprem35,
ov.coverprem36,
ov.coverprem37,
ov.coverprem38,
ov.coverprem39,
ov.coverprem41,
ov.coverprem42,
ov.coverprem43,
ov.coverprem44,
ov.coverprem46,
ov.coverprem47,
ov.coverprem48,
ov.coverprem49,
ov.coverprem51,
ov.coverprem52,
ov.coverprem53,
ov.coverprem54,
ov.coverprem55,
ov.coverprem56,
ov.coverprem57,
ov.coverprem58,
ov.coverprem59,
ov.coverprem61,
ov.coverprem62,
ov.coverprem63,
ov.coverprem64,
ov.coverprem65,
ov.coverprem66,
ov.coverprem67,
ov.coverprem68,
ov.coverprem69,
ov.coverprem101,
ov.coverprem102,
ov.coverprem103,
ov.coverprem104,
ov.coverprem105,
ov.coverprem106,
ov.coverprem107,
ov.coverprem108,
ov.coverprem109,
ov.coverprem210,
ov.coverprem310,
ov.coverprem410,
ov.coverprem510,
ov.coverprem610,
ov.coverprem810,
ov.coverprem910,
ov.coverprem1010,
ov.insured_loadingamt1,
ov.insured_loadingamt2,
ov.insured_loadingamt3,
ov.insured_loadingamt4,
ov.insured_loadingamt5,
ov.insured_loadingamt6
         FROM 
            ins.rne_healthtab ov
        INNER JOIN 
            ins.idst_renewal_data_rgs ovidst ON ov.policy_number = ovidst.certificate_no
        WHERE 
            ov.policy_number = @PolicyNo";
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    var optimavitalData = await connection.QueryAsync<OptimaVitalRNE>(sqlQuery, new { PolicyNo = policyNo }).ConfigureAwait(false);
                    return new List<OptimaVitalRNE>(optimavitalData);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while fetching GC data for policy: {PolicyNo}", policyNo);
                return new List<OptimaVitalRNE>();
            }
        }

        public async Task<OptimaVitalValidationResult> GetOptimaVitalValidation(string policyNo, Dictionary<string, Hashtable> baseRateHashTable)
        {
            List<OptimaVitalRNE> ovRNEData;
            IEnumerable<OptimaVitalRNE> optimavitalRNE = Enumerable.Empty<OptimaVitalRNE>();
            ovRNEData = await GetOptimaVitalDataAsync(policyNo);
            optimavitalRNE = await CalculateOptimaVitalPremium(policyNo,ovRNEData, baseRateHashTable);
            OptimaVitalValidationResult objOptimaVitalValidation = new OptimaVitalValidationResult
            {
                prod_code = ovRNEData.FirstOrDefault()?.prod_code,
                prod_name = ovRNEData.FirstOrDefault()?.prod_name,
                reference_num = ovRNEData.FirstOrDefault()?.reference_num,
                policy_number = ovRNEData.FirstOrDefault()?.policy_number,
                txt_family = ovRNEData.FirstOrDefault()?.txt_family,
                no_of_members = ovRNEData.FirstOrDefault()?.no_of_members,
                insured_loadingper1 = ovRNEData.FirstOrDefault()?.insured_loadingper1,
                insured_loadingper2 = ovRNEData.FirstOrDefault()?.insured_loadingper2,
                insured_loadingper3 = ovRNEData.FirstOrDefault()?.insured_loadingper3,
                insured_loadingper4 = ovRNEData.FirstOrDefault()?.insured_loadingper4,
                insured_loadingper5 = ovRNEData.FirstOrDefault()?.insured_loadingper5,
                insured_loadingper6 = ovRNEData.FirstOrDefault()?.insured_loadingper6,
                insured_loadingamt1 = ovRNEData.FirstOrDefault()?.insured_loadingamt1,
                insured_loadingamt2 = ovRNEData.FirstOrDefault()?.insured_loadingamt2,
                insured_loadingamt3 = ovRNEData.FirstOrDefault()?.insured_loadingamt3,
                insured_loadingamt4 = ovRNEData.FirstOrDefault()?.insured_loadingamt4,
                insured_loadingamt5 = ovRNEData.FirstOrDefault()?.insured_loadingamt5,
                insured_loadingamt6 = ovRNEData.FirstOrDefault()?.insured_loadingamt6,
                txt_insuredname1 = ovRNEData.FirstOrDefault()?.txt_insuredname1,
                txt_insuredname2 = ovRNEData.FirstOrDefault()?.txt_insuredname2,
                txt_insuredname3 = ovRNEData.FirstOrDefault()?.txt_insuredname3,
                txt_insuredname4 = ovRNEData.FirstOrDefault()?.txt_insuredname4,
                txt_insuredname5 = ovRNEData.FirstOrDefault()?.txt_insuredname5,
                txt_insuredname6 = ovRNEData.FirstOrDefault()?.txt_insuredname6,
                txt_insured_dob1 = ovRNEData.FirstOrDefault()?.txt_insured_dob1,
                txt_insured_dob2 = ovRNEData.FirstOrDefault()?.txt_insured_dob2,
                txt_insured_dob3 = ovRNEData.FirstOrDefault()?.txt_insured_dob3,
                txt_insured_dob4 = ovRNEData.FirstOrDefault()?.txt_insured_dob4,
                txt_insured_dob5 = ovRNEData.FirstOrDefault()?.txt_insured_dob5,
                txt_insured_dob6 = ovRNEData.FirstOrDefault()?.txt_insured_dob6,
                txt_insured_age1 = ovRNEData.FirstOrDefault()?.txt_insured_age1,
                txt_insured_age2 = ovRNEData.FirstOrDefault()?.txt_insured_age2,
                txt_insured_age3 = ovRNEData.FirstOrDefault()?.txt_insured_age3,
                txt_insured_age4 = ovRNEData.FirstOrDefault()?.txt_insured_age4,
                txt_insured_age5 = ovRNEData.FirstOrDefault()?.txt_insured_age5,
                txt_insured_age6 = ovRNEData.FirstOrDefault()?.txt_insured_age6,
                txt_insured_relation1 = ovRNEData.FirstOrDefault()?.txt_insured_relation1,
                txt_insured_relation2 = ovRNEData.FirstOrDefault()?.txt_insured_relation2,
                txt_insured_relation3 = ovRNEData.FirstOrDefault()?.txt_insured_relation3,
                txt_insured_relation4 = ovRNEData.FirstOrDefault()?.txt_insured_relation4,
                txt_insured_relation5 = ovRNEData.FirstOrDefault()?.txt_insured_relation5,
                txt_insured_relation6 = ovRNEData.FirstOrDefault()?.txt_insured_relation6,
                insured_cb1 = ovRNEData.FirstOrDefault()?.insured_cb1,
                insured_cb2 = ovRNEData.FirstOrDefault()?.insured_cb2,
                insured_cb3 = ovRNEData.FirstOrDefault()?.insured_cb3,
                insured_cb4 = ovRNEData.FirstOrDefault()?.insured_cb4,
                insured_cb5 = ovRNEData.FirstOrDefault()?.insured_cb5,
                insured_cb6 = ovRNEData.FirstOrDefault()?.insured_cb6,
                sum_insured1 = ovRNEData.FirstOrDefault()?.sum_insured1,
                sum_insured2 = ovRNEData.FirstOrDefault()?.sum_insured2,
                sum_insured3 = ovRNEData.FirstOrDefault()?.sum_insured3,
                sum_insured4 = ovRNEData.FirstOrDefault()?.sum_insured4,
                sum_insured5 = ovRNEData.FirstOrDefault()?.sum_insured5,
                sum_insured6 = ovRNEData.FirstOrDefault()?.sum_insured6,
                coverbaseloadingrate1 = ovRNEData.FirstOrDefault()?.coverbaseloadingrate1,
                coverbaseloadingrate2 = ovRNEData.FirstOrDefault()?.coverbaseloadingrate2,
                coverbaseloadingrate3 = ovRNEData.FirstOrDefault()?.coverbaseloadingrate3,
                coverbaseloadingrate4 = ovRNEData.FirstOrDefault()?.coverbaseloadingrate4,
                coverbaseloadingrate5 = ovRNEData.FirstOrDefault()?.coverbaseloadingrate5,
                coverbaseloadingrate6 = ovRNEData.FirstOrDefault()?.coverbaseloadingrate6,
                policy_start_date = ovRNEData.FirstOrDefault()?.policy_start_date,
                policy_expiry_date = ovRNEData.FirstOrDefault()?.policy_expiry_date,
                policy_type = ovRNEData.FirstOrDefault()?.policy_type,
                policy_period = ovRNEData.FirstOrDefault()?.policy_period,
                policyplan = ovRNEData.FirstOrDefault()?.policyplan,
                tier_type = ovRNEData.FirstOrDefault()?.tier_type,
                longterm_discount = ovRNEData.FirstOrDefault()?.longterm_discount,
                insured_Premium1 = ovRNEData.FirstOrDefault()?.insured_Premium1,
                insured_Premium2 = ovRNEData.FirstOrDefault()?.insured_Premium2,
                insured_Premium3 = ovRNEData.FirstOrDefault()?.insured_Premium3,
                insured_Premium4 = ovRNEData.FirstOrDefault()?.insured_Premium4,
                insured_Premium5 = ovRNEData.FirstOrDefault()?.insured_Premium5,
                insured_Premium6 = ovRNEData.FirstOrDefault()?.insured_Premium6,
                base_Premium = ovRNEData.FirstOrDefault()?.base_Premium,
                loading_Premium1 = ovRNEData.FirstOrDefault()?.loading_Premium1,
                loading_Premium2 = ovRNEData.FirstOrDefault()?.loading_Premium2,
                loading_Premium3 = ovRNEData.FirstOrDefault()?.loading_Premium3,
                loading_Premium4 = ovRNEData.FirstOrDefault()?.loading_Premium4,
                loading_Premium5 = ovRNEData.FirstOrDefault()?.loading_Premium5,
                loading_Premium6 = ovRNEData.FirstOrDefault()?.loading_Premium6,
                base_loading_Premium = ovRNEData.FirstOrDefault()?.base_loading_Premium,
                optimaVital_FinalBasePremium_Loading = ovRNEData.FirstOrDefault()?.optimaVital_FinalBasePremium_Loading,
                optimaVital_LongTerm_Discount = ovRNEData.FirstOrDefault()?.optimaVital_LongTerm_Discount,
                optimaVital_LongTerm_Amount = ovRNEData.FirstOrDefault()?.optimaVital_LongTerm_Amount,
                net_premium = ovRNEData.FirstOrDefault()?.net_premium.HasValue == true
              ? (decimal?)Math.Round(ovRNEData.FirstOrDefault().net_premium.Value, 2)
              : (decimal?)0,
                final_Premium = ovRNEData.FirstOrDefault()?.final_Premium.HasValue == true
              ? (decimal?)Math.Round(ovRNEData.FirstOrDefault().final_Premium.Value, 2)
              : (decimal?)0,
                gst = ovRNEData.FirstOrDefault()?.gst.HasValue == true
              ? (decimal?)Math.Round(ovRNEData.FirstOrDefault().gst.Value, 2)
              : (decimal?)0,
                cross_Check = ovRNEData.FirstOrDefault()?.cross_Check.HasValue == true
              ? (decimal?)Math.Round(ovRNEData.FirstOrDefault().cross_Check.Value, 2)
              : (decimal?)0,
                optimavital_total_Premium = ovRNEData.FirstOrDefault()?.optimavital_total_Premium.HasValue == true
              ? (decimal?)Math.Round(ovRNEData.FirstOrDefault().optimavital_total_Premium.Value, 2)
              : (decimal?)0,
                optimavital_netpremium = ovRNEData.FirstOrDefault()?.optimavital_netpremium.HasValue == true
              ? (decimal?)Math.Round(ovRNEData.FirstOrDefault().optimavital_netpremium.Value, 2)
              : (decimal?)0,
                optimavital_gst = ovRNEData.FirstOrDefault()?.optimavital_gst.HasValue == true
              ? (decimal?)Math.Round(ovRNEData.FirstOrDefault().optimavital_gst.Value, 2)
              : (decimal?)0,
            };
            decimal? crosscheck1 = optimavitalRNE.FirstOrDefault()?.cross_Check;
            decimal? netPremium = optimavitalRNE.FirstOrDefault()?.net_premium ?? 0;
            decimal? finalPremium = optimavitalRNE.FirstOrDefault()?.final_Premium ?? 0;
            decimal? gst = optimavitalRNE.FirstOrDefault()?.gst ?? 0;

            if (objOptimaVitalValidation?.policy_number == null)
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
                    if (ovRNEData.FirstOrDefault()?.insured_cb1 == string.Empty && ovRNEData.FirstOrDefault()?.insured_cb1 == null)
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
                            await HandleCrosschecksAndUpdateStatus(policyNo, ovRNEData.FirstOrDefault(), crosscheck1Value, netPremium, finalPremium, gst);
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

                if (objOptimaVitalValidation?.policy_number == null)
                {
                    Console.WriteLine("Policy number not found.");
                }

                if (objOptimaVitalValidation != null)
                {
                    var no_of_members = optimavitalRNE.FirstOrDefault()?.no_of_members;
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
                    var insertQuery = @"
                                INSERT INTO ins.rne_calculated_cover_rg (policy_number, referencenum, suminsured, premium, totalpremium, riskname, covername)
                                VALUES (@policy_number, @referencenum, @suminsured, @premium, @totalpremium, @riskname, @covername);
                                ";

                    await dbConnection.ExecuteAsync(insertQuery, newRecord).ConfigureAwait(false);
                }
            }

            return objOptimaVitalValidation;
        }
        private async Task<IEnumerable<OptimaVitalRNE>> CalculateOptimaVitalPremium(string policyNo,IEnumerable<OptimaVitalRNE> ovRNEData, Dictionary<string, Hashtable> baseRateHashTable)
        {
            OptimaVitalRNE ov = null;
            var columnNames = new List<string>();
            var finalPremiumValues = new List<decimal?>();
            IEnumerable<IdstData> idstData = await GetIdstRenewalData(policyNo);
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
                    ageValues.Add(age);
                }
                var noOfMembers = ageValues.Count(age => age > 0);
                int? eldestMember = ageValues.Max();
                int? count = noOfMembers;
                var numberOfMemberss = noOfMembers;
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
                decimal? insuredPremium1 = await GetInsuredPremium1(policyPeriod, 0,sumInsuredList, ageValues, baseRateHashTable);
                decimal? insuredPremium2 = await GetInsuredPremium1(policyPeriod, 1,sumInsuredList, ageValues, baseRateHashTable);
                decimal? insuredPremium3 = await GetInsuredPremium1(policyPeriod, 2,sumInsuredList, ageValues, baseRateHashTable);
                decimal? insuredPremium4 = await GetInsuredPremium1(policyPeriod, 3,sumInsuredList, ageValues, baseRateHashTable);
                decimal? insuredPremium5 = await GetInsuredPremium1(policyPeriod, 4,sumInsuredList, ageValues, baseRateHashTable);
                decimal? insuredPremium6 = await GetInsuredPremium1(policyPeriod, 5,sumInsuredList, ageValues, baseRateHashTable);

                var insuredPremiumvalues = new List<decimal?> { insuredPremium1, insuredPremium2, insuredPremium3, insuredPremium4, insuredPremium5, insuredPremium6 };
                var policyType = row.policy_type;
                string condition = policyType;
                decimal? basePremium = GetBasePremium(condition, insuredPremiumvalues);
                decimal? loadingPremInsured1 = GetBasePremLoadingInsured1(0, insuredPremiumvalues, basicLoadingRates);
                decimal? loadingPremInsured2 = GetBasePremLoadingInsured1(0, insuredPremiumvalues, basicLoadingRates);
                decimal? loadingPremInsured3 = GetBasePremLoadingInsured1(0, insuredPremiumvalues, basicLoadingRates);
                decimal? loadingPremInsured4 = GetBasePremLoadingInsured1(0, insuredPremiumvalues, basicLoadingRates);
                decimal? loadingPremInsured5 = GetBasePremLoadingInsured1(0, insuredPremiumvalues, basicLoadingRates);
                decimal? loadingPremInsured6 = GetBasePremLoadingInsured1(0, insuredPremiumvalues, basicLoadingRates);
                var loadingPremvalues = new List<decimal?> { loadingPremInsured1 + loadingPremInsured2 + loadingPremInsured3 + loadingPremInsured4 + loadingPremInsured5 + loadingPremInsured6 };
                decimal? baseLoadingPremium = (loadingPremvalues.Sum()) ?? 0;

                decimal? optimavitalFinalBasePremiumLoading = (basePremium + baseLoadingPremium) ?? 0;
                decimal? optimavitalLongTermDiscount = GetOVLongTermDiscount(policyPeriod);
                decimal? optimavitalLongTermDiscountAmount = (optimavitalFinalBasePremiumLoading * optimavitalLongTermDiscount) ?? 0;

                decimal? netPremium = (optimavitalFinalBasePremiumLoading - optimavitalLongTermDiscountAmount) ?? 0;
                decimal? GST = (netPremium * 0.18m) ?? 0;
                decimal? finalPremium = (netPremium + GST) ?? 0;
                decimal? Crosscheck = finalPremium - row.num_tot_premium;
                ov = new OptimaVitalRNE
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
                    coverbaseloadingrate1 = row.coverbaseloadingrate1,
                    coverbaseloadingrate2 = row.coverbaseloadingrate2,
                    coverbaseloadingrate3 = row.coverbaseloadingrate3,
                    coverbaseloadingrate4 = row.coverbaseloadingrate4,
                    coverbaseloadingrate5 = row.coverbaseloadingrate5,
                    coverbaseloadingrate6 = row.coverbaseloadingrate6,
                    policy_start_date = row.policy_start_date,
                    policy_expiry_date = row.policy_expiry_date,
                    policy_type = row.policy_type,
                    policy_period = row.policy_period,
                    policyplan = row.policyplan,
                    claimcount = row.claimcount,
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
                    net_premium = netPremium.HasValue ? Math.Round(netPremium.Value, 2) : (decimal?)0,
                    final_Premium = finalPremium.HasValue ? Math.Round(finalPremium.Value, 2) : (decimal?)0,
                    gst = GST.HasValue ? Math.Round(GST.Value, 2) : (decimal?)0,
                    cross_Check = Crosscheck.HasValue ? Math.Round(Crosscheck.Value, 2) : (decimal?)0,
                    optimavital_total_Premium = row.num_tot_premium.HasValue ? Math.Round(row.num_tot_premium.Value, 2) : (decimal?)0,
                    optimavital_netpremium = row.num_net_premium.HasValue ? Math.Round(row.num_net_premium.Value, 2) : (decimal?)0,
                    optimavital_gst = row.num_service_tax.HasValue ? Math.Round(row.num_service_tax.Value, 2) : (decimal?)0

                };
            }
            return new List<OptimaVitalRNE> { ov };
        }
        async Task HandleCrosschecksAndUpdateStatus(string policyNo, OptimaVitalRNE osRNEData, decimal? crosscheck1, decimal? netPremium, decimal? finalPremium, decimal? gst)
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
            if  ( i >= 0 && i < sumInsuredList.Count && i < ageValues.Count )
            { 
                var insuredPrem = baseRateHashTable
                    .Where(row =>
                        row.Value is Hashtable rateDetails &&
                        (int)rateDetails["si"] == sumInsuredList[i] &&
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
        public async Task<Dictionary<string, Hashtable>> GetRatesAsync(HDFCDbContext dbContext)
        {
            var ratesTable = new Dictionary<string, Hashtable>();
            var rates = await dbContext.optimavital_baserates.ToListAsync().ConfigureAwait(false);
            foreach (var rate in rates)
            {
                var compositeKey = $"{rate.si}-{rate.age}";
                var rateDetails = new Hashtable
                {
                    { "si", rate.si },
                    { "age", rate.age },
                    { "age_band", rate.age_band },
                    { "per_miles_rate", rate.per_miles_rate },
                    { "one_year", rate.one_year },
                    { "two_years", rate.two_years }
                };
                ratesTable[compositeKey] = rateDetails;
            }
            return ratesTable;
        }
    }
}
