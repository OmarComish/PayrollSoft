using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class PayrollHistLog
    {  
        
        
        [Required]
        public int LogID { get; set; }
        [Key]
        public string EmpID { get; set; }
        public DateTime dateStamp { get; set; }
        public double NetPay { get; set; }
        public double GrossPAY { get; set; }
        public double PAYE { get; set; }
        public Double Earnings { get; set; }
        public Double OvertimePay { get; set; }
        public Double Deductions {get;set;}
        public Double PensionContribution { get; set; }

    }
}