using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollSoft.Models;

namespace PayrollSoft.Controllers
{
    public class PaymentTypeController : Controller
    {
        private DataContext db = new DataContext();

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /PaymentType/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /PaymentType/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /PaymentType/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(PaymentType paymenttype)
        {
            //try
            //{
                if (ModelState.IsValid)
                {
                    db.PaymentTypes.Add(paymenttype);
                    db.SaveChanges();
                    return Json(true);
                }

                return Json(false);
           // }
           // catch
           // {
               // return Json(false);
           // }
        }
        
        //
        // GET: /PaymentType/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /PaymentType/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /PaymentType/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /PaymentType/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
