
using ElectronicsStoreMVC.AdminReference;
using ElectronicsStoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using ElectronicsStoreMVC.CustomerReference;

namespace ElectronicsStoreMVC.Controllers
{
    public class CustomersController : Controller
    {


        // GET: Customers
        public ActionResult Index()
        {
            CustomerReference.CustomerServiceSoapClient service = new CustomerReference.CustomerServiceSoapClient();
            CustomerReference.ServiceResponseOfListOfCustomer allCustomers = service.GetCustomers();
            List<Models.Customer> customerList = new List<Models.Customer>();

            foreach (CustomerReference.Customer custFromService in allCustomers.Data)
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
            CustomerReference.CustomerServiceSoapClient service = new CustomerReference.CustomerServiceSoapClient();
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Email,Password,PhoneNumber,Address")] Models.Customer customer)
        {
            if (ModelState.IsValid)
            {
                var serviceCust = new CustomerReference.Customer
                {
                    Username = customer.Username,
                    Email = customer.Email,
                    Password = customer.Password,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address
                    

                };
                CustomerReference.CustomerServiceSoapClient service = new CustomerReference.CustomerServiceSoapClient();
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
            CustomerReference.CustomerServiceSoapClient service = new CustomerReference.CustomerServiceSoapClient();
            var serviceCust = service.GetCustomer((int)id);
            var customer = new Models.Customer(serviceCust);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Email,Password,PhoneNumber,Address")] Models.Customer customer)
        {
            if (ModelState.IsValid)
            {
                var serviceCust = new CustomerReference.Customer
                {
                    Id = customer.Id,
                    Username = customer.Username,
                    Email = customer.Email,
                    Password = customer.Password,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address

                };
                CustomerReference.CustomerServiceSoapClient service = new CustomerReference.CustomerServiceSoapClient();
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
            CustomerReference.CustomerServiceSoapClient service = new CustomerReference.CustomerServiceSoapClient();
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
            CustomerReference.CustomerServiceSoapClient service = new CustomerReference.CustomerServiceSoapClient();

            service.DeleteCustomer(id);
            return RedirectToAction("Index");
        }

        
    }
}
