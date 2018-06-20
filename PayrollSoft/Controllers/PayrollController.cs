using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using PayrollSoft.Models;
using PayrollSoft.BusinessLogicLayer;
using PayrollSoft.AuditTrails;
using PayrollSoft.Utility;




namespace PayrollSoft.Controllers
{
    
    public class PayrollList
    {

        public PayrollList() { }

        public string EmpName { get; set;}
        public string EmpId { get; set; }
        public string GradeId { get; set; }
        public string salPayCode { get; set; }
        private string overtimePayCode { get; set; }
        public Double MinSal{ get; set; }
        public Double HoursWkd { get; set; }
        public Double overTimeRate { get; set; }
        public Double insuranceBill { get; set; }
        public Double salaryAdvanceDeduct { get; set; }
        public int normalWorkingHrs { get; set; }
        public Double allowancePay { get; set; }
        public Double PAYERate {get; set;}
        public rateDefs PAYEDEFS = new rateDefs();
        public pensionRate PENSIONRATES = new pensionRate();
        private Double result1, result2, result3, ROTH1, ROTH2, ROTH3, ROTH4;
        private Double THL1, THL2, THL3, THL4;
        private Double THAmount1, THAmount2, THAmount3, THAmount;
        private Double grossPay;
        public Double totalDeduction;
        public Double totalEarning;
        private Double pensionAmount;


        private DataContext db = new DataContext();

        public Double BasicPay()
        {
            
            using (var context = new DataContext())
            {
                
                IEnumerable<PayrollList> MinSalary = context.EarningPayments.Where(g => g.payCode == salPayCode && g.EmpID == EmpId).Select(s => new PayrollList {MinSal = s.ActualAmount });

                foreach (var x in MinSalary)
                {
                    MinSal = x.MinSal;
                }

                return MinSal;
            }
        }

        public Double Earnings()
        {
            var totalearnings = from ern in db.EarningPayments
                                where ern.EmpID == EmpId
                                group ern by ern.EmpID into earningGroup
                                select new { totalEarning = earningGroup.Sum(s => s.ActualAmount) };
            foreach (var totalearning in totalearnings)
            {
                totalEarning = totalearning.totalEarning;
            }

            return totalEarning;
        }

        public Double Deductions()
        {
            var deductionsTotal = from d in db.DeductionPayments
                                  where d.EmpID == EmpId
                                  group d by d.EmpID into deductionGroup
                                  select new { totalDeduction = deductionGroup.Sum(x => x.ActualAmount) };
            foreach (var deductiontotal in deductionsTotal)
            {
                totalDeduction = deductiontotal.totalDeduction;
            }

            return totalDeduction;
        }

        public Double Pension() 
        {
            Double employeeContrib = BasicPay() * (Double)PENSIONRATES.employeesContribution / 100;
            Double employerContrib = BasicPay() * (Double)PENSIONRATES.employersContribution / 100;

            return pensionAmount = employeeContrib + employerContrib;
        }

        public Double overtimeRt()
        { 
           IEnumerable<searchparam> paycode = db.AttendanceLogs.Where(att => att.EmpId == EmpId).Select(y => new searchparam { payCode = y.payCode }).Distinct();
          foreach(var p in paycode)
          {
              overtimePayCode = p.payCode;
          }
          IEnumerable<PayrollList> overtimeRate = db.Earnings.Where(x => x.payCode == overtimePayCode).Select(y => new PayrollList {overTimeRate = y.payRate}).Distinct();
          foreach(var r in overtimeRate)
          {
              overTimeRate = r.overTimeRate;
          }

          return overTimeRate;
        }

         public Double OvertimePay()
         {
             double overtimepayment;
             return overtimepayment = ( MinSal * overTimeRate / 100) * HoursWkd;
         }

         public Double Allowance 
         {
             get { return (allowancePay / 100) * MinSal; }
         }

         public Double GrossPay() 
         {

             double dblgrossPay;

             //return dblgrossPay = totalEarning + OvertimePay() + MinSal;
             return dblgrossPay = totalEarning + OvertimePay();
         }

         public Double PAYE() 
         {
            THL1 = PAYEDEFS.firstThresholdLimit;
            THL2 = PAYEDEFS.secondThresholdLimit;
            THL3 = PAYEDEFS.thirdThresholdLimit;
            THL4 = PAYEDEFS.fourthThresholdLimit;
            ROTH1 = PAYEDEFS.rateOffirstThresholdLimit;
            ROTH2 = PAYEDEFS.rateOfsecondThresholdLimit;
            ROTH3 = PAYEDEFS.rateOfthirdThresholdLimit;
            ROTH4 = PAYEDEFS.rateOffourthThreshold;
           
            grossPay = GrossPay();

             if (grossPay < (Double)THL1){

                 THAmount1 = 0.0;
             }
             if(grossPay >= (Double)THL1){

                 THAmount1 = (Double)THL1 * (ROTH1 /100);
                 result1 = grossPay - (Double)THL1;
             }
             if(result1 >= (Double)THL2){

                 THAmount2 = (Double)THL2 * (ROTH2 /100);
                 result2 = result1 - (Double)THL2;
             }
             if(result2 > (Double)THL2) {

                 THAmount3 = result2 * (ROTH3 /100);
             }

            return THAmount1 + THAmount2 + THAmount3;
 
         }

         public Double NetPay()
         {
             return GrossPay() - PAYE() - totalDeduction - Pension();

         }
    
    }

    public class PaymentRecord
    {
        public string paymentNumber { get; set; }
        public string payCode { get; set; }
        public string groupEligible { get; set; }
        public double actualPayAmount { get; set; }
        public string EmpId { get; set; }
        public string Date { get; set; }
    }

    public class rateDefs 
    {
        public Double firstThresholdLimit { get; set; }
        public Double secondThresholdLimit { get; set; }
        public Double thirdThresholdLimit { get; set; }
        public Double fourthThresholdLimit { get; set; }
        public Double rateOffirstThresholdLimit { get; set; }
        public Double rateOfsecondThresholdLimit { get; set; }
        public Double rateOfthirdThresholdLimit { get; set; }
        public Double rateOffourthThreshold { get; set; }
    }

    public class searchparam 
    {
        public string id { get; set; }
        public int recId { get; set; }
        public string payCode { get; set; } //this field will be specifically used to keep the paycode for Overtime calculations, to simplify data fetching
        public string salpayCode { get; set; } //this field will be specifically used to keep the paycode for SALARY , to simplify data fetching
        public string Date { get; set; }
        public string groupEligible { get; set; }
        public double actualPayAmount { get; set; } //used to hold the basic salary for the employee currently because the lambda expression to specifically fetch
                                                    //the basic salary is not executing. Later after we figure out the work around we will use a specifi code for this action
                                                    //DATE: 23-Jun-2016
                                                    //DATE UPDATED: 
    }

    public class paymentCode
    {
        public string paycode { get; set; }
        public paymentCode(string pcode) 
        {
            this.paycode = pcode;
        }
    }

    public class employeeSal
    {
        public string EmpName { get; set; }
        public string EmpID { get; set; }
        public string groupEligible { get; set; }
        public double MinSalary { get; set; }
    
    }

    public class PayrollPaymentDetails
    {
        public string id { get; set; }
        public string gradeName { get; set; }
        public string empNumber { get; set; }
        public Double actualPayAmount { get; set; }
        public DateTime payrollRunDate { get; set; }
        public string PayrollCode { get; set; }
        public double hoursWorked { get; set; }
    }

    public class PayrollPayment
    {
        public int LogId { get; set; }
        public string paymentNumber { get; set; }
        public string EmpId { get; set; }
        public string payCode { get; set; }
        public double ActualAmount { get; set; }
        public string Date { get; set; }
    }

