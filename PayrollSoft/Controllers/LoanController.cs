using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;
using PayrollSoft.AuditTrails;
using PayrollSoft.Models;
using PayrollSoft.BusinessLogicLayer;
using PayrollSoft.Utility;


namespace PayrollSoft.Controllers
{
    public class LoanReference
    {
        public string loanrefNumber { get; set; }
        public DateTime resumeDate { get; set; }
        public string status { get; set; }
    }
    
    public class LoanController : Controller
    {
        private DataContext db = new DataContext();

        private UtilityBase util = new UtilityBase();

        public ActionResult Index()
        {
            ViewBag.Message = Session["LoggedUserFullName"];
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult SearchParam(string id)
        {
           
            var results = from lm in db.LoanPortifolios
                          where lm.LoanRefNumber == id
                          select new {lm.EntryDate,lm.InterestPaid,lm.PrincipalPaid,lm.LoanBalance,lm.EndOfPeriod};       
            
            return Json(results, JsonRequestBehavior.AllowGet);

        }
       
        [HttpGet]
        public ActionResult getLoanRecord(string refnumber)
        {
            var results = from lm in db.LoanMasters
                          join lt in db.LoanTypes on lm.LoanTypeNumber equals lt.Code
                          where lm.LoanRefNumber == refnumber
                          select new {lm.LoanRefNumber,lt.Description,lm.startDate,lm.EndDate,lm.PaybackPeriods,lm.LoanAmount,lm.LoanBalance };
            return Json(results);
        }


        public ActionResult Details(string id)
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetRecordToAuthorize(string id)
        {
            var record = from lm in db.LoanMasters
                         join lt in db.LoanTypes on lm.LoanTypeNumber equals lt.Code
                         join emp in db.Employees on lm.EmpID equals emp.EmpID
                         where lm.LoanRefNumber == id
                         select new {lm.LoanRefNumber,
                                     lm.LoanAmount,
                                     lm.LoanBalance,
                                     lt.Description,
                                     emp.EmpName,
                          lm.PaybackPeriods,lm.MonthlyRepayment,lm.startDate,lm.EndDate,lm.Status};
            return Json(record, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AuthorizeLoanContract(string Id)
        {
            string _message = string.Empty, _status = string.Empty;

            using (var context = new DataContext())
            {
                try
                {
                    PendingTransaction pendingTxn = new PendingTransaction(Id, (int)Session["LoggedUserId"], "Loans","Loan", "loanMasters");
                    loanMaster loan = context.LoanMasters.Find(Id);

                    loan.RecordStatusChangedBy = (int)Session["LoggedUserId"];
                    loan.RecordStatusId = util.GetRecordStatusId("AUTHORIZED");
                    loan.RecordStatusDateChanged = DateTime.Now.Date;

                    context.Entry(loan).State = EntityState.Modified;
                    context.SaveChanges();

                    pendingTxn.ClearPendingTransaction();

                    _status = "success";
                    _message = "Record authorized successfully!";
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " " + e.InnerException.Message.ToString());
                    _status = "error";
                    _message = e.Message.ToString();
                }

                return Json(new { status = _status, _message = _message });
            }
        }


        public ActionResult Create()
        {
            return View();
        }

        [Audit]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UpdateLoanDetails(LoanReference loanref)
        {
            string _status = string.Empty, _message = string.Empty;
            using (var context = new DataContext())
            {
                try
                {
                    loanMaster loan = context.LoanMasters.Find(loanref.loanrefNumber);
                    loan.NextDateOfUpdate = loanref.resumeDate;
                    loan.DateModified = DateTime.Now.Date;
                    loan.ModifiedBy = (int)Session["LoggedUserId"];
                    loan.Status = loanref.status;
                    loan.RecordStatusId = util.GetRecordStatusId("PENDING");
                    loan.RecordStatusChangedBy = (int)Session["LoggedUserId"];
                    loan.RecordStatusDateChanged = DateTime.Now.Date;

                    if (ChangeLoanState(loanref.loanrefNumber, loanref.status, loanref.resumeDate))
                    {
                        PendingTransaction pendTxn = new PendingTransaction(loanref.loanrefNumber, loan.ModifiedBy, "Loan", "Loan", "loanMasters");
                        context.Entry(loan).State = EntityState.Modified;
                        context.SaveChanges();
                        pendTxn.LogPendingTransaction();
                        _status = "success";
                        _message = "Loan status changed successfully";
                    }
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " " + e.InnerException.Message.ToString());
                    _status = "error";
                    _message = e.Message.ToString() + " " + e.InnerException.Message.ToString();
                }

                return Json(new {status = _status, message = _message });
            }
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Loan/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Loan/Edit/5

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
        // GET: /Loan/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Loan/Delete/5

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

        private Boolean ChangeLoanState(string refnum, string status, DateTime __date)
        {
            
            using (var context = new DataContext())
            {
                Boolean _result = false;

                if (status == "Frozen")
                {
                    if (!context.FrozenLoans.Any(x => x.LoanRefNumber == refnum))
                    {
                        FrozenLoan frozenloan = new FrozenLoan();
                        frozenloan.LoanRefNumber = refnum;
                        frozenloan.ResumeDate = __date;
                        frozenloan.CreatedBy = (int)Session["LoggedUserId"];
                        frozenloan.DateCreated = DateTime.Now.Date;
                        context.FrozenLoans.Add(frozenloan);
                        context.SaveChanges();
                        _result = true;
                    }
                    else
                    {
                        FrozenLoan frozenloan = context.FrozenLoans.Find(refnum);
                        frozenloan.ResumeDate = __date;
                        frozenloan.CreatedBy = (int)Session["LoggedUserId"];
                        frozenloan.DateCreated = DateTime.Now.Date;
                        context.Entry(frozenloan).State = EntityState.Modified;
                        context.SaveChanges();
                        _result = true;
                    }
                }
                else
                {
                    FrozenLoan frozenloan = context.FrozenLoans.Find(refnum);
                    frozenloan.Voided = 1;
                    frozenloan.VoidedBy = (int)Session["LoggedUserId"];
                    frozenloan.DateVoided = DateTime.Now.Date;
                    context.Entry(frozenloan).State = EntityState.Modified;
                    context.SaveChanges();
                    _result = true;
                }

                return _result;
            }
        }
    }
}
