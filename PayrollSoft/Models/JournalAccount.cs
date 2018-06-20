using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class JournalAccount
    {
        [Key]
        public int id { get; set; }
        public string transactionType { get; set; }
        public string transactionCode { get; set; }
        public double Amount { get; set; }
        public string Account { get; set; }
        public DateTime? Period { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public int Voided { get; set; }
        public int VoidedBy { get; set; }
        public DateTime? DateVoided { get; set; }
    }
}