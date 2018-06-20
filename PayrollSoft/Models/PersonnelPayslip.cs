using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class PersonnelPayslip
    {
        [Key]
        public int PayslipNum { get; set; }
        public IEnumerable<EarningPayments> PersonnelEarnings { get; set; }
        public IEnumerable<DeductionPayments> PersonnelDeductions { get; set; }
        public IEnumerable<LoanPortifolio> LoanDeductions { get; set; }
        public IEnumerable<loanMaster> InitialLoanAmount { get; set; }
        public IEnumerable<EarningPayments> EmployeeGrossPay { get; set; }
        public IEnumerable<PayrollHistLog>NetSalaryPay { get; set; }
        public IEnumerable<PayrollHistLog>PayAsYouEarn { get; set; }
        public string ImageUrl { get; set; }

        //add LoanPortifolio type,LoanMasters and other Employee details we might need on Payslip
    }
}