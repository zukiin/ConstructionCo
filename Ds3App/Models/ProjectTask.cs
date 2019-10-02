namespace Ds3App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProjectTask
    {
        public Guid Id { get; set; }
        [Display(Name = "Project")]
        public Guid ProjectId { get; set; }
        [Display(Name = "Task Name")]
        public Guid ConstructionTask { get; set; }
        [Display(Name = "Date Assigned")]
        public DateTime DateAssigned { get; set; }
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        [Display(Name = "Assigned To")]
        public string WorkerId { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; }
    }
}
