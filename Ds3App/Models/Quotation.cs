namespace Ds3App.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Quotation
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }
        [Display(Name = "Foreman")]
        public int Foreman { get; set; }
        [Display(Name = "Skilled Workers")]
        public int SkilledWorkers { get; set; }
        [Display(Name = "Semi-Skilled Workers")]
        public int SemiSkilledWorkers { get; set; }
        [Display(Name = "General Workers")]
        public int GeneralWorkers { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Work Weekends")]
        public bool WorkWeekends { get; set; }
        [Display(Name = "Shifts")]
        public int Shifts { get; set; }
        [Display(Name = "Shift Hours")]
        public int ShiftHours { get; set; }
        [Display(Name = "Estimated Cost")]
        public decimal EstimatedCost { get; set; }
        [Display(Name = "IsDeleted")]
        public bool IsDeleted { get; set; }
        [Required]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Required]
        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; }
        public string ClientId { get; set; }
        [Display(Name = "")]
        public string Documents { get; set; }
        [Display(Name = "Foreman Tasks")]
        public int ForemanTasks { get; set; }
        [Display(Name = "Skilled Worker Tasks")]
        public int SkilledWorkerTasks { get; set; }
        [Display(Name = "General Worker Tasks")]
        public int GeneralWorkerTasks { get; set; }
        [Display(Name = "Semi-Skilled Worker Tasks")]
        public int SemiSkilledWorkerTasks { get; set; }

        [Display(Name = "Foreman Task Description")]
        [DataType(DataType.MultilineText)]
        public string FTaskDescription { get; set; }
        [Display(Name = "Skilled Worker Task Description")]
        [DataType(DataType.MultilineText)]
        public string SWTaskDescription { get; set; }
        [Display(Name = "Semi-skilled Worker Task Description")]
        [DataType(DataType.MultilineText)]
        public string SSWTaskDescription { get; set; }
        [Display(Name = "General Worker Task Description")]
        [DataType(DataType.MultilineText)]
        public string GWTaskDescription { get; set; }

        [Display(Name = "Approved")]
        public bool Approved { get; set; }
        [Display(Name = "")]
        public bool Declined { get; set; }
    }
}
