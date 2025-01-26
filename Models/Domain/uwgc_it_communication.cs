using System.ComponentModel.DataAnnotations;

namespace RenewalGovernancePremiumValidation.Models.Domain
{
    public class uwgc_it_communication
    {
        [Key]
        public string? policy_no { get; set; }
        public string? commented_by { get; set; }
        public string? comments { get; set; }
        public bool? approve { get; set; }
        public bool? decline { get; set; }
        public bool? differ { get; set; }
        public DateTime? modifieddatetime { get; set; }

    }
}
