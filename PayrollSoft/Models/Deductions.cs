using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class Deductions
    {


        [Key]
        public string payCode { get; set; }
        public string payType { get; set; }
        public string payCodeDescription { get; set; }
        public string payTypeDescription { get; set; }
        public string gradeId { get; set; } //Alias in the view is groupEligible
        public string groupEligibleDescription { get; set; }
        public string creditGl { get; set; }
        public string creditGlDescription { get; set; }
        public int priorityCode { get; set; }
        public string priorityDescription { get; set; }
        public int includeInretrospectPayments { get; set; }
        public int deductionTakenFromEmployee { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int Voided { get; set; }
        public int VoidedBy { get; set; }
        public DateTime DateVoided { get; set; }

        public virtual EmployeeGrade EmployeeGrade { get; set; }
        public virtual PriorityCodes PriorityCodes { get; set; }
        //public virtual creditGeneralLedger creditGeneralLedger { get; set; }
        public virtual ICollection<creditGeneralLedger> creditGeneralLedger { get; set; }
        //public virtual ICollection<Payments> Payments { get; set; }
        //public virtual Payments Payments { get; set; }
    }
}