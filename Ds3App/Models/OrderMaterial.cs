using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ds3App.Models
{
    public class OrderMaterial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderId { get; set; }



        public int Quantity { get; set; }


        [ForeignKey("SupplierId")]
        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}