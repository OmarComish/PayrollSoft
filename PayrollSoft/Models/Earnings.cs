using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class Earnings
    {


        [Key]  
        public string payCode { get; set; }
        public string payType { get; set; }
        public string payCodeDescription { get; set; }
        public string payTypeDescription { get; set; }
        public string groupEligible { get; set; } //Alias in the view is groupEligible  //[ForeignKey("GradeId")]
        public string groupEligibleDescription { get; set; }
        public string debitGL { get; set; }
        public string debitGlDescription { get; set; }
        public Double payRate { get; set; }
        public string rateDerivedSource { get; set; }
        public double actualPayAmount { set; get; }
        public string payMethod { get; set; }
        public int timesheetEntryAllowed { get; set; }
        public int paidAsEarnings { get; set; }
        public int taxableIncome { get; set; }
        public int priority { get; set; }
        public string higherPrecedenceFactor { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int Voided { get; set; }
        public int VoidedBy { get; set; }
        public DateTime DateVoided { get; set; }

        //public virtual ICollection<EmployeeGrade> EmployeeGrade {get;set;}
       // public virtual Payments Payments { get; set; }
        

    }
}