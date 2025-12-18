using ElectronicsStoreMVC.AdminReference;
using ElectronicsStoreMVC.CustomerReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ElectronicsStoreMVC.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login() {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            string role = "";
            string username = "";
            int userId = 0;

            // 1. Try to login as Customer
           CustomerReference.CustomerServiceSoapClient custService = new CustomerServiceSoapClient();
            var customer = custService.LogIn(email, password); // You need this method in your ASMX

            if (customer.Data != null)
            {
                role = "Customer";
                username = customer.Data.Username;
                userId = customer.Data.Id;
            }
            else
            {
                // 2. If not Customer, try to login as Admin
                AdminServicesSoapClient adminService = new AdminServicesSoapClient();
                var admin = adminService.LogIn(email, password); // You need this method in your Admin ASMX

                if (admin.Success)
                {
                    role = "Admin";
                    username = "Admin";
                    userId = 0;
                }
            }

            // 3. If User Found, Create the Secure Ticket
            if (!string.IsNullOrEmpty(role))
            {
                // Create a ticket that carries the Role in the "UserData" slot
                var ticket = new FormsAuthenticationTicket(
                    1,                          // version
                    username,                   // user name
                    DateTime.Now,               // created
                    DateTime.Now.AddMinutes(30),// expires
                    false,                      // persistent?
                    role                        // <--- IMPORTANT: We store the role here!
                );

                // Encrypt the ticket
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(cookie);

                // Store ID in session for easy access
                Session["UserId"] = userId;
                Session["UserRole"] = role;

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid Login Attempt";
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}