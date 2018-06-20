using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using PayrollSoft.Models;
using PayrollSoft.Utility;

namespace PayrollSoft.Controllers
{
    public class DeductionsController : Controller
    {
        public class paymentCode 
        {
            public string code { get; set; }
            public string description { get; set; }
        }

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
        public JsonResult getDeductionPayments()
        {
            try
            {
                var payments = from pt in db.Deductions
                               where pt.Voided != 1
                               select new {pt.payCode,
                                           pt.payType,
                                           pt.payCodeDescription,
                                           pt.payTypeDescription,
                                           pt.priorityCode,
                                           pt.includeInretrospectPayments,
                                           pt.groupEligibleDescription,
                                           pt.gradeId,
                                           pt.creditGl,
                                           pt.creditGlDescription,
                                           pt.deductionTakenFromEmployee,
                                           pt.priorityDescription
                                           };

                return Json(payments, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string err_msg = e.Message.ToString();
                return Json(false);
            }
        }

        [HttpPost]
        public ActionResult Create(Deductions deduction)
        {
            string Message = null;
            string Status = null;
            deduction.DateCreated = DateTime.Now.Date;
            deduction.CreatedBy = (int)Session["LoggedUserId"];
            try
            {
                if (ModelState.IsValid)
                {
                    using (var context = new DataContext())
                    {
                        if(!context.Deductions.Any(j => j.payCode == deduction.payCode))
                        {
                            db.Deductions.Add(deduction);
                            db.SaveChanges();
                            Message = "Deduction payment created successfully";
                            Status = "success";
                        }
                    }

                }
                else 
                {

                    Message = "Incomplete data for the deduction payment. Please enter all data required";
                    Status = "error";
                }
            }
            catch (Exception e)
            {
                Status = "error";
                Message = e.Message;
            }

            return Json(new { Status = Status, Message = Message });
                
        }
        
        public ActionResult EditDeductionPayment(Deductions deduction)
        {
            string Message = null;
            string Status = null;

            Deductions payment = new Deductions();
            payment = DeductionPaymentDetails(deduction.payCode);

            deduction.UpdatedBy = (int)Session["LoggedUserId"];
            deduction.DateUpdated = DateTime.Now.Date;
            deduction.CreatedBy = payment.CreatedBy;
            deduction.DateCreated = payment.DateCreated;

            if (UpdateDeductionPayment(deduction))
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

        private Deductions DeductionPaymentDetails(string id)
        {
            Deductions deduction = new Deductions();

            var results = from deduct in db.Deductions
                          where deduct.payCode == id
                          select new
                          {
                            deduct.payCode,
                            deduct.payType,
                            deduct.payCodeDescription,
                            deduct.payTypeDescription,
                            deduct.priorityCode,
                            includeInrestrospectPayments = deduct.includeInretrospectPayments,
                            deduct.groupEligibleDescription,
                            deduct.gradeId,
                            deduct.creditGl,
                            deduct.creditGlDescription,
                            deduct.deductionTakenFromEmployee,
                            deduct.priorityDescription,
                            deduct.DateCreated,
                            deduct.CreatedBy,
                            deduct.UpdatedBy,
                            deduct.DateUpdated,
                            deduct.Voided,
                            deduct.VoidedBy,
                            deduct.DateVoided
                          };
            foreach (var x in results)
            {
                deduction.payCode = x.payCode;
                deduction.payType = x.payType;
                deduction.payCodeDescription = x.payCodeDescription;
                deduction.payTypeDescription = x.payTypeDescription;
                deduction.priorityCode = x.priorityCode;
                deduction.includeInretrospectPayments = x.includeInrestrospectPayments;
                deduction.groupEligibleDescription = x.groupEligibleDescription;
                deduction.gradeId = x.gradeId;
                deduction.creditGl = x.creditGl;
                deduction.creditGlDescription = x.creditGlDescription;
                deduction.deductionTakenFromEmployee = x.deductionTakenFromEmployee;
                deduction.priorityDescription = x.priorityDescription;
                deduction.DateCreated = x.DateCreated;
                deduction.CreatedBy = x.CreatedBy;
                deduction.UpdatedBy = x.UpdatedBy;
                deduction.DateUpdated = x.DateUpdated;
                deduction.Voided = x.Voided;
                deduction.VoidedBy = x.VoidedBy;
                deduction.DateVoided = x.DateVoided;
            }

            return deduction;
        }

        private Boolean UpdateDeductionPayment(Deductions deduction)
        {
            Boolean success = false;
            using (var context = new DataContext())
            {
                try
                {
                    context.Entry(deduction).State = EntityState.Modified;
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
        public ActionResult DeleteDeductionPayment(string id)
        {
            string Message = null;
            string Status = null;
            Deductions deduction = new Deductions();

            deduction = DeductionPaymentDetails(id);

            deduction.Voided = 1;
            deduction.VoidedBy = (int)Session["LoggedUserId"];
            deduction.DateVoided = DateTime.Now.Date;

            if (UpdateDeductionPayment(deduction))
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
