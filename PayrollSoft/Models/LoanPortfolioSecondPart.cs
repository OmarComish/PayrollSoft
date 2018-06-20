using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class LoanPortfolioSecondPart
    {
        [Key]
        public string EntryNumber { get; set; }
        public string LoanRefNumber { get; set; }
        public double InterestPaid { get; set; }
        public double PrincipalPaid { get; set; }
        public double LoanBalance { get; set; }
    }
}