namespace Ds3App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Material
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name ="MaterialName")]
        public string MaterialName { get; set; }

        [Required]
        [Display(Name ="Material Description")]
        public string MaterialDescription { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Display(Name ="Stock Quantity")]
        public int StockQuantity { get; set; }

        public bool IsDeleted { get; set; }

        public Guid Supplier { get; set; }
        [Display(Name ="Supplier Name")]
        public string SupplierName { get; set; }
        [Display(Name ="Material Image")]
        public string MaterialImage { get; set; }
    }
}
