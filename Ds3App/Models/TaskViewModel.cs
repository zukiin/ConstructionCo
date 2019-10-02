using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ds3App.Models
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }
        [Display(Name = "Task Description")]
        [Required]
        public string TaskDescription { get; set; }
        [Display(Name = "Rate Per Hour")]
        public decimal RatePerHour { get; set; }
        [Display(Name = "Worker Type")]
        public string WorkerTypeId { get; set; }

        public bool IsDeleted { get; set; }
    }
}