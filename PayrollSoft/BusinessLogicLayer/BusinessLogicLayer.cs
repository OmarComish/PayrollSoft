using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using PayrollSoft.Models;
using PayrollSoft.Utility;

namespace PayrollSoft.BusinessLogicLayer
{
    public class overtimeParams
    {
        
        public string overtimePayCode { get; set; }
        public Double overtimeRate { get; set; }
        public double totalAllowableHoursPerDay { get; set; }
        public double totalAllowableDaysPerMonth { get; set; }
    }

    public class loanRepaymentReturnValues
    {
        public double interest { get; set; }
        public double principalRepayment { get; set; }
        public double loanBalanceAmount { get; set; }
    }

    public class EmployeeSalary
    {

        private UtilityBase util = new UtilityBase();
        private DataContext db = new DataContext();

        private string empId;
        private Double basicSal;
        private string groupEligible;
        private double totaldeductions;
        private double totalEarnings;
        

        public EmployeeSalary(string id, string grade)
        {
            this.empId = id;
            this.groupEligible = grade;

        }

        public Double salary()
        {
            try
            {
                var sal = from e in db.Earnings
                          where e.groupEligible == this.groupEligible && e.payRate == 0
                          select new { e.actualPayAmount };

                foreach (var x in sal)
                {
                    this.basicSal = x.actualPayAmount;
                }
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString() + " Exception: " + e.InnerException.ToString());
            }

            return basicSal;
        }

        public double getDeductions()
        {
            try
            {
                using (var context = new DataContext())
                {
                    var deductionsTotal = from d in context.DeductionPayments
                                          where d.EmpID == this.empId && d.voided == 0
                                          group d by d.EmpID into deductionGroup
                                          select new { totalDeduction = deductionGroup.Sum(x => x.ActualAmount) };

                    foreach (var d in deductionsTotal)
                    {
                        this.totaldeductions = d.totalDeduction;
                    }
                }
            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString() + " Exception: " + e.InnerException.ToString());
            }

