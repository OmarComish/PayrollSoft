using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    /// <summary>
    /// Payslip model which extends PayrollHistLog collection
    /// </summary>
    public class Payslip: List<PayrollHistLog>
    {
        [Key]
        public string payslipNum { get; set; }
        public string ImageUrl { get; set; }
    }


}