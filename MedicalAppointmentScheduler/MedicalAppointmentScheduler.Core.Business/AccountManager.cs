using System.Linq;
using MedicalAppointmentScheduler.Core.Data;

namespace MedicalAppointmentScheduler.Models.BusinessClass
{
    public class AccountManager
    {
        /// <summary>
        /// This method will validated the user credentials against the database
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidatedUser(string userName, string password)
        {
            using (MedicalSchedulerDBEntities dbContext = new MedicalSchedulerDBEntities())
            {
                var user = dbContext.User_Login.Where(o => o.Email.ToLower().Equals(userName) && o.Password.Equals(password));
                if (user.Any())
                    return true;
                else
                    return false;
            }
        }
    }
}