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
    public class TaxationThresholdController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /TaxationThreshold/

        public ViewResult Index()
        {
            
            return View(db.TaxationThresholds.ToList());
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetTaxThreshold()
        {

            using (DataContext context = new DataContext())
            {
                var thresholdId = from thr in context.TaxationThresholds
                                  select new { thr.ThresholdID };

                return Json(thresholdId, JsonRequestBehavior.AllowGet);
                 
            }

        
        }

        //
        // GET: /TaxationThreshold/Details/5

        public ViewResult Details(string id)
        {
            TaxationThreshold taxationthreshold = db.TaxationThresholds.Find(id);
            return View(taxationthreshold);
        }

        //
        // GET: /TaxationThreshold/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /TaxationThreshold/Create

        [HttpPost]
        public ActionResult Create(TaxationThreshold taxationthreshold)
        {
            if (ModelState.IsValid)
            {
                db.TaxationThresholds.Add(taxationthreshold);
                db.SaveChanges();
                return Json(true);  
            }

            return Json(false);
        }
        
        //
        // GET: /TaxationThreshold/Edit/5
 
        public ActionResult Edit(string id)
        {
            TaxationThreshold taxationthreshold = db.TaxationThresholds.Find(id);
            return View(taxationthreshold);
        }

        //
        // POST: /TaxationThreshold/Edit/5

        [HttpPost]
        public ActionResult Edit(TaxationThreshold taxationthreshold)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taxationthreshold).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taxationthreshold);
        }

        //
        // GET: /TaxationThreshold/Delete/5
 
        public ActionResult Delete(string id)
        {
            TaxationThreshold taxationthreshold = db.TaxationThresholds.Find(id);
            return View(taxationthreshold);
        }

        //
        // POST: /TaxationThreshold/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {            
            TaxationThreshold taxationthreshold = db.TaxationThresholds.Find(id);
            db.TaxationThresholds.Remove(taxationthreshold);
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