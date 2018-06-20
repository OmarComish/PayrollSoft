using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class LeaveType
    {
        [Key]
        public string LeaveTypeId { get; set; }
        public string Description { get; set; }
        public double AnnualEntitlement { get; set; }
        public string GroupEligible { get; set; }
        public int EmailAlerts { get; set; }
        public string PaymentCalculationFormular { get; set; } //basically, this is the PaymentCode
        public string LeavePayFrequency { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public int Voided { get; set; }
        public int VoidedBy { get; set; }
        public DateTime DateVoided { get; set; }
        public int RecordStatusId { get; set; }
        public int RecordStatusChangedBy { get; set; }
        public DateTime RecordStatusDateChanged { get; set; }


    }
}