            return this.totaldeductions;
        }

        public double getEarnings()
        {
            try
            {
                using (var context = new DataContext())
                {
                    var earningsTotal = from e in context.EarningPayments
                                        where e.EmpID == this.empId && e.voided == 0
                                        group e by e.EmpID into earningGroup
                                        select new { totalEarning = earningGroup.Sum(y => y.ActualAmount) };

                    foreach (var t in earningsTotal)
                    {
                        this.totalEarnings = t.totalEarning;
                    }
                }

            }
            catch (Exception e)
            {
                util.WriteToLog(e.Message.ToString() + " Exception: " + e.InnerException.ToString());
            }


            return this.totalEarnings;
        }

    }

    public class loanRepayment
    {
        private DataContext db = new DataContext();
        private string refnumber;
        private DateTime EndDate, StartDate;
        private string tableName, id;
        private int rowCount;
        loanRepaymentReturnValues loanrepaymentvals = new loanRepaymentReturnValues();



        public int countRowsPerRefNumber(string tableName, string id)
        {
            switch (this.tableName)
            {
                case "loanMaster":
                    //TODO: code for counting the occurence of a specific loan payment in the table filtered by the referenceNumber

                    break;

                case "LoanPortifolio":

                    var loanrefnumCount = from Amort in db.LoanPortifolios
                                          where Amort.LoanRefNumber == this.refnumber
                                          select new { Amort.LoanRefNumber };
                    this.rowCount = loanrefnumCount.Count();

                    break;

                case "LoanType":

                    break;

            }

            return this.rowCount;
        }

        public loanRepaymentReturnValues initialLoanBalanceRecord(string refNumber)
        {


            var resultset = from lmaster in db.LoanMasters
                            join ltype in db.LoanTypes on lmaster.LoanTypeNumber equals ltype.Code
                            where lmaster.LoanRefNumber == refNumber
                            select new { lmaster.LoanAmount, ltype.InterestRate, lmaster.MonthlyRepayment };

            foreach (var resultSet in resultset)
            {
                loanrepaymentvals = loanBalance(resultSet.LoanAmount, resultSet.InterestRate, resultSet.MonthlyRepayment);
            }


            return loanrepaymentvals;
        }

        public loanRepaymentReturnValues LoanBalanceRecord(string refNumber, DateTime endDate)
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

            return loanrepaymentvals;
        }

        public double PMT(double yearlyInt, int numbrOfmnths, double loanAmount)
        {
            var rate = (double)yearlyInt / 1200;
            var denominator = Math.Pow((1 + rate), numbrOfmnths) - 1;
            return (rate + (rate / denominator)) * loanAmount;
        }

        private loanRepaymentReturnValues loanBalance(double loanAmount, double interestRate, double pmt)
        {

            loanRepaymentReturnValues loanreturnvals = new loanRepaymentReturnValues();
            loanreturnvals.interest = (interestRate / 100) * loanAmount;
            loanreturnvals.principalRepayment = pmt - loanreturnvals.interest;
            loanreturnvals.loanBalanceAmount = loanAmount - loanreturnvals.principalRepayment;
            return loanreturnvals;
        }

    }

    public class utilityfunctions
    {
        private DataContext db = new DataContext();
        private string tblname;
        private string rowId;
        private string fieldname;
        public int count;

        public utilityfunctions(string table, string Id = null, string fieldname = null)
        {
            this.tblname = table;
            this.rowId = Id;
            this.fieldname = fieldname;
        }

        public int rowsCount()
        {
            var rows = from r in db.LoanAmortizations
                       select new { r.EntryID };

            return count = rows.Count();


        }

        //TODO: create a generic method that retrieves the count of rows from a table.
        //use the select case construct to determine the specific table the code is required to perform the operation
        public int rowCount()
        {
            switch (this.tblname)
            {
                case "AllowanceCategories":

                    //perform a general row count without a filter
                    var rows = from r in db.AllowanceCategories
                               select new { r.CategoryID };
                    count = rows.Count();

                    break;
                case "Allowances":

                    var alcatg = from r in db.AllowanceCategories
                                 select new { r.CategoryID };
                    count = alcatg.Count();

                    break;
                case "AppConfig_LeavePay":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter

                        var appleaverows = from r in db.AppConfig_LeavePay
                                           select new { r.ID };
                        count = appleaverows.Count();
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "AttendanceLogs":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "creditGeneralLedger":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "DeductionPayments":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "Deductions":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "Payments":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "Departments":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "EarningPayments":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "Earnings":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "Employees":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "EmployeeGrades":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "InsuranceCategories":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "Insurances":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "LoanAmortization":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter

                        var loanAmortrows = from r in db.LoanAmortizations
                                            select new { r.LoanRefNumber };
                        count = loanAmortrows.Count();
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                        var loanAmortrows = from r in db.LoanAmortizations
                                            where r.LoanRefNumber == this.rowId
                                            select new { r.LoanRefNumber };
                        count = loanAmortrows.Count();
                    }
                    break;
                case "LoanMasters":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter

                        var loanrows = from r in db.LoanMasters
                                       select new { r.LoanRefNumber };
                        count = loanrows.Count();
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "LoanTypes":
                    if (this.rowId == null)
                    {

                        var loantyperows = from r in db.LoanTypes
                                           select new { r.Code };
                        count = loantyperows.Count();
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "PaymentCodes":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "PaymentTypes":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "Payrolls":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "PayrollHistLogs":
                    if (this.rowId == null)
                    {
                        //perform a general row count without a filter
                        var logs = from r in db.PayrollHistLogs
                                   select new { r.LogID };
                        count = logs.Count();
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "Pensions":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "PriorityCodes":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "Settings":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "Taxations":
                    if (this.rowId == null)
                    {
                        //perform a general row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
                case "TaxationThresholds":
                    if (this.rowId == null)
                    {
                        //perform a eneral row count without a filter
                    }
                    else
                    {
                        //perform a row count using rowid and fieldname as filter
                    }
                    break;
            }

            return count;

        }


    }

    public class SeverancePayRates
    {
        public double firstThreshold { get; set; }
        public double secondThreshold { get; set; }
        public double thirdThreshold { get; set; }
        public int firstThresholdMinPeriod { get; set; }
        public int firstThresholdMaxPeriod { get; set; }
        public int secondThresholdMinPeriod { get; set; }
        public int secondThresholdMaxPeriod { get; set; }
        public int thirdThresholdMinPeriod { get; set; }
        public int thirdThresholdMaxPeriod { get; set; }
    }

    public class SeverancePay
    {
        private SeverancePayRates thresholdRates = new SeverancePayRates();
        private DataContext _context = new DataContext();
        private UtilityBase _util = new UtilityBase();
        private double __wages;
        private int __periodOfservce;
        private int firstdiff; //used to keep of the years if period has exceeded  5 yrs
        private int secdiff; //used to keep of the years if period has exceeded 10 yrs
        private double Amount;  //total amount of payment of each segment or threshhold

        public SeverancePay(double __wages, int __periodofservice)
        {
            this.__wages = __wages;
            this.__periodOfservce = __periodofservice;

        }

        public double SeverancePayAmount()
        {
            thresholdRates = PayRates();
            if (__periodOfservce > thresholdRates.firstThresholdMaxPeriod)
            {
                firstdiff = __periodOfservce - thresholdRates.firstThresholdMaxPeriod;
                Amount = (thresholdRates.firstThreshold / 100) * __wages * thresholdRates.firstThresholdMaxPeriod;

                if (firstdiff > thresholdRates.firstThresholdMaxPeriod)
                {
                    secdiff = firstdiff - thresholdRates.firstThresholdMaxPeriod;
                    Amount += (thresholdRates.secondThreshold / 100) * __wages * thresholdRates.firstThresholdMaxPeriod;

                    Amount += (thresholdRates.thirdThreshold / 100) * __wages * secdiff;
                }
                else
                {
                    Amount += (thresholdRates.secondThreshold / 100) * __wages * firstdiff;
                }
            }
            else
            {
                Amount = (thresholdRates.firstThreshold / 100) * __wages * __periodOfservce;
            }

            return Amount;

        }

        private SeverancePayRates PayRates()
        {
            SeverancePayRates __severance = new SeverancePayRates();
            try
            {
                using (var context = new DataContext())
                {
                    var result = from svp in context.SeveranceEarnings
                                 select new
                                 {
                                     svp.RefNumber,
                                     svp.FirstThresholdRate,
                                     svp.SecondThresholdRate,
                                     svp.ThirdThresholdRate,
                                     svp.FirstThresholdMinPeriod,
                                     svp.FirstThresholdMaxPeriod,
                                     svp.SecondThresholdMinPeriod,
                                     svp.SecondThresholdMaxPeriod,
                                     svp.ThirdThresholdMinPeriod,
                                     svp.ThirdThresholdMaxPeriod
                                 };
                    foreach (var x in result)
                    {
                        __severance.firstThreshold = x.FirstThresholdRate;
                        __severance.secondThreshold = x.SecondThresholdRate;
                        __severance.thirdThreshold = x.ThirdThresholdRate;
                        __severance.firstThresholdMinPeriod = x.FirstThresholdMinPeriod;
                        __severance.secondThresholdMinPeriod = x.SecondThresholdMinPeriod;
                        __severance.thirdThresholdMinPeriod = x.ThirdThresholdMinPeriod;
                        __severance.firstThresholdMaxPeriod = x.FirstThresholdMaxPeriod;
                        __severance.secondThresholdMaxPeriod = x.SecondThresholdMaxPeriod;
                        __severance.thirdThresholdMaxPeriod = x.ThirdThresholdMaxPeriod;
                    }

                }
            }
            catch (Exception e)
            {
                _util.WriteToLog(e.Message.ToString() + " " + e.InnerException.Message.ToString());

            }

            return __severance;

        }
    }

    public class PensionRates
    {
        public double employeeContribRate { get; set; }
        public double employerContribRate { get; set; }
        public double groupLifeAssuranceRate { get; set; }
        public double adminFeeRate { get; set; }
        public double brokerageRate { get; set; }
    }

    public class PensionDeduction
    {
        private PensionRates _pensionrates = new PensionRates();
        private double _basicSalary, Amount, brokerageFeeAmount, AdminFeeAmount, employerAmount, grouplifeAssuranceAmount;
        private string __empId;
        private DataContext _context = new DataContext();
        private UtilityBase _util = new UtilityBase();

        public PensionDeduction(double __basicsal, string __Id)
        {
            this._basicSalary = __basicsal;
            this.__empId = __Id;
        }

        private void InitParams()
        {
            using (var context = new DataContext())
            {
                try
                {
                    var results = from p in context.Pensions
                                  select new { p.AdminiFeeRate, p.EmployeeContrRate, p.EmployerContrRate, p.BrokerageFeeRate, p.GroupLifeAssuranceRate };

                    foreach (var p in results)
                    {
                        _pensionrates.adminFeeRate = p.AdminiFeeRate;
                        _pensionrates.employeeContribRate = p.EmployeeContrRate;
                        _pensionrates.employerContribRate = p.EmployerContrRate;
                        _pensionrates.brokerageRate = p.BrokerageFeeRate;
                    }
                }
                catch (Exception e)
                {
                    _util.WriteToLog(e.Message.ToString() + " " + e.InnerException.Message.ToString());
                }
            }

        }

        public double CalculatePension()
        {
            InitParams();

            Amount = ((_pensionrates.employeeContribRate * _basicSalary) / 100);
            brokerageFeeAmount = ((_pensionrates.brokerageRate * _basicSalary) / 100);
            AdminFeeAmount = ((_pensionrates.adminFeeRate * _basicSalary) / 100);
            employerAmount = ((_pensionrates.employerContribRate * _basicSalary) / 100);
            grouplifeAssuranceAmount = ((_pensionrates.groupLifeAssuranceRate * _basicSalary) / 100);

            SavePensionToAccount();
            return Amount;
        }

        private Boolean DuplicateFound(string Id)
        {
            ///<summary>
            /// Method: DuplicateFound -> checks if the entry into the PensionAccount exist to avoid saving duplicate entries
            ///         Params: Id of the employee, and within the LINQ query of this method, it also uses the current date as another parameter
            ///         Returns: Boolean value. False if entry is absent and true otherwise
            ///</summary>
            
            Boolean found;
            using (var context = new DataContext())
            {
                
                DateTime _date = DateTime.Now.Date;
                var results = from pa in context.PensionAccounts
                              where pa.EmpId == Id && pa.EntryDate == _date
                              select new {pa.EmpId };

                found = results.Count() != 0 ? true : false;
            }
            return found;
        }

        private void SavePensionToAccount()
        {
            PensionAccount account = new PensionAccount();
            account.Id = 1;
            account.EmpId = this.__empId;
            account.EmployeeContribution = this.Amount;
            account.EmployerContribution = employerAmount;
            account.AdministrationFee = AdminFeeAmount;
            account.BasicSalary = this._basicSalary;
            account.BrokerageFee = brokerageFeeAmount;
            account.GroupLifeAssurance = grouplifeAssuranceAmount;
            account.EntryDate = DateTime.Now.Date;


            if (!DuplicateFound(this.__empId))
            {
                using (var context = new DataContext())
                {
                    try
                    {
                        context.PensionAccounts.Add(account);
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        _util.WriteToLog(e.Source.ToString() + e.InnerException.ToString());
                    }

                }
            }
        }

    }

    public class PendingTransaction
    {
        /// <summary>
        /// Method: LogPendingTransaction ->Logs the pending transactions
        ///        Params: __itemId -> holds the id of the item whose status is pending
        ///                __source -> module where the item is
        ///                __controller -> the controller the action is invoked from
        ///                __object -> the model which is database table with the pending item
        ///                __creator -> the user who created/modified the item.
        ///        Returns: Currently it is returning nothing (void)
        ///        
        /// Method: ClearPendingTransaction -> clears the pending transactions
        ///         Params: none, but uses the argument initialized on the contructor to lookup for the transaction
        ///                 in PendingItem object
        ///         Returns: void
        ///        
        ///  
        /// </summary>

        private UtilityBase _util = new UtilityBase();
        private string __itemId, __source, __controller, __object;
        private int __creator;

        public PendingTransaction(string __itemId, int __initiator, string __source, string __controller, string __object)
        {
            this.__itemId = __itemId;
            this.__creator = __initiator;
            this.__source = __source;
            this.__object = __object;
            this.__controller = __controller;
        }

        public void LogPendingTransaction()
        {
            PendingItem item = new PendingItem();
            item.ReferenceNumber = __itemId;
            item.Initiator = __creator;
            item.Source = __source;
            item.Controller = __controller;
            item.TimeStamp = DateTime.Now;
            using (var context = new DataContext())
            {
                try
                {
                    context.PendingItems.Add(item);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    _util.WriteToLog(e.Message.ToString() + " " + e.Source.ToString());
                }
            }
        }

        public void ClearPendingTransaction()
        {
            using (var context = new DataContext())
            {
                try
                {
                    PendingItem pendingitem = context.PendingItems.FirstOrDefault(x => x.ReferenceNumber == __itemId);
                    context.PendingItems.Remove(pendingitem);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    _util.WriteToLog(e.Message.ToString() + " " + e.Source.ToString());
                }

            }
        }
    }

    public class CreatePaymentCode
    {
        private UtilityBase _util = new UtilityBase();

        public Boolean AddPaymentCode(string __code, string __description, int __creator)
        {
             using(var context = new DataContext())
             {
                 PaymentCodes paymentcode = new PaymentCodes();
                 Boolean created = false;
                 paymentcode.payCode = __code;
                 paymentcode.Description = __description;

                try 
	            {
                    if(context.PaymentCodes.Any(x => x.payCode != __code))
                    {
                        context.PaymentCodes.Add(paymentcode);
                        context.SaveChanges();
                        created = true;
                    }
                
	            }
	            catch (Exception e)
	            {
                    _util.WriteToLog(e.InnerException.Message.ToString() + " " + e.Source.ToString());
	            }

                return created;
             }
        }
    }

    public class OverTimePayment
    {
        private overtimeParams overtimeparams = new overtimeParams();
        private UtilityBase _util = new UtilityBase();
        private double __hoursworked, __overtimePay;
        private string __payCode, __empId, __empGrade;

        public OverTimePayment(string __paycode, string __empId, string __grade, double __hours)
        {
           this. __hoursworked = __hours;
           this.__payCode = __paycode;
           this.__empId = __empId;
           this.__empGrade = __grade;
        }

        private void InitParams()
        {
            using(var context = new DataContext())
            {
                //Overtime oparams = context.OverTimes.Find(__payCode);
                List<overtimeParams> oparams = (from ot in context.OverTimes
                                                where ot.Code == __payCode
                                                select new overtimeParams {totalAllowableHoursPerDay = ot.AllowableWorkHours, totalAllowableDaysPerMonth = ot.AllowableWorkDays, overtimeRate = ot.Rate }).ToList();
                foreach (var x in oparams)
                {
                    overtimeparams.totalAllowableDaysPerMonth = x.totalAllowableDaysPerMonth;
                    overtimeparams.totalAllowableHoursPerDay = x.totalAllowableHoursPerDay;
                    overtimeparams.overtimeRate = x.overtimeRate;
                }

            }
        }

        public double OverTimePay()
        {
            InitParams();
            double __basicPay = BasicPay();
            double __dailyPay = (__basicPay / this.overtimeparams.totalAllowableDaysPerMonth);
            double __hourlyPay = (__dailyPay / this.overtimeparams.totalAllowableHoursPerDay);
            __overtimePay =( __hourlyPay * overtimeparams.overtimeRate * __hoursworked);

            return __overtimePay;
        }

        private double BasicPay()
        {
            EmployeeSalary empsal = new EmployeeSalary(this.__empId, this.__empGrade);
            return empsal.salary();
        }

    }

    public class StraightLineLoanComputation
    {
        private double __period, __loanAmount, __interestrate;

        public StraightLineLoanComputation( double _period, double __amount, double __rate)
        {
            __period = _period;
            __loanAmount = __amount;
            __interestrate = __rate;
        }
        public double MonthlyRepayment()
        {
            double interest = ((__interestrate * __loanAmount) / 100);
            return ((__loanAmount + interest)/__period);
        }
        public double LoanBalance()
        {
            double interest = ((__interestrate * __loanAmount) / 100);
            return __loanAmount + interest;
        }
    }

    public class ReducingBalanceComputation
    {
        private double __period;
        private double __loanAmount, __yearlyInt;
        public ReducingBalanceComputation(int _numofMonths, double __loanAmount, double __interestRate)
        {
            this.__period = _numofMonths;
            this.__loanAmount = __loanAmount;
            this.__yearlyInt = __interestRate;
        }
        public double PMT()
        {
            var rate = (double)__yearlyInt / 1200;
            var denominator = Math.Pow((1 + rate), __period) - 1;
            return (rate + (rate / denominator)) * __loanAmount;
        }

        public double FV(double monthlypayments)
        {
            var rate = (double)__yearlyInt / 100;
            var divident = Math.Pow((1 + rate), __period) - 1;
            double __fv = monthlypayments * (divident / rate);

            return Math.Round(__fv,2);
        }
    }

    public class WithHoldingTaxParams
    {
        public double Rate { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public int ThresholdNumber { get; set; }
    }

    public class WithholdingTaxDeduction
    {
        private UtilityBase _util = new UtilityBase();
        private double[] Amount;
        private double Diffs;
        private double[] Rate;
        private double[] MinAmount;
        private double[] MaxAmount;
        private int[] _sequenceNumber;
        private int ThresholdCount = 0;
        private int __currentThreshold = 1;
        private double __withHoldingTaxTotal, __beforeTaxTotal;


        public WithholdingTaxDeduction(double __beforeTaxAmount)
        {
           this.__beforeTaxTotal  = __beforeTaxAmount;
        }

        private List<WithHoldingTaxParams> WithHoldingTaxParameters()
        {

            using (var context = new DataContext())
            {
                List<WithHoldingTaxParams> results = new List<WithHoldingTaxParams>();
                try
                {
                    string taxcode = ActiveTaxation();
                    results = (from wt in context.WithHoldingTaxes
                                where wt.TaxRefCode == taxcode
                                select new WithHoldingTaxParams { Rate = wt.Rate, ThresholdNumber = wt.ThresholdNumber, MinAmount = wt.MinAmount, MaxAmount = wt.MaxAmount }).ToList();

                    ThresholdCount = results.Count();

                    
                }
                catch (Exception e)
                {
                    _util.WriteToLog(e.InnerException.ToString() + " " + e.Source.ToString());
                }

                return results;
            }
        }

        private string ActiveTaxation()
        {
            using (var context = new DataContext())
            {
                Taxation code = context.Taxations.Where(x => x.Active == 1).FirstOrDefault();
                return code.TaxRefCode;
            }
        }

        private void InitParams()
        {
            int __index = 0;
            List<WithHoldingTaxParams> tax = WithHoldingTaxParameters();

            this.Rate = new double[ThresholdCount];
            this.MinAmount = new double[ThresholdCount];
            this.MaxAmount = new double[ThresholdCount];
            this.Amount = new double[ThresholdCount];
            this._sequenceNumber = new int[ThresholdCount];
           
            foreach (var val in tax)
            {
               
                this.Rate[__index] = val.Rate;
                this.MinAmount[__index] = val.MinAmount;
                this.MaxAmount[__index] = val.MaxAmount;
                this._sequenceNumber[__index] = val.ThresholdNumber;
               
                __index++;
            }
        }

        public double CalculateWithHoldingTax()
        {
            ///<summary>
            ///Calll the InitParams method to instatiate the varables used in the calculation
            ///</summary>

            InitParams();

            for (int i = 0; i < ThresholdCount; i++)
            {
                if (__currentThreshold == 1)
                {
                    if(__beforeTaxTotal < MinAmount[i])
                    {
                        Diffs = __beforeTaxTotal;
                    }
                    if (__beforeTaxTotal >= MinAmount[i])
                    { 
                        Amount[i] = (MaxAmount[i] * (Rate[i] / 100));
                        Diffs = __beforeTaxTotal - MaxAmount[i];
                    }

                    __currentThreshold ++;
                }

                if(__currentThreshold == 2)
                {
                    if((Diffs > MinAmount[i+1]) || (Diffs >= MaxAmount[i+1]))
                    {
                        Amount[1] = (MaxAmount[1] * (Rate[1] / 100));
                        Diffs = Diffs - MaxAmount[1];
                    }
                    __currentThreshold ++;
                }

                if ((_sequenceNumber[i] > 2) && (_sequenceNumber[i] < ThresholdCount))
                {
                    if ((Diffs >= MinAmount[i]) || (Diffs <= MaxAmount[i]))
                    {
                        Amount[i] = (Diffs * (Rate[i] / 100));
                        Diffs = Diffs - Amount[i];
                    }
                    __currentThreshold ++;
                }
               
                if ((_sequenceNumber[i] == ThresholdCount))
                {
                    if (Diffs >= MinAmount[ThresholdCount - 1])
                    {
                        Amount[ThresholdCount - 1] = (Diffs * (Rate[ThresholdCount - 1] / 100));
                    }
                }
                
            }

            for (int x = 0; x < ThresholdCount; x ++ )
            {
                __withHoldingTaxTotal += Amount[x];
            }

            return __withHoldingTaxTotal;
        }
    }
}

public class Loan
{
    private double yearlyInterest;
    private int totalNumberOfMonths;
    private double loanAmount;

    public Loan(double yearlyInt, int numbrOfmnths, double loanamount)
    {
        this.yearlyInterest = yearlyInt;
        this.totalNumberOfMonths = numbrOfmnths;
        this.loanAmount = loanamount;
    }

    private  double PMT()
    {
        var rate = (double)this.yearlyInterest / 100 / 12;
        var denominator = Math.Pow((1 + rate), this.totalNumberOfMonths) - 1;
        return (rate + (rate / denominator)) * this.loanAmount;
    }


}