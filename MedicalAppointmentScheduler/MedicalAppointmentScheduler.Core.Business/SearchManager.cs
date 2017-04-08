using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalAppointmentScheduler.Core.Data;

namespace MedicalAppointmentScheduler.Core.Business
{
    public interface ISearchManager
    {
        List<UserDetails> GetPatientList(string firstName, string lastName);
    }

    /// <summary>
    /// The below class will implement IAdminManger
    /// </summary>

    public class SearchManager : ISearchManager
    {
        private MedicalSchedulerDBEntities dbContext;

        public SearchManager()
        {
            this.dbContext = new MedicalSchedulerDBEntities();
        }

        // Constructor for testing purposes
        public SearchManager(MedicalSchedulerDBEntities _dbContext)
        {
            this.dbContext = _dbContext;
        }

        // Return list of patients matching criteria
        public List<UserDetails> GetPatientList(string firstName, string lastName)
        {
            if (lastName == "")
                return dbContext.UserDetails.Where(u => (u.FirstName == firstName || firstName == null) && (u.RoleID == 2)).ToList();
            else if (firstName == "")
                return dbContext.UserDetails.Where(u => (u.LastName == lastName || lastName == null) && (u.RoleID == 2)).ToList();
            else
                return dbContext.UserDetails.Where(u => (u.FirstName == firstName || firstName == null) && (u.LastName == lastName || lastName == null) && (u.RoleID == 2)).ToList();
        }
    }
}
