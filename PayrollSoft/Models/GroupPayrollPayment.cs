using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class GroupPayrollPayment
    {
        [Key]
        public int LogId { get; set; }
        public string paymentNumber { get; set; }
        public string EmpId {get;set;}
        public string payCode { get; set; }
        public double ActualAmount { get; set; }
        public DateTime DATE { get; set; }

    }
}