using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication10.Models
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            bool isInRoles = false;

            String[] roles = this.Roles.Split(new string[] { ", " }, StringSplitOptions.None);
            foreach(String role in roles)
            {
                if(filterContext.HttpContext.User.IsInRole(role))
                {
                    isInRoles = true;
                    break;
                }
            }

            if (!isInRoles)
            {
                filterContext.Result = new RedirectResult("~/Home/NoAccess");
                return;
            }

         
        }
    }
}