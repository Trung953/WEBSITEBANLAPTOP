using WebLT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLT.ModBM;

namespace WebLTModBM
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(SessionPersister.UserName))
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "ADMINLOGIN", Action = "Index" }));
            }
            else
            {
                using (QL_LPDataContext dbContext = new QL_LPDataContext() ) // Thay YourDbContext bằng tên DbContext của bạn
                {
                    var account = dbContext.TAIKHOANs.FirstOrDefault(a => a.username == SessionPersister.UserName);
                    if (account != null)
                    {
                        CustomPrincipal cp = new CustomPrincipal(account);
                        if (!cp.IsInRole(Roles))
                        {
                            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "ADMINLOGIN", Action = "Index" }));
                        }
                    }
                }
            }
        }

    }
}