using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalAppointmentScheduler.Core.Data;

namespace MedicalAppointmentScheduler.Core.Business
{
    public interface IConditionsManager
    {
        Patient GetDetails(int patientID);
        List<Condition> GetConditions(int patientID);
    }

    /// <summary>
    /// The below class will implement IConditionsManger
    /// </summary>

    public class ConditionsManager : IConditionsManager
    {
        private MedicalSchedulerDBEntities dbContext;

        public ConditionsManager()
        {
            this.dbContext = new MedicalSchedulerDBEntities();
        }

        // Constructor for testing purposes
        public ConditionsManager(MedicalSchedulerDBEntities _dbContext)
        {
            this.dbContext = _dbContext;
        }

        // Return list of patients matching criteria
        public Patient GetDetails(int patientID)
        {
            return dbContext.Patients.Where(u => u.PatientID == patientID).Single();
        }

        public List<Condition> GetConditions(int patientID)
        {
            dbContext
        }
    }
}
