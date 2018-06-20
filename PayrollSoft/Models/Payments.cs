using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayrollSoft.Models;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class Payments
    {
        [Key]
        public string paymentNumber { get; set; }
        public string payCode { get; set; }
        public double ActualAmount { get; set; }
        public string EmpID { get; set; }
        public string DATE { get; set; }

        //public virtual ICollection<Earnings> Earnings { get; set; }
        //public virtual Deductions Deductions { get; set; }
        
    }
}