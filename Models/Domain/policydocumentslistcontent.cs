using System.ComponentModel.DataAnnotations;

namespace RenewalGovernancePremiumValidation.Models.Domain
{
    public class policydocumentslistcontent
    {
        [Key]
        public int id { get; set; }
        public string filename { get; set; }
        public string mimetype { get; set; }
        public string filecontent { get; set; }

        public string policynumber { get; set; }
        public string objectid { get; set; }
        public string filetype { get;set; }
    }
}
