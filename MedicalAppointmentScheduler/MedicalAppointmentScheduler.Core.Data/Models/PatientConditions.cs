using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointmentScheduler.Core.Data.Models
{
    [MetadataType(typeof(PatientConditionsMetadata))]
    public partial class PatientConditions
    {

    }

    public class PatientConditionsMetadata
    {
        public int PatientID { get; set; }
        
        public int ID { get; set; }

        public virtual UserDetails User_Details { get; set; }

        public virtual Condition Condition { get; set; }
    }
}
