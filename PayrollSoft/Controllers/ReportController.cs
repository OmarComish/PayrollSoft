using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using PayrollSoft.Models;

namespace PayrollSoft.Controllers
{
    public class pensionReportParams
    {
        public DateTime mindate { get; set; }
        public DateTime maxdate { get; set; }
    }
    public class ReportController : Controller
    {
        private DataContext db = new DataContext();

        private List<User> user;

        public ViewResult PensionReport()
        {
            return View();
        }

        public ActionResult LoansReport() 
        {
            var grouploanProtifolios = new GroupLoanPortifolios
            {
                PortifolioDetails = GetGrouploanPortifolios(),
                Total = TotalGroupLoanPortifolios()
            };

            return View(grouploanProtifolios);
        }

        public ActionResult RenderLoansReport()
        {
            return new ActionAsPdf("LoansReport",user);
        }

        public ViewResult newPayslipRpt()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetPensionAccounts(pensionReportParams queryparams)
        {
            DateTime _date = queryparams.mindate;
            var result = from p in db.PensionAccounts
                         join e in db.Employees on p.EmpId equals e.EmpID
                         select new {e.EmpName,p.EntryDate,p.EmployeeContribution,
                             p.EmployerContribution,p.BrokerageFee,p.AdministrationFee,p.BasicSalary,p.GroupLifeAssurance };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            if (Session["LoggedUserId"] != null)
            {
                var finalPayslip = new PersonnelPayslip
                {
                    PersonnelDeductions = employeeDeductions(),
                    PersonnelEarnings = employeeEarnings(),
                    LoanDeductions = employeeLoanDeductions(),
                    InitialLoanAmount = InitialLoanBalance(),
                    EmployeeGrossPay = EmployeeTotalEarnngs(),
                    PayAsYouEarn = EmployeePayAsYouEarn(),
                    NetSalaryPay = EmployeeNetEarnings()
                };

                ViewBag.Message = Session["LoggedUserFullName"];

                return View(finalPayslip);
            }
            else
            {
                return RedirectToAction("/Home/Login");
            }
           
        
        }

