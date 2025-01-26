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
    public class AarogyaSanjeevani
    {
        private readonly HDFCDbContext dbContext;
        private readonly ILogger<AarogyaSanjeevani> _logger;
        public AarogyaSanjeevani(HDFCDbContext hDFCDbContext, ILogger<AarogyaSanjeevani> logger)
        {
            this.dbContext = hDFCDbContext;
            _logger = logger;
        }

        public async Task<AarogyaSanjeevaniValidationResult> GetAarogyaSanjeevaniValidation(string policyNo)
        {
            _logger.LogInformation("GetAarogyaSanjeevaniValidation is Started!");
            //step 1 get data from gc
            IEnumerable<AarogyaSanjeevaniRNE> AarogyaSanjeevaniValidationResult;
            var asRNEData = await GetGCAarogyaSanjeevaniDataAsync(policyNo, 1, 1000);
            //step 2 calculation
            _logger.LogInformation("CalculateAarogyaSanjeevaniPremium is Started!");
            AarogyaSanjeevaniValidationResult = await CalculateAarogyaSanjeevaniPremium(asRNEData);

            AarogyaSanjeevaniValidationResult objAarogyaSanjeevaniValidation = new AarogyaSanjeevaniValidationResult
            {
                prod_code = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.prod_code,
                prod_name = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.prod_name,
                policy_number = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.policy_number,
                batchid = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.batchid,
                customer_id = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.customer_id,
                customername = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.customername,
                txt_salutation = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_salutation,
                location_code = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.location_code,
                txt_apartment = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_apartment,
                txt_street = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_street,
                txt_areavillage = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_areavillage,
                txt_citydistrict = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_citydistrict,
                txt_state = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_state,
                state_code = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.state_code,
                state_regis = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.state_regis,
                txt_pincode = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_pincode,
                txt_nationality = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_nationality,
                txt_mobile = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_mobile,
                txt_telephone = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_telephone,
                txt_email = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_email,
                txt_dealer_cd = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_dealer_cd,//intermediary_code in gc mapping
                imdname = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.imdname,//intermediary_name in gc
                verticalname = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.verticalname,//psm_name in gc
                txt_family = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_family,
                isrnflag = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.isrnflag,//chk
                reference_num = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.reference_num,//proposal no in gc
                split_flag = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.split_flag,
                isvipflag = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.isvipflag,//chk 
                txt_insured_entrydate1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_entrydate1,//chk inceptiondate in gc
                txt_insured_entrydate2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_entrydate2,
                txt_insured_entrydate3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_entrydate3,
                txt_insured_entrydate4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_entrydate4,
                txt_insured_entrydate5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_entrydate5,
                txt_insured_entrydate6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_entrydate6,
                txt_insured_entrydate7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_entrydate7,
                txt_insured_entrydate8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_entrydate8,
                txt_insured_entrydate9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_entrydate9,
                txt_insured_entrydate10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_entrydate10,
                txt_insured_entrydate11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_entrydate11,
                txt_insured_entrydate12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_entrydate12,

                member_id1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.member_id1,
                member_id2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.member_id2,
                member_id3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.member_id3,
                member_id4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.member_id4,
                member_id5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.member_id5,
                member_id6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.member_id6,
                member_id7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.member_id7,
                member_id8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.member_id8,
                member_id9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.member_id9,
                member_id10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.member_id10,
                member_id11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.member_id11,
                member_id12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.member_id12,

                insured_loadingper1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_per_insured_1,
                insured_loadingper2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_per_insured_2,
                insured_loadingper3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_per_insured_3,
                insured_loadingper4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_per_insured_4,
                insured_loadingper5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_per_insured_5,
                insured_loadingper6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_per_insured_6,
                insured_loadingper7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_per_insured_7,
                insured_loadingper8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_per_insured_8,
                insured_loadingper9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_per_insured_9,
                insured_loadingper10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_per_insured_10,
                insured_loadingper11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_per_insured_11,
                insured_loadingper12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_per_insured_12,

                insured_loadingamt1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_loadingamt1,
                insured_loadingamt2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_loadingamt2,
                insured_loadingamt3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_loadingamt3,
                insured_loadingamt4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_loadingamt4,
                insured_loadingamt5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_loadingamt5,
                insured_loadingamt6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_loadingamt6,
                insured_loadingamt7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_loadingamt7,
                insured_loadingamt8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_loadingamt8,
                insured_loadingamt9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_loadingamt9,
                insured_loadingamt10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_loadingamt10,
                insured_loadingamt11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_loadingamt11,
                insured_loadingamt12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_loadingamt12,

                txt_insured_dob1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_dob1,
                txt_insured_dob2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_dob2,
                txt_insured_dob3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_dob3,
                txt_insured_dob4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_dob4,
                txt_insured_dob5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_dob5,
                txt_insured_dob6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_dob6,
                txt_insured_dob7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_dob7,
                txt_insured_dob8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_dob8,
                txt_insured_dob9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_dob9,
                txt_insured_dob10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_dob10,
                txt_insured_dob11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_dob11,
                txt_insured_dob12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_dob12,

                txt_insured_age1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_age1,
                txt_insured_age2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_age2,
                txt_insured_age3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_age3,
                txt_insured_age4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_age4,
                txt_insured_age5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_age5,
                txt_insured_age6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_age6,
                txt_insured_age7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_age7,
                txt_insured_age8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_age8,
                txt_insured_age9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_age9,
                txt_insured_age10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_age10,
                txt_insured_age11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_age11,
                txt_insured_age12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_age12,

                txt_insured_relation1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_relation1,//coming as "string"
                txt_insured_relation2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_relation2,
                txt_insured_relation3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_relation3,
                txt_insured_relation4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_relation4,
                txt_insured_relation5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_relation5,
                txt_insured_relation6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_relation6,
                txt_insured_relation7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_relation7,
                txt_insured_relation8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_relation8,
                txt_insured_relation9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_relation9,
                txt_insured_relation10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_relation10,
                txt_insured_relation11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_relation11,
                txt_insured_relation12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.txt_insured_relation12,


                insured_relation_tag_1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_relation_tag_1,
                insured_relation_tag_2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_relation_tag_2,
                insured_relation_tag_3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_relation_tag_3,
                insured_relation_tag_4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_relation_tag_4,
                insured_relation_tag_5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_relation_tag_5,
                insured_relation_tag_6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_relation_tag_6,
                insured_relation_tag_7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_relation_tag_7,
                insured_relation_tag_8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_relation_tag_8,
                insured_relation_tag_9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_relation_tag_9,
                insured_relation_tag_10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_relation_tag_10,
                insured_relation_tag_11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_relation_tag_11,
                insured_relation_tag_12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_relation_tag_12,

                pre_existing_disease1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.pre_existing_disease1,
                pre_existing_disease2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.pre_existing_disease2,
                pre_existing_disease3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.pre_existing_disease3,
                pre_existing_disease4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.pre_existing_disease4,
                pre_existing_disease5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.pre_existing_disease5,
                pre_existing_disease6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.pre_existing_disease6,
                pre_existing_disease7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.pre_existing_disease7,
                pre_existing_disease8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.pre_existing_disease8,
                pre_existing_disease9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.pre_existing_disease9,
                pre_existing_disease10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.pre_existing_disease10,
                pre_existing_disease11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.pre_existing_disease11,
                pre_existing_disease12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.pre_existing_disease12,


                insured_cb1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_cb1,
                insured_cb2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_cb2,
                insured_cb3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_cb3,
                insured_cb4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_cb4,
                insured_cb5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_cb5,
                insured_cb6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_cb6,
                insured_cb7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_cb7,
                insured_cb8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_cb8,
                insured_cb9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_cb9,
                insured_cb10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_cb10,
                insured_cb11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_cb11,
                insured_cb12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_cb12,

                sum_insured1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.sum_insured1,
                sum_insured2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.sum_insured2,
                sum_insured3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.sum_insured3,
                sum_insured4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.sum_insured4,
                sum_insured5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.sum_insured5,
                sum_insured6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.sum_insured6,
                sum_insured7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.sum_insured7,
                sum_insured8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.sum_insured8,
                sum_insured9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.sum_insured9,
                sum_insured10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.sum_insured10,
                sum_insured11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.sum_insured11,
                sum_insured12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.sum_insured12,

                insured_deductable1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_deductable1,
                insured_deductable2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_deductable2,
                insured_deductable3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_deductable3,
                insured_deductable4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_deductable4,
                insured_deductable5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_deductable5,
                insured_deductable6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_deductable6,
                insured_deductable7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_deductable7,
                insured_deductable8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_deductable8,
                insured_deductable9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_deductable9,
                insured_deductable10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_deductable10,
                insured_deductable11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_deductable11,
                insured_deductable12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_deductable12,


                wellness_discount1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.wellness_discount1,
                wellness_discount2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.wellness_discount2,
                wellness_discount3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.wellness_discount3,
                wellness_discount4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.wellness_discount4,
                wellness_discount5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.wellness_discount5,
                wellness_discount6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.wellness_discount6,
                wellness_discount7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.wellness_discount7,
                wellness_discount8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.wellness_discount8,
                wellness_discount9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.wellness_discount9,
                wellness_discount10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.wellness_discount10,
                wellness_discount11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.wellness_discount11,
                wellness_discount12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.wellness_discount12,


                stayactive1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.stayactive1,
                stayactive2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.stayactive2,
                stayactive3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.stayactive3,
                stayactive4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.stayactive4,
                stayactive5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.stayactive5,
                stayactive6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.stayactive6,
                stayactive7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.stayactive7,
                stayactive8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.stayactive8,
                stayactive9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.stayactive9,
                stayactive10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.stayactive10,
                stayactive11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.stayactive11,
                stayactive12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.stayactive12,

                coverbaseloadingrate1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.coverbaseloadingrate1,
                coverbaseloadingrate2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.coverbaseloadingrate2,
                coverbaseloadingrate3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.coverbaseloadingrate3,
                coverbaseloadingrate4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.coverbaseloadingrate4,
                coverbaseloadingrate5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.coverbaseloadingrate5,
                coverbaseloadingrate6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.coverbaseloadingrate6,
                coverbaseloadingrate7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.coverbaseloadingrate7,
                coverbaseloadingrate8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.coverbaseloadingrate8,
                coverbaseloadingrate9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.coverbaseloadingrate9,
                coverbaseloadingrate10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.coverbaseloadingrate10,
                coverbaseloadingrate11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.coverbaseloadingrate11,
                coverbaseloadingrate12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.coverbaseloadingrate12,

                health_incentive1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.health_incentive1,
                health_incentive2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.health_incentive2,
                health_incentive3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.health_incentive3,
                health_incentive4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.health_incentive4,
                health_incentive5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.health_incentive5,
                health_incentive6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.health_incentive6,
                health_incentive7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.health_incentive7,
                health_incentive8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.health_incentive8,
                health_incentive9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.health_incentive9,
                health_incentive10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.health_incentive10,
                health_incentive11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.health_incentive11,
                health_incentive12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.health_incentive12,

                fitness_discount1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.fitness_discount1,
                fitness_discount2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.fitness_discount2,
                fitness_discount3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.fitness_discount3,
                fitness_discount4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.fitness_discount4,
                fitness_discount5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.fitness_discount5,
                fitness_discount6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.fitness_discount6,
                fitness_discount7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.fitness_discount7,
                fitness_discount8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.fitness_discount8,
                fitness_discount9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.fitness_discount9,
                fitness_discount10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.fitness_discount10,
                fitness_discount11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.fitness_discount11,
                fitness_discount12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.fitness_discount12,

                reservbenefis1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.reservbenefis1,
                reservbenefis2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.reservbenefis2,
                reservbenefis3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.reservbenefis3,
                reservbenefis4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.reservbenefis4,
                reservbenefis5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.reservbenefis5,
                reservbenefis6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.reservbenefis6,
                reservbenefis7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.reservbenefis7,
                reservbenefis8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.reservbenefis8,
                reservbenefis9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.reservbenefis9,
                reservbenefis10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.reservbenefis10,
                reservbenefis11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.reservbenefis11,
                reservbenefis12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.reservbenefis12,

                insured_rb_claimamt1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_rb_claimamt1,
                insured_rb_claimamt2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_rb_claimamt2,
                insured_rb_claimamt3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_rb_claimamt3,
                insured_rb_claimamt4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_rb_claimamt4,
                insured_rb_claimamt5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_rb_claimamt5,
                insured_rb_claimamt6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_rb_claimamt6,
                insured_rb_claimamt7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_rb_claimamt7,
                insured_rb_claimamt8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_rb_claimamt8,
                insured_rb_claimamt9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_rb_claimamt9,
                insured_rb_claimamt10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_rb_claimamt10,
                insured_rb_claimamt11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_rb_claimamt11,
                insured_rb_claimamt12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.insured_rb_claimamt12,


                preventive_hc = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.preventive_hc,
                policy_start_date = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.policy_start_date,
                policy_expiry_date = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.policy_expiry_date,
                policy_type = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.policy_type,
                policy_period = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.policy_period,
                policyplan = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.policyplan,
                claimcount = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.claimcount,

                no_of_members = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.no_of_members,
                eldest_member = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.eldest_member,
                tier_type = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.tier_type,

                employee_discount = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.employee_discount,
                online_discount = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.online_discount,
                loyalty_discount = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loyalty_discount,
                family_discount = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.family_discount,
                rural_discount = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.rural_discount,
                capping_discount = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.capping_discount,

                base_premium_1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.base_premium_1,
                base_premium_2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.base_premium_2,
                base_premium_3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.base_premium_3,
                base_premium_4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.base_premium_4,
                base_premium_5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.base_premium_5,
                base_premium_6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.base_premium_6,
                base_premium_7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.base_premium_7,
                base_premium_8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.base_premium_8,
                base_premium_9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.base_premium_9,
                base_premium_10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.base_premium_10,
                base_premium_11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.base_premium_11,
                base_premium_12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.base_premium_12,
                base_premium = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.base_premium,

                loading_prem_1 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_prem1,
                loading_prem_2 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_prem2,
                loading_prem_3 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_prem3,
                loading_prem_4 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_prem4,
                loading_prem_5 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_prem5,
                loading_prem_6 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_prem6,
                loading_prem_7 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_prem7,
                loading_prem_8 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_prem8,
                loading_prem_9 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_prem9,
                loading_prem_10 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_prem10,
                loading_prem_11 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_prem11,
                loading_prem_12 = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_prem12,
                loading_premium = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.loading_prem,

                basepremiumLoading = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.basepremiumLoading,
                baseAndLoading_LoyaltyDiscount = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.baseAndLoading_LoyaltyDiscount,
                baseAndLoading_EmployeeDiscount = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.baseAndLoading_EmployeeDiscount,
                baseAndLoading_OnlineDiscount = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.baseAndLoading_OnlineDiscount,
                baseAndLoading_FamilyDiscount = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.baseAndLoading_FamilyDiscount,
                baseAndLoading_RuralDiscount = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.baseAndLoading_RuralDiscount,
                baseAndLoading_CapppingDiscount = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.baseAndLoading_CapppingDiscount,


                // netPremium = netPremium,
                netPremium = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.netPremium.HasValue == true
                             ? (decimal?)Math.Round(AarogyaSanjeevaniValidationResult.FirstOrDefault().netPremium.Value, 2)
                             : (decimal?)null,

                // finalPremium = finalPremium,
                finalPremium = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.finalPremium.HasValue == true
                             ? (decimal?)Math.Round(AarogyaSanjeevaniValidationResult.FirstOrDefault().finalPremium.Value, 2)
                             : (decimal?)null,


                //GST = GST,
                GST = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.GST.HasValue == true
                             ? (decimal?)Math.Round(AarogyaSanjeevaniValidationResult.FirstOrDefault().GST.Value, 2)
                             : (decimal?)null,
                //crossCheck = Crosscheck,
                crossCheck = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.crossCheck.HasValue == true
                             ? (decimal?)Math.Round(AarogyaSanjeevaniValidationResult.FirstOrDefault().crossCheck.Value, 2)
                             : (decimal?)null,

                aarogyasanjeevani_total_Premium = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.aarogyasanjeevani_total_Premium.HasValue == true
                             ? (decimal?)Math.Round(AarogyaSanjeevaniValidationResult.FirstOrDefault().aarogyasanjeevani_total_Premium.Value, 2)
                             : (decimal?)null,

                aarogyasanjeevani_netpremium = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.aarogyasanjeevani_netpremium.HasValue == true
                             ? (decimal?)Math.Round(AarogyaSanjeevaniValidationResult.FirstOrDefault().aarogyasanjeevani_netpremium.Value, 2)
                             : (decimal?)null,

                aarogyasanjeevani_GST = AarogyaSanjeevaniValidationResult.FirstOrDefault()?.aarogyasanjeevani_GST.HasValue == true
                             ? (decimal?)Math.Round(AarogyaSanjeevaniValidationResult.FirstOrDefault().aarogyasanjeevani_GST.Value, 2)
                             : (decimal?)null,

            };
            if (objAarogyaSanjeevaniValidation?.policy_number == null)
            {
                Console.WriteLine("Policy number not found.");
            }
            var record = await dbContext.rne_calculated_cover_rg.AsNoTracking().FirstOrDefaultAsync(item => item.policy_number == policyNo.ToString());

            if (objAarogyaSanjeevaniValidation != null)
            {
                var no_of_members = objAarogyaSanjeevaniValidation.no_of_members;
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
                dbContext.rne_calculated_cover_rg.AddRange(newRecord);
                await dbContext.SaveChangesAsync();
            }
            return objAarogyaSanjeevaniValidation;
        }
        private async Task<IEnumerable<AarogyaSanjeevaniRNE>> GetGCAarogyaSanjeevaniDataAsync(string policyNo, int pageNumber, int pageSize)
        {
            // Query to get data from GC to calculate optima secure premium
            var asRNEData = await (
          from aas in dbContext.rne_healthtab
          join asidst in dbContext.idst_renewal_data_rgs on aas.policy_number equals asidst.certificate_no
          where (aas.policy_number == policyNo)
          select new AarogyaSanjeevaniRNE
          {
              prod_code = aas.prod_code,
              batchid = aas.batchid,
              prod_name = aas.prod_name,
              reference_num = aas.reference_num,
              //propaasal_no = gc.propaasal_no,
              //policy_no = gc.policy_no,
              policy_number = aas.policy_number,
              split_flag = aas.split_flag,
              customer_id = aas.customer_id,
              customername = aas.customername,
              verticalname = aas.verticalname,//psm name in old gc mapping
              policy_start_date = aas.policy_start_date,
              policy_expiry_date = aas.policy_expiry_date,
              policy_period = aas.policy_period,
              tier_type = aas.tier_type,//tier in old gc mapping
              policyplan = aas.policyplan,
              policy_type = aas.policy_type,
              txt_family = aas.txt_family,//family size in old gc mapping
              claimcount = aas.claimcount,
              num_tot_premium = aas.num_tot_premium,
              num_net_premium = aas.num_net_premium,
              num_service_tax = aas.num_service_tax,
              //optima_secure_gst = gc.gst,
              // rn_flag = gc.rn_flag,
              //vip_flag = gc.vip_flag,
              txt_insured_entrydate1 = aas.txt_insured_entrydate1,
              txt_insured_entrydate2 = aas.txt_insured_entrydate2,
              txt_insured_entrydate3 = aas.txt_insured_entrydate3,
              txt_insured_entrydate4 = aas.txt_insured_entrydate4,
              txt_insured_entrydate5 = aas.txt_insured_entrydate5,
              txt_insured_entrydate6 = aas.txt_insured_entrydate6,
              txt_insured_entrydate7 = aas.txt_insured_entrydate7,
              txt_insured_entrydate8 = aas.txt_insured_entrydate8,
              txt_insured_entrydate9 = aas.txt_insured_entrydate9,
              txt_insured_entrydate10 = aas.txt_insured_entrydate10,
              txt_insured_entrydate11 = aas.txt_insured_entrydate11,
              txt_insured_entrydate12 = aas.txt_insured_entrydate12,


              coverbaseloadingrate1 = aas.coverbaseloadingrate1,
              coverbaseloadingrate2 = aas.coverbaseloadingrate2,
              coverbaseloadingrate3 = aas.coverbaseloadingrate3,
              coverbaseloadingrate4 = aas.coverbaseloadingrate4,
              coverbaseloadingrate5 = aas.coverbaseloadingrate5,
              coverbaseloadingrate6 = aas.coverbaseloadingrate6,
              coverbaseloadingrate7 = aas.coverbaseloadingrate7,
              coverbaseloadingrate8 = aas.coverbaseloadingrate8,
              coverbaseloadingrate9 = aas.coverbaseloadingrate9,
              coverbaseloadingrate10 = aas.coverbaseloadingrate10,
              coverbaseloadingrate11 = aas.coverbaseloadingrate11,
              coverbaseloadingrate12 = aas.coverbaseloadingrate12,

              insured_loadingper1 = asidst.loading_per_insured1,
              insured_loadingper2 = asidst.loading_per_insured2,
              insured_loadingper3 = asidst.loading_per_insured3,
              insured_loadingper4 = asidst.loading_per_insured4,
              insured_loadingper5 = asidst.loading_per_insured5,
              insured_loadingper6 = asidst.loading_per_insured6,
              insured_loadingper7 = asidst.loading_per_insured7,
              insured_loadingper8 = asidst.loading_per_insured8,
              insured_loadingper9 = asidst.loading_per_insured9,
              insured_loadingper10 = asidst.loading_per_insured10,
              insured_loadingper11 = asidst.loading_per_insured11,
              insured_loadingper12 = asidst.loading_per_insured12,

              insured_loadingamt1 = aas.insured_loadingamt1,
              insured_loadingamt2 = aas.insured_loadingamt2,
              insured_loadingamt3 = aas.insured_loadingamt3,
              insured_loadingamt4 = aas.insured_loadingamt4,
              insured_loadingamt5 = aas.insured_loadingamt5,
              insured_loadingamt6 = aas.insured_loadingamt6,
              insured_loadingamt7 = aas.insured_loadingamt7,
              insured_loadingamt8 = aas.insured_loadingamt8,
              insured_loadingamt9 = aas.insured_loadingamt9,
              insured_loadingamt10 = aas.insured_loadingamt10,
              insured_loadingamt11 = aas.insured_loadingamt11,
              insured_loadingamt12 = aas.insured_loadingamt12,


              txt_insured_relation1 = aas.txt_insured_relation1,
              txt_insured_relation2 = aas.txt_insured_relation2,
              txt_insured_relation3 = aas.txt_insured_relation3,
              txt_insured_relation4 = aas.txt_insured_relation4,
              txt_insured_relation5 = aas.txt_insured_relation5,
              txt_insured_relation6 = aas.txt_insured_relation6,
              txt_insured_relation7 = aas.txt_insured_relation7,
              txt_insured_relation8 = aas.txt_insured_relation8,
              txt_insured_relation9 = aas.txt_insured_relation9,
              txt_insured_relation10 = aas.txt_insured_relation10,
              txt_insured_relation11 = aas.txt_insured_relation11,
              txt_insured_relation12 = aas.txt_insured_relation12,

              //insured_relation_tag_1 = gc.insured_relation_tag_1,
              //insured_relation_tag_2 = gc.insured_relation_tag_2,
              //insured_relation_tag_3 = gc.insured_relation_tag_3,
              //insured_relation_tag_4 = gc.insured_relation_tag_4,
              //insured_relation_tag_5 = gc.insured_relation_tag_5,
              //insured_relation_tag_6 = gc.insured_relation_tag_6,
              //insured_relation_tag_7 = gc.insured_relation_tag_7,
              //insured_relation_tag_8 = gc.insured_relation_tag_8,
              //insured_relation_tag_9 = gc.insured_relation_tag_9,
              //insured_relation_tag_10 = gc.insured_relation_tag_10,
              //insured_relation_tag_11 = gc.insured_relation_tag_11,
              //insured_relation_tag_12 = gc.insured_relation_tag_12,

              txt_insured_age1 = aas.txt_insured_age1,
              txt_insured_age2 = aas.txt_insured_age2,
              txt_insured_age3 = aas.txt_insured_age3,
              txt_insured_age4 = aas.txt_insured_age4,
              txt_insured_age5 = aas.txt_insured_age5,
              txt_insured_age6 = aas.txt_insured_age6,
              txt_insured_age7 = aas.txt_insured_age7,
              txt_insured_age8 = aas.txt_insured_age8,
              txt_insured_age9 = aas.txt_insured_age9,
              txt_insured_age10 = aas.txt_insured_age10,
              txt_insured_age11 = aas.txt_insured_age11,
              txt_insured_age12 = aas.txt_insured_age12,


              member_id1 = aas.member_id1,
              member_id2 = aas.member_id2,
              member_id3 = aas.member_id3,
              member_id4 = aas.member_id4,
              member_id5 = aas.member_id5,
              member_id6 = aas.member_id6,
              member_id7 = aas.member_id7,
              member_id8 = aas.member_id8,
              member_id9 = aas.member_id9,
              member_id10 = aas.member_id10,
              member_id11 = aas.member_id11,
              member_id12 = aas.member_id12,

              pollddesc1 = aas.pollddesc1,
              pollddesc2 = aas.pollddesc2,
              pollddesc3 = aas.pollddesc3,
              pollddesc4 = aas.pollddesc4,
              pollddesc5 = aas.pollddesc5,

              sum_insured1 = aas.sum_insured1,
              sum_insured2 = aas.sum_insured2,
              sum_insured3 = aas.sum_insured3,
              sum_insured4 = aas.sum_insured4,
              sum_insured5 = aas.sum_insured5,
              sum_insured6 = aas.sum_insured6,
              sum_insured7 = aas.sum_insured7,
              sum_insured8 = aas.sum_insured8,
              sum_insured9 = aas.sum_insured9,
              sum_insured10 = aas.sum_insured10,
              sum_insured11 = aas.sum_insured11,
              sum_insured12 = aas.sum_insured12,

              insured_cb1 = aas.insured_cb1,
              insured_cb2 = aas.insured_cb2,
              insured_cb3 = aas.insured_cb3,
              insured_cb4 = aas.insured_cb4,
              insured_cb5 = aas.insured_cb5,
              insured_cb6 = aas.insured_cb6,
              insured_cb7 = aas.insured_cb7,
              insured_cb8 = aas.insured_cb8,
              insured_cb9 = aas.insured_cb9,
              insured_cb10 = aas.insured_cb10,
              insured_cb11 = aas.insured_cb11,
              insured_cb12 = aas.insured_cb12,

              insured_deductable1 = aas.insured_deductable1,
              insured_deductable2 = aas.insured_deductable2,
              insured_deductable3 = aas.insured_deductable3,
              insured_deductable4 = aas.insured_deductable4,
              insured_deductable5 = aas.insured_deductable5,
              insured_deductable6 = aas.insured_deductable6,
              insured_deductable7 = aas.insured_deductable7,
              insured_deductable8 = aas.insured_deductable8,
              insured_deductable9 = aas.insured_deductable9,
              insured_deductable10 = aas.insured_deductable10,
              insured_deductable11 = aas.insured_deductable11,
              insured_deductable12 = aas.insured_deductable12,

              covername11 = aas.covername11,
              covername12 = aas.covername12,
              covername13 = aas.covername13,
              covername14 = aas.covername14,
              covername15 = aas.covername15,
              covername16 = aas.covername16,
              covername17 = aas.covername17,
              covername18 = aas.covername18,
              covername19 = aas.covername19,
              covername21 = aas.covername21,
              covername22 = aas.covername22,
              covername23 = aas.covername23,
              covername24 = aas.covername24,
              covername25 = aas.covername25,
              covername26 = aas.covername26,
              covername27 = aas.covername27,
              covername28 = aas.covername28,
              covername29 = aas.covername29,
              covername31 = aas.covername31,
              covername32 = aas.covername32,
              covername33 = aas.covername33,
              covername34 = aas.covername34,
              covername35 = aas.covername35,
              covername36 = aas.covername36,
              covername37 = aas.covername37,
              covername38 = aas.covername38,
              covername39 = aas.covername39,
              covername41 = aas.covername41,
              covername42 = aas.covername42,
              covername43 = aas.covername43,
              covername44 = aas.covername44,
              covername45 = aas.covername45,
              covername46 = aas.covername46,
              covername47 = aas.covername47,
              covername48 = aas.covername48,
              covername49 = aas.covername49,
              covername51 = aas.covername51,
              covername52 = aas.covername52,
              covername53 = aas.covername53,
              covername54 = aas.covername54,
              covername55 = aas.covername55,
              covername56 = aas.covername56,
              covername57 = aas.covername57,
              covername58 = aas.covername58,
              covername59 = aas.covername59,
              covername61 = aas.covername61,
              covername62 = aas.covername62,
              covername63 = aas.covername63,
              covername64 = aas.covername64,
              covername65 = aas.covername65,
              covername66 = aas.covername66,
              covername67 = aas.covername67,
              covername68 = aas.covername68,
              covername69 = aas.covername69,
              covername71 = aas.covername71,
              covername72 = aas.covername72,
              covername73 = aas.covername73,
              covername74 = aas.covername74,
              covername75 = aas.covername75,
              covername76 = aas.covername76,
              covername77 = aas.covername77,
              covername78 = aas.covername78,
              covername79 = aas.covername79,
              covername81 = aas.covername81,
              covername82 = aas.covername82,
              covername83 = aas.covername83,
              covername84 = aas.covername84,
              covername85 = aas.covername85,
              covername86 = aas.covername86,
              covername87 = aas.covername87,
              covername88 = aas.covername88,
              covername89 = aas.covername89,
              covername91 = aas.covername91,
              covername92 = aas.covername92,
              covername93 = aas.covername93,
              covername94 = aas.covername94,
              covername95 = aas.covername95,
              covername96 = aas.covername96,
              covername97 = aas.covername97,
              covername98 = aas.covername98,
              covername99 = aas.covername99,
              covername101 = aas.covername101,
              covername102 = aas.covername102,
              covername103 = aas.covername103,
              covername104 = aas.covername104,
              covername105 = aas.covername105,
              covername106 = aas.covername106,
              covername107 = aas.covername107,
              covername108 = aas.covername108,
              covername109 = aas.covername109,
              covername110 = aas.covername110,
              covername210 = aas.covername210,
              covername310 = aas.covername310,
              covername410 = aas.covername410,
              covername510 = aas.covername510,
              covername610 = aas.covername610,
              covername710 = aas.covername710,
              covername810 = aas.covername810,
              covername910 = aas.covername910,
              covername1010 = aas.covername1010,

              coversi11 = aas.coversi11,
              coversi12 = aas.coversi12,
              coversi13 = aas.coversi13,
              coversi14 = aas.coversi14,
              coversi15 = aas.coversi15,
              coversi16 = aas.coversi16,
              coversi17 = aas.coversi17,
              coversi18 = aas.coversi18,
              coversi19 = aas.coversi19,
              coversi21 = aas.coversi21,
              coversi22 = aas.coversi22,
              coversi23 = aas.coversi23,
              coversi24 = aas.coversi24,
              coversi25 = aas.coversi25,
              coversi26 = aas.coversi26,
              coversi27 = aas.coversi27,
              coversi28 = aas.coversi28,
              coversi29 = aas.coversi29,
              coversi31 = aas.coversi31,
              coversi32 = aas.coversi32,
              coversi33 = aas.coversi33,
              coversi34 = aas.coversi34,
              coversi35 = aas.coversi35,
              coversi36 = aas.coversi36,
              coversi37 = aas.coversi37,
              coversi38 = aas.coversi38,
              coversi39 = aas.coversi39,
              coversi41 = aas.coversi41,
              coversi42 = aas.coversi42,
              coversi43 = aas.coversi43,
              coversi44 = aas.coversi44,
              coversi45 = aas.coversi46,
              coversi47 = aas.coversi47,
              coversi48 = aas.coversi48,
              coversi49 = aas.coversi49,
              coversi51 = aas.coversi51,
              coversi52 = aas.coversi52,
              coversi53 = aas.coversi53,
              coversi54 = aas.coversi54,
              coversi55 = aas.coversi55,
              coversi56 = aas.coversi56,
              coversi57 = aas.coversi57,
              coversi58 = aas.coversi58,
              coversi59 = aas.coversi59,
              coversi61 = aas.coversi61,
              coversi62 = aas.coversi62,
              coversi63 = aas.coversi63,
              coversi64 = aas.coversi64,
              coversi65 = aas.coversi65,
              coversi66 = aas.coversi66,
              coversi67 = aas.coversi67,
              coversi68 = aas.coversi68,
              coversi69 = aas.coversi69,
              coversi71 = aas.coversi71,
              coversi72 = aas.coversi72,
              coversi73 = aas.coversi73,
              coversi74 = aas.coversi74,
              coversi75 = aas.coversi75,
              coversi76 = aas.coversi76,
              coversi77 = aas.coversi77,
              coversi78 = aas.coversi78,
              coversi79 = aas.coversi79,
              coversi81 = aas.coversi81,
              coversi82 = aas.coversi82,
              coversi83 = aas.coversi83,
              coversi84 = aas.coversi84,
              coversi85 = aas.coversi85,
              coversi86 = aas.coversi86,
              coversi87 = aas.coversi87,
              coversi88 = aas.coversi88,
              coversi89 = aas.coversi89,
              coversi91 = aas.coversi91,
              coversi92 = aas.coversi92,
              coversi93 = aas.coversi93,
              coversi94 = aas.coversi94,
              coversi95 = aas.coversi95,
              coversi96 = aas.coversi96,
              coversi97 = aas.coversi97,
              coversi98 = aas.coversi98,
              coversi99 = aas.coversi99,
              coversi101 = aas.coversi101,
              coversi102 = aas.coversi102,
              coversi103 = aas.coversi103,
              coversi104 = aas.coversi104,
              coversi105 = aas.coversi105,
              coversi106 = aas.coversi106,
              coversi107 = aas.coversi107,
              coversi108 = aas.coversi108,
              coversi109 = aas.coversi109,
              coversi210 = aas.coversi210,
              coversi310 = aas.coversi310,
              coversi410 = aas.coversi410,
              coversi510 = aas.coversi510,
              coversi610 = aas.coversi610,
              coversi810 = aas.coversi810,
              coversi910 = aas.coversi910,
              coversi1010 = aas.coversi1010,

              coverprem11 = aas.coverprem11,
              coverprem12 = aas.coverprem12,
              coverprem13 = aas.coverprem13,
              coverprem14 = aas.coverprem14,
              coverprem15 = aas.coverprem15,
              coverprem16 = aas.coverprem16,
              coverprem17 = aas.coverprem17,
              coverprem18 = aas.coverprem18,
              coverprem19 = aas.coverprem19,
              coverprem21 = aas.coverprem21,
              coverprem22 = aas.coverprem22,
              coverprem23 = aas.coverprem23,
              coverprem24 = aas.coverprem24,
              coverprem25 = aas.coverprem25,
              coverprem26 = aas.coverprem26,
              coverprem27 = aas.coverprem27,
              coverprem28 = aas.coverprem28,
              coverprem29 = aas.coverprem29,
              coverprem31 = aas.coverprem31,
              coverprem32 = aas.coverprem32,
              coverprem33 = aas.coverprem33,
              coverprem34 = aas.coverprem34,
              coverprem35 = aas.coverprem35,
              coverprem36 = aas.coverprem36,
              coverprem37 = aas.coverprem37,
              coverprem38 = aas.coverprem38,
              coverprem39 = aas.coverprem39,
              coverprem41 = aas.coverprem41,
              coverprem42 = aas.coverprem42,
              coverprem43 = aas.coverprem43,
              coverprem44 = aas.coverprem44,
              coverprem45 = aas.coverprem46,
              coverprem47 = aas.coverprem47,
              coverprem48 = aas.coverprem48,
              coverprem49 = aas.coverprem49,
              coverprem51 = aas.coverprem51,
              coverprem52 = aas.coverprem52,
              coverprem53 = aas.coverprem53,
              coverprem54 = aas.coverprem54,
              coverprem55 = aas.coverprem55,
              coverprem56 = aas.coverprem56,
              coverprem57 = aas.coverprem57,
              coverprem58 = aas.coverprem58,
              coverprem59 = aas.coverprem59,
              coverprem61 = aas.coverprem61,
              coverprem62 = aas.coverprem62,
              coverprem63 = aas.coverprem63,
              coverprem64 = aas.coverprem64,
              coverprem65 = aas.coverprem65,
              coverprem66 = aas.coverprem66,
              coverprem67 = aas.coverprem67,
              coverprem68 = aas.coverprem68,
              coverprem69 = aas.coverprem69,
              coverprem71 = aas.coverprem71,
              coverprem72 = aas.coverprem72,
              coverprem73 = aas.coverprem73,
              coverprem74 = aas.coverprem74,
              coverprem75 = aas.coverprem75,
              coverprem76 = aas.coverprem76,
              coverprem77 = aas.coverprem77,
              coverprem78 = aas.coverprem78,
              coverprem79 = aas.coverprem79,
              coverprem81 = aas.coverprem81,
              coverprem82 = aas.coverprem82,
              coverprem83 = aas.coverprem83,
              coverprem84 = aas.coverprem84,
              coverprem85 = aas.coverprem85,
              coverprem86 = aas.coverprem86,
              coverprem87 = aas.coverprem87,
              coverprem88 = aas.coverprem88,
              coverprem89 = aas.coverprem89,
              coverprem91 = aas.coverprem91,
              coverprem92 = aas.coverprem92,
              coverprem93 = aas.coverprem93,
              coverprem94 = aas.coverprem94,
              coverprem95 = aas.coverprem95,
              coverprem96 = aas.coverprem96,
              coverprem97 = aas.coverprem97,
              coverprem98 = aas.coverprem98,
              coverprem99 = aas.coverprem99,
              coverprem101 = aas.coverprem101,
              coverprem102 = aas.coverprem102,
              coverprem103 = aas.coverprem103,
              coverprem104 = aas.coverprem104,
              coverprem105 = aas.coverprem105,
              coverprem106 = aas.coverprem106,
              coverprem107 = aas.coverprem107,
              coverprem108 = aas.coverprem108,
              coverprem109 = aas.coverprem109,
              coverprem210 = aas.coverprem210,
              coverprem310 = aas.coverprem310,
              coverprem410 = aas.coverprem410,
              coverprem510 = aas.coverprem510,
              coverprem610 = aas.coverprem610,
              coverprem810 = aas.coverprem810,
              coverprem910 = aas.coverprem910,
              coverprem1010 = aas.coverprem1010,

              coverloadingrate11 = aas.coverloadingrate11,
              coverloadingrate12 = aas.coverloadingrate12,
              coverloadingrate13 = aas.coverloadingrate13,
              coverloadingrate14 = aas.coverloadingrate14,
              coverloadingrate15 = aas.coverloadingrate15,
              coverloadingrate16 = aas.coverloadingrate16,
              coverloadingrate17 = aas.coverloadingrate17,
              coverloadingrate18 = aas.coverloadingrate18,
              coverloadingrate19 = aas.coverloadingrate19,
              coverloadingrate21 = aas.coverloadingrate21,
              coverloadingrate22 = aas.coverloadingrate22,
              coverloadingrate23 = aas.coverloadingrate23,
              coverloadingrate24 = aas.coverloadingrate24,
              coverloadingrate25 = aas.coverloadingrate25,
              coverloadingrate26 = aas.coverloadingrate26,
              coverloadingrate27 = aas.coverloadingrate27,
              coverloadingrate28 = aas.coverloadingrate28,
              coverloadingrate29 = aas.coverloadingrate29,
              coverloadingrate31 = aas.coverloadingrate31,
              coverloadingrate32 = aas.coverloadingrate32,
              coverloadingrate33 = aas.coverloadingrate33,
              coverloadingrate34 = aas.coverloadingrate34,
              coverloadingrate35 = aas.coverloadingrate35,
              coverloadingrate36 = aas.coverloadingrate36,
              coverloadingrate37 = aas.coverloadingrate37,
              coverloadingrate38 = aas.coverloadingrate38,
              coverloadingrate39 = aas.coverloadingrate39,
              coverloadingrate41 = aas.coverloadingrate41,
              coverloadingrate42 = aas.coverloadingrate42,
              coverloadingrate43 = aas.coverloadingrate43,
              coverloadingrate44 = aas.coverloadingrate44,
              coverloadingrate45 = aas.coverloadingrate46,
              coverloadingrate47 = aas.coverloadingrate47,
              coverloadingrate48 = aas.coverloadingrate48,
              coverloadingrate49 = aas.coverloadingrate49,
              coverloadingrate51 = aas.coverloadingrate51,
              coverloadingrate52 = aas.coverloadingrate52,
              coverloadingrate53 = aas.coverloadingrate53,
              coverloadingrate54 = aas.coverloadingrate54,
              coverloadingrate55 = aas.coverloadingrate55,
              coverloadingrate56 = aas.coverloadingrate56,
              coverloadingrate57 = aas.coverloadingrate57,
              coverloadingrate58 = aas.coverloadingrate58,
              coverloadingrate59 = aas.coverloadingrate59,
              coverloadingrate61 = aas.coverloadingrate61,
              coverloadingrate62 = aas.coverloadingrate62,
              coverloadingrate63 = aas.coverloadingrate63,
              coverloadingrate64 = aas.coverloadingrate64,
              coverloadingrate65 = aas.coverloadingrate65,
              coverloadingrate66 = aas.coverloadingrate66,
              coverloadingrate67 = aas.coverloadingrate67,
              coverloadingrate68 = aas.coverloadingrate68,
              coverloadingrate69 = aas.coverloadingrate69,
              coverloadingrate71 = aas.coverloadingrate71,
              coverloadingrate72 = aas.coverloadingrate72,
              coverloadingrate73 = aas.coverloadingrate73,
              coverloadingrate74 = aas.coverloadingrate74,
              coverloadingrate75 = aas.coverloadingrate75,
              coverloadingrate76 = aas.coverloadingrate76,
              coverloadingrate77 = aas.coverloadingrate77,
              coverloadingrate78 = aas.coverloadingrate78,
              coverloadingrate79 = aas.coverloadingrate79,
              coverloadingrate81 = aas.coverloadingrate81,
              coverloadingrate82 = aas.coverloadingrate82,
              coverloadingrate83 = aas.coverloadingrate83,
              coverloadingrate84 = aas.coverloadingrate84,
              coverloadingrate85 = aas.coverloadingrate85,
              coverloadingrate86 = aas.coverloadingrate86,
              coverloadingrate87 = aas.coverloadingrate87,
              coverloadingrate88 = aas.coverloadingrate88,
              coverloadingrate89 = aas.coverloadingrate89,
              coverloadingrate91 = aas.coverloadingrate91,
              coverloadingrate92 = aas.coverloadingrate92,
              coverloadingrate93 = aas.coverloadingrate93,
              coverloadingrate94 = aas.coverloadingrate94,
              coverloadingrate95 = aas.coverloadingrate95,
              coverloadingrate96 = aas.coverloadingrate96,
              coverloadingrate97 = aas.coverloadingrate97,
              coverloadingrate98 = aas.coverloadingrate98,
              coverloadingrate99 = aas.coverloadingrate99,
              coverloadingrate101 = aas.coverloadingrate101,
              coverloadingrate102 = aas.coverloadingrate102,
              coverloadingrate103 = aas.coverloadingrate103,
              coverloadingrate104 = aas.coverloadingrate104,
              coverloadingrate105 = aas.coverloadingrate105,
              coverloadingrate106 = aas.coverloadingrate106,
              coverloadingrate107 = aas.coverloadingrate107,
              coverloadingrate108 = aas.coverloadingrate108,
              coverloadingrate109 = aas.coverloadingrate109,
              coverloadingrate210 = aas.coverloadingrate210,
              coverloadingrate310 = aas.coverloadingrate310,
              coverloadingrate410 = aas.coverloadingrate410,
              coverloadingrate510 = aas.coverloadingrate510,
              coverloadingrate610 = aas.coverloadingrate610,
              coverloadingrate810 = aas.coverloadingrate810,
              coverloadingrate910 = aas.coverloadingrate910,
              coverloadingrate1010 = aas.coverloadingrate1010,
          }
           ).Skip((pageNumber - 1) * pageSize) // Skip the appropriate number of records
            .Take(pageSize) // Take only the specified page size
        .ToListAsync();
            return new List<AarogyaSanjeevaniRNE>(asRNEData);
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
        private async Task<IEnumerable<AarogyaSanjeevaniRNE>> CalculateAarogyaSanjeevaniPremium(IEnumerable<AarogyaSanjeevaniRNE> asRNEData)
        {
            AarogyaSanjeevaniRNE os = null;

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
                table.loading_per_insured7,
                table.loading_per_insured8,
                table.loading_per_insured9,
                table.loading_per_insured10,
                table.loading_per_insured11,
                table.loading_per_insured12,
            }
        ).ToListAsync();
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


                decimal? deductablesInsured1 = row.insured_deductable1;//c99
                decimal? loyaltyDiscount = resultLoyaltyDescText;// row.loyalty_discount;//c798
                decimal? employeeDiscount = resultSearchEmployeeDescText;//c799
                decimal? onlineDiscount = resultSearchOnlineDescText;//c800

                decimal? ruralDiscount = resultRuralDescText;

                var policyType = row.policy_type;//c17
                var policyPeriod = row.policy_period;//c14

                var insured_relation_1 = dbContext.relations
                 .Where(ir => ir.insured_relation == row.txt_insured_relation1)
                 .Select(ir => ir.relation_tag)
                 .FirstOrDefault(); // Use FirstOrDefault to get a single result or null if not found
                insured_relation_1 = insured_relation_1 ?? string.Empty;

                var insured_relation_2 = dbContext.relations
                 .Where(ir => ir.insured_relation == row.txt_insured_relation2)
                 .Select(ir => ir.relation_tag)
                 .FirstOrDefault(); // Use FirstOrDefault to get a single result or null if not found
                insured_relation_2 = insured_relation_2 ?? string.Empty;

                var insured_relation_3 = dbContext.relations
                 .Where(ir => ir.insured_relation == row.txt_insured_relation3)
                 .Select(ir => ir.relation_tag)
                 .FirstOrDefault(); // Use FirstOrDefault to get a single result or null if not found
                insured_relation_3 = insured_relation_3 ?? string.Empty;

                var insured_relation_4 = dbContext.relations
                 .Where(ir => ir.insured_relation == row.txt_insured_relation4)
                 .Select(ir => ir.relation_tag)
                 .FirstOrDefault(); // Use FirstOrDefault to get a single result or null if not found
                insured_relation_4 = insured_relation_4 ?? string.Empty;

                var insured_relation_5 = dbContext.relations
                 .Where(ir => ir.insured_relation == row.txt_insured_relation5)
                 .Select(ir => ir.relation_tag)
                 .FirstOrDefault(); // Use FirstOrDefault to get a single result or null if not found
                insured_relation_5 = insured_relation_5 ?? string.Empty;

                var insured_relation_6 = dbContext.relations
                 .Where(ir => ir.insured_relation == row.txt_insured_relation6)
                 .Select(ir => ir.relation_tag)
                 .FirstOrDefault(); // Use FirstOrDefault to get a single result or null if not found
                insured_relation_6 = insured_relation_6 ?? string.Empty;

                var insured_relation_7 = dbContext.relations
             .Where(ir => ir.insured_relation == row.txt_insured_relation7)
             .Select(ir => ir.relation_tag)
             .FirstOrDefault(); // Use FirstOrDefault to get a single result or null if not found
                insured_relation_7 = insured_relation_7 ?? string.Empty;

                var insured_relation_8 = dbContext.relations
                 .Where(ir => ir.insured_relation == row.txt_insured_relation8)
                 .Select(ir => ir.relation_tag)
                 .FirstOrDefault(); // Use FirstOrDefault to get a single result or null if not found
                insured_relation_8 = insured_relation_8 ?? string.Empty;

                var insured_relation_9 = dbContext.relations
                 .Where(ir => ir.insured_relation == row.txt_insured_relation9)
                 .Select(ir => ir.relation_tag)
                 .FirstOrDefault(); // Use FirstOrDefault to get a single result or null if not found
                insured_relation_9 = insured_relation_9 ?? string.Empty;

                var insured_relation_10 = dbContext.relations
                 .Where(ir => ir.insured_relation == row.txt_insured_relation10)
                 .Select(ir => ir.relation_tag)
                 .FirstOrDefault(); // Use FirstOrDefault to get a single result or null if not found
                insured_relation_10 = insured_relation_10 ?? string.Empty;


                var insured_relation_11 = dbContext.relations
                 .Where(ir => ir.insured_relation == row.txt_insured_relation11)
                 .Select(ir => ir.relation_tag)
                 .FirstOrDefault(); // Use FirstOrDefault to get a single result or null if not found
                insured_relation_11 = insured_relation_11 ?? string.Empty;

                var insured_relation_12 = dbContext.relations
                 .Where(ir => ir.insured_relation == row.txt_insured_relation12)
                 .Select(ir => ir.relation_tag)
                 .FirstOrDefault(); // Use FirstOrDefault to get a single result or null if not found
                insured_relation_12 = insured_relation_12 ?? string.Empty;


                var insuredRelationTAGOne = insured_relation_1;
                var insuredRelationTAGTwo = insured_relation_2;//c62                
                var insuredRelationTAGThree = insured_relation_3;//c63
                var insuredRelationTAGFour = insured_relation_4;//c64
                var insuredRelationTAGFive = insured_relation_5;//c65
                var insuredRelationTAGSix = insured_relation_6;//c66
                var insuredRelationTAGSeven = insured_relation_7;//c67
                var insuredRelationTAGEight = insured_relation_8;//c68
                var insuredRelationTAGNine = insured_relation_9;//c69
                var insuredRelationTAGTen = insured_relation_10;//c70
                var insuredRelationTAGElevan = insured_relation_11;//c71
                var insuredRelationTAGTwelve = insured_relation_12;//c72 

                var insuredAgeOne = Convert.ToInt32(row.txt_insured_age1);//c61
                var insuredAgeTwo = Convert.ToInt32(row.txt_insured_age2);//c62                
                var insuredAgeThree = Convert.ToInt32(row.txt_insured_age3); // c63
                var insuredAgeFour = Convert.ToInt32(row.txt_insured_age4); // c64
                var insuredAgeFive = Convert.ToInt32(row.txt_insured_age5); // c65
                var insuredAgeSix = Convert.ToInt32(row.txt_insured_age6); // c66
                var insuredAgeSeven = Convert.ToInt32(row.txt_insured_age7); // c67
                var insuredAgeEight = Convert.ToInt32(row.txt_insured_age8); // c68
                var insuredAgeNine = Convert.ToInt32(row.txt_insured_age9); // c69
                var insuredAgeTen = Convert.ToInt32(row.txt_insured_age10); // c70
                var insuredAgeEleven = Convert.ToInt32(row.txt_insured_age11); // c71
                var insuredAgeTwelve = Convert.ToInt32(row.txt_insured_age12); // c72 


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

                decimal? siOne = row.sum_insured1;//c75
                decimal? siTwo = row.sum_insured2;//c76
                decimal? siThree = row.sum_insured3;//c77
                decimal? siFour = row.sum_insured4;//c78
                decimal? siFive = row.sum_insured5;//c79
                decimal? siSix = row.sum_insured6;//c80
                decimal? siSeven = row.sum_insured7;//c81
                decimal? siEight = row.sum_insured8;//c82
                decimal? siNine = row.sum_insured9;//c83
                decimal? siTen = row.sum_insured10;//c84
                decimal? siElevan = row.sum_insured11;//c85
                decimal? siTwelve = row.sum_insured12;//c86

                decimal? totalsuminsured = (siOne ?? 0) + (siTwo ?? 0) + (siThree ?? 0) + (siFour ?? 0) + (siFive ?? 0) + (siSix ?? 0) + (siSeven ?? 0) + (siEight ?? 0) + (siNine ?? 0) + (siTen ?? 0) + (siElevan ?? 0) + (siTwelve ?? 0);

                decimal? cbOne = Convert.ToDecimal(row.insured_cb1);
                decimal? cbTwo = Convert.ToDecimal(row.insured_cb2);
                decimal? cbThree = Convert.ToDecimal(row.insured_cb3);
                decimal? cbFour = Convert.ToDecimal(row.insured_cb4);
                decimal? cbFive = Convert.ToDecimal(row.insured_cb5);
                decimal? cbSix = Convert.ToDecimal(row.insured_cb6);
                decimal? cbSeven = Convert.ToDecimal(row.insured_cb7);
                decimal? cbEight = Convert.ToDecimal(row.insured_cb8);
                decimal? cbNine = Convert.ToDecimal(row.insured_cb9);
                decimal? cbTen = Convert.ToDecimal(row.insured_cb10);
                decimal? cbEleven = Convert.ToDecimal(row.insured_cb11);
                decimal? cbTwelve = Convert.ToDecimal(row.insured_cb12);

                decimal? cumulativebonus = (cbOne ?? 0) + (cbTwo ?? 0) + (cbThree ?? 0) + (cbFour ?? 0) + (cbFive ?? 0) + (cbSix ?? 0) + (cbSeven ?? 0) + (cbEight ?? 0) + (cbNine ?? 0) + (cbTen ?? 0) + (cbEleven ?? 0) + (cbTwelve ?? 0);

                decimal? basicLoadingRateOne = iDSTData.loading_per_insured1 ?? 0;//c111
                decimal? basicLoadingRateTwo = iDSTData.loading_per_insured2 ?? 0;//c112
                decimal? basicLoadingRateThree = iDSTData.loading_per_insured3 ?? 0;//c113
                decimal? basicLoadingRateFour = iDSTData.loading_per_insured4 ?? 0;//c114
                decimal? basicLoadingRateFive = iDSTData.loading_per_insured5 ?? 0;//c115
                decimal? basicLoadingRateSix = iDSTData.loading_per_insured6 ?? 0;//c116
                decimal? basicLoadingRateSeven = iDSTData.loading_per_insured7 ?? 0;//c117
                decimal? basicLoadingRateEight = iDSTData.loading_per_insured8 ?? 0;//c118
                decimal? basicLoadingRateNine = iDSTData.loading_per_insured9 ?? 0;//c119
                decimal? basicLoadingRateTen = iDSTData.loading_per_insured10 ?? 0;//c120
                decimal? basicLoadingRateEleven = iDSTData.loading_per_insured11 ?? 0;//c121
                decimal? basicLoadingRateTwelve = iDSTData.loading_per_insured12 ?? 0;//c122


                decimal? loyaltyDiscountValue = loyaltyDiscount;
                loyaltyDiscountValue = loyaltyDiscount.HasValue && loyaltyDiscount.Value > 0 ? 2.5m : 0.0m;

                decimal? employeeDiscountValue = employeeDiscount;
                employeeDiscountValue = employeeDiscount.HasValue && employeeDiscount.Value > 0 ? 5.0m : 0.0m;

                decimal? onlineDiscountValue = onlineDiscount;
                onlineDiscountValue = onlineDiscount.HasValue && onlineDiscount.Value > 0 ? 5.0m : 0.0m;

                // Calculate the discount based on the policy type and number of members
                decimal? familyDiscountValue = CalculateFamilyDiscount(policyType, numberOfMembers);
                decimal? ruralDiscountValue = CalculateRuralDiscount(ruralDiscount);

                decimal? cappingDiscount = CalculateCappingDiscount(familyDiscountValue, onlineDiscountValue, employeeDiscountValue, loyaltyDiscountValue, ruralDiscountValue);
                //// Calculate the percentage based on the policy period
                var columnName = GetColumnNameForPolicyPeriod(policyPeriod);
                if (columnName == null)
                {
                    throw new ArgumentException($"Invalid policy period: {policyPeriod}");
                }

                var productName = row.prod_name;
                string grouporretail = GetGroupOrRetail(productName);
                string grpcolumnName = (grouporretail == "Group") ? "group" : "retail";

                // Construct the raw SQL query
                var sql = $@"
                    SELECT {grpcolumnName}
                    FROM aarogya_sanjeevani_baserates
                    WHERE suminsured = @p0 AND age = @p1";

                // Execute the SQL query
                var basePremium1 = await dbContext.aarogya_sanjeevani_baserates
                     .FromSqlRaw(sql, siOne, insuredAgeOne)
                      .Select(r => EF.Property<decimal?>(r, grpcolumnName))
                      .FirstOrDefaultAsync();
                // Execute the raw SQL query
                var basePremium2 = await dbContext.aarogya_sanjeevani_baserates
                    .FromSqlRaw(sql, siTwo, insuredAgeTwo)
                    .Select(r => EF.Property<decimal?>(r, grpcolumnName))
                    .FirstOrDefaultAsync();
                // Execute the raw SQL query
                var basePremium3 = await dbContext.aarogya_sanjeevani_baserates
                    .FromSqlRaw(sql, siThree, insuredAgeThree)
                    .Select(r => EF.Property<decimal?>(r, grpcolumnName))
                    .FirstOrDefaultAsync();
                var basePremium4 = await dbContext.aarogya_sanjeevani_baserates
                   .FromSqlRaw(sql, siFour, insuredAgeFour)
                   .Select(r => EF.Property<decimal?>(r, grpcolumnName))
                   .FirstOrDefaultAsync();
                // Execute the raw SQL query
                var basePremium5 = await dbContext.aarogya_sanjeevani_baserates
                    .FromSqlRaw(sql, siFive, insuredAgeFive)
                    .Select(r => EF.Property<decimal?>(r, grpcolumnName))
                    .FirstOrDefaultAsync();
                var basePremium6 = await dbContext.aarogya_sanjeevani_baserates
                   .FromSqlRaw(sql, siSix, insuredAgeSix)
                   .Select(r => EF.Property<decimal?>(r, grpcolumnName))
                   .FirstOrDefaultAsync();
                // Execute the raw SQL query
                var basePremium7 = await dbContext.aarogya_sanjeevani_baserates
                    .FromSqlRaw(sql, siSeven, insuredAgeSeven)
                    .Select(r => EF.Property<decimal?>(r, grpcolumnName))
                    .FirstOrDefaultAsync();
                var basePremium8 = await dbContext.aarogya_sanjeevani_baserates
                   .FromSqlRaw(sql, siEight, insuredAgeEight)
                   .Select(r => EF.Property<decimal?>(r, grpcolumnName))
                   .FirstOrDefaultAsync();
                // Execute the raw SQL query
                var basePremium9 = await dbContext.aarogya_sanjeevani_baserates
                    .FromSqlRaw(sql, siNine, insuredAgeNine)
                    .Select(r => EF.Property<decimal?>(r, grpcolumnName))
                      .FirstOrDefaultAsync();
                var basePremium10 = await dbContext.aarogya_sanjeevani_baserates
                    .FromSqlRaw(sql, siTen, insuredAgeTen)
                    .Select(r => EF.Property<decimal?>(r, grpcolumnName))
                    .FirstOrDefaultAsync();
                // Execute the raw SQL query
                var basePremium11 = await dbContext.aarogya_sanjeevani_baserates
                    .FromSqlRaw(sql, siElevan, insuredAgeEleven)
                    .Select(r => EF.Property<decimal?>(r, grpcolumnName))
                    .FirstOrDefaultAsync();
                var basePremium12 = await dbContext.aarogya_sanjeevani_baserates
                   .FromSqlRaw(sql, siTwelve, insuredAgeTwelve)
                   .Select(r => EF.Property<decimal?>(r, grpcolumnName))
                   .FirstOrDefaultAsync();

                var basePremiumvalues = new List<decimal?> { basePremium1, basePremium2, basePremium3, basePremium4, basePremium5, basePremium6, basePremium7, basePremium8, basePremium9, basePremium10, basePremium11, basePremium12 };
                string condition = policyType;

                decimal? basePremium = CalculateResult(condition, basePremiumvalues);

                decimal? loadingPrem_1 = CalculateLoadingPrem(basePremium1, basicLoadingRateOne);
                decimal? loadingPrem_2 = CalculateLoadingPrem(basePremium2, basicLoadingRateTwo);
                decimal? loadingPrem_3 = CalculateLoadingPrem(basePremium3, basicLoadingRateThree);
                decimal? loadingPrem_4 = CalculateLoadingPrem(basePremium4, basicLoadingRateFour);
                decimal? loadingPrem_5 = CalculateLoadingPrem(basePremium5, basicLoadingRateFive);
                decimal? loadingPrem_6 = CalculateLoadingPrem(basePremium6, basicLoadingRateSix);
                decimal? loadingPrem_7 = CalculateLoadingPrem(basePremium7, basicLoadingRateSeven);
                decimal? loadingPrem_8 = CalculateLoadingPrem(basePremium8, basicLoadingRateEight);
                decimal? loadingPrem_9 = CalculateLoadingPrem(basePremium9, basicLoadingRateNine);
                decimal? loadingPrem_10 = CalculateLoadingPrem(basePremium10, basicLoadingRateTen);
                decimal? loadingPrem_11 = CalculateLoadingPrem(basePremium11, basicLoadingRateEleven);
                decimal? loadingPrem_12 = CalculateLoadingPrem(basePremium12, basicLoadingRateTwelve);
                var loadingPremvalues = new List<decimal?> { loadingPrem_1, loadingPrem_2, loadingPrem_3, loadingPrem_4,
                    loadingPrem_5, loadingPrem_6, loadingPrem_7, loadingPrem_8, loadingPrem_9, loadingPrem_10, loadingPrem_11, loadingPrem_12 };
                decimal? loadingPrem = loadingPremvalues.Sum();

                decimal? BasePremiumLoading = basePremium + loadingPrem;

                decimal? BaseAndLoadingLoyaltyDiscount = loyaltyDiscountValue / 100 * BasePremiumLoading;
                decimal? BaseAndLoadingEmployeeDiscount = employeeDiscountValue / 100 * BasePremiumLoading;
                decimal? BaseAndLoadingOnlineDiscount = (onlineDiscountValue / 100) * BasePremiumLoading;
                decimal? BaseAndLoadingFamilyDiscount = familyDiscountValue * BasePremiumLoading;
                decimal? BaseAndLoadingRuralDiscount = ruralDiscountValue * BasePremiumLoading;
                decimal? BaseAndLoadingCappingDiscount = cappingDiscount * BasePremiumLoading;

                decimal? netPremium = BasePremiumLoading - BaseAndLoadingCappingDiscount;

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

                os = new AarogyaSanjeevaniRNE
                {
                    prod_code = row.prod_code,
                    prod_name = row.prod_name,
                    //policy_no = row.policy_no,//check val not correct 
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
                    txt_mobile = row.txt_mobile,
                    txt_telephone = row.txt_telephone,
                    txt_email = row.txt_email,
                    txt_dealer_cd = row.txt_dealer_cd,//intermediary_code in gc mapping
                    imdname = row.imdname,//intermediary_name in gc
                    verticalname = row.verticalname,//psm_name in gc
                    //ssm_name = row.ssm_name,
                    txt_family = row.txt_family,
                    isrnflag = row.isrnflag,//chk
                    reference_num = row.reference_num,//proposal no in gc
                    split_flag = row.split_flag,
                    isvipflag = row.isvipflag,//chk 
                    txt_insured_entrydate1 = row.txt_insured_entrydate1,//chk inceptiondate in gc
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

                    member_id1 = row.member_id1,
                    member_id2 = row.member_id2,
                    member_id3 = row.member_id3,
                    member_id4 = row.member_id4,
                    member_id5 = row.member_id5,
                    member_id6 = row.member_id6,
                    member_id7 = row.member_id7,
                    member_id8 = row.member_id8,
                    member_id9 = row.member_id9,
                    member_id10 = row.member_id10,
                    member_id11 = row.member_id11,
                    member_id12 = row.member_id12,

                    insured_loadingper1 = basicLoadingRateOne,
                    insured_loadingper2 = basicLoadingRateTwo,
                    insured_loadingper3 = basicLoadingRateThree,
                    insured_loadingper4 = basicLoadingRateFour,
                    insured_loadingper5 = basicLoadingRateFive,
                    insured_loadingper6 = basicLoadingRateSix,
                    insured_loadingper7 = basicLoadingRateSeven,
                    insured_loadingper8 = basicLoadingRateEight,
                    insured_loadingper9 = basicLoadingRateNine,
                    insured_loadingper10 = basicLoadingRateTen,
                    insured_loadingper11 = basicLoadingRateEleven,
                    insured_loadingper12 = basicLoadingRateTwelve,

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

                    txt_insured_relation1 = row.txt_insured_relation1,//coming as "string"
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


                    insured_relation_tag_1 = insuredRelationTAGOne,
                    insured_relation_tag_2 = insuredRelationTAGTwo,
                    insured_relation_tag_3 = insuredRelationTAGThree,
                    insured_relation_tag_4 = insuredRelationTAGFour,
                    insured_relation_tag_5 = insuredRelationTAGFive,
                    insured_relation_tag_6 = insuredRelationTAGSix,
                    insured_relation_tag_7 = insuredRelationTAGSeven,
                    insured_relation_tag_8 = insuredRelationTAGEight,
                    insured_relation_tag_9 = insuredRelationTAGNine,
                    insured_relation_tag_10 = insuredRelationTAGTen,
                    insured_relation_tag_11 = insuredRelationTAGElevan,
                    insured_relation_tag_12 = insuredRelationTAGTwelve,

                    pre_existing_disease1 = row.pre_existing_disease1,
                    pre_existing_disease2 = row.pre_existing_disease2,
                    pre_existing_disease3 = row.pre_existing_disease3,
                    pre_existing_disease4 = row.pre_existing_disease4,
                    pre_existing_disease5 = row.pre_existing_disease5,
                    pre_existing_disease6 = row.pre_existing_disease6,
                    pre_existing_disease7 = row.pre_existing_disease7,
                    pre_existing_disease8 = row.pre_existing_disease8,
                    pre_existing_disease9 = row.pre_existing_disease9,
                    pre_existing_disease10 = row.pre_existing_disease10,
                    pre_existing_disease11 = row.pre_existing_disease11,
                    pre_existing_disease12 = row.pre_existing_disease12,


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

                    insured_deductable1 = row.insured_deductable1,
                    insured_deductable2 = row.insured_deductable2,
                    insured_deductable3 = row.insured_deductable3,
                    insured_deductable4 = row.insured_deductable4,
                    insured_deductable5 = row.insured_deductable5,
                    insured_deductable6 = row.insured_deductable6,
                    insured_deductable7 = row.insured_deductable7,
                    insured_deductable8 = row.insured_deductable8,
                    insured_deductable9 = row.insured_deductable9,
                    insured_deductable10 = row.insured_deductable10,
                    insured_deductable11 = row.insured_deductable11,
                    insured_deductable12 = row.insured_deductable12,


                    wellness_discount1 = row.wellness_discount1,
                    wellness_discount2 = row.wellness_discount2,
                    wellness_discount3 = row.wellness_discount3,
                    wellness_discount4 = row.wellness_discount4,
                    wellness_discount5 = row.wellness_discount5,
                    wellness_discount6 = row.wellness_discount6,
                    wellness_discount7 = row.wellness_discount7,
                    wellness_discount8 = row.wellness_discount8,
                    wellness_discount9 = row.wellness_discount9,
                    wellness_discount10 = row.wellness_discount10,
                    wellness_discount11 = row.wellness_discount11,
                    wellness_discount12 = row.wellness_discount12,


                    stayactive1 = row.stayactive1,
                    stayactive2 = row.stayactive2,
                    stayactive3 = row.stayactive3,
                    stayactive4 = row.stayactive4,
                    stayactive5 = row.stayactive5,
                    stayactive6 = row.stayactive6,
                    stayactive7 = row.stayactive7,
                    stayactive8 = row.stayactive8,
                    stayactive9 = row.stayactive9,
                    stayactive10 = row.stayactive10,
                    stayactive11 = row.stayactive11,
                    stayactive12 = row.stayactive12,

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

                    health_incentive1 = row.health_incentive1,
                    health_incentive2 = row.health_incentive2,
                    health_incentive3 = row.health_incentive3,
                    health_incentive4 = row.health_incentive4,
                    health_incentive5 = row.health_incentive5,
                    health_incentive6 = row.health_incentive6,
                    health_incentive7 = row.health_incentive7,
                    health_incentive8 = row.health_incentive8,
                    health_incentive9 = row.health_incentive9,
                    health_incentive10 = row.health_incentive10,
                    health_incentive11 = row.health_incentive11,
                    health_incentive12 = row.health_incentive12,

                    fitness_discount1 = row.fitness_discount1,
                    fitness_discount2 = row.fitness_discount2,
                    fitness_discount3 = row.fitness_discount3,
                    fitness_discount4 = row.fitness_discount4,
                    fitness_discount5 = row.fitness_discount5,
                    fitness_discount6 = row.fitness_discount6,
                    fitness_discount7 = row.fitness_discount7,
                    fitness_discount8 = row.fitness_discount8,
                    fitness_discount9 = row.fitness_discount9,
                    fitness_discount10 = row.fitness_discount10,
                    fitness_discount11 = row.fitness_discount11,
                    fitness_discount12 = row.fitness_discount12,

                    reservbenefis1 = row.reservbenefis1,
                    reservbenefis2 = row.reservbenefis2,
                    reservbenefis3 = row.reservbenefis3,
                    reservbenefis4 = row.reservbenefis4,
                    reservbenefis5 = row.reservbenefis5,
                    reservbenefis6 = row.reservbenefis6,
                    reservbenefis7 = row.reservbenefis7,
                    reservbenefis8 = row.reservbenefis8,
                    reservbenefis9 = row.reservbenefis9,
                    reservbenefis10 = row.reservbenefis10,
                    reservbenefis11 = row.reservbenefis11,
                    reservbenefis12 = row.reservbenefis12,

                    insured_rb_claimamt1 = row.insured_rb_claimamt1,
                    insured_rb_claimamt2 = row.insured_rb_claimamt2,
                    insured_rb_claimamt3 = row.insured_rb_claimamt3,
                    insured_rb_claimamt4 = row.insured_rb_claimamt4,
                    insured_rb_claimamt5 = row.insured_rb_claimamt5,
                    insured_rb_claimamt6 = row.insured_rb_claimamt6,
                    insured_rb_claimamt7 = row.insured_rb_claimamt7,
                    insured_rb_claimamt8 = row.insured_rb_claimamt8,
                    insured_rb_claimamt9 = row.insured_rb_claimamt9,
                    insured_rb_claimamt10 = row.insured_rb_claimamt10,
                    insured_rb_claimamt11 = row.insured_rb_claimamt11,
                    insured_rb_claimamt12 = row.insured_rb_claimamt12,


                    preventive_hc = row.preventive_hc,
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
                    online_discount = (onlineDiscountValue * 100),//
                    loyalty_discount = loyaltyDiscountValue,
                    family_discount = (familyDiscountValue * 100),
                    rural_discount = ruralDiscountValue,
                    capping_discount = cappingDiscount,

                    base_premium_1 = basePremium1,//chk
                    base_premium_2 = basePremium2,
                    base_premium_3 = basePremium3,
                    base_premium_4 = basePremium4,
                    base_premium_5 = basePremium5,
                    base_premium_6 = basePremium6,
                    base_premium_7 = basePremium7,
                    base_premium_8 = basePremium8,
                    base_premium_9 = basePremium9,
                    base_premium_10 = basePremium10,
                    base_premium_11 = basePremium11,
                    base_premium_12 = basePremium12,
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

                    // netPremium = netPremium,
                    netPremium = netPremium.HasValue ? Math.Round(netPremium.Value, 2) : (decimal?)null,

                    // finalPremium = finalPremium,
                    finalPremium = finalPremium.HasValue ? Math.Round(finalPremium.Value, 2) : (decimal?)null,


                    //GST = GST,
                    GST = GST.HasValue ? Math.Round(GST.Value, 2) : (decimal?)null,
                    //crossCheck = Crosscheck,
                    crossCheck = Crosscheck.HasValue ? Math.Round(Crosscheck.Value, 2) : (decimal?)null,

                    aarogyasanjeevani_total_Premium = row.num_tot_premium.HasValue ? Math.Round(row.num_tot_premium.Value, 2) : (decimal?)null,

                    aarogyasanjeevani_netpremium = row.num_net_premium.HasValue ? Math.Round(row.num_net_premium.Value, 2) : (decimal?)null,

                    aarogyasanjeevani_GST = row.num_service_tax.HasValue ? Math.Round(row.num_service_tax.Value, 2) : (decimal?)null

                };
            }
            return new List<AarogyaSanjeevaniRNE> { os };
        }

        private static string GetGroupOrRetail(string productName)
        {
            try
            {
                // Check if "Group" is present in the product name
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
                cIFamilyDiscountValue = 0.1m; // 2.5%
            }

            return cIFamilyDiscountValue;
        }

        private static decimal? GetFamilyDiscount(int? noOfMembers)
        {
            decimal? cIFamilyDiscountValue = 0m;
            if (noOfMembers > 1)
            {
                cIFamilyDiscountValue = 0.1m; // 2.5%
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

        static decimal? CalculateLoadingPrem(decimal? basepremium1, decimal? basicloadingrate1)
        {
            if (basepremium1.HasValue && basicloadingrate1.HasValue)
            {
                return basepremium1.Value * (basicloadingrate1.Value / 100);
            }
            return null;
        }

        // Function to map policy_period to column name
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
            //if (values == null || !values.Any())
            //{
            //    throw new ArgumentException("Values list cannot be null or empty.");
            //}

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
            // Check the conditions and return the appropriate discount
            if (policyType == "Individual" && numberOfMembers > 1)
            {
                return 0.1m; // 10% discount
            }
            else
            {
                return 0.00m; // 0% discount
            }
        }

        private static decimal CalculateRuralDiscount(decimal? ruralDiscount)
        {
            // Check the conditions and return the appropriate discount
            if (ruralDiscount > 0)
            {
                return 0.15m; // 5.5% discount
            }
            else
            {
                return 0.00m; // 0% discount
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

    }
}



