using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ds3App.Models
{
    public class QuotationViewModel
    {
        public Guid QuoteRequestId { get; set; }
        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get;  set; }
        [Display(Name = "Project Name")]
        public string ProjectName { get;  set; }
        [Display(Name = "Project Description")]
        public string ProjectDescription { get;  set; }
        [Display(Name = "Client")]
        public string ClientId { get;  set; }
        [Display(Name = "Documents")]
        public string Documents { get;  set; }
        [Display(Name = "Skilled Workers")]
        public string SkilledWorkers { get;  set; }
        [Display(Name = "Shift Hours")]
        public string ShiftHours { get;  set; }
        [Display(Name = "Shifts")]
        public string Shifts { get;  set; }
        [Display(Name = "Work Weekends")]
        public bool WorkWeekends { get;  set; }
        [Display(Name = "End Date")]
        public string EndDate { get;  set; }
        [Display(Name = "Start Date")]
        public string StartDate { get;  set; }

        [Display(Name = "General Worker Tasks")]
        public string GeneralWorkerTasks { get; set; }
        [Display(Name = "Skilled Worker Tasks")]
        public string SkilledWorkerTasks { get; set; }
        [Display(Name = "Semi-Skilled Worker Tasks")]
        public string SemiSkilledWorkerTasks { get; set; }

        [Display(Name = "Skilled Worker Task Description")]
        [DataType(DataType.MultilineText)]
        public string SWTaskDescription { get; set; }
        [Display(Name = "Semi-Skilled Worker Task Description")]
        [DataType(DataType.MultilineText)]
        public string SSWTaskDescription { get; set; }
        [Display(Name = "General Worker Task Description")]
        [DataType(DataType.MultilineText)]
        public string GWTaskDescription { get; set; }

        [Display(Name = "Semi-Skilled Workers")]
        public string SemiSkilledWorkers { get;  set; }
        [Display(Name = "General Workers")]
        public string GeneralWorkers { get; set; }
        [Display(Name = "Foreman")]
        public string Foreman { get;  set; }
        public bool IsDeleted { get;  set; }
        public IEnumerable<QuoteEquipmentViewModel> QuoteEquipments { get; set; }
        public IEnumerable<QuoteMaterialViewModel> QuoteMaterials { get; set; }
    }
}