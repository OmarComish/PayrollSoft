using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class loanMaster
    {
        [Key]
        public string LoanRefNumber { get; set; }
        public string LoanTypeNumber { get; set; }
        public string Formular { get; set; }
        public string EmpID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PaybackPeriods { get; set; }
        public double LoanAmount { get; set; }
        public double MonthlyRepayment { get; set; }
        public string Status { get; set; }
        public int PeriodToUpdate { get; set; }
        public DateTime NextDateOfUpdate { get; set; }
        public double LoanBalance { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public int RecordStatusId { get; set; }
        public int RecordStatusChangedBy { get; set; }
        public DateTime RecordStatusDateChanged { get; set; }
    }
}