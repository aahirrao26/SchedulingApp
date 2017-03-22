using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MedicalAppointmentScheduler.Startup))]
namespace MedicalAppointmentScheduler
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
