using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointmentScheduler.Core.Data.Models
{
    [MetadataType(typeof(PatientMetadata))]
    public partial class Patient
    {

    }

    public class PatientMetadata
    {
        public int PatientID { get; set; }

        public String Gender { get; set; }

        public float HeightFeet { get; set; }

        public float HeightInches{ get; set; }

        public float Weight { get; set; }

        public virtual UserDetails User_Details { get; set; }

        public virtual Type Type { get; set; }
    }
}
