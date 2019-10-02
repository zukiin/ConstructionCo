using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ds3App.Models
{
    public class RegisterViewModelEdit
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "Contact number should be 10 digits long e.g(0811234567)")]
        [MaxLength(10, ErrorMessage = "Contact number should be 10 digits long e.g(0811234567)")]
        public string Contact { get; set; }
        [Required]
        public string Role { get; set; }
        public string UserId { get; set; }
    }
}