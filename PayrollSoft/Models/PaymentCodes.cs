using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class PaymentCodes
    {
        [Key]
        public string payCode { get; set; }
        public string Description { get; set; }
    }
}