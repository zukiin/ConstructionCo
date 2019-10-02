namespace Ds3App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Contract
    {
        public Guid Id { get; set; }
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }
         [Display(Name = "Client Surname")]
        public string ClientSurname { get; set; }
  
        public string Email { get; set; }

        public string Contact { get; set; }
        [Display(Name = "Quotation Reference")]
        public string QuotationReference { get; set; }
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Display(Name = "Project Cost")]
        public string ProjectCost { get; set; }
        [Display(Name = "Contract Content")]
        public string ContractContent { get; set; }

        public string DateTimeStamp { get; set; }

        public bool IsSigned { get; set; }

        public bool IsNotAccepted { get; set; }

        public string ClientId { get; set; }

        public string ContractIssuedBy { get; set; }

        public bool IsDeleted { get; set; }

        public string Status { get; set; }
    }
}
