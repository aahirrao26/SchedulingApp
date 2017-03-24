using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicalAppointmentScheduler.Core.Data;
using System.Linq;
using System.Web;

namespace MedicalAppointmentScheduler.Core.Data
{
    [MetadataType(typeof(UserDetailsMetadata))]
    public partial class UserDetails
    {


    }

    public class UserDetailsMetadata
    {
        public int ID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Name")]
        public string Phone { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAdress { get; set; }

        [Display(Name = "Role")]
        public Nullable<int> RoleID { get; set; }

        
        [ForeignKey("RoleID")]
        public virtual UserRole L_User_Roles { get; set; }
    }
}
