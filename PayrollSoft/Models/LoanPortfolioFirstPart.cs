using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class LoanPortfolioFirstPart
    {
        [Key]
        public string EntryNumber { get; set; }
        public string LoanRefNumber { get; set; }
        public DateTime EntryDate { get; set; }
        public int EndOfPeriod { get; set; }
    }
}