using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ds3App.Models
{
    public class ForemanAssignedToProject
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string ForemanId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateTimeStamp { get; set; }
    }
}