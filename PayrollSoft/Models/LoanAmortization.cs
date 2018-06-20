using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class LoanAmortization
    {
        [Key]
        public int EntryID { get; set; }
        public string LoanRefNumber { get; set; }
        public DateTime EntryDate { get; set; }
        public double InterestPaid { get; set; }
        public double PrincipalPaid { get; set; }
        public double LoanBalance { get; set; }
        public int EndOfPeriod { get; set; }
    }
}