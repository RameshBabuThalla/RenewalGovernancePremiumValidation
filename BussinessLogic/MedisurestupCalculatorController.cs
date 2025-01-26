using RenewalGovernancePremiumValidation.Data;
using RenewalGovernancePremiumValidation.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Npgsql;
using Serilog.Core;
using System.Data;
using System.Reflection;
using DataTable = System.Data.DataTable;

namespace RenewalGovernancePremiumValidation.BussinessLogic
{
    public class MedisurestupCalculator
    {
        private readonly HDFCDbContext dbContext;
        private readonly ILogger<MedisurestupCalculator> _logger;

        public MedisurestupCalculator(HDFCDbContext hDFCDbContext, ILogger<MedisurestupCalculator> logger)
        {
            this.dbContext = hDFCDbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<MedisureStupRNE>> GetMedisureStupValidation(string policyNo)
        {
            //step 1 get data from gcz
            _logger.LogInformation("GetMedisureStupValidation is Started!");
            var msRNEData = await GetGCMedisureStupDataAsync(policyNo, 1, 1000);
            //step 2 calculation           
            var result = await CalculateMedisureStupPremium(msRNEData);
            _logger.LogInformation("MedisureStupValidation is Started!");
            return (result);
        }
        private async Task<IEnumerable<MedisureStupRNE>> GetGCMedisureStupDataAsync(string policyNo, int pageNumber, int pageSize)
        {
            // Query to get data from GC to calculate optima secure premium
            var msRNEData = await (
            from ms in dbContext.tab_health_renewalnotice
            where (ms.policy_number == policyNo)
            select new MedisureStupRNE
            {
                s_no = ms.s_no,
                product_code = ms.product_code,
                product_name = ms.product_name,
                batch_id = ms.batch_id,
                prod_name = ms.prod_name,
                policy_number = ms.policy_number,
                cust_id = ms.cust_id,
                policy_start_date = ms.policy_start_date,
                policy_expiry_date = ms.policy_expiry_date,
                policy_end_date = ms.policy_end_date,
                policy_period = ms.policy_period,
                tier_type = ms.tier_type,//tier in old gc mapping
                policy_type = ms.policy_type,
                txt_family = ms.txt_family,//family size in old gc mapping


                total_premium = ms.total_premium,
                num_service_tax = ms.num_service_tax,
                num_upsell_si = ms.num_upsell_si,
                num_upsell_premium = ms.num_upsell_premium,
                num_net_premium = ms.num_net_premium,
                total_si = ms.total_si,

                reference_date = ms.reference_date,
                location_code = ms.location_code,
                policy_holders_name = ms.policy_holders_name,
                txt_location = ms.txt_location,
                txt_apartment = ms.txt_apartment,
                txt_street = ms.txt_street,
                txt_areavillage = ms.txt_areavillage,
                txt_citydistrict = ms.txt_citydistrict,
                txt_state = ms.txt_state,
                txt_pincode = ms.txt_pincode,
                txt_mobile = ms.txt_mobile,
                txt_telephone = ms.txt_telephone,
                txt_off_extn = ms.txt_off_extn,
                txt_policyinsured = ms.txt_policyinsured,
                pincode_zone = ms.pincode_zone,

                dat_renewal_notice = ms.dat_renewal_notice,
                txt_dealer_cd = ms.txt_dealer_cd,
                verticale_name = ms.verticale_name,
                dat_insert_date = ms.dat_insert_date,
                sync_date = ms.sync_date,

                two_year_premium = ms.two_year_premium,
                two_year_net_prem = ms.two_year_net_prem,
                three_year_premium = ms.three_year_premium,

                flag = ms.flag,
                processed_flag = ms.processed_flag,
                batchprint_flag = ms.batchprint_flag,
                prod_category = ms.prod_category,
                txt_inland_flag = ms.txt_inland_flag,
                flag_upsell_applicable = ms.flag_upsell_applicable,
                txt_intermediary_flag = ms.txt_intermediary_flag,
                change_in_age_slab = ms.change_in_age_slab,
                batchprint_filename = ms.batchprint_filename,
                filenet_filename = ms.filenet_filename,
                vertical_recepient = ms.vertical_recepient,
                int_batchprint_filename = ms.int_batchprint_filename,
                int_filenet_filename = ms.int_filenet_filename,
                branch_name = ms.branch_name,
                qr_code = ms.qr_code,
                state_code = ms.state_code,
                state_regis_num = ms.state_regis_num,
                mobile_new = ms.mobile_new,
                telephone_new = ms.telephone_new,

                txt_insuredname1 = ms.txt_insuredname1,
                txt_insuredname2 = ms.txt_insuredname2,
                txt_insuredname3 = ms.txt_insuredname3,
                txt_insuredname4 = ms.txt_insuredname4,
                txt_insuredname5 = ms.txt_insuredname5,
                txt_insuredname6 = ms.txt_insuredname6,

                txt_insured_dob1 = ms.txt_insured_dob1,
                txt_insured_dob2 = ms.txt_insured_dob2,
                txt_insured_dob3 = ms.txt_insured_dob3,
                txt_insured_dob4 = ms.txt_insured_dob4,
                txt_insured_dob5 = ms.txt_insured_dob5,
                txt_insured_dob6 = ms.txt_insured_dob6,

                txt_insured_age1 = ms.txt_insured_age1,
                txt_insured_age2 = ms.txt_insured_age2,
                txt_insured_age3 = ms.txt_insured_age3,
                txt_insured_age4 = ms.txt_insured_age4,
                txt_insured_age5 = ms.txt_insured_age5,
                txt_insured_age6 = ms.txt_insured_age6,

                txt_insured_relation1 = ms.txt_insured_relation1,
                txt_insured_relation2 = ms.txt_insured_relation2,
                txt_insured_relation3 = ms.txt_insured_relation3,
                txt_insured_relation4 = ms.txt_insured_relation4,
                txt_insured_relation5 = ms.txt_insured_relation5,
                txt_insured_relation6 = ms.txt_insured_relation6,

                txt_insured_gender1 = ms.txt_insured_gender1,
                txt_insured_gender2 = ms.txt_insured_gender2,
                txt_insured_gender3 = ms.txt_insured_gender3,
                txt_insured_gender4 = ms.txt_insured_gender4,
                txt_insured_gender5 = ms.txt_insured_gender5,
                txt_insured_gender6 = ms.txt_insured_gender6,

                txt_insured_cb1 = ms.txt_insured_cb1,
                txt_insured_cb2 = ms.txt_insured_cb2,
                txt_insured_cb3 = ms.txt_insured_cb3,
                txt_insured_cb4 = ms.txt_insured_cb4,
                txt_insured_cb5 = ms.txt_insured_cb5,
                txt_insured_cb6 = ms.txt_insured_cb6,

                member_id1 = ms.member_id1,
                member_id2 = ms.member_id2,
                member_id3 = ms.member_id3,
                member_id4 = ms.member_id4,
                member_id5 = ms.member_id5,
                member_id6 = ms.member_id6,

                pre_existing_disease1 = ms.pre_existing_disease1,
                pre_existing_disease2 = ms.pre_existing_disease2,
                pre_existing_disease3 = ms.pre_existing_disease3,
                pre_existing_disease4 = ms.pre_existing_disease4,
                pre_existing_disease5 = ms.pre_existing_disease5,
                pre_existing_disease6 = ms.pre_existing_disease6,

                nominee_name1 = ms.nominee_name1,
                nominee_name2 = ms.nominee_name2,
                nominee_name3 = ms.nominee_name3,
                nominee_name4 = ms.nominee_name4,
                nominee_name5 = ms.nominee_name5,
                nominee_name6 = ms.nominee_name6,

                nominee_relationship1 = ms.nominee_relationship1,
                nominee_relationship2 = ms.nominee_relationship2,
                nominee_relationship3 = ms.nominee_relationship3,
                nominee_relationship4 = ms.nominee_relationship4,
                nominee_relationship5 = ms.nominee_relationship5,
                nominee_relationship6 = ms.nominee_relationship6,

                sum_insured_1 = ms.sum_insured_1,
                sum_insured_2 = ms.sum_insured_2,
                sum_insured_3 = ms.sum_insured_3,
                sum_insured_4 = ms.sum_insured_4,
                sum_insured_5 = ms.sum_insured_5,
                sum_insured_6 = ms.sum_insured_6,

                next_si_1 = ms.next_si_1,
                next_si_2 = ms.next_si_2,
                next_si_3 = ms.next_si_3,
                next_si_4 = ms.next_si_4,
                next_si_5 = ms.next_si_5,
                next_si_6 = ms.next_si_6,

                next_si_prm_11 = ms.next_si_prm_11,
                next_si_prm_12 = ms.next_si_prm_12,
                next_si_prm_21 = ms.next_si_prm_21,
                next_si_prm_22 = ms.next_si_prm_22,
                next_si_prm_31 = ms.next_si_prm_32,
                next_si_prm_32 = ms.next_si_prm_32,
                next_si_prm_41 = ms.next_si_prm_41,
                next_si_prm_42 = ms.next_si_prm_42,
                next_si_prm_51 = ms.next_si_prm_51,
                next_si_prm_52 = ms.next_si_prm_52,
                next_si_prm_61 = ms.next_si_prm_61,
                next_si_prm_62 = ms.next_si_prm_62,

                total_next_si_prm_1year = ms.total_next_si_prm_1year,
                total_next_si_prm_2year = ms.total_next_si_prm_2year,
                total_next_si = ms.total_next_si,

                basic_premium_insured1 = ms.basic_premium_insured1,
                basic_premium_insured2 = ms.basic_premium_insured2,
                basic_premium_insured3 = ms.basic_premium_insured3,
                basic_premium_insured4 = ms.basic_premium_insured4,
                basic_premium_insured5 = ms.basic_premium_insured5,
                basic_premium_insured6 = ms.basic_premium_insured6,

                net_premium_insured1 = ms.net_premium_insured1,
                net_premium_insured2 = ms.net_premium_insured2,
                net_premium_insured3 = ms.net_premium_insured3,
                net_premium_insured4 = ms.net_premium_insured4,
                net_premium_insured5 = ms.net_premium_insured5,
                net_premium_insured6 = ms.net_premium_insured6,

                insured_deductable1 = ms.insured_deductable1,
                insured_deductable2 = ms.insured_deductable2,
                insured_deductable3 = ms.insured_deductable3,
                insured_deductable4 = ms.insured_deductable4,
                insured_deductable5 = ms.insured_deductable5,
                insured_deductable6 = ms.insured_deductable6,

                other_loading = ms.other_loading,
                other_discount = ms.other_discount,
                family_discount = ms.family_discount,
                insured_risk_cover = ms.insured_risk_cover,

                txt_lms_id = ms.txt_lms_id,
                auto_pay = ms.auto_pay,

            }
            ).Skip((pageNumber - 1) * pageSize) // Skip the appropriate number of records
            .Take(pageSize) // Take only the specified page size
        .ToListAsync();
            return new List<MedisureStupRNE>(msRNEData);
        }

        private async Task<IEnumerable<MedisureStupRNE>> CalculateMedisureStupPremium(IEnumerable<MedisureStupRNE> msRNEData)
        {
            MedisureStupRNE ms = null;

            var columnNames = new List<string>();
            var finalPremiumValues = new List<decimal?>();
            foreach (var row in msRNEData)
            {
                var policNo16 = row.policy_number;
                DateTime policy_start_date = row.policy_start_date;
                DateTime policy_end_date = row.policy_end_date;
                TimeSpan dateDifference = policy_end_date - policy_start_date;
                double policyperiod = Math.Abs(dateDifference.TotalDays / 365.25);
                policyperiod = Math.Round(policyperiod);

                var insuredAgeOne = Convert.ToInt32(row.txt_insured_age1);
                var insuredAgeTwo = Convert.ToInt32(row.txt_insured_age2);
                var insuredAgeThree = Convert.ToInt32(row.txt_insured_age3);
                var insuredAgeFour = Convert.ToInt32(row.txt_insured_age4);
                var insuredAgeFive = Convert.ToInt32(row.txt_insured_age5);
                var insuredAgeSix = Convert.ToInt32(row.txt_insured_age6);


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

                var nonNullAges = ageValues.Where(age => age.HasValue && age.Value != 0).ToList();
                var noOfMembers = nonNullAges.Count();
                var eldestMember = ageValues.Max();
                var numberOfMembers = noOfMembers;//calculate this field
                int? count = noOfMembers;

                decimal? siOne = row.sum_insured_1;
                decimal? siTwo = row.sum_insured_2;
                decimal? siThree = row.sum_insured_3;
                decimal? siFour = row.sum_insured_4;
                decimal? siFive = row.sum_insured_5;
                decimal? siSix = row.sum_insured_6;

                var ageBand = dbContext.medisure_age_group
                .Where(ag => ag.age == eldestMember)
                .Select(ir => ir.age_bracket)
                .FirstOrDefault();

                var totalsuminsured = row.total_si;

                // Construct the raw SQL query
                var sql = $@"
                SELECT deductible
                FROM medisure_si_deductible
                WHERE suminsured = @p0";

                // Deductible Total
                var deductible = await dbContext.medisure_si_deductible
                .FromSqlRaw(sql, totalsuminsured)
                .Select(r => r.deductible)
                .FirstOrDefaultAsync();

                // Deductibles 1 to 6
                decimal? deductible1 = await dbContext.medisure_si_deductible
                .FromSqlRaw(sql, siOne)
                .Select(r => r.deductible)
                .FirstOrDefaultAsync();

                decimal? deductible2 = await dbContext.medisure_si_deductible
               .FromSqlRaw(sql, siTwo)
               .Select(r => r.deductible)
               .FirstOrDefaultAsync();

                decimal? deductible3 = await dbContext.medisure_si_deductible
               .FromSqlRaw(sql, siThree)
               .Select(r => r.deductible)
               .FirstOrDefaultAsync();

                decimal? deductible4 = await dbContext.medisure_si_deductible
               .FromSqlRaw(sql, siFour)
               .Select(r => r.deductible)
               .FirstOrDefaultAsync();

                decimal? deductible5 = await dbContext.medisure_si_deductible
               .FromSqlRaw(sql, siFive)
               .Select(r => r.deductible)
               .FirstOrDefaultAsync();

                decimal? deductible6 = await dbContext.medisure_si_deductible
               .FromSqlRaw(sql, siSix)
               .Select(r => r.deductible)
               .FirstOrDefaultAsync();


                //Insured relation
                var policyType = row.policy_type;

                var insured_relation1 = row.txt_insured_relation1;
                var insured_relation2 = row.txt_insured_relation2;
                var insured_relation3 = row.txt_insured_relation3;
                var insured_relation4 = row.txt_insured_relation4;
                var insured_relation5 = row.txt_insured_relation5;
                var insured_relation6 = row.txt_insured_relation6;

                var insuredRelations = new List<string>
                {
                    row.txt_insured_relation1,
                    row.txt_insured_relation2,
                    row.txt_insured_relation3,
                    row.txt_insured_relation4,
                    row.txt_insured_relation5,
                    row.txt_insured_relation6
                };

                var relationTags = new List<string>();

                var relationsql = @"
                 SELECT insured_relation 
                 FROM medisure_family_defn";

                //var rateChart = await dbContext.medisure_family_defn
                //.FromSqlRaw(relationsql)
                //.Select(ir => new { ir.insured_relation, ir.familyall })
                //.ToListAsync();

                var insured_relation_1 = await dbContext.medisure_family_defn
                 .Where(ir => ir.insured_relation == row.txt_insured_relation1)
                 .Select(ir => ir.familyall)
                 .FirstOrDefaultAsync() ?? string.Empty;

                var insured_relation_2 = await dbContext.medisure_family_defn
                    .Where(ir => ir.insured_relation == row.txt_insured_relation2)
                    .Select(ir => ir.familyall)
                    .FirstOrDefaultAsync() ?? string.Empty;

                var insured_relation_3 = await dbContext.medisure_family_defn
                    .Where(ir => ir.insured_relation == row.txt_insured_relation3)
                    .Select(ir => ir.familyall)
                    .FirstOrDefaultAsync() ?? string.Empty;

                var insured_relation_4 = await dbContext.medisure_family_defn
                    .Where(ir => ir.insured_relation == row.txt_insured_relation4)
                    .Select(ir => ir.familyall)
                    .FirstOrDefaultAsync() ?? string.Empty;

                var insured_relation_5 = await dbContext.medisure_family_defn
                    .Where(ir => ir.insured_relation == row.txt_insured_relation5)
                    .Select(ir => ir.familyall)
                    .FirstOrDefaultAsync() ?? string.Empty;

                var insured_relation_6 = await dbContext.medisure_family_defn
                    .Where(ir => ir.insured_relation == row.txt_insured_relation6)
                    .Select(ir => ir.familyall)
                    .FirstOrDefaultAsync() ?? string.Empty;

                // Family Defn
                var insuredRelationTAGOne = insured_relation_1;
                var insuredRelationTAGTwo = insured_relation_2;
                var insuredRelationTAGThree = insured_relation_3;
                var insuredRelationTAGFour = insured_relation_4;
                var insuredRelationTAGFive = insured_relation_5;
                var insuredRelationTAGSix = insured_relation_6;
                var insuredRelationTAGValues = new List<string>
                {
                    insuredRelationTAGOne,
                    insuredRelationTAGTwo,
                    insuredRelationTAGThree,
                    insuredRelationTAGFour,
                    insuredRelationTAGFive,
                    insuredRelationTAGSix
                };
                // Perform the count and apply the logic
                string aValue = ProcessCountForA(insuredRelationTAGValues);
                string cValue = ProcessCountForC(insuredRelationTAGValues);
                string? A = aValue;

                string? C = cValue;
                string? familyDefn = (A ?? string.Empty) + (C ?? string.Empty);
                familyDefn = CalculateFamilyDefn(policyType, aValue, cValue);

                //Floater
                var floater1sql = @"
                SELECT 
                baseRates.[digi_assets_prem]
                FROM 
                medisure_baserates AS baseRates
                WHERE 
                medisure_baserates.deductible = @Deductible
                AND medisure_baserates.suminsured = @SumInsured
                AND medisure_baserates.age_bracket = @AgeBracket
                AND medisure_baserates.family_definition = @FamilyDefinition;";

                string lookupValue = deductible.ToString() + totalsuminsured.ToString() + ageBand + familyDefn + "1";

                decimal? floater1 = await dbContext.medisure_baserates
                        .Where(r => r.deductible.ToString() + r.suminsured.ToString() + r.age_bracket + r.family_definition + "1" == lookupValue)
                        .Select(r => r.digi_assets_prem)
                        .FirstOrDefaultAsync();


                //1 Year Base Premium

                var insuredDeductible1 = row.insured_deductable1;
                var insuredDeductible2 = row.insured_deductable2;
                var insuredDeductible3 = row.insured_deductable3;
                var insuredDeductible4 = row.insured_deductable4;
                var insuredDeductible5 = row.insured_deductable5;
                var insuredDeductible6 = row.insured_deductable6;



                var basepremium1sql = $@"SELECT
                (SELECT baseRates.digi_assets_prem
                 FROM medisure_baserates AS baseRates
                 WHERE baseRates.deductible = @InsuredDeductible1
                   AND baseRates.suminsured = @SumInsured1
                   AND baseRates.age_bracket = (
                       SELECT ageGroup.age_bracket
                       FROM medisure_age_group AS ageGroup
                       WHERE ageGroup.age = @InsuredAge1 
                       LIMIT 1
                   )
                   AND baseRates.family_definition = @FamilyDefinition
                 LIMIT 1
                ) AS insuredBasePremium1;";

                decimal? year1basepremium_insured1 = 0;
                decimal? year1basepremium_insured2 = 0;
                decimal? year1basepremium_insured3 = 0;
                decimal? year1basepremium_insured4 = 0;
                decimal? year1basepremium_insured5 = 0;
                decimal? year1basepremium_insured6 = 0;
                decimal? totalbasepremium1year = 0;
                if (row.prod_name.Contains("Family"))
                {
                    totalbasepremium1year = CalculateTotalBasePremiumOneYear(policyType, year1basepremium_insured1, year1basepremium_insured2, year1basepremium_insured3, year1basepremium_insured4, year1basepremium_insured5, year1basepremium_insured6, floater1);
                }
                else if (row.prod_name.Contains("Individual"))
                {
                    year1basepremium_insured1 = await dbContext.medisure_baserates
                  .FromSqlRaw(basepremium1sql, insuredDeductible1, siOne, insuredAgeOne, familyDefn)
                  .Select(r => EF.Property<decimal?>(r, familyDefn))
                  .FirstOrDefaultAsync();

                    year1basepremium_insured2 = await dbContext.medisure_baserates
                      .FromSqlRaw(basepremium1sql, insuredDeductible2, siTwo, insuredAgeTwo, familyDefn)
                      .Select(r => EF.Property<decimal?>(r, familyDefn))
                      .FirstOrDefaultAsync();

                    year1basepremium_insured3 = await dbContext.medisure_baserates
                      .FromSqlRaw(basepremium1sql, insuredDeductible3, siThree, insuredAgeThree, familyDefn)
                      .Select(r => EF.Property<decimal?>(r, familyDefn))
                      .FirstOrDefaultAsync();

                    year1basepremium_insured4 = await dbContext.medisure_baserates
                      .FromSqlRaw(basepremium1sql, insuredDeductible4, siFour, insuredAgeFour, familyDefn)
                      .Select(r => EF.Property<decimal?>(r, familyDefn))
                      .FirstOrDefaultAsync();

                    year1basepremium_insured5 = await dbContext.medisure_baserates
                      .FromSqlRaw(basepremium1sql, insuredDeductible5, siFive, insuredAgeFive, familyDefn)
                      .Select(r => EF.Property<decimal?>(r, familyDefn))
                      .FirstOrDefaultAsync();

                    year1basepremium_insured6 = await dbContext.medisure_baserates
                      .FromSqlRaw(basepremium1sql, insuredDeductible6, siSix, insuredAgeSix, familyDefn)
                      .Select(r => EF.Property<decimal?>(r, familyDefn))
                      .FirstOrDefaultAsync();

                    var oneyearbasePremiumvalues = new List<decimal?> { year1basepremium_insured1, year1basepremium_insured2, year1basepremium_insured3, year1basepremium_insured4, year1basepremium_insured5, year1basepremium_insured6 };
                    totalbasepremium1year = CalculateTotalBasePremiumOneYear(policyType, year1basepremium_insured1, year1basepremium_insured2, year1basepremium_insured3, year1basepremium_insured4, year1basepremium_insured5, year1basepremium_insured6, floater1);
                }

                decimal? familyDiscount1year = GetFamilyDiscount(policyType, noOfMembers);
                decimal? netPremium1year = totalbasepremium1year - (totalbasepremium1year * familyDiscount1year);
                decimal? GST1year = netPremium1year * 0.18m;
                decimal? finalPremium1year = netPremium1year + GST1year;

                // 2 Year Base Premium

                //Floater 2
                var floater2sql = @"
                SELECT 
                baseRates.[digi_assets_prem]
                FROM 
                medisure_baserates AS baseRates
                WHERE 
                medisure_baserates.deductible = @Deductible
                AND medisure_baserates.suminsured = @SumInsured
                AND medisure_baserates.age_bracket = @AgeBracket
                AND medisure_baserates.family_definition = @FamilyDefinition;";

                string lookupValue2 = deductible.ToString() + totalsuminsured.ToString() + ageBand + familyDefn + "2";

                decimal? floater2 = await dbContext.medisure_baserates
                        .Where(r => r.deductible.ToString() + r.suminsured.ToString() + r.age_bracket + r.family_definition + "2" == lookupValue2)
                        .Select(r => r.digi_assets_prem)
                        .FirstOrDefaultAsync();

                var basepremium2sql = $@"SELECT
                (SELECT baseRates.digi_assets_prem
                 FROM medisure_baserates AS baseRates
                 WHERE baseRates.deductible = @InsuredDeductible1
                   AND baseRates.suminsured = @SumInsured1
                   AND baseRates.age_bracket = (
                       SELECT ageGroup.age_bracket
                       FROM medisure_age_group AS ageGroup
                       WHERE ageGroup.age = @InsuredAge1 
                       LIMIT 1
                   )
                   AND baseRates.family_definition = @FamilyDefinition
                 LIMIT 1
                ) AS insuredBasePremium1;";


                decimal? year2basepremium_insured1 = 0;
                decimal? year2basepremium_insured2 = 0;
                decimal? year2basepremium_insured3 = 0;
                decimal? year2basepremium_insured4 = 0;
                decimal? year2basepremium_insured5 = 0;
                decimal? year2basepremium_insured6 = 0;
                decimal? totalbasepremium2year = 0;


                if (row.prod_name.Contains("Family"))
                {
                    totalbasepremium2year = CalculateTotalBasePremiumTwoYear(policyType, year2basepremium_insured1, year2basepremium_insured2, year2basepremium_insured3, year2basepremium_insured4, year2basepremium_insured5, year2basepremium_insured6, floater2);
                }
                else if (row.prod_name.Contains("Individual"))
                {
                    year2basepremium_insured1 = await dbContext.baserate
                   .FromSqlRaw(basepremium1sql, insuredDeductible1, siOne, insuredAgeOne, familyDefn)
                   .Select(r => EF.Property<decimal?>(r, familyDefn))
                   .FirstOrDefaultAsync();

                    year2basepremium_insured2 = await dbContext.baserate
                   .FromSqlRaw(basepremium1sql, insuredDeductible2, siTwo, insuredAgeTwo, familyDefn)
                   .Select(r => EF.Property<decimal?>(r, familyDefn))
                   .FirstOrDefaultAsync();

                    year2basepremium_insured3 = await dbContext.baserate
                   .FromSqlRaw(basepremium1sql, insuredDeductible3, siThree, insuredAgeThree, familyDefn)
                   .Select(r => EF.Property<decimal?>(r, familyDefn))
                   .FirstOrDefaultAsync();

                    year2basepremium_insured4 = await dbContext.baserate
                    .FromSqlRaw(basepremium1sql, insuredDeductible4, siFour, insuredAgeFour, familyDefn)
                    .Select(r => EF.Property<decimal?>(r, familyDefn))
                    .FirstOrDefaultAsync();

                    year2basepremium_insured5 = await dbContext.baserate
                    .FromSqlRaw(basepremium1sql, insuredDeductible5, siFive, insuredAgeFive, familyDefn)
                    .Select(r => EF.Property<decimal?>(r, familyDefn))
                    .FirstOrDefaultAsync();

                    year2basepremium_insured6 = await dbContext.baserate
                    .FromSqlRaw(basepremium1sql, insuredDeductible6, siSix, insuredAgeSix, familyDefn)
                    .Select(r => EF.Property<decimal?>(r, familyDefn))
                    .FirstOrDefaultAsync();

                    var twoyearbasePremiumvalues = new List<decimal?> { year2basepremium_insured1, year2basepremium_insured2, year2basepremium_insured3, year2basepremium_insured4, year2basepremium_insured5, year2basepremium_insured6 };
                    totalbasepremium2year = CalculateTotalBasePremiumTwoYear(policyType, year2basepremium_insured1, year2basepremium_insured2, year2basepremium_insured3, year2basepremium_insured4, year2basepremium_insured5, year2basepremium_insured6, floater2);
                }

                decimal? familyDiscount2year = GetFamilyDiscount(policyType, noOfMembers);
                decimal? afterFamilyDiscount2year = totalbasepremium2year - (totalbasepremium2year * familyDiscount2year);
                decimal? tenureDiscount2year = 0.05m;
                decimal? netPremium2year = afterFamilyDiscount2year - (afterFamilyDiscount2year * tenureDiscount2year);
                decimal? GST2year = netPremium2year * 0.18m;
                decimal? finalPremium2year = netPremium2year + GST2year;
                decimal? Crosscheck1 = null;
                decimal? Crosscheck2 = null;
                decimal? num_tot_premium = totalbasepremium1year + totalbasepremium2year;

                if (policyperiod == 1)
                {
                    Crosscheck1 = row.total_premium - finalPremium1year;
                }
                if (policyperiod == 2)
                {
                    Crosscheck2 = row.total_premium - finalPremium2year;
                }
                decimal? Crosscheck = policyperiod == 1 ? Crosscheck1 : (policyperiod == 2 ? Crosscheck2 : null);
                var record = dbContext.idst_renewal_data_rgs.FirstOrDefault(item => item.certificate_no == policNo16.ToString());

                if (Crosscheck.HasValue && record.rn_generation_status == "Reconciliation Successful")
                {
                    if (Math.Abs(Crosscheck.Value) <= 10)
                    {
                        if (record != null)
                        {
                            record.final_remarks = "RN Not Dispatched";
                            record.dispatch_status = "PDF Gen Under Process With CLICK PSS Team";
                            record.rn_generation_status = "RN Not Dispatched";
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
                if (policyperiod == 1)
                {
                    // Updating record with values for 1 year policy
                    record.verified_prem = netPremium1year;
                    record.verified_gst = GST1year;
                    record.verified_total_prem = finalPremium1year;
                }
                else if (policyperiod == 2)
                {
                    // Updating record with values for 2 year policy
                    record.verified_prem = netPremium2year;
                    record.verified_gst = GST2year;
                    record.verified_total_prem = finalPremium2year;
                }
                dbContext.idst_renewal_data_rgs.Update(record);
                await dbContext.SaveChangesAsync();

                ms = new MedisureStupRNE
                {
                    prod_name = row.prod_name,
                    policy_number = row.policy_number,
                    location_code = row.location_code,
                    txt_apartment = row.txt_apartment,
                    txt_street = row.txt_street,
                    txt_areavillage = row.txt_areavillage,
                    txt_citydistrict = row.txt_citydistrict,
                    txt_state = row.txt_state,
                    state_code = row.state_code,
                    txt_pincode = row.txt_pincode,
                    txt_mobile = row.txt_mobile,
                    txt_telephone = row.txt_telephone,
                    txt_dealer_cd = row.txt_dealer_cd,//intermediary_code in gc mapping
                    txt_family = row.txt_family,
                    reference_num = row.reference_num,//proposal no in gc
                    policy_start_date = row.policy_start_date,
                    policy_expiry_date = row.policy_expiry_date,
                    policy_type = row.policy_type,
                    policy_period = row.policy_period,
                    tier_type = row.tier_type,

                    insured_deductable1 = row.insured_deductable1,
                    insured_deductable2 = row.insured_deductable2,
                    insured_deductable3 = row.insured_deductable3,
                    insured_deductable4 = row.insured_deductable4,
                    insured_deductable5 = row.insured_deductable5,
                    insured_deductable6 = row.insured_deductable6,

                    sum_insured_1 = row.sum_insured_1,
                    sum_insured_2 = row.sum_insured_2,
                    sum_insured_3 = row.sum_insured_3,
                    sum_insured_4 = row.sum_insured_4,
                    sum_insured_5 = row.sum_insured_5,
                    sum_insured_6 = row.sum_insured_6,

                    next_si_1 = row.next_si_1,
                    next_si_2 = row.next_si_2,
                    next_si_3 = row.next_si_3,
                    next_si_4 = row.next_si_4,
                    next_si_5 = row.next_si_5,
                    next_si_6 = row.next_si_6,

                    next_si_prm_11 = row.next_si_prm_11,
                    next_si_prm_12 = row.next_si_prm_12,
                    next_si_prm_21 = row.next_si_prm_21,
                    next_si_prm_22 = row.next_si_prm_22,
                    next_si_prm_31 = row.next_si_prm_31,
                    next_si_prm_32 = row.next_si_prm_32,
                    next_si_prm_41 = row.next_si_prm_41,
                    next_si_prm_42 = row.next_si_prm_42,
                    next_si_prm_51 = row.next_si_prm_51,
                    next_si_prm_52 = row.next_si_prm_52,
                    next_si_prm_61 = row.next_si_prm_61,
                    next_si_prm_62 = row.next_si_prm_62,

                    total_next_si = row.total_next_si,
                    total_next_si_prm_1year = row.total_next_si_prm_1year,
                    total_next_si_prm_2year = row.total_next_si_prm_2year,

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

                    txt_insured_cb1 = row.txt_insured_cb1,
                    txt_insured_cb2 = row.txt_insured_cb2,
                    txt_insured_cb3 = row.txt_insured_cb3,
                    txt_insured_cb4 = row.txt_insured_cb4,
                    txt_insured_cb5 = row.txt_insured_cb5,
                    txt_insured_cb6 = row.txt_insured_cb6,

                    year1basepremium_insured1 = year1basepremium_insured1,
                    year1basepremium_insured2 = year1basepremium_insured2,
                    year1basepremium_insured3 = year1basepremium_insured3,
                    year1basepremium_insured4 = year1basepremium_insured4,
                    year1basepremium_insured5 = year1basepremium_insured5,
                    year1basepremium_insured6 = year1basepremium_insured6,

                    year2basepremium_insured1 = year2basepremium_insured1,
                    year2basepremium_insured2 = year2basepremium_insured2,
                    year2basepremium_insured3 = year2basepremium_insured3,
                    year2basepremium_insured4 = year2basepremium_insured4,
                    year2basepremium_insured5 = year2basepremium_insured5,
                    year2basepremium_insured6 = year2basepremium_insured6,

                    floater1 = floater1,
                    floater2 = floater2,

                    num_net_premium = row.num_net_premium.HasValue
                                    ? Math.Round(row.num_net_premium.Value, 2)
                                    : (decimal?)null,
                    num_service_tax = row.num_service_tax.HasValue
                                    ? Math.Round(row.num_service_tax.Value, 2)
                                    : (decimal?)null,
                    total_premium = row.total_premium.HasValue
                                    ? Math.Round(row.total_premium.Value, 2)
                                    : (decimal?)null,

                    no_of_members = noOfMembers,
                    eldest_member = eldestMember,

                    //1year Premiums
                    totalbasepremium1year = totalbasepremium1year.HasValue ? Math.Round(totalbasepremium1year.Value, 2) : (decimal?)null,
                    familyDiscount1year = familyDiscount1year.HasValue ? Math.Round(familyDiscount1year.Value, 2) : (decimal?)null,
                    netPremium1year = netPremium1year.HasValue ? Math.Round(netPremium1year.Value, 2) : (decimal?)null,
                    GST1year = GST1year.HasValue ? Math.Round(GST1year.Value, 2) : (decimal?)null,

                    //2year Premiums
                    totalbasepremium2year = totalbasepremium2year.HasValue ? Math.Round(totalbasepremium2year.Value, 2) : (decimal?)null,
                    familyDiscount2year = familyDiscount2year.HasValue ? Math.Round(familyDiscount2year.Value, 2) : (decimal?)null,
                    afterFamilyDiscount2year = afterFamilyDiscount2year.HasValue ? Math.Round(afterFamilyDiscount2year.Value, 2) : (decimal?)null,
                    tenureDiscount2year = tenureDiscount2year.HasValue ? Math.Round(tenureDiscount2year.Value, 2) : (decimal?)null,
                    netPremium2year = netPremium2year.HasValue ? Math.Round(netPremium2year.Value, 2) : (decimal?)null,
                    GST2year = GST2year.HasValue ? Math.Round(GST2year.Value, 2) : (decimal?)null,
                    finalPremium2year = finalPremium2year.HasValue ? Math.Round(finalPremium2year.Value, 2) : (decimal?)null,

                    Crosscheck1 = Crosscheck1.HasValue ? Math.Round(Crosscheck1.Value, 2) : (decimal?)null,
                    Crosscheck2 = Crosscheck2.HasValue ? Math.Round(Crosscheck2.Value, 2) : (decimal?)null,

                };
            }
            return new List<MedisureStupRNE> { ms };
        }

        private string CalculateFamilyDefn(string policyType, string avalue, string cvalue)
        {
            var sum = avalue + cvalue;
            if (policyType == "Individual")
            {
                return "1A";
            }
            else
            {
                return sum;
            }
        }

        private decimal? CalculateTotalBasePremiumOneYear(string policyType, decimal? year1basepremium_insured1, decimal? year1basepremium_insured2, decimal? year1basepremium_insured3, decimal? year1basepremium_insured4, decimal? year1basepremium_insured5, decimal? year1basepremium_insured6, decimal? floater1)
        {
            var sum = year1basepremium_insured1 + year1basepremium_insured2 + year1basepremium_insured3 + year1basepremium_insured4 + year1basepremium_insured5 + year1basepremium_insured6;
            if (policyType == "Individual")
            {
                return sum;
            }
            else
            {
                return floater1;
            }
        }

        private decimal? CalculateTotalBasePremiumTwoYear(string policyType, decimal? year2basepremium_insured1, decimal? year2basepremium_insured2, decimal? year2basepremium_insured3, decimal? year2basepremium_insured4, decimal? year2basepremium_insured5, decimal? year2basepremium_insured6, decimal? floater2)
        {
            var sum = year2basepremium_insured1 + year2basepremium_insured2 + year2basepremium_insured3 + year2basepremium_insured4 + year2basepremium_insured5 + year2basepremium_insured6;
            if (policyType == "Individual")
            {
                return sum;
            }
            else
            {
                return floater2;
            }
        }

        private static decimal? GetFamilyDiscount(string policyType, int? noOfMembers)
        {
            var FamilyDiscountValue = 0.0m;
            if (policyType == "Individual" && noOfMembers > 1)
            {
                FamilyDiscountValue = 0.1m;
            }

            return FamilyDiscountValue;
        }

        private string ProcessCountForA(List<string> cellValues)
        {
            if (cellValues == null)
            {
                return string.Empty;
            }
            // Count the number of entries starting with "A"
            int count = cellValues.Count(value => value != null && value.StartsWith("A", StringComparison.OrdinalIgnoreCase));

            // Return the formatted result based on the count
            return count > 0 ? $"{count}A" : string.Empty;
        }

        static string ProcessCountForC(List<string> cellValues)
        {
            if (cellValues == null)
            {
                return string.Empty;
            }
            // Count the number of entries starting with "P"
            int count = cellValues.Count(value => value != null && value.StartsWith("C", StringComparison.OrdinalIgnoreCase));

            // Return the formatted result based on the count
            return count > 0 ? $"{count}C" : string.Empty;
        }

    }
}
