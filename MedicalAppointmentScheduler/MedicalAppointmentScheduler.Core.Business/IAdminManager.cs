using MedicalAppointmentScheduler.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAppointmentScheduler.Core.Business
{
    public interface IAdminManager
    {
        void DeleteUser(int userId);

        void EditUser(UserDetails userDetails);

        void CreateUser(UserDetails userDetails);
    };
}
