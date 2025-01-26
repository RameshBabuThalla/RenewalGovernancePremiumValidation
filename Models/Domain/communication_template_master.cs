using System.ComponentModel.DataAnnotations;

namespace RenewalGovernancePremiumValidation.Models.Domain
{
    public class communication_template_master
    {
        [Key]
        public int templateid { get; set; }
        public string templatename { get; set; }
        public string template_content { get; set; }
    }
}
