using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayrollSoft.Models;
using PayrollSoft.Utility;
using PayrollSoft.BusinessLogicLayer;
using PayrollSoft.AuditTrails;

namespace PayrollSoft.Controllers
{ 
    public class TaxationController : Controller
    {
        private DataContext db = new DataContext();
        private UtilityBase _util = new UtilityBase();

        public ViewResult Index()
        {
            return View(db.Taxations.ToList());
        }

        public ViewResult Details(string id)
        {
            Taxation taxation = db.Taxations.Find(id);
            return View(taxation);
        }

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult Create(Taxation taxation)
        {
            if (ModelState.IsValid)
            {
                db.Taxations.Add(taxation);
                db.SaveChanges();
                return Json(true);  
            }

            //return View(taxation);
            return Json(false);
        }
   
        public ActionResult Edit(string id)
        {
            Taxation taxation = db.Taxations.Find(id);
            return View(taxation);
        }

        [Audit]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AuthorizeTaxation(int Id)
        {
            using (var context = new DataContext())
            {
                string __status = string.Empty, __message = string.Empty;
                try
                {

                    WithHoldingTax taxation = context.WithHoldingTaxes.Find(Id);

                    PendingTransaction pendingTxn = new PendingTransaction(Id.ToString(), taxation.CreatedBy, "Taxation","Taxation", "WithHoldingTaxes");
                    taxation.RecordStatusId = _util.GetRecordStatusId("AUTHORIZED");
                    taxation.RecordStatusChangedBy = (int)Session["LoggedUserId"];
                    taxation.RecordStatusDateChanged = DateTime.Now.Date;

                    context.Entry(taxation).State = EntityState.Modified;
                    context.SaveChanges();
                    pendingTxn.ClearPendingTransaction();

                    __status = "success";
                    __message = "Record successfully authorized";
                }
                catch (Exception e)
                {
                    _util.WriteToLog(e.Message.ToString() + " " + e.InnerException.Message.ToString());
                    __status = "error";
                    __message = e.InnerException.Message.ToString();
                }

                return Json(new { status = __status, message = __message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetRecordToAuthorize(int Id)
        {
            var record = from wt in db.WithHoldingTaxes
                         where wt.Id == Id
                         select new {wt.TaxRefCode,wt.MinAmount,wt.MaxAmount,wt.Rate,wt.ThresholdNumber };

            return Json(record, JsonRequestBehavior.AllowGet);
        }

        [Audit]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Edit(WithHoldingTax taxation)
        {

          using (var context = new DataContext())
          {
            string __status = string.Empty, __message = string.Empty;
            try
            {
                WithHoldingTax _rec = context.WithHoldingTaxes.FirstOrDefault(x => x.TaxRefCode == taxation.TaxRefCode && x.ThresholdNumber == taxation.ThresholdNumber);

                PendingTransaction pendingTxn = new PendingTransaction(_rec.Id.ToString(), (int)Session["LoggedUserId"], "Taxation","Taxation", "WithHoldingTaxes");
                    
                _rec.DateModified = DateTime.Now.Date;
                _rec.ModifiedBy = (int)Session["LoggedUserId"];
                _rec.RecordStatusChangedBy = (int)Session["LoggedUserId"];
                _rec.RecordStatusDateChanged = DateTime.Now.Date;
                _rec.RecordStatusId = _util.GetRecordStatusId("PENDING");
                _rec.MinAmount = taxation.MinAmount;
                _rec.MaxAmount = taxation.MaxAmount;

                context.Entry(_rec).State = EntityState.Modified;
                context.SaveChanges();
                pendingTxn.LogPendingTransaction();

                 __status = "success";
                __message = "Changes successfully saved";
                
            }
            catch (Exception e)
            {
                _util.WriteToLog(e.Message.ToString() + " " + e.InnerException.Message.ToString());
                __status = "error";
                __message = e.InnerException.Message.ToString();
            }

            return Json(new { status = __status, message = __message });
          }
            
        }

        private WithHoldingTax ReadTaxationSettings()
        {
            WithHoldingTax t = new WithHoldingTax();

            using(var context = new DataContext())
            {
                var results = from tx in context.WithHoldingTaxes
                              select new {tx.CreatedBy,tx.DateCreated};
                foreach(var x in results)
                {
                    t.DateCreated = x.DateCreated;
                    t.CreatedBy = x.CreatedBy;
                }
                
            }

            return t;
        }
 
        public ActionResult Delete(string id)
        {
            Taxation taxation = db.Taxations.Find(id);
            return View(taxation);
        }

        //
        // POST: /Taxation/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {            
            Taxation taxation = db.Taxations.Find(id);
            db.Taxations.Remove(taxation);
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