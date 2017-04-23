using MedicalAppointmentScheduler.Core.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedicalAppointmentScheduler.Security
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
       private readonly int userAssignedRoles;
       public AuthorizeRoleAttribute(int role)
        {
            this.userAssignedRoles = role;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;

            AccountManager accountManager = new AccountManager();
            authorize= accountManager.IsUserInRole(httpContext.User.Identity.Name, userAssignedRoles);
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult { ViewName = "Error" };
        }
    }
}