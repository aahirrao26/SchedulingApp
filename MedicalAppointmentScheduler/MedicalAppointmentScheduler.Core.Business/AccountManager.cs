using System.Linq;
using MedicalAppointmentScheduler.Core.Data;
using System;

namespace MedicalAppointmentScheduler.Core.Business
{
    public interface IAccountManager
    {
        int ValidateUser(string userName, string password);
        string GetUserRole(int userId);
        bool IsUserInRole(string loginName, int RoleID);
    }

    public class AccountManager : IAccountManager
    {
        private MedicalSchedulerDBEntities dbContext;

        public AccountManager()
        {
            this.dbContext = new MedicalSchedulerDBEntities();
        }
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

        /// <summary>
        /// Method will check is the user is in the role
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public bool IsUserInRole(string loginName, int RoleID)
        {
            int? roleID = dbContext.UserDetails.Where(o => o.EmailAdress.ToLower().Equals(loginName)).Select(u => u.RoleID).FirstOrDefault();

            if (RoleID == roleID)
            {
                return true;
            }

            return false;
        }
    }
}