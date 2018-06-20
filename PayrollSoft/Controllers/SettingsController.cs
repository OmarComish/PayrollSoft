using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using PayrollSoft.AuditTrails;
using PayrollSoft.Models;
using PayrollSoft.BusinessLogicLayer;
using PayrollSoft.Utility;


namespace PayrollSoft.Controllers
{
    public class MonthlyPMT
    {
        public double rate { get; set; }
        public double Amount { get; set; }
        public int months { get; set; }
    }

    public class searchResults
    {
        public Boolean recordExist { get; set; }
        public string tblName { get; set; }
        public string higherPrecedenceFactor { get; set; }
        public double actualPayAmount { get; set; }
        public string payCodeDescription { get; set; }
        public double payRate { get; set; }
        public string rateDerivedFrom { get; set; }
        public string payType { get; set; }
        public string groupEligible { get; set; }
    }

    public class searchParams
    {
        public string id {get; set;}
        public string gradeName {get; set;}
        public string empNumber { get; set; }
        public Double actualPayAmount { get; set; }
    }

    public class paymentsearchParamters
    {
        public string recId { get; set; }
        public string DATE { get; set; }
    }

    public class timesheetlog
    {
        public string logNum { get; set; }
        public string payCode { get; set; }
        public string EmpID { get; set; }
        public string Date { get; set; }
        public double overtimeHours { get; set; }
    }

    public class loanReturnValue: loanRepaymentReturnValues
    {
        //this class is extending from the loanRepaymentReturnValues class which is declared in the BusinessLogicLayer Namespace
    }

    public class loanParams 
    {
        public double interestRate { get; set; }
    }

    public class loanLookUptbl

    {
        public string loanrefNumber { get; set; }
        public DateTime EndDate { get; set; }

    }

    public class loanportTestCls
    {
        public string EntryID { get; set; }
        public string LoanRefNumber { get; set; }
        public DateTime EntryDate { get; set; }
        public double InterestPaid { get; set; }
        public double PrincipalPaid { get; set; }
        public double LoanBalance { get; set; }
        public int EndOfPeriod { get; set; }
    }

    public class lookUpTable
    {
        public string LoanRefNumber { get; set; }
        public int PaybackPeriods { get; set; }
        public double MonthlyRepayment { get; set; }
        public double InterestRate { get; set; }
        public double LoanBalance { get; set; }
        public int PeriodToUpdate { get; set; }
    }

    public class loanMasterRecord
    {
        public string LoanRefNumber { get; set; }
        public string LoanTypeNumber { get; set; }
        public string EmpID { get; set; }
        public string startDate { get; set; }
        public string EndDate { get; set; }
        public int PaybackPeriods { get; set; }
        public double LoanAmount { get; set; }
        public string Formular { get; set; }
        public double MonthlyRepayment { get; set; }
        public string Status { get; set; }
        public int EndOfPeriod { get; set; }
        public string EntryID { get; set; }
    }

