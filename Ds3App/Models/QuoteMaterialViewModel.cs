using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ds3App.Models
{
    public class QuoteMaterialViewModel
    {
        public Guid Id { get; set; }
        public string MaterialImage { get; set; }
        [Display(Name = "Material Name")]
        public string MaterialName { get; set; }
        [Display(Name = "MaterialDescription")]
        public string MaterialDescription { get; set; }
        public int Quantity { get; set; }
        public bool IsAdded { get; set; }
    }
}