        public ActionResult PrintPayslip()
        {

            var finalPayslip = new PersonnelPayslip
             {
                 PersonnelDeductions = employeeDeductions(),
                 PersonnelEarnings = employeeEarnings(),
                 LoanDeductions = employeeLoanDeductions(),
                 InitialLoanAmount = InitialLoanBalance(),
                 EmployeeGrossPay = EmployeeTotalEarnngs(),
                 PayAsYouEarn = EmployeePayAsYouEarn(),
                 NetSalaryPay = EmployeeNetEarnings()
             };

            return View(finalPayslip);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RenderReport()
        {
            return new ActionAsPdf("LoansReport");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult RetrievePayslipList(DateTime __mindate, DateTime __maxdate)
        {
            string _empId = GetEmployeeId();

                var payslip = from e in db.Employees
                              join d in db.Departments on e.DeptID equals d.DeptID
                              join p in db.EarningPayments on e.EmpID equals p.EmpID
                              where p.DATE >= __mindate && p.DATE <= __maxdate && e.EmpID == _empId
                              select new { e.EmpID, e.EmpName, e.GradeID, d.DeptName, e.JobTitle, p.DATE };

                return Json(payslip, JsonRequestBehavior.AllowGet);       
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult RetrievePayslips(DateTime __date)
        {
            if (__date == null) __date = DateTime.Now.Date;

            var payslip = from e in db.Employees
                          join d in db.Departments on e.DeptID equals d.DeptID
                          join p in db.EarningPayments on e.EmpID equals p.EmpID
                          where p.DATE == __date
                          select new { e.EmpID, e.EmpName, e.GradeID, d.DeptName, e.JobTitle, p.DATE };

            return Json(payslip, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult RetrievePayrollEarnings(DateTime __date)
        {
                SessionActive();

                string _empId = GetEmployeeId();

                var payments = from ep in db.EarningPayments
                               join e in db.Earnings on ep.payCode equals e.payCode
                               where ep.DATE == __date && ep.EmpID == _empId
                               select new { ep.paymentNumber, ep.EmpID, e.payCodeDescription, ep.ActualAmount, ep.DATE };

                int i = payments.Count(); 
        
          
            return Json(payments, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult RetrievePayrollDeductions(DateTime __date)
        {
            string _empId = GetEmployeeId();
            var payments = from ep in db.DeductionPayments
                           join d in db.Deductions on ep.payCode equals d.payCode
                           where ep.DATE == __date && ep.EmpID == _empId
                           select new { ep.EmpID, d.payCodeDescription, ep.ActualAmount, ep.DATE };

            int i = payments.Count();

            return Json(payments, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult RetrieveDefaultDeduction(DateTime __date)
        {

            var payments = from pl in db.PayrollHistLogs 
                           where pl.dateStamp == __date
                           select new { pl.EmpID,pl.PAYE,pl.PensionContribution};

         
            return Json(payments, JsonRequestBehavior.AllowGet);

        }

        private void SessionActive()
        {
            if (Session["LoggedUserId"] == null)
            {
                RedirectToAction("Login","Home");
            }
        }

        private string GetEmployeeId()
        {
            using (var _context = new DataContext())
            {
                string _empId = string.Empty;
                int userId = (int)Session["LoggedUserId"];

                var result = from u in _context.Users
                             join e in _context.Employees on u.FullName equals e.EmpName
                             where u.UserId == userId
                             select new { e.EmpID };
                foreach (var t in result)
                {
                    _empId = t.EmpID;
                }

                return _empId;
            }
        }

        private ActionResult ReportMaster()
        {

            var finalPayslip = new PersonnelPayslip
            {
                PersonnelDeductions = employeeDeductions(),
                PersonnelEarnings = employeeEarnings(),
                LoanDeductions = employeeLoanDeductions(),
                InitialLoanAmount = InitialLoanBalance(),
                EmployeeGrossPay = EmployeeTotalEarnngs(),
                PayAsYouEarn = EmployeePayAsYouEarn(),
                NetSalaryPay = EmployeeNetEarnings()
            };

            return View(finalPayslip);
        }

        private List<EarningPayments> employeeEarnings()
        {
            var earn = (from ep in db.EarningPayments
                        join e in db.Earnings on ep.payCode equals e.payCode
                        where ep.EmpID == "BHT01"
                        select new
                        {
                            ActualAmount = ep.ActualAmount,
                            payCode = e.payCodeDescription
                        }).ToList().Select(x => new EarningPayments() { ActualAmount = x.ActualAmount, payCode = x.payCode });
            //return db.EarningPayments.Where(x => x.EmpID == "BIT182").Select(x => new EarningPayments { ActualAmount = x.ActualAmount, payCode = x.payCode }).AsEnumerable();
            return earn.ToList();
        }

        private List<DeductionPayments> employeeDeductions()
        {

            var deduct = (from dp in db.DeductionPayments
                          join d in db.Deductions on dp.payCode equals d.payCode
                          where dp.EmpID == "BHT01"
                          select new
                          {
                              ActualAmount = dp.ActualAmount,
                              payCode = d.payCodeDescription
                          }).ToList().Select(y => new DeductionPayments() { ActualAmount = y.ActualAmount, payCode = y.payCode });

            return deduct.ToList();
        }

        private List<LoanPortifolio> employeeLoanDeductions()
        {
            var loandeduction = (from lp in db.LoanPortifolios
                                 where lp.LoanRefNumber == "HSELN3050596043" &&
                                 lp.EndOfPeriod == db.LoanPortifolios.Where(y => y.LoanRefNumber == "HSELN3050596043").Select(y => y.EndOfPeriod).Max()
                                 select new
                                 {
                                     PrincipalPaid = lp.PrincipalPaid,
                                     InterestPaid = lp.InterestPaid,
                                     LoanBalance = lp.LoanBalance
                                 }).ToList().Select(x => new LoanPortifolio() { PrincipalPaid = x.PrincipalPaid, InterestPaid = x.InterestPaid, LoanBalance = x.LoanBalance });

            return loandeduction.ToList();
        }

        private List<loanMaster> InitialLoanBalance()
        {
            var amount = (from lm in db.LoanMasters
                          where lm.LoanRefNumber == "HSELN3050596043"
                          select new
                          {
                              LoanAmount = lm.LoanAmount
                          }).ToList().Select(x => new loanMaster() { LoanAmount = x.LoanAmount });


            return amount.ToList();
        }

        private List<EarningPayments> EmployeeTotalEarnngs()
        {
            var totalearning = (from tot in db.EarningPayments
                                where tot.EmpID == "BHT01" //&& DATE ==
                                select new
                                {
                                    ActualAmount = (from g in db.EarningPayments
                                                    where g.EmpID == tot.EmpID
                                                    select g.ActualAmount).Sum()
                                }).ToList().Select(x => new EarningPayments() { ActualAmount = x.ActualAmount });

            return totalearning.ToList();
        }

        private List<PayrollHistLog> EmployeePayAsYouEarn()
        {
            var paye = (from p in db.PayrollHistLogs
                        where p.EmpID == "BHT01" //&& DATE ==
                        select new
                        {
                            PAYE = p.PAYE
                        }).ToList().Select(v => new PayrollHistLog() { PAYE = v.PAYE });

            return paye.ToList();
        }

        private List<PayrollHistLog> EmployeeNetEarnings()
        {
            var paye = (from p in db.PayrollHistLogs
                        where p.EmpID == "BHT01" //&& DATE ==
                        select new
                        {
                            NetPay = p.NetPay
                        }).ToList().Select(v => new PayrollHistLog() { NetPay = v.NetPay });

            return paye.ToList();
        }

        private List<loanMaster> GetGrouploanPortifolios()
        {
            using (var context = new DataContext())
            {
                var results = (from lm in context.LoanMasters
                               join emp in context.Employees on lm.EmpID equals emp.EmpID
                               join lt in context.LoanTypes on lm.LoanTypeNumber equals lt.Code
                               where lm.Status == "Matured"
                               select new
                               {
                                   LoanRefNumber = lm.LoanRefNumber,
                                   LoanTypeNumber = lt.Description,
                                   EmpID = emp.EmpName,
                                   startDate = lm.startDate,
                                   EndDate = lm.EndDate,
                                   PaybackPeriods = lm.PaybackPeriods,
                                   LoanAmount = lm.LoanAmount,
                                   MonthlyRepayment = lm.MonthlyRepayment,
                                   Status = lm.Status
                               }).ToList().Select(x => new loanMaster() {LoanRefNumber = x.LoanRefNumber,LoanTypeNumber = x.LoanTypeNumber,EmpID = x.EmpID,
                               startDate = x.startDate,EndDate = x.EndDate,PaybackPeriods = x.PaybackPeriods,LoanAmount = x.LoanAmount,
                               MonthlyRepayment = x.MonthlyRepayment,Status = x.Status});

                return results.ToList();

            }
        }

        private List<loanMaster> TotalGroupLoanPortifolios()
        { 
             using(var context = new DataContext())
             {
                 var results = (from lm in context.LoanMasters
                                where lm.Status == "Matured"
                                group lm by lm.LoanAmount into g
                                select new { LoanAmount = g.Sum(x => x.LoanAmount) }).ToList().Select(x => new loanMaster() { LoanAmount = x.LoanAmount });

                 return results.ToList();
                                
             }
        }
    }
}