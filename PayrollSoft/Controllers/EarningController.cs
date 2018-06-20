using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using PayrollSoft.Models;
using PayrollSoft.Utility;

namespace PayrollSoft.Controllers
{
    public class EarningController : Controller
    {
        private DataContext db = new DataContext();

        private UtilityBase util = new UtilityBase();

        public ActionResult Index()
        {
            ViewBag.Message = Session["LoggedUserFullName"];
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getEarningPayments()
        {
           
                try
                {
                    var payments = from pt in db.Earnings
                                   where pt.Voided != 1
                                   select new { pt.payTypeDescription,
                                                pt.groupEligible,
                                                pt.payCode,
                                                pt.payType,
                                                pt.payCodeDescription,
                                                pt.paidAsEarnings,
                                                pt.payMethod,
                                                pt.payRate,
                                                pt.priority,
                                                pt.rateDerivedSource,
                                                pt.taxableIncome,
                                                pt.timesheetEntryAllowed,
                                                pt.higherPrecedenceFactor,
                                                pt.groupEligibleDescription,
                                                pt.debitGlDescription,
                                                pt.debitGL
                                    };

                    return Json(payments, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    
                    util.WriteToLog(e.Message.ToString());
                    return Json(false);
                }
            
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getPaymentType()
        {

            using (var context = new DataContext())
            {
                

                try
                {
                    var payments = from pt in context.Earnings
                                   select new { pt.payTypeDescription,pt.groupEligible };

                    return Json(payments, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString());
                    return Json(false);
                }
            }
         
        }

        [HttpPost]
        public ActionResult Create(Earnings earning)
        {

            string Message = null;
            string Status = null;
            earning.DateCreated = DateTime.Now.Date;
            earning.CreatedBy = (int)Session["LoggedUserId"];
            try
            {
                if (ModelState.IsValid)
                {
                    using (var context = new DataContext())
                    {
                        if(!context.Earnings.Any(j => j.payCode == earning.payCode))
                        {
                            db.Earnings.Add(earning);
                            db.SaveChanges();
                            Message = "Earning payment created successfully";
                            Status = "success";
                        }
                    }

                }
                else 
                {

                    Message = "Incomplete data for the earning payment. Please enter all data required";
                    Status = "error";
                }
                
           }
           catch(Exception e)
           {
               Status = "error";
               Message = e.Message;
           }

            return Json(new { Status = Status, Message = Message });
        }
        
        public ActionResult EditEarningPayment(Earnings earning)
        {
            string Message = null;
            string Status = null;

            Earnings payment = new Earnings();
            payment = EarningPaymentDetails(earning.payCode);

            earning.UpdatedBy = (int)Session["LoggedUserId"];
            earning.DateUpdated = DateTime.Now.Date;
            earning.CreatedBy = payment.CreatedBy;
            earning.DateCreated = payment.DateCreated;

            if (UpdateEarningPayment(earning))
            {
                Message = "Payment changes successfully saved";
                Status = "success";
            }
            else
            {
                Message = "Error updating payment";
                Status = "error";
            }

            return Json(new { Status = Status, Message = Message });
           
        }

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

        [HttpPost]
        public ActionResult DeleteEarningPayment(string id)
        {
            string Message = null;
            string Status = null;
            Earnings earning = new Earnings();

            earning = EarningPaymentDetails(id);

            earning.Voided = 1;
            earning.VoidedBy = (int) Session["LoggedUserId"];
            earning.DateVoided = DateTime.Now.Date;

            if (UpdateEarningPayment(earning))
            {
                Message = "Payment successfully deleted";
                Status = "success";
            }
            else 
            {
                Message = "Error deleting payment";
                Status = "error";
            }

            return Json(new { Status = Status, Message = Message });
        }

        private Earnings EarningPaymentDetails(string id)
        {
            Earnings earning = new Earnings();

            var results = from earn in db.Earnings
                          where earn.payCode == id
                          select new
                          {
                              earn.payTypeDescription,
                              earn.groupEligible,
                              earn.payCode,
                              earn.payType,
                              earn.payCodeDescription,
                              earn.paidAsEarnings,
                              earn.payMethod,
                              earn.payRate,
                              earn.priority,
                              earn.rateDerivedSource,
                              earn.taxableIncome,
                              earn.timesheetEntryAllowed,
                              earn.higherPrecedenceFactor,
                              earn.groupEligibleDescription,
                              earn.debitGlDescription,
                              earn.debitGL,
                              earn.DateCreated,
                              earn.CreatedBy,
                              earn.UpdatedBy,
                              earn.DateUpdated,
                              earn.Voided,
                              earn.VoidedBy,
                              earn.DateVoided
                          };
            foreach (var x in results)
            { 
                earning.payTypeDescription = x.payTypeDescription;
                earning.groupEligible = x.groupEligible;
                earning.payCode = x.payCode;
                earning.payType = x.payType;
                earning.payCodeDescription = x.payCodeDescription;
                earning.paidAsEarnings = x.paidAsEarnings;
                earning.payMethod = x.payMethod;
                earning.payRate = x.payRate;
                earning.priority = x.priority;
                earning.rateDerivedSource = x.rateDerivedSource;
                earning.taxableIncome = x.taxableIncome;
                earning.timesheetEntryAllowed = x.timesheetEntryAllowed;
                earning.higherPrecedenceFactor = x.higherPrecedenceFactor;
                earning.groupEligibleDescription = x.groupEligibleDescription;
                earning.debitGlDescription = x.debitGlDescription;
                earning.debitGL = x.debitGL;
                earning.DateCreated = x.DateCreated;
                earning.CreatedBy = x.CreatedBy;
                earning.UpdatedBy = x.UpdatedBy;
                earning.DateUpdated = x.DateUpdated;
                earning.Voided = x.Voided;
                earning.VoidedBy = x.VoidedBy;
                earning.DateVoided = x.DateVoided;
            }

            return earning;
        }

        private Boolean UpdateEarningPayment(Earnings earning)
        {
            Boolean success = false;
            using (var context = new DataContext())
            {
                try
                {
                    context.Entry(earning).State = EntityState.Modified;
                    context.SaveChanges();
                    success = true;
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString());
                    success = false;
                }

                return success;
            }
        }

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
