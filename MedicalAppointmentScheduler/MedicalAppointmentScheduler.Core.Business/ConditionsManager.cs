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
        List<String> GetConditions(int patientID, int type);
        String GetTypes(int type);
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

        public List<String> GetConditions(int patientID, int type)
        {
            var listOfConditions = from P in dbContext.Patient_Conditions.Where(u => u.PatientID == patientID && u.TypeID == type)
                                   join C in dbContext.Conditions.Where(u => u.TypeID == type)
                                   on P.ConditionID equals C.ID
                                   select C.Name.ToString();
            List<String> list = listOfConditions.Distinct().ToList();
            return list;
        }

        public String GetTypes(int type)
        {
               return dbContext.Types.Where(u => u.ID == type).Single().Name.ToString();
        }
    }
}
