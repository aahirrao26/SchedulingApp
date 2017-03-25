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
        private MedicalSchedulerDBEntities dbContext;

        public AdminManager(MedicalSchedulerDBEntities _dbContext)
        {
            this.dbContext = _dbContext; 
        }
        public void DeleteUser(int userId)
        {        
             UserDetails userDetails = dbContext.UserDetails.Find(userId);
             dbContext.UserDetails.Remove(userDetails);
             dbContext.SaveChanges();
        }
    }
}