    public class PayrollAccountEntry
    {
        public double Amount { get; set; }
        public string SourceAccount { get; set; }
        public string ContraAccount { get; set; }
        public string TransactionCode { get; set; }
    }

    public class PayrollController : Controller
    {
        private DataContext db = new DataContext();

        private UtilityBase util = new UtilityBase();

        private Double PayeRate(Double minSalary) 
        {
            Double payeRate = 0;
          
            //    var paye = from tx in db.TaxationThreholds
            //               where tx.MinimumAmount <= minSalary
            //               select new { tx.MinimumAmount};

            //    int x = paye.Count();
            //    if (x > 1) 
            //    {
            //        //var payee = from tx in db.TaxationThreholds
            //        //           where tx.MinimumAmount = (from t in db.TaxationThreholds
            //        //                                     select new { t.MaximumAmount = MAX(t.MaximumAmount)})
            //        //           select new { tx.MinimumAmount };
            //    }
            //    else
            //    {
            //        //payeRate = (Double)minAmount.MinimumAmount;
            //    }

            //    foreach (var minAmount in paye)
            //    {
            //       // payeRate = (Double)minAmount.MinimumAmount;
            //    }
            

            return payeRate;
        }

        public void SavePayrollLogEntry(PayrollHistLog PayrollHistLog)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.PayrollHistLogs.Add(PayrollHistLog);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString());
                }

            }

        }

        public Boolean FuncLogEntry(PayrollHistLog PayHistLog)
        {
            Boolean recordsaved = false;
            if (ModelState.IsValid)
            {

                try
                {
                    db.PayrollHistLogs.Add(PayHistLog);
                    db.SaveChanges();
                    recordsaved = true;
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " Exception: " + e.InnerException.ToString());
                    recordsaved = false;
                }

              
            }


            return recordsaved;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SetSession(employeeSal emp)
        {
            Session["CurrentUserEmpId"] = emp.EmpID.Trim();

            return Json(true);
        }

        private object MAX(double p)
        {
            throw new NotImplementedException();
        }

        public ViewResult Index()
        {
            ViewBag.Message = Session["LoggedUserFullName"];
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult PayrollList() 
        {

            var emp = from e in db.Employees
                      join d in db.Departments on e.DeptID equals d.DeptID
                      where e.onPayroll == 1 && e.EmploymentStatus == "Active"
                      select new {e.EmpID,e.EmpName,e.GradeID,d.DeptName,e.JobTitle};
        
            return Json(emp ,JsonRequestBehavior.AllowGet);      
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetEmployeeOnPayrollList(string prefix)
        {

            var emp = from e in db.Employees
                      join d in db.Departments on e.DeptID equals d.DeptID
                      where e.EmpName.StartsWith(prefix)  //e.onPayroll == 1 && e.EmploymentStatus == "Active" && 
                      select new { e.EmpID, e.EmpName, e.GradeID, d.DeptName, e.JobTitle };

            return Json(emp, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult PayrollListRecordCount()
        {
            DateTime currentDate = DateTime.Now.Date;

            var emp = from e in db.PayrollHistLogs
                      where e.dateStamp == currentDate
                      select new { e.LogID,e.EmpID};

            return Json(emp, JsonRequestBehavior.AllowGet);

        }

        private Boolean writetoFile(string datatowrite)
        {
            var datafile = Server.MapPath("~/Scripts/jqwidgets/datagrid_datasource.js");

            using (var tw = new StreamWriter(datafile))
            {
                tw.WriteLine(datatowrite);
                tw.Close();
            }

            return true;
        
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetPayrollPayments(string empId)
        {

            //string EmpId = Session["CurrentUserEmpId"].ToString();
            DateTime __date = DateTime.Now.Date;
            if (empId != null)
            {

                var payments = from gpp in db.GroupPayrollPayments
                               where gpp.EmpId == empId && gpp.DATE == __date
                               select new { gpp.payCode, gpp.ActualAmount, gpp.paymentNumber,gpp.LogId};

                int i = payments.Count();

                return Json(payments, JsonRequestBehavior.AllowGet);
            }
            else 
            {
                return Json(false);
            }
        }

        [Audit]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletePayment(GroupPayrollPayment payment)
        {

            EarningPayments earnpayment = new EarningPayments();
            DeductionPayments deductpayment = new DeductionPayments();

            //payment.Date = DateTime.Now;
            //payment.EmpId = Session["CurrentUserEmpId"].ToString().Trim();

               using (var context = new DataContext())
               {
                   context.Entry(payment).State = EntityState.Deleted;
                   context.SaveChanges();
               }

               if (db.EarningPayments.Any(x => x.payCode == payment.payCode))
               {
                   earnpayment.ActualAmount = payment.ActualAmount;
                   earnpayment.payCode = payment.payCode;
                   earnpayment.paymentNumber = payment.paymentNumber;
                   earnpayment.EmpID = payment.EmpId;
                   earnpayment.DATE = DateTime.Now.Date;

                   DeleteEarningPayment(earnpayment,"Delete");
               }
               else
               {
                   deductpayment.ActualAmount = payment.ActualAmount;
                   deductpayment.payCode = payment.payCode;
                   deductpayment.paymentNumber = payment.paymentNumber;
                   deductpayment.EmpID = payment.EmpId;
                   deductpayment.DATE = DateTime.Now.Date;

                   DeleteDeductionPayment(deductpayment,"Delete");
               }


                return Json(true);
            
        }

        private Boolean DeleteGroupPayrollPayment(GroupPayrollPayment payment)
        {
            Boolean deletesuccessful = false;
            using(DataContext context = new DataContext())
            {
                try
                {
                    GroupPayrollPayment grouppayrollpayment = context.GroupPayrollPayments.Find(payment.payCode);
                    context.GroupPayrollPayments.Remove(grouppayrollpayment);
                    context.SaveChanges();
                    deletesuccessful = true;
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString());
                    
                }
                return deletesuccessful;
            }
        }

        private Boolean DeleteEarningPayment(EarningPayments payment, string action)
        {
            Boolean deletesuccessful = false;
            using (var context = new DataContext())
            {
                try
                {
                    if (action == "Delete")
                    {
                        context.Entry(payment).State = EntityState.Deleted;
                        context.SaveChanges();
                        deletesuccessful = true;
                    }
                    else
                    {
                        context.Entry(payment).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString());
                }


                return deletesuccessful;
            }

        }

        private Boolean DeleteDeductionPayment(DeductionPayments payment, string action)
        {
            Boolean deletesuccessful = false;
            using (var context = new DataContext())
            {
                try
                {
                    if (action == "Delete")
                    {
                        context.Entry(payment).State = EntityState.Deleted;
                        context.SaveChanges();
                        deletesuccessful = true;
                    }
                    else
                    {
                        context.Entry(payment).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString());
                }


                return deletesuccessful;
            }

        }
     
        public void InitializeGroupPayrollPaymentData(string empId)
        {
            GroupPayrollPayment payrollEarnPayment = new GroupPayrollPayment();
            GroupPayrollPayment payrollpayment = new GroupPayrollPayment();
            GroupPayrollPayment payment = new GroupPayrollPayment();


            try
            {
                var payrollEarning = employeeEarnings(empId);

                if (payrollEarning.Count() != 0)
                {
                    foreach (var x in payrollEarning)
                    {
                        payrollEarnPayment.LogId = 1;
                        payrollEarnPayment.paymentNumber = x.paymentNumber;
                        payrollEarnPayment.payCode = x.payCode;
                        payrollEarnPayment.EmpId = empId;
                        payrollEarnPayment.DATE = DateTime.Now;
                        payrollEarnPayment.ActualAmount = x.ActualAmount;
                    }

                    ///<summary>
                    /// verify that a similar payment type has not been added already to the grouppayrollpayment table
                    /// this code gaurds against duplicating of payments when adding paymnts to the employee account.
                    /// </summary>

                    var grouppayment = verifyIfPaymentExist(payrollEarnPayment.EmpId, payrollEarnPayment.payCode);

                    if (grouppayment.Count() == 0)
                    {
                        LogGroupPayrollPayment(payrollEarnPayment);
                    }
                }


                var payrollDeduction = employeeDeductions(empId);

                if (payrollDeduction.Count() != 0)
                {
                    foreach (var x in payrollDeduction)
                    {
                        payrollpayment.LogId = 1;
                        payrollpayment.paymentNumber = x.paymentNumber;
                        payrollpayment.payCode = x.payCode;
                        payrollpayment.EmpId = empId;
                        payrollpayment.DATE = DateTime.Now;
                        payrollpayment.ActualAmount = x.ActualAmount;
                    }

                    ///<summary>
                    /// verify that a similar payment type has not been added already to the grouppayrollpayment table
                    /// this code gaurds against duplicating of payments when adding paymnts to the employee account.
                    /// </summary>

                    var grouppayment = verifyIfPaymentExist(payrollpayment.EmpId, payrollpayment.payCode);

                    if(grouppayment.Count() == 0)
                    {
                        LogGroupPayrollPayment(payrollpayment);
                    }
                    
                }
            
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString());
            }
           
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult EmpList() 
        {
            var list = from e in db.Employees
                       join d in db.Payrolls on e.EmpID equals d.EmpID
                       select new { e.EmpID };

            return Json(list, JsonRequestBehavior.AllowGet);
 
        }

        [Audit]
        public JsonResult CreatePayment(PayrollPaymentDetails payment)
        {
            string Message = null;
            string Status = null;
            searchResults searchresults = new searchResults();
            JournalAccount journalentry = new JournalAccount();
            JournalAccount contrajournalentry = new JournalAccount();
            PayrollAccountEntry accountentry = new PayrollAccountEntry();
            Payroll payroll = new Payroll();

            if (db.Earnings.Any(u => u.payCode == payment.id))
            {
                searchresults.tblName = "Earnings";
                searchresults.recordExist = true;

                var details = from dt in db.Earnings
                              where dt.payCode == payment.id
                              select new { dt.higherPrecedenceFactor, dt.payCodeDescription, dt.actualPayAmount, dt.payRate, dt.rateDerivedSource, dt.groupEligible, dt.payType };

                foreach (var x in details)
                {

                    searchresults.higherPrecedenceFactor = x.higherPrecedenceFactor;
                    searchresults.payCodeDescription = x.payCodeDescription;
                    searchresults.actualPayAmount = x.actualPayAmount;
                    searchresults.payRate = x.payRate;
                    searchresults.rateDerivedFrom = x.rateDerivedSource;
                    searchresults.groupEligible = x.groupEligible;
                    searchresults.payType = x.payType;
                }



                if (searchresults.higherPrecedenceFactor.Trim() == "Pay rate")
                {
                    if (searchresults.groupEligible.Trim() == "General")
                    {
                        searchresults.groupEligible = payment.gradeName;
                    }

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
                else if (searchresults.higherPrecedenceFactor.Trim() == "FORMULAR")
                {
                    if (payment.id.Trim() == "SVP")
                    {
                        EmployeeSalary empSal = new EmployeeSalary(payment.empNumber,payment.gradeName);
                        double __basicSalary = empSal.salary();
                        int __period = PeriodOfService(payment.empNumber);
                      
                        SeverancePay severancepay = new SeverancePay(__basicSalary, __period);

                        searchresults.actualPayAmount = severancepay.SeverancePayAmount();
                    }

                    if (payment.id.Trim() == "OVT1")
                    {
                        OverTimePayment otimepayment = new OverTimePayment(payment.id, payment.empNumber, payment.gradeName, payment.hoursWorked);
                        searchresults.actualPayAmount = otimepayment.OverTimePay();
                    }

                    if (payment.id.Trim() == "OVT2")
                    {
                        OverTimePayment otimepayment = new OverTimePayment(payment.id, payment.empNumber, payment.gradeName, payment.hoursWorked);
                        searchresults.actualPayAmount = otimepayment.OverTimePay();
                    }
                }
                else
                {
                    var amount = from e in db.Earnings
                                 where e.payCode == payment.id
                                 select new { e.actualPayAmount };

                    foreach (var x in amount)
                    {
                        searchresults.actualPayAmount = x.actualPayAmount;
                    }

                }

                ///<summary>
                ///the paymentNumber  is the Primary key field in the Payments Table. Each entry into this column
                ///will be formed by combining the gradeName and the count of records in this table plus 1 to form a unique key
                ///NOTE: the generation of the unique ID for this column is subject to change
                ///</summary>

                var result = from p in db.EarningPayments
                             select new { p.paymentNumber };

                int recCount = result.Count() + 1;

                EarningPayments paylog = new EarningPayments
                {
                    paymentNumber = payment.gradeName.Trim() + "00" + recCount,
                    payCode = payment.id.Trim(),
                    ActualAmount = searchresults.actualPayAmount,
                    EmpID = payment.empNumber.Trim(),
                    DATE = DateTime.Now.Date,
                    voided = 0
                };


                ///<summary>
                ///Call the LogEarningPaymentLog and InitializaGroupPayrollPaymentData methods consecutively, to save the Earning payment to database 
                ///and copy the same payment entry from the EarningsPayments table to GroupPayrollPayments. Finally a journal entry has to be made into the
                /// GL account for book keeping purposes through calling the CreateJournalEntry method.
                ///</summary>

                journalentry.transactionType = "DR";
                journalentry.transactionCode = paylog.paymentNumber;
                journalentry.Amount = paylog.ActualAmount;
                journalentry.Account = "CASA"; //default account name. The application should be able to pick from the accounts table as defined by the system.
                journalentry.Period = DateTime.Now.Date;
                journalentry.CreatedBy = (int)Session["LoggedUserId"];
                journalentry.Description = getPaymentName(payment.id.Trim());

                contrajournalentry.transactionType = "CR";
                contrajournalentry.transactionCode = paylog.paymentNumber;
                contrajournalentry.Amount = paylog.ActualAmount;
                contrajournalentry.Account = paylog.EmpID;
                contrajournalentry.Period = DateTime.Now.Date;
                contrajournalentry.CreatedBy = (int)Session["LoggedUserId"];
                contrajournalentry.Description = getPaymentName(payment.id.Trim());

                payroll.EmpID = payment.empNumber;
                payroll.PayrollCode = payment.PayrollCode;

                Create(payroll);


                if (LogEarningPayment(paylog))
                {
                    
                    InitializeGroupPayrollPaymentData(paylog.EmpID);
                    CreateJournalEntry(journalentry);
                    CreateContraJournalEntry(contrajournalentry);
                    Message = "payment added successfully";
                    Status = "success";
                }
                else 
                {
                    Message = "error adding payment";
                    Status = "error";
                }
               
            }

            else if (db.Deductions.Any(d => d.payCode == payment.id))
            {
                searchresults.tblName = "Deductions";
                searchresults.recordExist = true;


                ///<summary>
                ///paymentNumber  is the Primary key field in the Payments Table. Each entry into this column
                ///will be formed by combining the gradeName and the count of records in this table plus 1 to form a unique key
                ///NOTE: the generation of the unique ID for this column is subject to change
                ///</summary>

                var result = from p in db.DeductionPayments
                             select new { p.paymentNumber };

                int recCount = result.Count() + 1;

                DeductionPayments paylog = new DeductionPayments
                {
                    paymentNumber = payment.gradeName.Trim() + "00" + recCount,
                    payCode = payment.id.Trim(),
                    ActualAmount = payment.actualPayAmount,
                    EmpID = payment.empNumber.Trim(),
                    DATE = DateTime.Now.Date,
                    voided = 0
                };

                ///<summary>
                ///Call the LogDeductionPaymentLog and InitializaGroupPayrollPaymentData methods consecutively, to save the Deduction payment to database 
                ///and copy the same payment entry from the DeductionPayments table to GroupPayrollPayments. Finally a journal entry has to be made into the
                /// GL account for book keeping purposes through calling the CreateJournalEntry method.
                ///</summary>

                journalentry.transactionType = "DR";
                journalentry.transactionCode = paylog.paymentNumber;
                journalentry.Amount = paylog.ActualAmount;
                journalentry.Account = paylog.EmpID; //default account name. The application should be able to pick from the accounts table as defined by the system.
                journalentry.Period = DateTime.Now.Date;
                journalentry.CreatedBy = (int)Session["LoggedUserId"];
                journalentry.Description = getPaymentName(payment.id.Trim());

                contrajournalentry.transactionType = "CR";
                contrajournalentry.transactionCode = paylog.paymentNumber;
                contrajournalentry.Amount = paylog.ActualAmount;
                contrajournalentry.Account = "LIAB";
                contrajournalentry.Period = DateTime.Now.Date;
                contrajournalentry.CreatedBy = (int)Session["LoggedUserId"];
                contrajournalentry.Description = getPaymentName(payment.id.Trim());

                payroll.EmpID = payment.empNumber;
                payroll.PayrollCode = payment.PayrollCode;

                Create(payroll);

                if (LogDeductionPayment(paylog))
                {
                    
                    InitializeGroupPayrollPaymentData(paylog.EmpID);
                    CreateJournalEntry(journalentry);
                    CreateContraJournalEntry(contrajournalentry);
                    Message = "payment added successfully";
                    Status = "success";
                }
                else 
                {
                    Message = "error adding payment";
                    Status = "error";
                }

            }


            return Json(new {Status = Status, Message = Message });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult InitiatePayroll(searchparam param)
         {

             ///<summary>
             ///This code block picks all the employees in the Employees Table whose onPayroll value equal 1
             ///emprecord is list object of such employees
             ///paymentrecord is a list object that will hold all the payments which have the Priority = 1 relating to the particular employee
             ///During the Foreach loop, the LogEarningPayment method will be called for each payment of the Employee to save to the EarningsPaymment
             ///</summary>
             
             string Message = null;
             string Status = null;

             List<employeeSal> emprecord = new List<employeeSal>();

             List<PaymentRecord> paymentrecord = new List<PaymentRecord>();

             EarningPayments payment = new EarningPayments();

             try
             {

                 emprecord = personnelOnPayroll();
                 foreach (var x in emprecord)
                 {
                     paymentrecord = personnelDefaultPayment(x.groupEligible);

                     foreach (var y in paymentrecord)
                     {
                         payment.paymentNumber = paymentNumber(x.EmpID,"EarningPayments");
                         payment.payCode = y.payCode;
                         payment.ActualAmount = y.actualPayAmount;
                         payment.DATE = DateTime.Now.Date;
                         payment.EmpID = x.EmpID;
                         payment.voided = 0;

                         if (TotalDuplicateFound(x.EmpID, y.payCode,"EarningPayments") == 0)
                         {
                             LogEarningPayment(payment);
                         }
                     }

                 }

                 List<rateDefs> ratedefs = new List<rateDefs>();

                 rateDefs defs = new rateDefs();

                 ratedefs = GetTaxationRates();

                 foreach (var payerate in ratedefs)
                 {
                     defs.firstThresholdLimit = payerate.firstThresholdLimit;
                     defs.secondThresholdLimit = payerate.secondThresholdLimit;
                     defs.thirdThresholdLimit = payerate.thirdThresholdLimit;
                     defs.rateOffirstThresholdLimit = payerate.rateOffirstThresholdLimit;
                     defs.rateOfsecondThresholdLimit = payerate.rateOfsecondThresholdLimit;
                     defs.rateOfthirdThresholdLimit = payerate.rateOfthirdThresholdLimit;

                 }

                List<pensionRate> pensionrates = new List<pensionRate>();
                pensionRate returnedPensionRates = new pensionRate();

                 pensionrates = GetPensionRate();

                 foreach (var rates in pensionrates)
                 {
                     returnedPensionRates.employeesContribution = rates.employeesContribution;
                     returnedPensionRates.employersContribution = rates.employersContribution;
                 }


                 PayrollList employ = new PayrollList();

                 employ.PAYEDEFS = defs;
                 employ.PENSIONRATES = returnedPensionRates;
                 employ.EmpId = param.id;
                 employ.salPayCode = GetPayCode(param.groupEligible);

                 EmployeeSalary empsal = new EmployeeSalary(param.id, param.groupEligible);

                 employ.MinSal = empsal.salary();
                 employ.totalDeduction = empsal.getDeductions();
                 employ.totalEarning = empsal.getEarnings();

                 overtimeParams overtimeparam = new overtimeParams();

                 //overtimeparam = empsal.overtimehours();
                 //employ.HoursWkd = overtimeparam.overtimehours;
                 //employ.overTimeRate = overtimeparam.overtimeRate;


                 //Call method to create LOGID automatically from the UtilityFunctions class
                 utilityfunctions funcs = new utilityfunctions("PayrollHistLog");


                 PayrollHistLog logentry = new PayrollHistLog
                 {
                     LogID = CountNumberOfRows(),
                     EmpID = employ.EmpId,
                     dateStamp = DateTime.Now.Date,
                     NetPay = employ.NetPay(),
                     PAYE = employ.PAYE(),
                     GrossPAY = employ.GrossPay(),
                     Earnings = employ.totalEarning,
                     OvertimePay = employ.OvertimePay(),
                     Deductions = employ.totalDeduction,
                     PensionContribution = employ.Pension()
                 };

                 //Save the record to the database using the FuncLogEntry method...

                 if (FuncLogEntry(logentry))
                 {
                     Message = "Payroll processing completed";
                     Status = "success";
                 }
                 else 
                 {
                     Message = "Payroll processing encountered error(s)";
                     Status = "error";
                 }

             }
             catch (Exception e)
             {

                 util.WriteToLog(e.Message.ToString());
                 Message = "Payroll processing encountered error(s)";
                 Status = "error";
             }

             return Json(new { Message = Message, Status = Status});



             //*****************************************RESERVED OLD COMMENTED CODE*************************************************************

                 /* IEnumerable<searchparam> paycode = db.AttendanceLog.Where(att => att.EmpId == param.id).Select(y => new searchparam { payCode = y.payCode }).Distinct();
                      foreach(var p in paycode)
                      {
                          param.payCode = p.payCode;
                      }
                  var overtimepaycode = from ot in db.AttendanceLog
                                        where ot.EmpId == param.id
                                        select new{ot.payCode};
                  foreach(var p in overtimepaycode)
                  {
                      param.payCode = p.payCode;
                  }
                 */
             //return db.EarningPayments.Where(x => x.EmpID == "BIT182").Select(x => new EarningPayments { ActualAmount = x.ActualAmount, payCode = x.payCode }).AsEnumerable();
             //IEnumerable<PayrollList> overimeHrs = db.AttendanceLog.Where(x => x.EmpId == param.id).Select(s => new PayrollList { HoursWkd = s.overtimeHours });
             //IEnumerable<PayrollList> overtimeRt = db.Earnings.Where(r => r.payCode == param.payCode).Select(code => new PayrollList {PAYERate = code.payRate });
             //IEnumerable<PayrollList> empDetails = db.Employees.Where(q => q.EmpID == param.id).Select(details => new  PayrollList {EmpName = details.EmpName,EmpId = details.EmpID,GradeId = details.GradeID});
             //IEnumerable<PayrollList> MinSalary = db.Earnings.Where(g => g.payType == param.salpayCode).Select(s => new PayrollList {MinSal = s.actualPayAmount });

             //var salary = from em in db.Employees
             //              join e in db.Earnings on em.EmpID equals e.groupEligible
             //              where em.EmpID == param.id && e.groupEligible == param.groupEligible
             //                select new {
             //                  em.EmpName,
             //                  em.EmpID,
             //                  e.groupEligible,
             //                  e.actualPayAmount
             //                 };


             //MinSalary = (from ern in db.Earnings
             //            where ern.groupEligible == e.groupEligible
             //            select ern.actualPayAmount),
             //var overtimeHours = from at in db.AttendanceLog
             // where at.EmpId == param.id && at.Date == DateTime.Now.ToString("d")
             // select new {at.overtimeHours};

             //var overtimeHours = db.AttendanceLog.Where(at => at.EmpId == param.id).Select(new y=})           

             //foreach(var o in overtimeHours){}


             //var overtimeHours = from at in db.AttendanceLog
             //                where at.EmpId == param.id && at.Date == DateTime.Now.ToString("d")
             //                select new { at.overtimeHours};


             //OvertimeRate =(from en in db.Earnings
             //                where en.payCode == param.payCode
             //               select en.payRate).FirstOrDefault(),

             //  var TotalEarning =(from payment in db.EarningPayments
             //  where payment.EmpID == param.id && payment.DATE == DateTime.Now.ToString("d")
             //  select payment.ActualAmount).Sum();

             //  var TotalDeduction =(from deduction in db.DeductionPayments
             // where deduction.EmpID == param.id && deduction.DATE == DateTime.Now.ToString("d")
             //select deduction.ActualAmount).Sum();





             //earnpaycode.Contains(p.payCode) && p.EmpID == param.id && p.DATE == param.Date

             //var salary = from e in db.Employees
             //              join p in db.Payroll on e.EmpID equals p.EmpID where p.EmpID == id
             //              join empGds in db.EmployeeGrades on e.GradeID equals empGds.GradeId
             //              join x in db.TaxationThreholds on empGds.ThresholdID equals x.ThresholdID
             //              select new{e.EmpName,
             //                         p.EmpID,
             //                         empGds.GradeId,
             //                         empGds.MinSalary,
             //                         p.overtimeHours,
             //                         empGds.OvertimeRate,
             //                         p.insuranceBill,
             //                         p.salaryAdvanceDeduction,TotalAllowance =(from al in db.Allowance
             //                                                                   where al.AllowanceRefNum == empGds.AllowanceRefNum
             //                                                                   select al.allowanceRate).Sum()
             //              };


             //var salary = from e in db.Employees
             //             join p in db.Payroll on e.EmpID equals p.EmpID where e.EmpID == id
             //             join empGds in db.EmployeeGrades on e.GradeID equals empGds.GradeName
             //             select new { e.EmpName,
             //                          empGds.GradeId,
             //                          empGds.MinSalary,
             //                          p.overtimeHours, 
             //                          p.insuranceBill,p.salaryAdvanceDeduction,TotalAllowance =(from al in db.Allowance
             //                                                                                     where al.AllowanceRefNum == empGds.AllowanceRefNum
             //                                                                                      select al.allowanceRate).Sum()
             //             };



             //PayrollList emp = from e in db.Employees
             //                  join p in db.Payroll on new {e.EmpID} equals new {EmpID= p.EmpID}
             //                  join empGds in db.EmployeeGrades on new { e.GradeID } equals new {GradeID = empGds.GradeId }
             //                  select new PayrollList { EmpName = e.EmpName, EmpId = p.EmpID, GradeId = empGds.GradeId, MinSal = empGds.MinSalary, HoursWkd = p.overtimeHours, overTimeRate = empGds.OvertimeRate, insuranceBill = p.insuranceBill, salaryAdvanceDeduct = p.salaryAdvanceDeduction };

             //var emp = from e in db.Employees
             //            select new {e.EmpID};

             //Double overtm = employ.OverTime;
             //Double grossP = employ.GrossPay();
             //Double netPay = employ.NetPay();
             //Double Bill = employ.insuranceBill;
             //PayrollHistLog log = new PayrollHistLog();

             //List<PayrollHistLog> LogEntry = new List<PayrollHistLog> 
             //{ 
             //   new PayrollHistLog {LogID = 001,
             //                       EmpID = employ.EmpId,
             //                       NetPay = employ.NetPay(),
             //                       PAYE = employ.PAYE(),
             //                       GrossPAY = employ.GrossPay(),
             //                       OvertimePay = employ.OverTime,
             //                       Deductions = employ.totalDeduction
             //                       }

             //};


             //foreach (var sal in salary)
             //{

             //    //Call the method of the class to compute the payroll for the chosen employeeID

             //    employ.EmpName = sal.EmpName;
             //    employ.GradeId = sal.groupEligible;
             //    employ.EmpId = sal.EmpID;
             //    //employ.MinSal = sal.MinSalary;
             //   // employ.HoursWkd = sal.overtimeHours;
             //    //employ.overTimeRate = sal.OvertimeRate;
             //    //employ.totalDeduction = sal.TotalDeduction;
             //    //employ.totalEarning = sal.TotalEarning;
             //    //employ.salaryAdvanceDeduct = sal.salaryAdvanceDeduction;
             //   // employ.allowancePay = sal.TotalAllowance;
             //    //employ.insuranceBill = sal.insuranceBill;

             //}
         }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult RunPayroll()
        { 
            ///<summary>
            /// Retrieve the list of personnel on payroll
            /// Loop through the list and process the earnings and deductions for each accordingly
            /// </summary>
            
            string _message = null;
            string _status = null;
            List<employeeSal> emprecord = new List<employeeSal>();
            List<PaymentRecord> paymentrecord = new List<PaymentRecord>();
            EarningPayments payment = new EarningPayments();
            DeductionPayments deduction = new DeductionPayments();
            JournalAccount journalaccountentry = new JournalAccount();
            JournalAccount contrajournalentry = new JournalAccount();
           
            List<payrollFormularRecord> formular = personnelDefaultDeduction();

            try
            {
                emprecord = personnelOnPayroll();
                foreach (var x in emprecord)
                {
                    paymentrecord = personnelDefaultPayment(x.groupEligible);

                    foreach (var y in paymentrecord)
                    {
                        payment.paymentNumber = paymentNumber(x.EmpID,"EarningPayments");
                        payment.payCode = y.payCode;
                        payment.ActualAmount = y.actualPayAmount;
                        payment.DATE = DateTime.Now.Date;
                        payment.EmpID = x.EmpID;
                        payment.voided = 0;

                        journalaccountentry.transactionType = "DR";
                        journalaccountentry.transactionCode = payment.paymentNumber;
                        journalaccountentry.Amount = y.actualPayAmount;
                        journalaccountentry.Account = "CASA";
                        journalaccountentry.Period = DateTime.Now.Date;
                        journalaccountentry.CreatedBy = (int)Session["LoggedUserId"];
                        journalaccountentry.Description = "Salary pay";

                        contrajournalentry.transactionType = "CR";
                        contrajournalentry.transactionCode = payment.paymentNumber;
                        contrajournalentry.Amount = y.actualPayAmount;
                        contrajournalentry.Account = x.EmpID;
                        contrajournalentry.Period = DateTime.Now.Date;
                        contrajournalentry.CreatedBy = (int)Session["LoggedUserId"];
                        contrajournalentry.Description = "Salary pay";

                        if (TotalDuplicateFound(x.EmpID,y.payCode,"EarningPayments") == 0)
                        {
                            
                            LogEarningPayment(payment);
                            CreateJournalEntry(journalaccountentry);
                            CreateContraJournalEntry(contrajournalentry);
                        }
                    }

                    foreach(var z in formular)
                    {
                        if (z.Formular.Trim() == "TAX")
                        { 
                            //COMPUTE PAYE here
                            //SAVE the record in the deductions table
                            WithholdingTaxDeduction withholdingTax = new WithholdingTaxDeduction(getTotalEarnings(x.EmpID));

                            deduction.paymentNumber = paymentNumber(x.EmpID,"DeductionPayments");
                            deduction.payCode = z.Paycode;
                            deduction.ActualAmount = withholdingTax.CalculateWithHoldingTax();
                            deduction.DATE = DateTime.Now.Date;
                            deduction.EmpID = x.EmpID;
                            deduction.voided = 0;

                            journalaccountentry.transactionType = "DR";
                            journalaccountentry.transactionCode = deduction.paymentNumber;
                            journalaccountentry.Amount = deduction.ActualAmount;
                            journalaccountentry.Account = x.EmpID;
                            journalaccountentry.Description = "Pay as you earn (PAYE)";
                            journalaccountentry.Period = DateTime.Now.Date;
                            journalaccountentry.CreatedBy = (int)Session["LoggedUserId"];

                            contrajournalentry.transactionType = "CR";
                            contrajournalentry.transactionCode = deduction.paymentNumber;
                            contrajournalentry.Amount = deduction.ActualAmount;
                            contrajournalentry.Account = "LIAB";
                            journalaccountentry.Description = "Pay as you earn (PAYE)";
                            contrajournalentry.Period = DateTime.Now.Date;
                            contrajournalentry.CreatedBy = (int)Session["LoggedUserId"];

                            if (TotalDuplicateFound(x.EmpID, z.Paycode, "DeductionPayments")== 0)
                            {
                                LogDeductionPayment(deduction);
                                CreateJournalEntry(journalaccountentry);
                                CreateContraJournalEntry(contrajournalentry);
                            }
                           

                        }
                        if(z.Formular.Trim() == "PENSION")
                        {
                            //COMPUTE PENSION here
                            //SAVE the record in the deductions table
                            
                            EmployeeSalary empsal = new EmployeeSalary(x.EmpID, x.groupEligible);
                            PensionDeduction pension = new PensionDeduction(empsal.salary(),x.EmpID);

                            deduction.paymentNumber = paymentNumber(x.EmpID, "DeductionPayments");
                            deduction.payCode = z.Paycode;
                            deduction.ActualAmount = pension.CalculatePension();  //getPension(empsal.salary()); commented was the old method used
                            deduction.DATE = DateTime.Now.Date;
                            deduction.EmpID = x.EmpID;
                            deduction.voided = 0;

                            journalaccountentry.transactionType = "DR";
                            journalaccountentry.transactionCode = deduction.paymentNumber;
                            journalaccountentry.Amount = deduction.ActualAmount;
                            journalaccountentry.Account = x.EmpID;
                            journalaccountentry.Period = DateTime.Now.Date;
                            journalaccountentry.CreatedBy = (int)Session["LoggedUserId"];
                            journalaccountentry.Description = "Pension contribution";

                            contrajournalentry.transactionType = "CR";
                            contrajournalentry.transactionCode = deduction.paymentNumber;
                            contrajournalentry.Amount = deduction.ActualAmount;
                            contrajournalentry.Account = "LIAB";
                            contrajournalentry.Period = DateTime.Now.Date;
                            contrajournalentry.CreatedBy = (int)Session["LoggedUserId"];
                            contrajournalentry.Description = "Pension contribution";

                            if (TotalDuplicateFound(x.EmpID, z.Paycode, "DeductionPayments") == 0)
                            {
                                LogDeductionPayment(deduction);
                                CreateJournalEntry(journalaccountentry);
                                CreateContraJournalEntry(contrajournalentry);
                            }
                        }
                    }

                    
                }

                _message = "Payroll processing completed successfully";
                _status = "success";
            }
            catch (Exception e)
            {
                  util.WriteToLog(e.InnerException.Message.ToString() + " " + e.Source.ToString());
                 _message = "Payroll processing encountered error(s)";
                 _status = "error";
            }

            return Json(new { message = _message, status = _status });
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

        private string getPaymentName(string _paycode)
        {
            string paycode = null;
            using (var context = new DataContext())
            {
                try
                {
                    var names = from pc in context.PaymentCodes
                                where pc.payCode == _paycode
                                select new {pc.Description };
                    foreach(var name in names)
                    {
                        paycode = name.Description;
                    }
                }
                catch (Exception e)
                {

                    util.WriteToLog(e.Message.ToString() + " Exception: " + e.InnerException.ToString());
                }

                return paycode;
            }
        }

        private Double getTotalEarnings(string empId)
        {
            double totalEarnings = 0;
            try
            {
                using (var context = new DataContext())
                {
                    var earningsTotal = from e in context.EarningPayments
                                        where e.EmpID == empId && e.voided == 0
                                        group e by e.EmpID into earningGroup
                                        select new { totalEarning = earningGroup.Sum(y => y.ActualAmount) };

                    foreach (var t in earningsTotal)
                    {
                       totalEarnings = t.totalEarning;
                    }
                }
                
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString() + " Exception: " + e.InnerException.ToString());
            }


            return totalEarnings;
        }

        private Double getPAYE(Double earnings)
        { 
             Double result1 = 0, result2 = 0 ,ROTH1, ROTH2, ROTH3;
             Double THL1, THL2, THL3;
             Double THAmount1 = 0, THAmount2 = 0, THAmount3 = 0;

             List<rateDefs> PAYEDEFS = GetTaxationRates();
             rateDefs defs = new rateDefs();

             foreach (var payerate in PAYEDEFS)
             {
                 defs.firstThresholdLimit = payerate.firstThresholdLimit;
                 defs.secondThresholdLimit = payerate.secondThresholdLimit;
                 defs.thirdThresholdLimit = payerate.thirdThresholdLimit;
                 defs.rateOffirstThresholdLimit = payerate.rateOffirstThresholdLimit;
                 defs.rateOfsecondThresholdLimit = payerate.rateOfsecondThresholdLimit;
                 defs.rateOfthirdThresholdLimit = payerate.rateOfthirdThresholdLimit;

             }

             THL1 = defs.firstThresholdLimit;
             THL2 = defs.secondThresholdLimit;
             THL3 = defs.thirdThresholdLimit;
             ROTH1 = defs.rateOffirstThresholdLimit;
             ROTH2 = defs.rateOfsecondThresholdLimit;
             ROTH3 = defs.rateOfthirdThresholdLimit;

             if (earnings < (Double)THL1)
             {

                 THAmount1 = 0.0;
             }
             if (earnings >= (Double)THL1)
             {

                 THAmount1 = (Double)THL1 * (ROTH1 / 100);
                 result1 = earnings - (Double)THL1;
             }
             if (result1 >= (Double)THL2)
             {

                 THAmount2 = (Double)THL2 * (ROTH2 / 100);
                 result2 = result1 - (Double)THL2;
             }
             if (result2 > (Double)THL2)
             {

                 THAmount3 = result2 * (ROTH3 / 100);
             }

             return THAmount1 + THAmount2 + THAmount3;


        }

        private Double getPension(Double __basicSalary)
        {
            List<pensionRate> pensionrates = new List<pensionRate>();
            pensionRate __rates = new pensionRate();

            pensionrates = GetPensionRate();

            foreach (var rates in pensionrates)
            {
                __rates.employeesContribution = rates.employeesContribution;
                __rates.employersContribution = rates.employersContribution;
            }

            Double employeeContrib = __basicSalary * (Double)__rates.employeesContribution / 100;
            Double employerContrib = __basicSalary * (Double)__rates.employersContribution / 100;

            return employeeContrib;

        }

        private string GetPayCode(string groupEligible)
        {
            string paycode = null;
           // IEnumerable<searchparam> salpaycode = db.Earnings.Where(code => code.actualPayAmount > 0).Select(x => new searchparam { payCode = x.payCode }).Distinct();
            using (var context = new DataContext()) 
            {
                var salpaycode = from spc in context.Earnings
                                 where spc.actualPayAmount > 0 && spc.groupEligible == groupEligible
                                 select new {spc.payCode };
                foreach (var u in salpaycode)
                {
                    paycode = u.payCode;

                }
                return paycode;
            }
 
        }

        private List<pensionRate> GetPensionRate()
        {

            using (var context = new DataContext())
            {
                List<pensionRate> rates = (from p in context.Pensions
                                          select new pensionRate { employeesContribution = p.EmployeeContrRate, 
                                                                   employersContribution = p.EmployerContrRate }).ToList();
                return rates;
            }
      
        }

       private List<rateDefs> GetTaxationRates()
        {
            using (var context = new DataContext())
            {
                List<rateDefs> rates = (from txt in context.Taxations
                                        select new rateDefs
                                        {
                                           // firstThresholdLimit = txt.FirstThresholdLimit,
                                            //secondThresholdLimit = txt.SecondThresholdLimit,
                                           // thirdThresholdLimit = txt.ThirdThresholdLimit,
                                            //rateOffirstThresholdLimit = txt.RateOffirstThreshold,
                                           // rateOfsecondThresholdLimit = txt.RateOfSecondThreshold,
                                           // rateOfthirdThresholdLimit = txt.RateOfThirdThreshold 
                                        }).ToList();
                return rates;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ChangePaymentState()
         {
             DeductionPayments deductpayment = new DeductionPayments();
             EarningPayments earningpayment = new EarningPayments();

             using (var context = new DataContext())
             {
                 try
                 {
                     var paymentnumber = from ed in context.EarningPayments
                                         where ed.voided == 0
                                         select new { ed.paymentNumber, ed.payCode, ed.EmpID, ed.DATE, ed.ActualAmount };
                     foreach (var x in paymentnumber)
                     {
                         earningpayment.paymentNumber = x.paymentNumber;
                         earningpayment.payCode = x.payCode;
                         earningpayment.ActualAmount = x.ActualAmount;
                         earningpayment.EmpID = x.EmpID;
                         earningpayment.DATE = x.DATE;
                         earningpayment.voided = 1;
                         earningpayment.voidedBy = "System";
                         earningpayment.voidedDate = DateTime.Now.Date;

                         DeleteEarningPayment(earningpayment, "void payment");

                     }
                 }
                 catch (Exception e)
                 {

                     util.WriteToLog(e.Message.ToString());
                 }


                
             }

             using(var context = new DataContext())
             {
                 var paymentnumber = from dd in context.DeductionPayments
                                     where dd.voided == 0
                                     select new { dd.paymentNumber, dd.payCode, dd.EmpID, dd.DATE, dd.ActualAmount };

                foreach(var x in paymentnumber)
                 {
                     deductpayment.paymentNumber = x.paymentNumber;
                     deductpayment.payCode = x.payCode;
                     deductpayment.ActualAmount = x.ActualAmount;
                     deductpayment.EmpID = x.EmpID;
                     deductpayment.DATE = x.DATE;
                     deductpayment.voided = 1;
                     deductpayment.voidedBy = "System";
                     deductpayment.voidedDate = DateTime.Now.Date;

                     DeleteDeductionPayment(deductpayment, "void payment");
                 }

                
             }

             return Json(true);
         }

        private int CountNumberOfRows()
         {

             var numberofrows = from ph in db.PayrollHistLogs
                                select new { ph.LogID };

             int rows = numberofrows.Count() + 1;

             return rows;
         }

        public ViewResult Details(string id)
        {
            Payroll payroll = db.Payrolls.Find(id);
            return View(payroll);
        }

        public ActionResult Create()
        {
            return View();
        } 

        private Boolean Create(Payroll payroll)
        {
            Boolean recordSaved = false;
            ///<summary>
            ///COMMENT: before an attempt to save the record into the Payroll table a check has to be made first
            ///         if the match is found that means the record already exist. This is one way of reducing the PK violation exceptions
            ///</summary>
            payroll.Id = 1;
            payroll.payrollRunDate = DateTime.Now.Date;

            try
            {
                if (!db.Payrolls.Any(p => p.EmpID == payroll.EmpID))
                {
                        db.Payrolls.Add(payroll);
                        db.SaveChanges();
                        recordSaved = true;   
                }

            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString() + " Exception: " + e.InnerException.ToString());
                recordSaved = false;
            }

            return recordSaved;
        }

        private string paymentNumber(string eligibleGrade, string __model)
        {
            int numOfRecords = 0;

            if (__model == "EarningPayments")
            {

                var results = from ep in db.EarningPayments
                              select new { ep.paymentNumber };

                numOfRecords = results.Count() + 1;
            }
            else
            {
                var results = from dp in db.DeductionPayments
                              select new { dp.paymentNumber };

                numOfRecords = results.Count() + 1;
            }
               
            return eligibleGrade + "00" + numOfRecords.ToString();
            
        }

        private List<employeeSal> personnelOnPayroll()
        {
            using (var context = new DataContext())
            {
                List<employeeSal> results = (from emp in context.Employees
                                             where emp.onPayroll == 1 && emp.EmploymentStatus == "Active"
                                             select new employeeSal {EmpID = emp.EmpID, groupEligible =emp.GradeID }).ToList();
                return results;
            }
        }

        private List<PaymentRecord> personnelDefaultPayment(string groupEligible)
        { 

           using(var context = new DataContext())
           { 
              List<PaymentRecord> payment =( from earnp in context.Earnings
                                            where earnp.groupEligible== groupEligible && earnp.priority ==1
                                            select new PaymentRecord{payCode = earnp.payCode, actualPayAmount = earnp.actualPayAmount}).ToList();
              return payment;
           }
        }

        private List<payrollFormularRecord> personnelDefaultDeduction()
        { 
            using(var context = new DataContext())
            {
                List<payrollFormularRecord> formulae = (from pf in context.PayrollFormulae
                                                        //join d in context.Deductions on pf.Paycode equals d.payCode
                                                       // where d.priorityCode == 1
                                                        select new payrollFormularRecord { Paycode = pf.Paycode, Formular = pf.Formular }).ToList();
                return formulae;
            }
        }

        private int TotalDuplicateFound(string __empid, string __paycode, string __tablename)
        {
            int count = 0;
        
                using(var context = new DataContext())
                {
                    try
                    {
                        if (__tablename == "EarningPayments")
                        {
                            var results = from ep in context.EarningPayments
                                          where ep.EmpID == __empid && ep.payCode == __paycode && ep.voided == 0
                                          select new { ep.paymentNumber };
                            count = results.Count();
                        }
                        else
                        {
                            var results = from ep in context.DeductionPayments
                                          where ep.EmpID == __empid && ep.payCode == __paycode && ep.voided == 0
                                          select new { ep.paymentNumber };
                            count = results.Count();
                        }
                    }
                    catch (Exception e)
                    {
                        util.WriteToLog(e.Message.ToString());
                    }

                }

                return count;
       
        }

        private Boolean LogEarningPayment(EarningPayments payment)
        {
            Boolean recordsved = false;
         
                using (var context = new DataContext())
                {
                    try
                    {
                        context.EarningPayments.Add(payment);
                        context.SaveChanges();
                        recordsved = true;
                    }
                    catch (Exception e)
                    {

                        util.WriteToLog(e.Message.ToString() + " Exception: " + e.InnerException.ToString());
                        recordsved = false;
                    }


                    return recordsved;
                }

        }

        private Boolean LogDeductionPayment(DeductionPayments payment)
        {
            Boolean recordsaved = false;
            
                using (var context = new DataContext())
                {
                    try
                    {
                        context.DeductionPayments.Add(payment);
                        context.SaveChanges();
                        recordsaved = true;
                    }
                    catch (Exception e)
                    {
                        util.WriteToLog(e.Message.ToString() + " Exception: " + e.InnerException.ToString());
                        recordsaved = false;
                    }


                    return recordsaved;
                }

        }

        private Boolean LogGroupPayrollPayment(GroupPayrollPayment payment)
        {
                Boolean recordsaved = false;
                using (var context = new DataContext())
                {
                    try
                    {
                            context.GroupPayrollPayments.Add(payment);
                            context.SaveChanges();
                            recordsaved = true;
                    }
                    catch (Exception e)
                    {
                        util.WriteToLog(e.Message.ToString() + "" + e.InnerException.Source.ToString());
      
                    }

                    return recordsaved;
                }

        }

        private Boolean CreateJournalEntry(JournalAccount journalEntry)
        {
            Boolean success = false;
            using(var context = new DataContext())
            {
                try
                {
                    context.JournalAccounts.Add(journalEntry);
                    context.SaveChanges();
                    success = true;
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + "" + e.InnerException.Message.ToString());
                    success = false;
                }

               return success;
            }
        }

        private Boolean CreateContraJournalEntry(JournalAccount journalEntry)
        {
            Boolean success = false;
            using (var context = new DataContext())
            {
                try
                {
                    context.JournalAccounts.Add(journalEntry);
                    context.SaveChanges();
                    success = true;
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + "" + e.InnerException.Source.ToString());
                    success = false;
                }

                return success;
            }
        }
        
        public ActionResult Edit(string id)
        {
            Payroll payroll = db.Payrolls.Find(id);
            return View(payroll);
        }

        private List<DeductionPayments> employeeDeductions(string empId)
        {
            using (DataContext dc = new DataContext())
            {
                var deduct = (from dp in dc.DeductionPayments
                              join d in dc.Deductions on dp.payCode equals d.payCode
                              where dp.EmpID == empId && dp.voided == 0 
                              select new
                              {
                                  ActualAmount = dp.ActualAmount,
                                  payCode = dp.payCode,
                                  paymentNumber = dp.paymentNumber
                              }).ToList().Select(y => new DeductionPayments() { ActualAmount = y.ActualAmount, payCode = y.payCode,paymentNumber = y.paymentNumber });

                return deduct.ToList();
            }

        }

        private List<EarningPayments> employeeEarnings(string empId)
        {

            using (DataContext dc = new DataContext())
            {
                var earn = (from ep in dc.EarningPayments
                            join e in dc.Earnings on ep.payCode equals e.payCode
                            where ep.EmpID == empId && ep.voided == 0
                            select new
                            {
                                ActualAmount = ep.ActualAmount,
                                payCode = e.payCode,
                                paymentNumber = ep.paymentNumber
                            }).ToList().Select(x => new EarningPayments() { ActualAmount = x.ActualAmount, payCode = x.payCode,paymentNumber = x.paymentNumber });

                return earn.ToList();
            }

            //return db.EarningPayments.Where(x => x.EmpID == "BIT182").Select(x => new EarningPayments { ActualAmount = x.ActualAmount, payCode = x.payCode }).AsEnumerable();

        }

        private List<GroupPayrollPayment> verifyIfPaymentExist(string empid, string paycode)
        {
            DateTime currentdate = DateTime.Now.Date;
            using (var context = new DataContext())
            {
                var result = (from gp in context.GroupPayrollPayments
                              where gp.EmpId == empid && gp.payCode == paycode && gp.DATE == currentdate
                              select new
                              {
                                  EmpId = gp.EmpId,
                                  payCode = gp.payCode
                              }).ToList().Select(x => new GroupPayrollPayment() {EmpId = x.EmpId, payCode = x.payCode});

                return result.ToList();
            }
        }

        [HttpPost]
        public ActionResult Edit(Payroll payroll)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(payroll).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch(OptimisticConcurrencyException)
                {
                   
                }
            }
            return View(payroll);
        }

        [AcceptVerbs(HttpVerbs.Post), ActionName("Update")]
        public ActionResult Update(Payroll payroll) {
            if (ModelState.IsValid)
            {
                 /*try
                 {
                     db.Entry(payroll).State = EntityState.Modified;
                     db.SaveChanges();
                     return Json(true);
                 }
                 catch(OptimisticConcurrencyException)
                 {
                    
                 }*/

                var empid = payroll.EmpID;
                //call the delete method to delete the record before the update action
                //this is to avoid duplicate key exception
                
            }
            else { 
                foreach(var modelStateErrors in ModelState){
                    string propertyName = modelStateErrors.Key;
                }
            }
            return Json(false);
        }

        public ActionResult Delete(string id)
        {
            Payroll payroll = db.Payrolls.Find(id);
             return View(payroll);
            //return Json(true);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult TestMethod(double val)
        {

           //WithholdingTaxDeduction withholdingtaxdeduct = new WithholdingTaxDeduction(val);
           //double tax =  withholdingtaxdeduct.CalculateWithHoldingTax();
           //OverTimePayment overtimepayment = new OverTimePayment("OVT1","AFG1131","Grade-A1",4);
           //double overtimeAmount = overtimepayment.OverTimePay();
            ReducingBalanceComputation rbl = new ReducingBalanceComputation(4, 120000, 6);
            double pmt = rbl.PMT();
            double fv = rbl.FV(pmt);
           return Json(true);

        }
  
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
           /* Payroll payroll = new Payroll();
            var pay = from p in db.Payroll
                      where p.EmpID == id
                      select new {p.EmpID,p.PayrollCode,p.insuranceBill,p.salaryAdvanceDeduction,p.overtimeHours };

            foreach (var x in pay){
                payroll.EmpID = x.EmpID;
                payroll.PayrollCode = x.PayrollCode;
                payroll.insuranceBill = x.insuranceBill;
                payroll.salaryAdvanceDeduction = x.salaryAdvanceDeduction;
                payroll.overtimeHours = x.overtimeHours;

            }

            db.Payroll.Remove(payroll);
            db.SaveChanges();
            return Json(true);*/

            //Payroll payroll = db.Payroll.Find(id);
            Payroll payroll = db.Payrolls.FirstOrDefault(x=>x.EmpID==id);
            db.Payrolls.Remove(payroll);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return Json(true);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}