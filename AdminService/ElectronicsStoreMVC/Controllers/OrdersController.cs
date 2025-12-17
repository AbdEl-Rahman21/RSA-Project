using ElectronicsStoreMVC.AdminReference;// Assuming Orders are handled here
using ElectronicsStoreMVC.CustomerReference;
using ElectronicsStoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ElectronicsStoreMVC.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Orders
        public ActionResult Index()
        {
            CustomerServiceSoapClient service = new CustomerServiceSoapClient();
            AdminServicesSoapClient adminService = new AdminServicesSoapClient();

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            
            int currentUserId = (int)Session["UserId"];

            var allOrders = service.GetOrders(currentUserId);
            var allProducts = adminService.GetProducts();
            List<Models.Order> orderList = new List<Models.Order>();

            var myOrderList = (from order in allOrders.Data
                               where order.CustomerId == currentUserId
                               join product in allProducts.Data
                               on order.ProductId equals product.Id
                               select new Models.Order
                               {
                                   // Map standard fields
                                   Id = order.Id,
                                   Date = order.Date,
                                   Status = order.Status,
                                   ProductCount = order.ProductCount,
                                   ProductPrice = order.ProductPrice,
                                   CustomerId = order.CustomerId,
                                   ProductId = order.ProductId,

                                   // KEY PART: We manually fill the 'Product' property here!
                                   Product = new Models.Product
                                   {
                                       Name = product.Name,
                                       // Map other product fields if you need them (like Category)
                                       Category = product.Category
                                   }
                               }).ToList();

            
            return View(myOrderList);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustomerServiceSoapClient service = new CustomerServiceSoapClient();

            // Assuming GetOrder(int) returns ServiceResponseOfOrder
            var serviceResponse = service.GetOrder((int)id);

            // Check if data exists inside the response
            if (serviceResponse.Data == null)
            {
                return HttpNotFound();
            }

            var order = new Models.Order(serviceResponse.Data);
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create(int? productId)
        {
            var order = new Models.Order();
            order.Date = DateTime.Now;        
            order.Status = "Pending";
            if (Session["UserId"] != null)
            {
                order.CustomerId = (int)Session["UserId"]; 
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            if (productId != null)
            {
                order.ProductId = productId.Value; 

                
                AdminReference.AdminServicesSoapClient prodService = new AdminReference.AdminServicesSoapClient();

                
                var product = prodService.GetProduct(productId.Value);

                if (product != null)
                {
                    
                    order.ProductPrice = product.Data.Price;

                    
                    ViewBag.ProductName = product.Data.Name;
                }
            }
                return View(order);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Status,ProductCount,ProductPrice,CustomerId,ProductId")] Models.Order order)
        {
            if (ModelState.IsValid)
            {
                
                var serviceOrder = new CustomerReference.Order
                {
                    Date = order.Date,
                    Status = order.Status,
                    ProductCount = order.ProductCount,
                    ProductPrice = order.ProductPrice,
                    CustomerId = order.CustomerId,
                    ProductId = order.ProductId
                };

                AdminServicesSoapClient AdminService = new AdminReference.AdminServicesSoapClient();
                var serviceProduct = AdminService.GetProduct(serviceOrder.ProductId);

                if (order.ProductCount > serviceProduct.Data.CountAvailable) {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                serviceProduct.Data.CountAvailable -= order.ProductCount;
                
                AdminService.EditProduct(serviceProduct.Data);

                CustomerServiceSoapClient service = new CustomerServiceSoapClient();
                service.AddOrder(serviceOrder);

                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    CustomerServiceSoapClient service = new CustomerServiceSoapClient();
        //    var serviceResponse = service.GetOrder((int)id);

        //    if (serviceResponse.Data == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var order = new Models.Order(serviceResponse.Data);
        //    return View(order);
        //}

        // POST: Orders/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Date,Status,ProductCount,ProductPrice,CustomerId,ProductId")] Models.Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var serviceOrder = new CustomerReference.Order
        //        {
        //            Id = order.Id,
        //            Date = order.Date,
        //            Status = order.Status,
        //            ProductCount = order.ProductCount,
        //            ProductPrice = order.ProductPrice,
        //            CustomerId = order.CustomerId,
        //            ProductId = order.ProductId
        //        };


        //        CustomerServiceSoapClient service = new CustomerServiceSoapClient();
        //        service.EditOrder(serviceOrder);

        //        return RedirectToAction("Index");
        //    }
        //    return View(order);
        //}

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustomerServiceSoapClient service = new CustomerServiceSoapClient();
            var serviceResponse = service.GetOrder((int)id);

            if (serviceResponse.Data == null)
            {
                return HttpNotFound();
            }

            var order = new Models.Order(serviceResponse.Data);
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerServiceSoapClient service = new CustomerServiceSoapClient();
            service.DeleteOrder(id);
            return RedirectToAction("Index");
        }
    }
}