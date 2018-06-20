using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollSoft.Models;

namespace PayrollSoft.Controllers
{ 
    public class InsuranceController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Insurance/

        public ViewResult Index()
        {
            return View(db.Insurances.ToList());
        }

        //
        // GET: /Insurance/Details/5

        public ViewResult Details(string id)
        {
            Insurance insurance = db.Insurances.Find(id);
            return View(insurance);
        }

        //
        // GET: /Insurance/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Insurance/Create

        [HttpPost]
        public ActionResult Create(Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                db.Insurances.Add(insurance);
                db.SaveChanges();
                //return RedirectToAction("Index");  
                return Json(true);
            }

            //return View(insurance);
            return Json(false);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult getInsuranceCode()
        {
            //var result = db.Insurances.ToList();
            var result = db.InsuranceCategories.ToList();
            var codes = from cds in result
                        select new { cds.CategoryID,cds.Description };

            return Json(codes, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetInsuranceCategory(string columnId)
        {
            var insuranceCat = from i in db.InsuranceCategories
                               where i.InsuranceCode == columnId
                               select new { i.InsuranceCode, i.Description, i.MonthlyRate };

            return Json(insuranceCat, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetServiceProvider()
        {
            var serviceProvider = from sp in db.Insurances
                                  select new { sp.InsuranceCode, sp.ServiceProvider };
            return Json(serviceProvider, JsonRequestBehavior.AllowGet);
        }

        // GET: /Insurance/Edit/5
 
        public ActionResult Edit(string id)
        {
            Insurance insurance = db.Insurances.Find(id);
            return View(insurance);
        }

        //
        // POST: /Insurance/Edit/5

        [HttpPost]
        public ActionResult Edit(Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insurance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insurance);
        }

        //
        // GET: /Insurance/Delete/5
 
        public ActionResult Delete(string id)
        {
            Insurance insurance = db.Insurances.Find(id);
            return View(insurance);
        }

        //
        // POST: /Insurance/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {            
            Insurance insurance = db.Insurances.Find(id);
            db.Insurances.Remove(insurance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}