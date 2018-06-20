using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class AllowanceCategory
    {
        [Key]
        public string CategoryID { get; set; }
        public string CategoryDescription { get; set; }

        public ICollection<Allowance> Allowance { get; set; }
    }
}