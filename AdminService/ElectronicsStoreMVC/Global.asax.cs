using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace ElectronicsStoreMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                try
                {
                    // 1. Decrypt the cookie
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);

                    // 2. Get the role from the ticket data we saved earlier
                    string role = ticket.UserData;

                    // 3. Create the "Principal" (The User Identity)
                    var identity = new System.Security.Principal.GenericIdentity(ticket.Name);
                    var principal = new System.Security.Principal.GenericPrincipal(identity, new[] { role });

                    // 4. Attach it to the request
                    HttpContext.Current.User = principal;
                }
                catch
                {
                    // If something fails (e.g. hack attempt), clear the cookie
                    // FormsAuthentication.SignOut();
                }
            }
        }
    }

}
