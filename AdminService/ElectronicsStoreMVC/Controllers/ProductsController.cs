using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerService.DbContexts;
using ElectronicsStoreMVC.Models;

namespace ElectronicsStoreMVC.Controllers
{
    public class ProductsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Products
        public ActionResult Index()
        {
            AdminReference.AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
            var allProducts = service.GetProducts();
            return View(allProducts);
            
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            AdminReference.AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
            var product = service.GetProduct((int)id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Description,CountAvailable,Category")] Product product)
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

                
                using (var service = new AdminReference.AdminServicesSoapClient())
                {
                    service.AddProduct(serviceProduct);
                }
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            AdminReference.AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
            var product = service.GetProduct((int)id);

            
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Description,CountAvailable,Category")] Product product)
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


                using (var service = new AdminReference.AdminServicesSoapClient())
                {
                    service.EditProduct(serviceProduct);
                }
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            AdminReference.AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
            var product = service.GetProduct((int)id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
