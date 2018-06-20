using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    
    public class LoanDetails
    {
        
        [Key]
        public string LoanRefNumber { get; set; }
        public string LoanTypeNumber { get; set; }
        public string EmpID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PaybackPeriods { get; set; }
        public double LoanAmount { get; set; }
        public double MonthlyRepayment { get; set; }
        public string Active { get; set; }
    }
}