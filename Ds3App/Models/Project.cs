namespace Ds3App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Project
    {
        public Guid Id { get; set; }
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; }
        [Display(Name = "Project Cost")]
        public decimal ProjectCost { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Progress")]
        public string Progress { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        public bool IsCompleted { get; set; }
        public string ClientId { get; set; }
        public bool IsDeleted { get; set; }
        public string CompleteTasks { get; set; }
        public int ForemanTasks { get; set; }
        public int SkilledWorkerTasks { get; set; }
        public int SemiSkilledWorkerTasks { get; set; }
        public int GeneralWorkerTasks { get; set; }
        public string QuotationReference { get; set; }
        [Display(Name = "Project Report")]
        [DataType(DataType.MultilineText)]
        public string ProjectReport { get; set; }
    }
}
