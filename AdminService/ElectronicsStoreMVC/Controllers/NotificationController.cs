using ElectronicsStoreMVC.AdminReference;
using ElectronicsStoreMVC.CustomerReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectronicsStoreMVC.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult Index()
        {
            CustomerServiceSoapClient service = new CustomerServiceSoapClient();
            

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }


            int currentUserId = (int)Session["UserId"];

            var allNotifications = service.GetNotifications(currentUserId);
            
            List<Models.Notification> notifications = new List<Models.Notification>();
            foreach (CustomerReference.Notification notiFromService in allNotifications.Data)
            {
                var serviceProduct = new Models.Notification(notiFromService);
                notifications.Add(serviceProduct);

            }

            return View(notifications);
        }
    }
}