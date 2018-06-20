using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class FrozenLoan
    {
        [Key]
        public string LoanRefNumber { get; set; }
        public DateTime ResumeDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int Voided { get; set; }
        public int VoidedBy { get; set; }
        public DateTime DateVoided { get; set; }
    }
}