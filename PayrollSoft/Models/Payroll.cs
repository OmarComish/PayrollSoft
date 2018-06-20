using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class Payroll
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string EmpID { get; set; }
        public DateTime payrollRunDate { get; set; }
        public string PayrollCode { get; set; }
 
    }
}