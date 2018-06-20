using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace PayrollSoft.Models
{
    public class TaxationThreshold
    {
        [Key]
        public string ThresholdID { get; set; }
        [Required]
        public string Tax_Ref_Code { get; set; }
        public Decimal firstThresholdLimit { get; set; }
        public Decimal secondThresholdLimit { get; set; }
        public Decimal thirdThresholdLimit { get; set; }
        public Double rateOffirstThreshold { get; set; }
        public Double rateOfsecondThreshold { get; set; }
        public Double rateOfthirdThreshold { get; set; }

        //public virtual ICollection<Taxation> Taxation { get; set; }
        public virtual Taxation Taxations { get; set; }
        public virtual EmployeeGrade EmployeeGrades { get; set; }

    }
}