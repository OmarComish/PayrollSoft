using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class Allowance
    {
        [Key]
        public string AllowanceRefNum { get; set; }
        public string Description { get; set; }
        public Double allowanceRate { get; set; }
        public string CategoryID { get; set; } //foreign key from the AllowanceCategory Model.

        //public virtual ICollection<EmployeeGrade> EmployeeGrades {get;set;}
        public virtual Allowance Allowances { get; set; }
    }
}