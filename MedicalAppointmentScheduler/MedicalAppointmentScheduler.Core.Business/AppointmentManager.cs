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
        List<AvailableSlots> GetAvailableSlots(int doctorID, DateTime date);
        List<Appointment> GetAppointmentList();
        Appointment FindAppointment(int? AppointmentID);
        void DeleteAppointment(int AppointmentID);
        List<Appointment> GetPatientAppointmentHistory(int patientID);
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
                newAppointment.BookedDate = DateTime.Now.Date;
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

        /// <summary>
        /// This gets the list of all the slots based on the provided date and doctor's ID
        /// </summary>
        /// <param name="doctorID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<AvailableSlots> GetAvailableSlots(int doctorID, DateTime date)
        {
            List<AvailableSlots> availableSlots = (from slots in dbContext.Slots
                                          join appointments in dbContext.Appointments
                                          on slots.ID equals appointments.SlotID
                                          into sa
                                          from t in sa.Where(f => f.DoctorID == doctorID && f.Date == date.Date && f.IsCancelled == false).DefaultIfEmpty()
                                          where t == null
                                          select new AvailableSlots { ID = slots.ID, EndTime = slots.EndTime, StartTime = slots.StartTime }).ToList();
            if (date == DateTime.Today)
            {
                var currentTime = DateTime.Now.TimeOfDay;
                availableSlots = availableSlots.Where(f => f.StartTime > currentTime).ToList();
            }
            return availableSlots;
        }

        /// <summary>
        /// Get the list of all appointments order by appointment date and starttime
        /// </summary>
        /// <returns></returns>
        public List<Appointment> GetAppointmentList()
        {
            List<Appointment> appointmentList = dbContext.Appointments.Where(u => u.Date>=DateTime.Today && u.IsCancelled==false).OrderBy(u=>u.Date).ThenBy(u =>u.L_Slots.StartTime).ToList();
            return appointmentList;
        }

        /// <summary>
        /// Find appointment for the passed appointmentID
        /// </summary>
        /// <param name="AppointmentID"></param>
        /// <returns></returns>
        public Appointment FindAppointment(int? AppointmentID)
        {
            Appointment appointment = dbContext.Appointments.Find(AppointmentID);
            return appointment;
        }

        /// <summary>
        /// Delete appointment
        /// </summary>
        /// <param name="AppointmentID"></param>
        public void DeleteAppointment(int AppointmentID)
        {
            try
            {
                //Delete appointment entry
                Appointment appointment = dbContext.Appointments.SingleOrDefault(u => u.ID == AppointmentID);
                if (appointment != null)
                {
                    appointment.IsCancelled = true;
                   // dbContext.Appointments.Remove(appointment);
                }
                               
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// This method will get the appointment history for the supplied patient 
        /// </summary>
        /// <param name="patientID"></param>
        /// <returns></returns>
        public List<Appointment> GetPatientAppointmentHistory(int patientID)
        {
            List<Appointment> appointmentList = new List<Appointment>();
            if (patientID != 0)
            {
                var currentTime = DateTime.Now.TimeOfDay;
                var appointmentList1 = dbContext.Appointments.Where(u => u.PatientID == patientID && u.Date < DateTime.Today).ToList();
                var appointmentList2 = dbContext.Appointments.Where(u => u.PatientID == patientID && u.Date == DateTime.Today && u.L_Slots.EndTime < currentTime).ToList();
                var appointmentList3 = dbContext.Appointments.Where(u => u.PatientID == patientID && u.IsCancelled == true).ToList();
                appointmentList = appointmentList1.Union(appointmentList2).Union(appointmentList3).OrderBy(u=>u.Date).ThenBy(u => u.L_Slots.StartTime).ToList();
            }
            return appointmentList;
        }


    }
}