    public class payrollFormularRecord
    {
        public int Id { get; set; }
        public string Paycode { get; set; }
        public string Formular { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class SettingsController : Controller
    {
        private DataContext db = new DataContext();

        private UtilityBase util = new UtilityBase();

        public ViewResult OvertimeSettings()
        {
            return View();
        }
        public ViewResult PersonnelSettings()
        {
            return View();
        }

        public ActionResult LoanSettings()
        {
            //ViewBag.Message = "We are in the security Module";
            return View();
        }

        public ActionResult HolidaySettings()
        {
            //ViewBag.Message = "We are in the security Module";
            return View();
        }

        public ActionResult TaxationSettings()
        {
            return View();
        }

        public ActionResult PayrollSettings()
        {
            return View();
        }

        public ActionResult SeveranceSettings()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetPensionSettings()
        {
            int recstat = util.GetRecordStatusId("AUTHORIZED");
            var results = from pns in db.Pensions
                          where pns.RecordStatusId == recstat
                          select new {pns.EmployeeContrRate,pns.EmployerContrRate,
                                      pns.GroupLifeAssuranceRate,pns.PensionRefNum,pns.BrokerageFeeRate,pns.AdminiFeeRate };
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetPendingTransactions()
        {
            var pendingitems = from pi in db.PendingItems
                               join u in db.Users on pi.Initiator equals u.UserId
                               select new {ItemId = pi.ReferenceNumber,u.FullName,pi.Source,pi.Controller,pi.TimeStamp };

            return Json(pendingitems, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult readleavePaySettings() 
        {
            var leavepaySettings = from app in db.AppConfig_LeavePay
                                   select new {app.ID,app.LeaveFrequency,app.IncludeLeavePayAfter,app.RateBasedOn ,app.RateOfLeavePay };
            return Json(leavepaySettings, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getLeaveTypes()
        { 
            int _authorized = util.GetRecordStatusId("AUTHORIZED");

            var results = from lt in db.LeaveTypes
                          where lt.RecordStatusId == _authorized && lt.Voided == 0
                          select new {lt.Description,lt.LeaveTypeId};

            return Json(results, JsonRequestBehavior.AllowGet);
                          
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getPaymentCodes()
        {

            var paycodes = from pc in db.PaymentCodes
                           join d in db.Deductions on pc.payCode equals d.payCode
                           //where d.priorityCode == 1
                           select new { pc.payCode, pc.Description };
            //join a UNION query here between Earnings and Deductions

            return Json(paycodes,JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult readSeveranceSettings()
        {
            var settings = from svp in db.SeveranceEarnings
                           select new {svp.RefNumber,
                                       svp.Description,
                                       svp.FirstThresholdRate,
                                       svp.SecondThresholdRate,
                                       svp.ThirdThresholdRate,
                                       svp.FirstThresholdMinPeriod,
                                       svp.SecondThresholdMinPeriod,
                                       svp.ThirdThresholdMinPeriod,
                                       svp.FirstThresholdMaxPeriod,
                                       svp.SecondThresholdMaxPeriod,
                                       svp.ThirdThresholdMaxPeriod,
                                       svp.FirstThresholdMinPeriodIn,
                                       svp.SecondThresholdMinPeriodIn,
                                       svp.ThirdThresholdMinPeriodIn,
                           svp.FirstThresholdMaxPeriodIn,svp.SecondThresholdMaxPeriodIn,svp.ThirdThresholdMaxPeriodIn};

            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult readTaxationSettings()
        {
            var tax = from tx in db.WithHoldingTaxes
                      join t in db.Taxations on tx.TaxRefCode equals t.TaxRefCode
                      select new {tx.TaxRefCode,tx.MaxAmount, tx.MinAmount,tx.Rate,tx.ThresholdNumber,t.Description,tx.DateCreated };
            return Json(tax, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getExistingLoans()
        {
            var results = from lm in db.LoanMasters
                          join emp in db.Employees on lm.EmpID equals emp.EmpID
                          select new {emp.EmpID,emp.EmpName,
                                      lm.LoanTypeNumber,
                                      lm.startDate,
                                      lm.EndDate,
                                      lm.LoanAmount,
                                      lm.PaybackPeriods,
                                      lm.Status,
                                      lm.Formular,
                                      lm.LoanRefNumber };
            return Json(results, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getEmployee()
        {
            var emp = from e in db.Employees
                      select new { e.EmpID,e.EmpName};
            return Json(emp, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult getLoanMonthlyRepayment(MonthlyPMT monthlypmtvals)
        {
            double amount = PMT(monthlypmtvals.rate,monthlypmtvals.months, monthlypmtvals.Amount);
            var monthlyRepayment = new {Amount = Math.Round(amount,2) };
            return Json(monthlyRepayment);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getLoanDetails(string refnumber)
        {
            return Json(refnumber);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getPayment()
        {
            var payment = from e in db.Earnings
                      select new { e.payType, e.payTypeDescription };

            return Json(payment, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getPaymentType()
        { 
            using(var context = new DataContext())
            {
                var payments = from pt in context.Earnings
                               select new {pt.payTypeDescription};
                //searchResults results = new searchResults();

                //IEnumerable<searchResults> payments = context.Earnings.Select(x => new searchResults {payType = x.payType }).Distinct();
                //foreach(var payment in payments)
                //{
                //    results.payType = payment.payType;
                //}


                return Json(payments, JsonRequestBehavior.AllowGet);
          
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getEarningPayments()
        {
            using (var context = new DataContext())
            {
                var payments = from pt in context.Earnings
                               select new { pt.payCode,pt.payType, pt.payTypeDescription };
                //searchResults results = new searchResults();

                //IEnumerable<searchResults> payments = context.Earnings.Select(x => new searchResults {payType = x.payType }).Distinct();
                //foreach(var payment in payments)
                //{
                //    results.payType = payment.payType;
                //}


                return Json(payments, JsonRequestBehavior.AllowGet);

            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getPriorityCodes()
        {

                var codes = from pcode in db.PriorityCodes
                            where pcode.Voided != 1
                            select new { pcode.PriorityCode, pcode.Description };
                return Json(codes, JsonRequestBehavior.AllowGet);
       

        
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getOvertimeSettings()
        {
            int status = util.GetRecordStatusId("AUTHORIZED");
            var overtime = from ot in db.OverTimes
                           where ot.RecordStatusId == status
                           select new {ot.Code,ot.Rate,ot.AllowableWorkDays,ot.AllowableWorkHours,ot.Description };
            return Json(overtime, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getLoanType()
        {
            var loantype = from t in db.LoanTypes
                        select new { t.Code, t.Description,t.InterestRate,t.MaxLoanRepaymentPeriod,t.EnablePrecalculation,t.AutoGenerateLoanRefNum };
            return Json(loantype, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getPayment(paymentsearchParamters searchVals)
        {
            DateTime currentdate = DateTime.Now.Date;

            var payment = from p in db.EarningPayments
                          join e in db.Earnings on p.payCode equals e.payCode
                          where p.EmpID == searchVals.recId && p.DATE == currentdate
                          select new {p.paymentNumber,p.ActualAmount,e.payCodeDescription};

            //verifying if the query returned any rows from the table...
            //Insert a breakpoint at this point
            int x = payment.Count(); 

            return Json(payment, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult getattendanceLog(string id)
        {
            var log = from l in db.AttendanceLogs
                      where l.EmpId == id && l.Date == DateTime.Now.ToString("d")
                      select new {l.Date,l.overtimeHours };

            return Json(log, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Audit]
        public JsonResult PrecalculateLoan(loanParams loanparams)
        {

            ComputeloanRepayments();
           
            return Json(true);

        }

        private List<loanportTestCls> example()
        {
            List<loanportTestCls> results = (from lm in db.LoanPortifolios
                                            where lm.LoanRefNumber == "HSELN3512013421"
                                            select new loanportTestCls { EntryID = lm.EntryID, LoanRefNumber = lm.LoanRefNumber, EntryDate = lm.EntryDate,InterestPaid= lm.InterestPaid,PrincipalPaid =lm.PrincipalPaid, LoanBalance = lm.LoanBalance}).ToList();
            return results;
        }

        private void foo(LoanPortifolio record)
        {
            if (ModelState.IsValid)
            {
                db.Entry(record).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Audit]
        public JsonResult checkIfRecordExist(searchParams searchparam) 
        {
            
            searchResults searchresults = new searchResults();

            if (db.Earnings.Any(u => u.payCode == searchparam.id)) 
            {
                searchresults.tblName = "Earnings";
                searchresults.recordExist = true;

                var details = from dt in db.Earnings
                              where dt.payCode == searchparam.id
                              select new {dt.higherPrecedenceFactor,dt.payCodeDescription,dt.actualPayAmount,dt.payRate,dt.rateDerivedSource,dt.groupEligible,dt.payType };

                foreach(var x in details){

                    searchresults.higherPrecedenceFactor = x.higherPrecedenceFactor;
                    searchresults.payCodeDescription = x.payCodeDescription;
                    searchresults.actualPayAmount = x.actualPayAmount;
                    searchresults.payRate = x.payRate;
                    searchresults.rateDerivedFrom = x.rateDerivedSource;
                    searchresults.groupEligible = x.groupEligible;
                    searchresults.payType = x.payType;
                }



                if (searchresults.higherPrecedenceFactor.Trim() == "Payrate")
                {
                   

                        var amount = from e in db.Earnings
                                     where e.payType == searchresults.rateDerivedFrom && e.groupEligible == searchresults.groupEligible
                                     select new { e.actualPayAmount };

                        //place a breakpoint here to verify if the LINQ query returned any rows...
                        int count = amount.Count();

                        //calculate the actual payment Amount
                        foreach (var t in amount)
                        {
                            searchresults.actualPayAmount = t.actualPayAmount * (searchresults.payRate / 100);
                        }


                }
                else 
                {
                    var amount = from e in db.Earnings
                                 where e.payCode == searchparam.id
                                 select new {e.actualPayAmount };

                    foreach(var x in amount)
                    {
                        searchresults.actualPayAmount = x.actualPayAmount;
                    }

                    if (searchparam.id == "SVP")
                    {
                        int __period = PeriodOfService(searchparam.empNumber);
                        SeverancePay severancepay = new SeverancePay(searchresults.actualPayAmount,__period);

                        searchresults.actualPayAmount = severancepay.SeverancePayAmount();
                    }

                }

                //the paymentNumber  is the Primary key field in the Payments Table. Each entry into this column
                //will be formed by combining the gradeName and the count of records in this table plus 1 to form a unique key
                //NOTE: the generation of the unique ID for this column is subject to change

                var result = from p in db.EarningPayments
                             select new { p.paymentNumber };

                int recCount = result.Count() + 1;

                EarningPayments paylog = new EarningPayments
                {
                    paymentNumber = searchparam.gradeName.Trim() + "00" + recCount,
                    payCode = searchparam.id.Trim(),
                    ActualAmount = searchresults.actualPayAmount,
                    EmpID = searchparam.empNumber.Trim(),
                    DATE = DateTime.Now.Date
                };

                //save the payment to database...
                earningpaymentLog(paylog);


            }
            
            else if(db.Deductions.Any(d => d.payCode == searchparam.id))
            {
                searchresults.tblName = "Deductions";
                searchresults.recordExist = true;



                //paymentNumber  is the Primary key field in the Payments Table. Each entry into this column
                //will be formed by combining the gradeName and the count of records in this table plus 1 to form a unique key
                //NOTE: the generation of the unique ID for this column is subject to change

                var result = from p in db.DeductionPayments
                             select new { p.paymentNumber };

                int recCount = result.Count() + 1;

                DeductionPayments paylog = new DeductionPayments
                {
                    paymentNumber = searchparam.gradeName.Trim() + "00" + recCount,
                    payCode = searchparam.id.Trim(),
                    ActualAmount = searchparam.actualPayAmount,
                    EmpID = searchparam.empNumber.Trim(),
                    DATE = DateTime.Now.Date
                };

                //save the payment to database...
                deductionpaymentLog(paylog);
                
            }


            return Json(true);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Audit]
        public JsonResult UpdateSeveranceSettings(SeveranceEarning _severanceRec)
        {
             SeveranceEarning record = new SeveranceEarning();
             record = RetrieveSeveranceRecord();
             _severanceRec.ModifiedBy = (int)Session["LoggedUserId"];
             _severanceRec.DateModified = DateTime.Now.Date;
             _severanceRec.CreatedBy = record.CreatedBy;
             _severanceRec.DateCreated = record.DateCreated;

            try
            {
                using (var context = new DataContext())
                {
                    context.Entry(_severanceRec).State = EntityState.Modified;
                    context.SaveChanges();
                    return Json(new { status = "success", message = "Changes successfully saved" });
                }
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString() + " " + e.InnerException.Message.ToString());
                return Json(new {status ="error", message = "Failed to save changes. " + e.InnerException.Message.ToString() });
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Audit]
        public JsonResult CreateLeaveType( LeaveType record)
        {
            string _message = String.Empty, _status = String.Empty;
            using (var _context = new DataContext())
            {
                try
                {
                    if (record.PaymentCalculationFormular == null) record.PaymentCalculationFormular = "None";
                    PendingTransaction pendTxn = new PendingTransaction(record.LeaveTypeId, (int)Session["LoggedUserId"], "Settings", "Settings", "leaveTypes");
                    record.CreatedBy = (int)Session["LoggedUserId"];
                    record.DateCreated = DateTime.Now.Date;
                    record.RecordStatusId = util.GetRecordStatusId("PENDING");
                    record.RecordStatusChangedBy = (int)Session["LoggedUserId"];
                    record.RecordStatusDateChanged = DateTime.Now.Date;

                    _context.LeaveTypes.Add(record);
                    _context.SaveChanges();

                    pendTxn.LogPendingTransaction();

                    _status = "success";
                    _message = "Leave successfully created";
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " Source: CreateLeaveType() method in Settings Controller");
                    _status = "error";
                    _message = e.Message.ToString();
                }
                return Json(new {status = _status, message = _message });
            }          
        }

        public Boolean earningpaymentLog(EarningPayments payment) 
        {
            if (ModelState.IsValid)
            {

                db.EarningPayments.Add(payment);
                db.SaveChanges();
                return true;

            }
      
            return true;
        }
        
        public Boolean deductionpaymentLog(DeductionPayments payment)
        {
            if (ModelState.IsValid)
            {

                db.DeductionPayments.Add(payment);
                db.SaveChanges();
                return true;

            }

            return true;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult attendancelog(timesheetlog log) 
        {
            var x = from att in db.AttendanceLogs
                    select new { att.logNum};

            int count = x.Count() + 1;

            //DateTime starttime = Convert.ToDateTime(log.Timein);
            //DateTime endtime = Convert.ToDateTime(log.Timeout);
            //TimeSpan duration = endtime - starttime;
            //String timediff = duration.ToString();
            //DateTime timediff2 = Convert.ToDateTime(timediff);

           AttendanceLog logentry = new AttendanceLog
            {
                logNum = "00" + count,
                EmpId = log.EmpID,
                Date = DateTime.Now.ToString("d"),
                overtimeHours = log.overtimeHours

            };

            CreateAttendanceLog(logentry);

            //test the sum function on the time values..

            //DateTime totalhrs = new DateTime();
            //var otime = from att in db.AttendanceLog
            //            where att.EmpId == log.EmpID
            //            select new {att.overtimeHours };
            //foreach(var o in otime)
            //{
            //    totalhrs = totalhrs.AddHours(Convert.ToDateTime(o.overtimeHours).Hour);
            //}
            //DateTime newtime = totalhrs;

            return Json(true);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult loanType(LoanType loantype)
        { 
            if(ModelState.IsValid)
            {
                if (db.LoanTypes.Any(x => x.Code == loantype.Code))
                {
                    updateLoanType(loantype);
                }
                else 
                {
                    createLoanType(loantype);
                }

            }

            return Json(true);
        }

        private Boolean createLoanType(LoanType loantype)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    db.LoanTypes.Add(loantype);
                    db.SaveChanges();
                }
                catch (Exception e)
                {

                    util.WriteToLog(e.Message.ToString());
                }

                return true;
            }
            else 
            {
                return false;
            }

            
        }

        private Boolean updateLoanType(LoanType loantype)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(loantype).State = EntityState.Modified;
                    db.SaveChanges();
                    
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString());
                }

                return true;
            }
            else 
            {
                return false;
            }

           
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Audit]
        public JsonResult LoanDetails(loanMasterRecord loan)
        {

            loanMaster loanmaster = new loanMaster();
            LoanPortifolio portfolio = new LoanPortifolio();
            LoanRegister loanregister = new LoanRegister();
            string __status = string.Empty, __message = string.Empty;

            Double interestrate = GetInterestRate(loan.LoanTypeNumber);

            //<summary>
            //populating the object for loanMaster record
            //</summary>
            loanmaster.LoanRefNumber = loan.LoanRefNumber;
            loanmaster.LoanTypeNumber = loan.LoanTypeNumber;
            loanmaster.Formular = loan.Formular;
            loanmaster.EmpID = loan.EmpID;
             //review this code
            loanmaster.LoanAmount = loan.LoanAmount;
            loanmaster.Status = loan.Status;
            //loanmaster.startDate = DateTime.ParseExact(loan.startDate,"dd/mm/yyyy",System.Globalization.CultureInfo.InvariantCulture);
            loanmaster.startDate = DateTime.Parse(loan.startDate);
            loanmaster.EndDate = DateTime.Parse(loan.EndDate);
            //loanmaster.EndDate = DateTime.ParseExact(loan.EndDate, "dd/mm/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (loan.Formular == "RBL")
            {
                ReducingBalanceComputation rbl = new ReducingBalanceComputation(loan.PaybackPeriods,loan.LoanAmount,interestrate);
                loanmaster.PaybackPeriods = loan.PaybackPeriods;
                loanmaster.MonthlyRepayment = rbl.PMT();
                loanmaster.LoanBalance = rbl.FV(loanmaster.MonthlyRepayment);
            }
            if(loan.Formular == "STL")
            {
                loanmaster.PaybackPeriods = loan.PaybackPeriods;
                StraightLineLoanComputation stl = new StraightLineLoanComputation(loanmaster.PaybackPeriods, loanmaster.LoanAmount, interestrate);
                loanmaster.MonthlyRepayment = stl.MonthlyRepayment();
                loanmaster.LoanBalance = stl.LoanBalance();
            }
            
            loanmaster.MonthlyRepayment = Math.Round(loanmaster.MonthlyRepayment, 2);
            loanmaster.PeriodToUpdate = 1;
            loanmaster.NextDateOfUpdate = DateTime.Now;
            
            loanmaster.RecordStatusId = util.GetRecordStatusId("PENDING");
            loanmaster.RecordStatusChangedBy = (int)Session["LoggedUserId"];
            loanmaster.RecordStatusDateChanged = DateTime.Now.Date;
            loanmaster.CreatedBy = (int)Session["LoggedUserId"];
            loanmaster.DateCreated = DateTime.Now.Date;
            

            //<summary>
            //populating the object for loanPortfolio record
            //</summary>
            portfolio.EntryID = loan.EntryID;
            portfolio.LoanRefNumber = loan.LoanRefNumber;
            portfolio.EntryDate = DateTime.Now.Date;
            portfolio.InterestPaid = 0;
            portfolio.PrincipalPaid = 0;
            portfolio.LoanBalance = 0;
            portfolio.EndOfPeriod = 1;

            //<summary>
            //populating the object for loanRegister record
            //</summary>
            loanregister.LoanRefNumber = loan.LoanRefNumber;
            loanregister.LoanBalance = loan.LoanAmount;
            loanregister.PeriodToUpdate = 1;

            PendingTransaction pendingTxn = new PendingTransaction(loan.LoanRefNumber, (int)Session["LoggedUserId"], "Loans","Loan", "loanMasters");
            
            if (!db.LoanMasters.Any(y => y.LoanRefNumber == loanmaster.LoanRefNumber))
            {
                if (saveLoanMaster(loanmaster))
                {
                    AddLoanPeriod(portfolio, loan.PaybackPeriods);
                    pendingTxn.LogPendingTransaction();
                    __status = "success";
                    __message = "Loan record successfully saved!";
                    //addLoanRegister(loanregister);
                }                             
            }
            else 
            {
                __status = "error";
                __message = "A loan contract with the same details already exist";
                //TODO: the Loan record already exist in the table
                //Update Action /method should be added if applicable. That is subject to policies of the loan contract implemented.
               
            }
            return Json(new {status = __status, message = __message });
        }

        private Double GetInterestRate(string loantypeRef)
        {
            Double interestrate = 0;
            var interestRate = from lt in db.LoanTypes
                               where lt.Code == loantypeRef
                               select new { lt.InterestRate };

            foreach (var rate in interestRate)
            {
                interestrate = rate.InterestRate;
            }
            return interestrate;
        }

        public void ComputeloanRepayments()
        { 
            //TODO:
            //1. retrieve all the loanRefNumbers from the loanMaster table
            //   and use each one of them to check first if entry(ies) exist in the loanAmortization table

            loanReturnValue loanreturnVal = new loanReturnValue(); //object to hold the return values from the initialLoanBalanceRecord method
                                                                   //and LoanBalanceRecord method.
                                                                   //NOTE: -the two methods mentioned above also exist in the LoanRepayment class
                                                                   //        found in the BusinessLogicLayer namespace. The reason these methods have been dupilcated
                                                                   //        is because compile time errors of implicit conversion are occurring. Further
                                                                   //        debugging shall be done later when time avails itself
            
            loanRepayment loanrepayment = new loanRepayment();


            List<loanLookUptbl> lookuptbl = new List<loanLookUptbl>();


            //Create an object to pass as a parameter to the saveLoanPortifolio table 
            LoanPortifolio record = new LoanPortifolio();

           //An instance object of the class LoanRegister
            //PURPOSE: hold the returned values returned from the object which queries  the LoanRegister table

            LoanRegister loanregisterRec = new LoanRegister();


            lookuptbl =  loansearchParam();
            

            foreach (var x in lookuptbl) 
            {

                    //Returns the PaybackPeriod from the given loan Ref Number
                    int PaybackPeriod = PaybackPeriodCount(x.loanrefNumber);
             
                     //Returns the InterestPaid,PrincipalPaid and the LoanBalance
                    loanreturnVal = initialLoanBalanceRecord(x.loanrefNumber);
                    
                    //returns the PeriodToUpdate
                    int period = PeriodToUpdate(x.loanrefNumber);
                    
                     //Returns a record queried from the database which is to be updated later in the code blocks below
                    record = loanPortfolioDetails(x.loanrefNumber, period);

                     //Now assigning values to the properties of  the record before calling the Update method
                    record.EntryDate = DateTime.Now;
                    record.InterestPaid = loanreturnVal.interest;
                    record.PrincipalPaid = loanreturnVal.principalRepayment;
                    record.LoanBalance = loanreturnVal.loanBalanceAmount;
                    record.EndOfPeriod = period;
                    
                     //Calling the update method
                    UpdateLoanPortfolio(record);
                    
  
                if (period < PaybackPeriod)
                {
                    //Increment the PeriodToUpdate which in this context is referred to as Period then update the database
                    period++;

                    //query the databse to retrieve the record first that needs to be updated and assign it to the object instance 'loanregisterRec'
                    loanregisterRec = LoanRegisterDetails(x.loanrefNumber);
                    
                    //update the values of properties of this object instance that needs to be changed before calling the database update method
                    loanregisterRec.LoanBalance = loanreturnVal.loanBalanceAmount;
                    loanregisterRec.PeriodToUpdate = period;

                    //Now posting the details to the database
                    UpdateLoanRegister(loanregisterRec);

                }
                else if(period == PaybackPeriod)
                {
                    //When the period == PaybackPeriods, the assumption is that the Employee has paid back all the interest and principal for the loan
                    //period to Update changes to 0

                    period = 0; 
                    
                    //Update the LoanMaster table
                    loanMaster loanmaster = new loanMaster();

                    //This instance of the LoanMaster class will be populated with data from the LOANDETAILS method to facilitate the update following modification of the status
                    //of the loan
                    loanmaster = loanDetails(x.loanrefNumber);
                    
                    //change the status of the matured loan 
                    loanmaster.Status = "Matured";

                    //Post the update to the database
                    UpdateLoanMaster(loanmaster);
                }

            } //repeat the process until all loans in the object loanlookUptbl have been processed

         }

        private int rowCount(string TableName)
        {
            int count = 0;
            switch(TableName)
            {
                case "LoanPortfolioFirstPart":

                      var rowsfirstpart = from r in db.LoanPortfolioFirstParts
                                select new { r.LoanRefNumber };
                     count = rowsfirstpart.Count() + 1;
                     break;
                case "LoanPortfolioSecondPart":

                    var rowssecpart = from r in db.LoanPortfolioFirstParts
                                select new { r.LoanRefNumber };
                     count = rowssecpart.Count() +1;
                     break;
                case "LoanPortifolio":
                    var rowsportfolio = from r in db.LoanPortifolios
                                select new { r.LoanRefNumber };
                     count = rowsportfolio.Count() +1;
                     break;
            }

            return count + 1;
        
        }

        private List<loanLookUptbl> loansearchParam()
        {
            using (DataContext context = new DataContext())
            {



                List<loanLookUptbl> lookuptbl = (from lm in context.LoanMasters
                                                 where lm.Status == "Active"
                                                 select new loanLookUptbl { loanrefNumber = lm.LoanRefNumber, EndDate = lm.EndDate }).ToList();


                return lookuptbl;
            }


        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult CreateRecordStatusSetting(RecordStatus recordstat)
        {
            string __status = "", __message = ""; 
            using (var context = new DataContext())
            {
                try
                {
                    context.RecordStatuses.Add(recordstat);
                    context.SaveChanges();
                    __status = "success";
                    __message = "record saved successfully";
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " " + e.InnerException.ToString());
                    __status = "error";
                    __message = e.InnerException.Message.ToString();
                }
                return Json(new {status = __status, message = __message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult CreatePensionSettings(Pension pension)
        {
           string __status = "", __message = "";
           using (var context = new DataContext())
           {
               try
               {
                   context.Pensions.Add(pension);
                   context.SaveChanges();
               }
               catch (Exception e)
               {
                   util.WriteToLog(e.Message.ToString() + " " + e.InnerException.ToString());
                   __status = "error";
                   __message = e.Message.ToString();
               }

               return Json(new {status = __status, message = __message});
           }
        }
       
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SaveJobGrade(JobGrade jobgrade)
        {
            jobgrade.CreatedBy = (int)Session["LoggedUserId"];
            jobgrade.DateCreated = DateTime.Now.Date;

            if (db.JobGrades.Any(x => x.GradeId == jobgrade.GradeId))
            {
                UpdateJobGrade(jobgrade);
            }
            else
            {
                CreateJobGrade(jobgrade);
            }

            return Json(true);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SaveJobPosition(JobPosition positionName)
        {
            
            positionName.DateCreated = DateTime.Now.Date;
            positionName.CreatedBy = (int)Session["LoggedUserId"];
            
           
                if (db.JobPositions.Any(x => x.PositionId == positionName.PositionId))
                {
                   
                    UpdateJobPosition(positionName);
                }
                else
                {
                    CreateJobPosition(positionName);
                    
                }
                return Json(true);
                 
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SavePersonnelSettings(PersonnelSettings personnelsettings)
        {
            personnelsettings.CreatedBy = (int)Session["LoggedUserId"];
            personnelsettings.DateCreated = DateTime.Now.Date;
            personnelsettings.AutogenerateEmpNums = personnelsettings.AutogenerateEmpNums;

            if (db.PersonnelSettings.Any(x => x.Id == personnelsettings.Id))
            {
                UpdatePersonnelSettings(personnelsettings);
            }
            else 
            {
                CreatePersonnelSettings(personnelsettings);
            }

            return Json(true);
        }

        [Audit]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AuthorizeOvertime(string Id)
        {
            using (var context = new DataContext())
            {
                string __status = string.Empty, __message = string.Empty;
                try
                {
                    Overtime overtime = context.OverTimes.Find(Id);
                    PendingTransaction pendingTxn = new PendingTransaction(Id, overtime.CreatedBy, "Overtime","Settings", "Overtimes");
                    overtime.RecordStatusId = util.GetRecordStatusId("AUTHORIZED");
                    overtime.RecordStatusChangedBy = (int)Session["LoggedUserId"];
                    overtime.RecordStatusDateChanged = DateTime.Now.Date;

                    context.Entry(overtime).State = EntityState.Modified;
                    context.SaveChanges();
                    pendingTxn.ClearPendingTransaction();

                    __status = "success";
                    __message = "Record successfully authorized";
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.InnerException.Message.ToString() + "" + e.InnerException.Source.ToString());
                    __status = "error";
                    __message = e.InnerException.Message.ToString();
                }

                return Json(new { status = __status, message = __message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult CreateOvertimePaymentSetting(Overtime otime)
        {
            string __status = string.Empty, __message = string.Empty;

            otime.CreatedBy = (int)Session["LoggedUserId"];
            otime.DateCreated = DateTime.Now.Date;
            otime.RecordStatusId = util.GetRecordStatusId("PENDING");

            PendingTransaction pendingTxn = new PendingTransaction(otime.Code, otime.CreatedBy, "Overtime","Settings", "Overtimes");

            using (var context = new DataContext())
            {
                try
                {
                    context.OverTimes.Add(otime);
                    context.SaveChanges();
                    pendingTxn.LogPendingTransaction();
                    __message = "Record saved successfully";
                    __status = "success";
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.InnerException.Message.ToString() + " " + e.InnerException.Source.ToString());
                    __message = e.InnerException.Message.ToString();
                    __status = "error";
                }

                return Json(new { status = __status, message = __message });
            }
            
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult loanSettings(LoanSettings loansetting)
        {
            if (ModelState.IsValid) 
            {
                if (db.LoanSettings.Any(x => x.ID == loansetting.ID))
                {

                    updateLoanSettings(loansetting);
                }
                else 
                {
                    saveLoanSettings(loansetting);
                    
                }
            }

            return Json(true);
           
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SavePayrollSettings(PayrollFormulae record)
        {
            string status = null;
            string message = null;
            payrollFormularRecord payrollformulae = new payrollFormularRecord();
            List<payrollFormularRecord> formular = getPayrollFormular(record.Formular);

            using (var context = new DataContext())
            {
                foreach (var x in formular)
                {
                    record.Id = x.Id;
                    record.Formular = x.Formular;
                    record.UpdatedBy = (int)Session["LoggedUserId"];
                    record.DateUpdated = DateTime.Now.Date;
                }

                context.Entry(record).State = EntityState.Modified;
                context.SaveChanges();
                status = "success";
                message = "changes saved successfully";

                return Json(new { status = status, message = message });
            }
        }

        private loanMaster loanDetails(string refNumber)
        {
            loanMaster lmaster = new loanMaster();

            try
            {
                var loandetail = from lm in db.LoanMasters
                                 where lm.LoanRefNumber == refNumber
                                 select new { lm.LoanRefNumber, lm.LoanTypeNumber, lm.EmpID, lm.startDate, lm.EndDate, lm.PaybackPeriods, lm.LoanAmount, lm.MonthlyRepayment, lm.Status };
                foreach (var x in loandetail)
                {
                    lmaster.LoanRefNumber = x.LoanRefNumber;
                    lmaster.LoanTypeNumber = x.LoanTypeNumber;
                    lmaster.EmpID = x.EmpID;
                    lmaster.startDate = x.startDate;
                    lmaster.EndDate = x.EndDate;
                    lmaster.PaybackPeriods = x.PaybackPeriods;
                    lmaster.LoanAmount = x.LoanAmount;
                    lmaster.MonthlyRepayment = x.MonthlyRepayment;
                    lmaster.Status = x.Status;
                }

            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString());
            }

            return lmaster;
        }

        private LoanPortifolio loanPortfolioDetails(string refNumber,int EndOfPeriod)
        {
            LoanPortifolio portfolio = new LoanPortifolio();

            var results = from lp in db.LoanPortifolios
                          where lp.LoanRefNumber == refNumber && lp.EndOfPeriod == EndOfPeriod
                          select new {lp.LoanRefNumber,lp.EntryID,lp.EntryDate,lp.InterestPaid,lp.PrincipalPaid,lp.LoanBalance,lp.EndOfPeriod};
            foreach(var x in results)
            {
                portfolio.EntryID = x.EntryID;
                portfolio.LoanRefNumber = x.LoanRefNumber;
                portfolio.EntryDate = x.EntryDate;
                portfolio.InterestPaid = x.InterestPaid;
                portfolio.PrincipalPaid = x.PrincipalPaid;
                portfolio.LoanBalance = x.LoanBalance;
                portfolio.EndOfPeriod = x.EndOfPeriod;
            }

            return portfolio;
        }

        private LoanRegister LoanRegisterDetails(string refNumber)
        {
            LoanRegister loanreg = new LoanRegister();
            var results = from lrg in db.LoanRegisters
                          where lrg.LoanRefNumber == refNumber
                          select new {lrg.LoanRefNumber,lrg.LoanBalance,lrg.PeriodToUpdate };
            foreach(var x in results)
            {
                loanreg.LoanRefNumber = x.LoanRefNumber;
                loanreg.LoanBalance = x.LoanBalance;
                loanreg.PeriodToUpdate = x.PeriodToUpdate;
            }

            return loanreg;
        }

        public Boolean saveLoanMaster(loanMaster loan)
        {
            Boolean saved;
                using (var context = new DataContext())
                {
                    try
                    {
                        db.LoanMasters.Add(loan);
                        db.SaveChanges();
                        saved = true;
                    }
                    catch (Exception e)
                    {
                        util.WriteToLog(e.Message.ToString() + "" + e.InnerException.Source.ToString());
                        saved = false;
                    }
                    return saved;
                }
        }

        public Boolean saveLoanSettings(LoanSettings settings) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.LoanSettings.Add(settings);
                    db.SaveChanges();
                }
                catch (Exception e)
                {

                    util.WriteToLog(e.Message.ToString());
                }

                return true;
            }
            else
            {
                return false;
            }

            
        }

        public Boolean updateLoanSettings(LoanSettings settings)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(settings).State = EntityState.Modified;
                    db.SaveChanges();
                    
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString());
                }

                return true;
            }
            else
            {
                return false;
            }

            
        }

        public Boolean saveLoanAmortizationEntry(LoanAmortization loanamortization)
        {
            if (ModelState.IsValid) 
            {
                db.LoanAmortizations.Add(loanamortization);
                db.SaveChanges();
                return true;
            }

            return true;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [Audit]
        public JsonResult saveloanPortifolio(LoanPortifolio loanportfolio)
        {
            
            loanportfolio.EntryDate = DateTime.Now.Date;

            try
            {
                db.LoanPortifolios.Add(loanportfolio);
                db.SaveChanges();
                
           }
            catch (Exception e)
           {

                util.WriteToLog(e.Message.ToString());
           }

            return Json(true);


        }

        private void UpdateLoanPortfolio(LoanPortifolio portfolio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(portfolio).State = EntityState.Modified;
                    db.SaveChanges();

                }
                catch (Exception e)
                {

                    util.WriteToLog(e.Message.ToString());
                }

            }
        }

        private void UpdateLoanMaster(loanMaster loanmaster)
        { 
            if(ModelState.IsValid)
            {

                try
                {
                    db.Entry(loanmaster).State = EntityState.Modified;
                    db.SaveChanges();

                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString());
                }
            
            }
        }

        private void UpdateLoanRegister(LoanRegister loanregisterRecord)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(loanregisterRecord).State = EntityState.Modified;
                    db.SaveChanges();

                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString());
                    
                }

            }
        }

        [AcceptVerbs(HttpVerbs.Post)] 
        [Audit]
        public JsonResult addLoanRegister(LoanRegister loanregister)
        {
            loanregister.NextDateOfUpdate = DateTime.Now;
            loanregister.LoanRefNumber = loanregister.LoanRefNumber.Trim();

            if (ModelState.IsValid)
            {
                db.LoanRegisters.Add(loanregister);
                db.SaveChanges();
                return Json(true);
            }
            return Json(true);
        }
        
        private ActionResult Create(LoanPortifolio loanportfolio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.LoanPortifolios.Any(x => x.EntryID == loanportfolio.EntryID))
                    {
                        return Json(false);
                    }
                    else
                    {
                        db.LoanPortifolios.Add(loanportfolio);
                        db.SaveChanges();
                        return Json(true);

                    }
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString());
                }


            }
            return Json(true);
        }

        private SeveranceEarning RetrieveSeveranceRecord()
        {
            SeveranceEarning t = new SeveranceEarning();
            using (var context = new DataContext())
            {
                var results = from svp in context.SeveranceEarnings
                              select new { svp.CreatedBy, svp.DateCreated };
                foreach (var r in results)
                {
                    t.DateCreated = r.DateCreated;
                    t.CreatedBy = r.CreatedBy;
                }
            }

            return t;
        }

        private string ToString(int p)
        {
            throw new NotImplementedException();
        } 

        public Boolean CreateAttendanceLog(AttendanceLog attlog)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.AttendanceLogs.Add(attlog);
                    db.SaveChanges();
                }
                catch (Exception e)
                {

                    util.WriteToLog(e.Message.ToString());
                }

                return true;
            }
            else 
            {
                return false;
            }
        }

        public double PMT(double yearlyInt, int numbrOfmnths, double loanAmount)
        {
            var rate = (double)yearlyInt / 1200;
            var denominator = Math.Pow((1 + rate), numbrOfmnths) - 1;
            return (rate + (rate / denominator)) * loanAmount;
        }

        private loanReturnValue loanBalance(double loanAmount, double interestRate, double pmt) 
        {

            loanReturnValue loanreturnvals = new loanReturnValue();
            loanreturnvals.interest  = (interestRate / 100) * loanAmount;
            loanreturnvals.principalRepayment = pmt - loanreturnvals.interest;
            loanreturnvals.loanBalanceAmount = loanAmount - loanreturnvals.principalRepayment;
            return loanreturnvals;
        }

        public loanReturnValue initialLoanBalanceRecord(string refNumber)
        {
            loanReturnValue loanrepaymentvals = new loanReturnValue();
            try
            {
                var resultset = from lmaster in db.LoanMasters
                                join ltype in db.LoanTypes on lmaster.LoanTypeNumber equals ltype.Code
                                join lrg in db.LoanRegisters on lmaster.LoanRefNumber equals lrg.LoanRefNumber
                                where lmaster.LoanRefNumber == refNumber
                                select new { lrg.LoanBalance, ltype.InterestRate, lmaster.MonthlyRepayment };

                foreach (var resultSet in resultset)
                {
                    loanrepaymentvals = loanBalance(resultSet.LoanBalance, resultSet.InterestRate, resultSet.MonthlyRepayment);
                }

                loanrepaymentvals.loanBalanceAmount = Math.Round(loanrepaymentvals.loanBalanceAmount, 2);
                loanrepaymentvals.principalRepayment = Math.Round(loanrepaymentvals.principalRepayment, 2);
                loanrepaymentvals.interest = Math.Round(loanrepaymentvals.interest, 2);
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString());
            }

            return loanrepaymentvals;
        }

        public loanReturnValue LoanBalanceRecord(string refNumber, DateTime endDate)
        {
            loanReturnValue loanrepaymentvals = new loanReturnValue();
            try
            {
                var result = from l in db.LoanAmortizations
                             join ltype in db.LoanTypes on l.LoanRefNumber equals ltype.Code
                             join lm in db.LoanMasters on l.LoanRefNumber equals lm.LoanRefNumber
                             where l.LoanRefNumber == refNumber && lm.EndDate <= endDate
                             select new { l.LoanBalance, lm.MonthlyRepayment, ltype.InterestRate };

                foreach (var x in result)
                {
                    loanrepaymentvals = loanBalance(x.LoanBalance, x.InterestRate, x.MonthlyRepayment);

                }
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString());
            }

            return loanrepaymentvals;
        }

        private void AddLoanPeriod(LoanPortifolio portfolio, int periodCount) 
        {
            periodCount++;
            for (int x = 1; x <= periodCount; x ++ )
            {
                portfolio.EntryID = portfolio.EntryID + DateTime.Now.Millisecond.ToString();
                portfolio.EndOfPeriod = x;
                saveloanPortifolio(portfolio);
            }
        }

        private void UpdateJobPosition(JobPosition jobposition)
        {
            try 
            {
                db.Entry(jobposition).State = EntityState.Modified;
                db.SaveChanges();
                
            }
            catch(Exception e)
            {
                util.WriteToLog(e.Message.ToString());
            }

            
        }

        private void UpdateJobGrade(JobGrade jobgrade)
        {
            try
            {
                db.Entry(jobgrade).State = EntityState.Modified;
                db.SaveChanges();

            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString());
            }


        }

        private void CreateJobPosition(JobPosition position)
        { 

             try
              {
                 db.JobPositions.Add(position);
                 db.SaveChanges();
              }
              catch (Exception e)
              {
                 util.WriteToLog(e.Message.ToString());
              }

                      
        }

        private void CreateJobGrade(JobGrade jobgrade)
        {

            try
            {
                db.JobGrades.Add(jobgrade);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString());
            }

        }

        private void CreatePersonnelSettings(PersonnelSettings personnelsettings)
        {
            try
            {
                db.PersonnelSettings.Add(personnelsettings);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString());
            }

        }

        private void UpdatePersonnelSettings(PersonnelSettings personnelsettings)
        {
            try
            {
                db.Entry(personnelsettings).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString());
            }
        }

        private int PaybackPeriodCount(string refNumber)
        {
            int periodCount = 0;
            try
            {
                var count = from lm in db.LoanMasters
                            where lm.LoanRefNumber == refNumber
                            select new { lm.PaybackPeriods };
                foreach (var p in count)
                {
                    periodCount = p.PaybackPeriods;
                }
            }
            catch (Exception e)
            {

                util.WriteToLog(e.Message.ToString());
            }
      

            return periodCount;
        }

        private int PeriodToUpdate(string refNumber)
        {

            int periodtoUpdate = 0;
            try
            {
                var count = from lrg in db.LoanRegisters
                            where lrg.LoanRefNumber == refNumber
                            select new { lrg.PeriodToUpdate };
                foreach (var p in count)
                {
                    periodtoUpdate = p.PeriodToUpdate;
                }
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString());
            }
           

            return periodtoUpdate;
        }

        private int PeriodOfService(string EmpId)
        {
            DateTime dateOfServiceStart;
            int years = 0;
            try
            {
                var period = from e in db.Employees
                             where e.EmpID == EmpId
                             select new { e.HireDate };
                foreach (var y in period)
                {
                    dateOfServiceStart = (DateTime)y.HireDate;
                    years = DateTime.Now.Year - dateOfServiceStart.Year;
                }
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString() + " " + e.InnerException.Message.ToString());
            }

            return years;
        }

        private List<payrollFormularRecord> getPayrollFormular(string __formular)
        {
            using (var context = new DataContext())
            {
                List<payrollFormularRecord> result = (from pf in context.PayrollFormulae
                                                      where pf.Formular == __formular
                                                      select new payrollFormularRecord
                                                      {
                                                          Id = pf.Id,
                                                          Formular = pf.Formular,
                                                          //Paycode = pf.Paycode,
                                                          //UpdatedBy = pf.UpdatedBy,
                                                          //DateUpdated = pf.DateUpdated
                                                      }).ToList();
                return result;
            }
        }

        public ViewResult Index()
        {
            return View();
            //return View(true);
        }

        private static void LoanAmortization()
        {
           //Loan loan = new Loan();
            
        }

        [HttpPost]
        public ActionResult Create(AppConfig_LeavePay setting)
        {
            if (ModelState.IsValid)
            {
                db.AppConfig_LeavePay.Add(setting);
                db.SaveChanges();
                return Json(true);
            }

            return Json(false);
        }
        
        //
        // GET: /Settings/Edit/5
        
 
        public ActionResult Edit(string id)
        {
            Settings setting = db.Settings.Find(id);
            return View(setting);
        }

        //
        // POST: /Settings/Edit/5

        [HttpPost]
        public ActionResult Edit(Settings settings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(settings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(settings);
        }

        //
        // GET: /Settings/Delete/5
 
        public ActionResult Delete(string id)
        {
            Settings settings = db.Settings.Find(id);
            return View(settings);
        }

        //
        // POST: /Settings/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {            
            Settings settings = db.Settings.Find(id);
            db.Settings.Remove(settings);
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