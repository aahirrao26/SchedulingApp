using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointmentScheduler.Core.Business
{
    public class Helper
    {
        public enum ApplicationRole
        {
           Administrator = 1,
           Patient = 2,
           Doctor= 3,
           Receptionist =4,
           Nurse =5
        }
    }
}
