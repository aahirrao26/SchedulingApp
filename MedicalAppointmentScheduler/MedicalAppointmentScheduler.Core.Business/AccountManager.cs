using System.Linq;
using MedicalAppointmentScheduler.Core.Data;
using System.Data.Entity;

namespace MedicalAppointmentScheduler.Models.BusinessClass
{
    public class AccountManager
    {
        private MedicalSchedulerDBEntities dbContext;
        public AccountManager(MedicalSchedulerDBEntities _dbContext)
        {
            this.dbContext = _dbContext;
        }
        /// <summary>
        /// This method will validated the user credentials against the database
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int ValidateUser(string userName, string password)
        {
            var userId = dbContext.UserLogins.Where(o => o.Email.ToLower().Equals(userName) && o.Password.Equals(password)).Select(u => u.UserID).SingleOrDefault();
            return userId;
        }

        /// <summary>
        /// This method returns the role of the user 
        /// </summary>
        /// <param name="userId"></param>
        public string GetUserRole(int userId)
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