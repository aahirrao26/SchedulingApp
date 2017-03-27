using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace MedicalAppointmentScheduler.Core.Business
{
    public interface IAuthentication
    {
        void SetAuthCookie(string username);
        void LogOff();
    }

    public class FormsAuth : IAuthentication
    {
        public void SetAuthCookie(string username)
        {
            FormsAuthentication.SetAuthCookie(username, false);
        }

        public void LogOff()
        {
            FormsAuthentication.SignOut();
        }
    }
}
