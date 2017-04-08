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
        List<int> GetAvailableSlots(int doctorID, DateTime date);

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

        public List<int> GetAvailableSlots(int doctorID, DateTime date) {

            List<int> availableSlots = (from slots in dbContext.Slots
                                          join appointments in dbContext.Appointments
                                          on slots.ID equals appointments.SlotID
                                          into sa
                                          from t in sa.Where(f => f.DoctorID == doctorID && f.Date == date.Date).DefaultIfEmpty()
                                          where t == null
                                          select slots.ID).ToList();

            return availableSlots;
        }
    }
}
