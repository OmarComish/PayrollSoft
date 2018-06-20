using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class PayrollPayments
    {
        [Key]
        public string EmpId { get; set; }
        public IEnumerable<EarningPayments> payrollEarnings { get; set; }
        public IEnumerable<DeductionPayments> payrollDeductions { get; set; }
    }
}