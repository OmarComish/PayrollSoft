using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class PayrollFormulae
    {
        [Key]
        public int Id { get; set; }
        public string Paycode { get; set; }
        public string Formular { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }

    }
}