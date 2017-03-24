using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicalAppointmentScheduler.Core.Data;
using System.Web;

namespace MedicalAppointmentScheduler.Core.Data
{
    [MetadataType(typeof(UserLoginMetadata))]
    public partial class UserLogin
    {
              
    }

    public class UserLoginMetadata
    {
        public int ID { get; set; }
        public int UserID { get; set; }

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