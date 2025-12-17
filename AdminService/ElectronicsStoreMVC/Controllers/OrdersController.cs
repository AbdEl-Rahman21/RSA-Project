//using ElectronicsStoreMVC.Models;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Web.Mvc;
//using ElectronicsStoreMVC.CustomerReference; // Assuming Orders are handled here

//namespace ElectronicsStoreMVC.Controllers
//{
//    public class OrdersController : Controller
//    {
//        // GET: Orders
//        public ActionResult Index()
//        {
//            CustomerServiceSoapClient service = new CustomerServiceSoapClient();

//            // Assuming GetOrders() returns ServiceResponseOfListOfOrder
//            var allOrders = service.GetOrders();
//            List<Models.Order> orderList = new List<Models.Order>();

//            if (allOrders.Data != null)
//            {
//                foreach (CustomerReference.Order orderFromService in allOrders.Data)
//                {
//                    // Assumes you added a constructor: public Order(CustomerReference.Order o)
//                    var localOrder = new Models.Order(orderFromService);
//                    orderList.Add(localOrder);
//                }
//            }
//            return View(orderList);
//        }

//        // GET: Orders/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

//            CustomerServiceSoapClient service = new CustomerServiceSoapClient();

//            // Assuming GetOrder(int) returns ServiceResponseOfOrder
//            var serviceResponse = service.GetOrder((int)id);

//            // Check if data exists inside the response
//            if (serviceResponse.Data == null)
//            {
//                return HttpNotFound();
//            }

//            var order = new Models.Order(serviceResponse.Data);
//            return View(order);
//        }

//        // GET: Orders/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Orders/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,Date,Status,ProductCount,ProductPrice,CustomerId,ProductId")] Models.Order order)
//        {
//            if (ModelState.IsValid)
//            {
//                // Map Local Model -> Service Model
//                var serviceOrder = new CustomerReference.Order
//                {
//                    Date = order.Date,
//                    Status = order.Status,
//                    ProductCount = order.ProductCount,
//                    ProductPrice = order.ProductPrice,
//                    CustomerId = order.CustomerId,
//                    ProductId = order.ProductId
//                };

//                CustomerServiceSoapClient service = new CustomerServiceSoapClient();
//                service.AddOrder(serviceOrder);

//                return RedirectToAction("Index");
//            }

//            return View(order);
//        }

//        // GET: Orders/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

//            CustomerServiceSoapClient service = new CustomerServiceSoapClient();
//            var serviceResponse = service.GetOrder((int)id);

//            if (serviceResponse.Data == null)
//            {
//                return HttpNotFound();
//            }

//            var order = new Models.Order(serviceResponse.Data);
//            return View(order);
//        }

//        // POST: Orders/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,Date,Status,ProductCount,ProductPrice,CustomerId,ProductId")] Models.Order order)
//        {
//            if (ModelState.IsValid)
//            {
//                var serviceOrder = new CustomerReference.Order
//                {
//                    Id = order.Id,
//                    Date = order.Date,
//                    Status = order.Status,
//                    ProductCount = order.ProductCount,
//                    ProductPrice = order.ProductPrice,
//                    CustomerId = order.CustomerId,
//                    ProductId = order.ProductId
//                };

//                CustomerServiceSoapClient service = new CustomerServiceSoapClient();
//                service.EditOrder(serviceOrder);

//                return RedirectToAction("Index");
//            }
//            return View(order);
//        }

//        // GET: Orders/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

//            CustomerServiceSoapClient service = new CustomerServiceSoapClient();
//            var serviceResponse = service.GetOrder((int)id);

//            if (serviceResponse.Data == null)
//            {
//                return HttpNotFound();
//            }

//            var order = new Models.Order(serviceResponse.Data);
//            return View(order);
//        }

//        // POST: Orders/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            CustomerServiceSoapClient service = new CustomerServiceSoapClient();
//            service.DeleteOrder(id);
//            return RedirectToAction("Index");
//        }
//    }
//}