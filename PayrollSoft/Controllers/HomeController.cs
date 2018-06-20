using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using PayrollSoft.AuditTrails;
using PayrollSoft.Models;
using Rotativa;
using PayrollSoft.Utility;

namespace PayrollSoft.Controllers
{
    public class HomeController : Controller
    {

        private List<User> user;

        private DataContext db = new DataContext();

        private UtilityBase util = new UtilityBase();

  
        public ActionResult Login()
        {
            List<SelectListItem> modules = new List<SelectListItem>();
            modules.Add(new SelectListItem { Text = "ESS", Value = "ESS" });
            modules.Add(new SelectListItem { Text = "Payroll", Value = "Payroll" });

            ViewBag.SignedInTo = modules;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Audit]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid) 
            {
                using(DataContext context = new DataContext())
                {
                    try
                    {
                        if (context.Users.Any(x => x.UserName == user.UserName && x.Password == user.Password))
                        {
                            var v = from u in context.Users
                                    where u.UserName == user.UserName
                                    select new { u.UserId, u.Password, u.FullName, u.UserName };
                            foreach (var r in v)
                            {
                                Session["LoggedUserId"] = (int)r.UserId;
                                Session["LoggedUserFullName"] = r.FullName.ToString();
                            }

                            Session["SignedInTo"] = user.SignedInTo;

                            UpdateUserAccount((int)Session["LoggedUserId"]);

                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception e)
                    {
                        util.WriteToLog(e.Message.ToString());
                    }

                }
            
            }

            return View(user);
        }

        public ActionResult Index()
        {

            if (Session["LoggedUserId"] != null)
            {

                //UpdateUserAccount((int)Session["LoggedUserId"]);
               /* var finalPayslip = new PersonnelPayslip
                {
                    PersonnelDeductions = employeeDeductions(),
                    PersonnelEarnings = employeeEarnings(),
                    LoanDeductions = employeeLoanDeductions(),
                    InitialLoanAmount = InitialLoanBalance(),
                    EmployeeGrossPay = EmployeeTotalEarnngs(),
                    PayAsYouEarn = EmployeePayAsYouEarn(),
                    NetSalaryPay = EmployeeNetEarnings()
                };*/

                //return View(finalPayslip);

                ViewBag.Message = Session["LoggedUserFullName"];

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }


            //return View(user);
        }

        public ActionResult RenderReport()
        {
            return new ActionAsPdf("ReportMaster",user);
        }

