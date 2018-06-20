using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class creditGeneralLedger
    {
        [Key]
        public string glCode { get; set; }
        public string payCode { get; set; }
        public double Amount { get; set; }
        public string EmpId { get; set; }
        public DateTime? DATE { get; set; }

        //public virtual ICollection<Deductions> Deductions { get; set; }
        public virtual Deductions Deductions { get; set; }

    }
}