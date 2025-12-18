using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using ElectronicsStoreMVC.Models;
using ElectronicsStoreMVC.AdminReference;

namespace ElectronicsStoreMVC.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        

        // GET: Products
        public ActionResult Index(string keyword, string category, decimal? minPrice, decimal? maxPrice)
        {
            ViewBag.Keyword = keyword;
            ViewBag.Category = category;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            CustomerReference.CustomerServiceSoapClient service = new CustomerReference.CustomerServiceSoapClient();
            CustomerReference.ServiceResponseOfListOfProduct allProducts = service.SearchProducts(keyword,category,minPrice,maxPrice);
            List<Models.Product> products = new List<Models.Product>();

            foreach (CustomerReference.Product prodFromService in allProducts.Data)
            {
                var serviceProduct = new Models.Product(prodFromService);
                products.Add(serviceProduct);

            }
            return View(products);
            
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            AdminReference.AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
            ServiceResponseOfProduct product = service.GetProduct((int)id);



            var serviceProduct = new Models.Product(product);




            if (product == null)
            {
                return HttpNotFound();
            }
            return View(serviceProduct);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Description,CountAvailable,Category")]Models.Product product)
        {
            if (ModelState.IsValid)
            {

                var serviceProduct = new AdminReference.Product
                {
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    CountAvailable = product.CountAvailable,
                    Category = product.Category
                    
                };
                AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
               service.AddProduct(serviceProduct);

                
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            AdminReference.AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
            var product = service.GetProduct((int)id);
            var serviceProduct = new Models.Product(product);


            if (product == null)
            {
                return HttpNotFound();
            }
            return View(serviceProduct);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Description,CountAvailable,Category")]Models.Product product)
        {
            if (ModelState.IsValid)
            {
                
                var serviceProduct = new AdminReference.Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    CountAvailable = product.CountAvailable,
                    Category = product.Category

                };


                AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
                service.EditProduct(serviceProduct);

                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            AdminReference.AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
            var product = service.GetProduct((int)id);
            var serviceProduct = new Models.Product(product);
            

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(serviceProduct);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminReference.AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
           
            service.DeleteProduct(id);
            

            return RedirectToAction("Index");
        }

        
    }
}