        public ActionResult ReportMaster()
        {
            ///replace the list with an object that fetches data from the database
            ///The model that will be doing the fetch is PersonnelPayslip



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
        public JsonResult RetrieveUserRoles()
        {
            try
            {
                int userid = (int)Session["LoggedUserId"];
                var results = from u in db.UserPriviledges
                              join s in db.Users on u.UserId equals s.UserId
                              where s.UserId == userid
                              select new { u.UserId, s.UserName,s.FullName, u.ModuleId, u.CreateRole, u.ReadRole, u.UpdateRole, u.DeleteRole,u.AuthRole,s.SignedInTo };

                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                util.WriteToLog(e.Message.ToString());
                return Json(false);
            }

        }

        ///<summary>
        ///Private region
        ///</summary>
        
        private void UpdateUserAccount(int id)
        {
            using (var context = new DataContext())
            {
                try
                {
                    User user = context.Users.Find(id);
                    user.LastSignedOn = DateTime.Now.Date;
                    user.Status = "Active";
                    user.SignedInTo = Session["SignedInTo"].ToString();
                    context.Entry(user).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    util.WriteToLog(e.Message.ToString() + " Source: UpdateUserAccount in home controller");
                }

            }
        }

        private List<EarningPayments> employeeEarnings()
        {

            using (DataContext dc = new DataContext())
            {
                var earn = (from ep in dc.EarningPayments
                            join e in dc.Earnings on ep.payCode equals e.payCode
                            where ep.EmpID == "BHT01"
                            select new
                            {
                                ActualAmount = ep.ActualAmount,
                                payCode = e.payCodeDescription
                            }).ToList().Select(x => new EarningPayments() { ActualAmount = x.ActualAmount, payCode = x.payCode });

                return earn.ToList();
            }

            //return db.EarningPayments.Where(x => x.EmpID == "BIT182").Select(x => new EarningPayments { ActualAmount = x.ActualAmount, payCode = x.payCode }).AsEnumerable();
           
        }

        private List<DeductionPayments> employeeDeductions()
        {
            using (DataContext dc = new DataContext())
            {
                var deduct = (from dp in dc.DeductionPayments
                              join d in dc.Deductions on dp.payCode equals d.payCode
                              where dp.EmpID == "BHT01"
                              select new
                              {
                                  ActualAmount = dp.ActualAmount,
                                  payCode = d.payCodeDescription
                              }).ToList().Select(y => new DeductionPayments() { ActualAmount = y.ActualAmount, payCode = y.payCode });

                return deduct.ToList();
            }

        }

        private List<LoanPortifolio> employeeLoanDeductions()
        {
            using(DataContext dc = new DataContext())
            {
                var loandeduction = (from lp in dc.LoanPortifolios
                                     where lp.LoanRefNumber == "HSELN3050596043" &&
                                     lp.EndOfPeriod == dc.LoanPortifolios.Where(y => y.LoanRefNumber == "HSELN3050596043").Select(y => y.EndOfPeriod).Max()
                                     select new
                                     {
                                         PrincipalPaid = lp.PrincipalPaid,
                                         InterestPaid = lp.InterestPaid,
                                         LoanBalance = lp.LoanBalance
                                     }).ToList().Select(x => new LoanPortifolio() { PrincipalPaid = x.PrincipalPaid, InterestPaid = x.InterestPaid, LoanBalance = x.LoanBalance });

                return loandeduction.ToList();
            }

        }

        private List<loanMaster> InitialLoanBalance()
        {
            using(DataContext dc = new DataContext())
            {
                var amount = (from lm in dc.LoanMasters
                              where lm.LoanRefNumber == "HSELN3050596043"
                              select new
                              {
                                  LoanAmount = lm.LoanAmount
                              }).ToList().Select(x => new loanMaster() { LoanAmount = x.LoanAmount });


                return amount.ToList();
            }

        }

        private List<EarningPayments> EmployeeTotalEarnngs()
        {
            
            using (DataContext dc = new DataContext())
            {
                var totalearning = (from tot in dc.EarningPayments
                                    where tot.EmpID == "BHT01" //&& DATE ==
                                    select new
                                    {
                                        ActualAmount = (from g in dc.EarningPayments
                                                        where g.EmpID == tot.EmpID
                                                        select g.ActualAmount).Sum()
                                    }).ToList().Select(x => new EarningPayments() { ActualAmount = x.ActualAmount });

                return totalearning.ToList();
            }

        }

        private List<PayrollHistLog> EmployeePayAsYouEarn()
        {
            using(DataContext dc = new DataContext())
            {
                var paye = (from p in dc.PayrollHistLogs
                            where p.EmpID == "BHT01" //&& DATE ==
                            select new
                            {
                                PAYE = p.PAYE
                            }).ToList().Select(v => new PayrollHistLog() { PAYE = v.PAYE });

                return paye.ToList();
            }

        }

        private List<PayrollHistLog> EmployeeNetEarnings()
        {

            using(DataContext dc = new DataContext())
            {
                var paye = (from p in dc.PayrollHistLogs
                            where p.EmpID == "BHT01" //&& DATE ==
                            select new
                            {
                                NetPay = p.NetPay
                            }).ToList().Select(v => new PayrollHistLog() { NetPay = v.NetPay });

                return paye.ToList();

            }

        }
        
        ///<summary>
        ///End of the private Region
        ///</summary>
       

    }
}
