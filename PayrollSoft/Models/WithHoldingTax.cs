using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class WithHoldingTax
    {
        [Key]
        public int Id { get; set; }
        public string TaxRefCode { get; set; }
        public double Rate { get; set; }
        public double MinAmount { get; set; }
        public double MaxAmount { get; set; }
        public int ThresholdNumber { get; set; }
        public int RecordStatusId { get; set; }
        public int RecordStatusChangedBy { get; set; }
        public DateTime RecordStatusDateChanged { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
       
    }
}