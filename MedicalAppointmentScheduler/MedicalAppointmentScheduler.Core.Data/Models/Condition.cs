using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointmentScheduler.Core.Data.Models
{
    [MetadataType(typeof(ConditionMetadata))]
    public partial class Condition
    {

    }

    public class ConditionMetadata
    {
        public int ID { get; set; }

        public int Type_ID { get; set; }

        public String Name { get; set; }

        public virtual Type Type { get; set; }
    }
}
