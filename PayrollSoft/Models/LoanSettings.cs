using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class LoanSettings
    {
        [Key]
        public string ID { get; set; }
        public string AutoGenerateLoanRefNum { get; set; }
        public double MaxLoanRepaymentPeriod { get; set; }
        public string EnablePrecalculation { get; set; }
    }
}