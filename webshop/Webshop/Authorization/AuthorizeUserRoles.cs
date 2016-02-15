using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Webshop.Enums;

namespace Webshop.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]

    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params UserRole[] roles)
        {
            if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
            {
                throw new ArgumentException("roles");
            }
            this.Roles = string.Join(",", roles.Select(r => UserRole.GetName(r.GetType(), r)));
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            Session s = (Session)httpContext.Session["__MySessionObject"];

            if (s.User == null || s.LoggedIn ==false)
            {
                return false;
            }

            if (s.User.Role.ToString().Contains(Roles))
            {
                return true;
            }
            return false;
        }
    }
}