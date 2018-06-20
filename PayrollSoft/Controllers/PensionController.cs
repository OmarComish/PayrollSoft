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

namespace PayrollSoft.Controllers
{

    //This class holds the definition for the data to be pulled from the Pension object from the database, that holds the parameters
    //needed for PENSION Computation

    public class pensionRate 
    {
        public Double employersContribution { get; set; }
        public Double employeesContribution { get; set; }
    }

    public class PensionController : Controller
    {
        private DataContext db = new DataContext();
        private UtilityBase util = new UtilityBase();

        public ViewResult Index()
        {
            return View(db.Pensions.ToList());
        }

        public ViewResult Details(string id)
        {
            Pension pension = db.Pensions.Find(id);
            return View(pension);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetRecordToAuthorize(string id)
        {

                var record = from p in db.Pensions
                             where p.PensionRefNum == id
                             select new {p.PensionRefNum,p.GroupLifeAssuranceRate,p.EmployerContrRate,
                                         p.EmployeeContrRate,p.AdminiFeeRate,p.BrokerageFeeRate };

                return Json(record, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AuthorizePension(string Id)
        {
            using (var context = new DataContext())
            {
                string __status = string.Empty, __message = string.Empty;
                try
                {
                    
                    Pension pension = context.Pensions.Find(Id);

                    PendingTransaction pendingTxn = new PendingTransaction(Id, pension.CreatedBy, "Pension","Pension", "Pensions");
                    pension.RecordStatusId = util.GetRecordStatusId("AUTHORIZED");
                    pension.RecordStatusChangedBy = (int)Session["LoggedUserId"];
                    pension.RecordStatusDateChanged = DateTime.Now.Date;

                    context.Entry(pension).State = EntityState.Modified;
                    context.SaveChanges();
                    pendingTxn.ClearPendingTransaction();

                    __status = "success";
                    __message = "Record successfully authorized";
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " " + e.InnerException.Message.ToString());
                    __status = "error";
                    __message = e.InnerException.Message.ToString();
                }

                return Json(new {status = __status, message = __message });
            }
        }

        [HttpPost]
        public ActionResult Create(Pension pension)
        {
            string __status = string.Empty, __message = string.Empty;
            pension.CreatedBy = (int)Session["LoggedUserId"];
            pension.DateCreated = DateTime.Now.Date;
            pension.RecordStatusId = util.GetRecordStatusId("PENDING");
            pension.RecordStatusChangedBy = (int)Session["LoggedUserId"];
            pension.RecordStatusDateChanged = DateTime.Now.Date;

            using (var context = new DataContext())
            {
                if (ModelState.IsValid)
                {
                    PendingTransaction pendingTxn = new PendingTransaction(pension.PensionRefNum, pension.CreatedBy, "Pension","Pension", "Pensions");
                    CreatePaymentCode paymentcode = new CreatePaymentCode();
                    try
                    {
                        context.Pensions.Add(pension);
                        context.SaveChanges();
                        pendingTxn.LogPendingTransaction();
                        paymentcode.AddPaymentCode(pension.PensionRefNum, "PENSION", pension.CreatedBy);
                        __status = "success";
                        __message ="Pension added successfully";
                    }
                    catch (Exception e)
                    {
                        util.WriteToLog(e.Message.ToString() + " " + e.InnerException.ToString());
                        __status = "error";
                        __message = e.InnerException.Message.ToString();
                    }

                }
            }

            return Json(new {status = __status, message = __message });
        }

        //Get the pension Codes to populate the dropdownList control
        //on the UI
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getPensionCode() 
        {
            var result = db.Pensions.ToList();
            var pensionCodes = from p in result
                               select new { p.PensionRefNum};
            return Json(pensionCodes,JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(string id)
        {
            Pension pension = db.Pensions.Find(id);
            return View(pension);
        }

        [HttpPost]
        public ActionResult Edit(Pension pension)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pension).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pension);
        }

        //
        // GET: /Pension/Delete/5
 
        public ActionResult Delete(string id)
        {
            Pension pension = db.Pensions.Find(id);
            return View(pension);
        }

        //
        // POST: /Pension/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {            
            Pension pension = db.Pensions.Find(id);
            db.Pensions.Remove(pension);
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