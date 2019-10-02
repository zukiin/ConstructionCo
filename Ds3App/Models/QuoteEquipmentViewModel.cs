using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ds3App.Models
{
    public class QuoteEquipmentViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Equipment Image")]
        public string EquipmentImage { get; set; }
        [Display(Name = "Equipment Name")]
        public string EquipmentName { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public int Quantity { get; set; }
        public bool IsAdded { get; set; }
    }
}