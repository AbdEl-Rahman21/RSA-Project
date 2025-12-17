
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

namespace ElectronicsStoreMVC.Controllers
{
    public class FAQsController : Controller
    {
        

        // GET: FAQs
        public ActionResult Index()
        {
            AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
            ServiceResponseOfListOfFAQ allFaqs = service.GetFAQs();
            List<Models.FAQ> faqs = new List<Models.FAQ>();

            foreach (AdminReference.FAQ prodFromService in allFaqs.Data)
            {
                var serviceProduct = new Models.FAQ(prodFromService);
                faqs.Add(serviceProduct);

            }
            return View(faqs);
        }

        // GET: FAQs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminReference.AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
            ServiceResponseOfFAQ serviceFaq = service.GetFAQ((int)id);



            var faq = new Models.FAQ(serviceFaq);
            if (faq == null)
            {
                return HttpNotFound();
            }
            return View(faq);
        }

        // GET: FAQs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FAQs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Question,Answer")] Models.FAQ fAQ)
        {
            if (ModelState.IsValid)
            {
                var serviceFaq = new AdminReference.FAQ
                {
                    
                    Question = fAQ.Question,
                    Answer = fAQ.Answer

                };
                AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
                service.AddFAQ(serviceFaq);
                return RedirectToAction("Index");
            }

            return View(fAQ);
        }

        // GET: FAQs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminReference.AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
            var faq = service.GetFAQ((int)id);
            var serviceFaq = new Models.FAQ(faq);
            if (serviceFaq == null)
            {
                return HttpNotFound();
            }
            return View(serviceFaq);
        }

        // POST: FAQs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Question,Answer")] Models.FAQ fAQ)
        {
            if (ModelState.IsValid)
            {
                var serviceFaq = new AdminReference.FAQ
                {
                    Id = fAQ.Id,
                    Question = fAQ.Question,
                    Answer = fAQ.Answer

                };
                AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
                service.EditFAQ(serviceFaq);
                return RedirectToAction("Index");
            }
            return View(fAQ);
        }

        // GET: FAQs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminReference.AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();
            ServiceResponseOfFAQ serviceFaq = service.GetFAQ((int)id);
            var fAQ = new Models.FAQ(serviceFaq);
            if (fAQ == null)
            {
                return HttpNotFound();
            }
            return View(fAQ);
        }

        // POST: FAQs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdminReference.AdminServicesSoapClient service = new AdminReference.AdminServicesSoapClient();

            service.DeleteFAQ(id);
            return RedirectToAction("Index");
        }

        
    }
}
