using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using ElectronicsStoreMVC.CustomerReference;

namespace ElectronicsStoreMVC.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            CustomerServiceSoapClient service = new CustomerServiceSoapClient();

            ServiceResponseOfListOfCustomer allCustomers = service.GetCustomers();

            List<Models.Customer> customerList = new List<Models.Customer>();

            foreach (Customer custFromService in allCustomers.Data)
            {
                var serviceCustomer = new Models.Customer(custFromService);

                customerList.Add(serviceCustomer);
            }

            return View(customerList);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustomerServiceSoapClient service = new CustomerServiceSoapClient();

            ServiceResponseOfCustomer serviceFaq = service.GetCustomer((int)id);

            var customer = new Models.Customer(serviceFaq);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Email,Password,PhoneNumber,Address")] Models.Customer customer)
        {
            if (ModelState.IsValid)
            {
                var serviceCust = new Customer
                {
                    Username = customer.Username,
                    Email = customer.Email,
                    Password = customer.Password,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address
                };

                CustomerServiceSoapClient service = new CustomerServiceSoapClient();

                service.AddCustomer(serviceCust);

                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustomerServiceSoapClient service = new CustomerServiceSoapClient();

            var serviceCust = service.GetCustomer((int)id);

            var customer = new Models.Customer(serviceCust);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Email,Password,PhoneNumber,Address")] Models.Customer customer)
        {
            if (ModelState.IsValid)
            {
                var serviceCust = new Customer
                {
                    Id = customer.Id,
                    Username = customer.Username,
                    Email = customer.Email,
                    Password = customer.Password,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address
                };

                CustomerServiceSoapClient service = new CustomerServiceSoapClient();

                service.EditCustomer(serviceCust);

                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustomerServiceSoapClient service = new CustomerServiceSoapClient();

            var serviceCust = service.GetCustomer((int)id);

            var customer = new Models.Customer(serviceCust);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerServiceSoapClient service = new CustomerServiceSoapClient();

            service.DeleteCustomer(id);

            return RedirectToAction("Index");
        }
    }
}
