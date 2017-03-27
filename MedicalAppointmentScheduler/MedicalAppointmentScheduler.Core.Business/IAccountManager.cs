using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointmentScheduler.Core.Business
{
    public interface IAccountManager
    {
        int ValidateUser(string userName, string password);
        string GetUserRole(int userId);
    }
}
