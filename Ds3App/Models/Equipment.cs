namespace Ds3App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Equipment
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name ="Equipment Name")]
        public string EquipmentName { get; set; }

        [Required]
      
        public string Model { get; set; }

        [DataType(DataType.Date)]
        public DateTime LastMaintenance { get; set; }

        [Required]
        public string Brand { get; set; }
        [Display(Name ="Rate Per Hour")]
        [Required]
        public decimal RatePerHour { get; set; }

        public bool IsDeleted { get; set; }
        [Display(Name ="Equipment Image")]
        public string EquipmentImage { get; set; }

        public Equipment()
        {
            this.LastMaintenance = DateTime.Now;
        }
    }
}
