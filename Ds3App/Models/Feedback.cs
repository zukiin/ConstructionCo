using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ds3App.Models
{
    public class Feedback
    {
        public Guid id { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
        public string Comment { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string ClientId { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
        public Guid ProjectId { get; set; }
    }
}