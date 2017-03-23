using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalAppointmentScheduler.Models
{
    public class LoginViewModel
    {
        public int ID { get; set; }
        public int UserId { get; set; }

        [Required]
        [Display(Name = "UserName/Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}