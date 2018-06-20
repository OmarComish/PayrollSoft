
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayrollSoft.Models;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class DeductionPayments
    {
        [Key]
        public string paymentNumber { get; set; }
        public string payCode { get; set; }
        public double ActualAmount { get; set; }
        public string EmpID { get; set; }
        public DateTime DATE { get; set; }
        public int voided { get; set; }
        public DateTime voidedDate { get; set; }
        public string voidedBy { get; set; }
    }
}