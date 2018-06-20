using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class LoanRegister
    {
        [Key]
        public int Id { get; set; }
        public string LoanRefNumber { get; set; }
        public double LoanBalance { get; set; }
        public int PeriodToUpdate { get; set; }
        public DateTime NextDateOfUpdate { get; set; }
    }
}