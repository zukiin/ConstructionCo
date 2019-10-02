using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ds3App.Models
{
    public class ProjectWithForemanViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Quotation Reference")]
        public string QuotationReference { get; set; }
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; }
        [Display(Name = "Project Cost")]
        public decimal ProjectCost { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Foreman")]
        public string Foreman { get; set; }
    }
}