using MedicalAppointmentScheduler.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointmentScheduler.Core.Business
{
    public class AdminManager
    {
        public void DeleteUser(int userId)
        {
            using (MedicalSchedulerDBEntities dbContext = new MedicalSchedulerDBEntities())
            {
                UserDetails userDetails = dbContext.UserDetails.Find(userId);
                dbContext.UserDetails.Remove(userDetails);
                dbContext.SaveChanges();
            }
        }
    }
}
