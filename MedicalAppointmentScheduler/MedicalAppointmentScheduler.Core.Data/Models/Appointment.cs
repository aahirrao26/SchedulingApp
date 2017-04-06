using MedicalAppointmentScheduler.Core.Data.CustomAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointmentScheduler.Core.Data
{

    [MetadataType(typeof(AppointmentMetadata))]
    public partial class Appointment
    {


    }
    public class AppointmentMetadata
    {

        public int ID { get; set; }

        [Required]
        [Display(Name = "Appointment Detail")]
        public string Details { get; set; }

        [Required]
        [Display(Name = "Doctor Name")]
        public int DoctorID { get; set; }

        public Nullable<int> PatientID { get; set; }


        public int BookedBy { get; set; }

        [Required]
        [ValidateDateRange]
        [DataType(DataType.Date)]        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Appointment Date")]
        public System.DateTime Date { get; set; }

        public int SlotID { get; set; }



        public virtual UserDetails User_Details { get; set; }

        public virtual UserDetails User_Details1 { get; set; }

        public virtual UserDetails User_Details2 { get; set; }

        public virtual Slots L_Slots { get; set; }


    }
}
