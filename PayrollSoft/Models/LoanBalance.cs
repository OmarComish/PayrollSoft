using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayrollSoft.Models
{
    public class LoanBalance
    {
        public double loanBalanceAmount { get; set; }
        public double interest { get; set; }
        public double principalRepayment { get; set; }
        
    }
}