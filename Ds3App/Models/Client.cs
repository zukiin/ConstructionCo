namespace Ds3App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Client
    {
        public Guid Id { get; set; }
  
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Contact { get; set; }

        public bool IsDeleted { get; set; }

        public string UserId { get; set; }
    }
}
