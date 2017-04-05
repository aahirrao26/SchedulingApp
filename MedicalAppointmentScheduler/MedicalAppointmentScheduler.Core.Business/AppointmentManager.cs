using MedicalAppointmentScheduler.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using static MedicalAppointmentScheduler.Core.Business.Helper;

namespace MedicalAppointmentScheduler.Core.Business
{

    public interface IAppointmentManager
    {
        bool BookAppointment(Appointment newAppointment);
        List<UserDetails> GetDoctorList();

    }
    public class AppointmentManager:IAppointmentManager
    {

        private MedicalSchedulerDBEntities dbContext;

        public AppointmentManager()
        {
            this.dbContext = new MedicalSchedulerDBEntities();
        }

        public AppointmentManager(MedicalSchedulerDBEntities _dbContext)
        {
            this.dbContext = _dbContext;
        }

        /// <summary>
        /// This is use to create new appointment
        /// </summary>
        /// <param name="userDetails"></param>
        public bool BookAppointment(Appointment newAppointment)
        {
            try
            {
                //TODO: Remove hardcorded value for slot
                newAppointment.SlotID = 1;
                               
                dbContext.Appointments.Add(newAppointment);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get the list of Doctors
        /// </summary>
        /// <returns></returns>
        public List<UserDetails> GetDoctorList()
        { 
            return dbContext.UserDetails.Where(role => role.RoleID == (int)ApplicationRole.Doctor).ToList();
        }
    }
}
