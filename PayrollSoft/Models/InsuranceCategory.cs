using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class InsuranceCategory
    {
        [Key]
        public string CategoryID { get; set; }
        [Required]
        public string InsuranceCode { get; set; }
        public string Description { get; set; }
        [Required]
        public Double MonthlyRate { get; set; }

        public virtual Insurance Insurances { get; set; }
    }
}