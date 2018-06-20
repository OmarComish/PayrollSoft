using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace PayrollSoft.Models
{
    public class EmployeeGrade
    {
        [Key]
        public string GradeId { get; set; }
        [Required]
        public string GradeName { get; set; }
        [Required]
        public Double MinSalary { get; set; }
        [Required]
        public string AllowanceRefNum { get; set; }
        [Required]
        public string InsuranceCode { get; set; }
        [Required]
        public string PensionRefNum { get; set; }
        [Required]
        public Double OvertimeRate { get; set; }
        [Required]
        public string ThresholdID { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual Allowance Allowance { get; set; }
        public virtual Pension Pension { get; set; }
        public virtual ICollection<TaxationThreshold> TaxationThresholds { get; set; }
        //public virtual Earnings Earnings { get; set; }
    }
}