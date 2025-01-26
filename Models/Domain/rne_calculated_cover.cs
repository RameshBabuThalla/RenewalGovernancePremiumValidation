namespace RenewalGovernancePremiumValidation.Models.Domain
{
    public class rne_calculated_cover
    {
        public string jobid { get; set; }
        public string batchid { get; set; }
        public long referencenum { get; set; }
        public string splittype { get; set; }
        public string splitid { get; set; }
        public string upsellid { get; set; }
        public short isupsell { get; set; }
        public string sectionid { get; set; }
        public string riskid { get; set; }
        public string coverid { get; set; }
        public string riskname { get; set; }
        public string covername { get; set; }
        public decimal suminsured { get; set; }
        public decimal premium { get; set; }
        public TimeSpan processedon { get; set; }
        public short isarchived { get; set; }
        public TimeSpan archivedon { get; set; }
        public decimal loadingrate { get; set; }
        public decimal loadingamount { get; set; }
        public string exclusion { get; set; }
        public decimal isapplicable { get; set; }
        public int coverseq { get; set; }
        public decimal discoutpr { get; set; }
        public decimal discountamount { get; set; }
        public string coverstartdate { get; set; }
        public string coverendtdate { get; set; }
        public decimal covertax { get; set; }
        public decimal totalpremium { get; set; }
        public decimal userrate { get; set; }
    }
}
