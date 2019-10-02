using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ds3App.Models
{
    public class QuoteEquipment
    {
        public Guid Id { get; set; }
        public Guid QuotationRequestId { get; set; }
        public Guid EquipmentId { get; set; }
        public int Quantity { get; set; }
        public bool IsAdded { get; set; }
    }
}