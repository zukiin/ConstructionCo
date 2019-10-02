namespace Ds3App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Payment
    {
        public Guid Id { get; set; }
        [Display(Name = "Quotation Reference")]
        public string QuotationReference { get; set; }
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Display(Name = "Amount Due")]
        public decimal AmountDue { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }

        public Guid ContractId { get; set; }
        [Display(Name = "Proof Of Payment")]
        public string ProofOfPayment { get; set; }
        [Display(Name = "Online Payment")]
        public bool IsOnlinePayment { get; set; }
        public bool IsDeleted { get; set; }
        public string ClientId { get; internal set; }
    }
}
