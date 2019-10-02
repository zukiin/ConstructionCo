namespace Ds3App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class QuotationRequest
    {
        public Guid Id { get; set; }

        public string ClientId { get; set; }

        [Required]
        public string ReferenceNumber { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string ProjectDescription { get; set; }

        public string ProjectDocuments { get; set; }

        public DateTime DateTimeStamp { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsDeleted { get; set; }

        public string Status { get; set; }
    }
}
