using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using PayrollSoft.Models;
using PayrollSoft.Utility;

namespace PayrollSoft.Controllers
{

    public class PaymentsController : Controller
    {

        private DataContext db = new DataContext();
        private UtilityBase util = new UtilityBase();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(EarningPayments payment) 
        {
            if (ModelState.IsValid)
            {
                db.EarningPayments.Add(payment);
                db.SaveChanges();
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getpaymentCode() 
        {

            var deductpaycodes = from pc in db.PaymentCodes
                                 join d in db.Deductions on pc.payCode equals d.payCode
                                 where d.priorityCode != 1
                                 select new { pc.payCode, pc.Description };

            var earnpaycodes = from pc in db.PaymentCodes
                               join e in db.Earnings on pc.payCode equals e.payCode
                               where e.priority != 1
                               select new {pc.payCode, pc.Description };

            var query = earnpaycodes.Union(deductpaycodes).Distinct().ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
  
         }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetEarningPaycodes()
        {
            try
            {
                var codes = from p in db.PaymentCodes
                            join e in db.Earnings on p.payCode equals e.payCode
                            where e.priority != 1
                            select new { p.payCode, p.Description };

                return Json(codes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString() + " " + e.InnerException.ToString());
                return Json(false);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getearningpaymentCode()
        {
            var codes = from p in db.PaymentCodes
                        join e in db.Earnings on p.payCode equals e.payCode
                        select new { p.payCode, p.Description,e.payRate };

            return Json(codes, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getpaymentType() 
        {
                var paytype = from pt in db.PaymentTypes
                              select new {paymentType = pt.payType,pt.Description };
                return Json(paytype);
        }

       [AcceptVerbs(HttpVerbs.Post)]
       public ActionResult CreatePaymentType(PaymentType paymenttype)
       {
           string Message = null;
           string Status = null;

            if (ModelState.IsValid)
            {

                if (!db.PaymentTypes.Any(t => t.payType == paymenttype.payType))
                {
                    db.PaymentTypes.Add(paymenttype);
                    db.SaveChanges();
                    Message = "Record saved successfully";
                    Status = "success";

                }
                else 
                {

                    Message = "Record already exist";
                    Status = "error";
                }

             }

            return Json(new {Message = Message, Status = Status });
       
       }

       [AcceptVerbs(HttpVerbs.Post)]
       public ActionResult CreatePaymentCode(PaymentCodes paymentcode)
       {
            if (ModelState.IsValid)
            {
                if (!db.PaymentCodes.Any(x => x.payCode == paymentcode.payCode))
                {
                    db.PaymentCodes.Add(paymentcode);
                    db.SaveChanges();

                }

                return Json(true);
            }
            else
            {

                return Json(false);
            }
        }

       protected override void Dispose(bool disposing)
       {
            db.Dispose();
            base.Dispose(disposing);
       }

             
   }

}
