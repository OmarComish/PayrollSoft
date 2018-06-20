using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class LeaveRequest
    {
        [Key]
        public int RequestId { get; set; }
        public string EmployeeId { get; set; }
        public string LeaveTypeId { get; set; }
        public DateTime LeaveFrom { get; set; }
        public DateTime LeaveTo { get; set; }
        public double DaysRequested { get; set; }
        public string Reason { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int Modified { get; set; }
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