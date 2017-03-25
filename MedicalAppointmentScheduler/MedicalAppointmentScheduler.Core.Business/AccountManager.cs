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
        public int ValidatedUser(string userName, string password)
        {
            using (MedicalSchedulerDBEntities dbContext = new MedicalSchedulerDBEntities())
            {
                var userId = dbContext.UserLogins.Where(o => o.Email.ToLower().Equals(userName) && o.Password.Equals(password)).Select(u => u.UserID).SingleOrDefault();
                
                return userId;
            }
        }

        /// <summary>
        /// This method returns the role of the user 
        /// </summary>
        /// <param name="userId"></param>
        public string GetUserRole(int userId)
        {
            using (MedicalSchedulerDBEntities dbContext = new MedicalSchedulerDBEntities())
            {
                var userRole = (from roles in dbContext.UserRoles
                                join user in dbContext.UserDetails
                                on roles.ID equals user.RoleID
                                where user.ID == userId
                                select roles.RoleName).SingleOrDefault();
                return userRole;
            }
        }
    }
}