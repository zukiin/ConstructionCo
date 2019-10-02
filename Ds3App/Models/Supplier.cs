namespace Ds3App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Supplier
    {
        public Guid Id { get; set; }

        [Required]
        public string SupplierName { get; set; }

        [Required]
        [StringLength(10)]
        public string SupplierContact { get; set; }

        [Required]
        public string SupplierEmail { get; set; }

        public bool IsDeleted { get; set; }
    }
}
