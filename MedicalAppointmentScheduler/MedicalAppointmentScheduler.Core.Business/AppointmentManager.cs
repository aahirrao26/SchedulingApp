﻿using MedicalAppointmentScheduler.Core.Data;
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
                                          from t in sa.Where(f => f.DoctorID == doctorID && f.Date == date.Date).DefaultIfEmpty()
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
        /// Get the list of all appointments
        /// </summary>
        /// <returns></returns>
        public List<Appointment> GetAppointmentList()
        {
            List<Appointment> appointmentList = dbContext.Appointments.Where(u => u.Date>=DateTime.Today).ToList();
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
                    dbContext.Appointments.Remove(appointment);
                }
                               
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
            }
        }


    }
}
