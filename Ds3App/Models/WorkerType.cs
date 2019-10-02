namespace Ds3App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WorkerType
    {
        public Guid Id { get; set; }

        [Required]
        public string Type { get; set; }
        public string Slug { get; set; }

        public bool IsDeleted { get; set; }
    }
}
