using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace PayrollSoft.Models
{
    public class Insurance
    {
        [Key]
        public string InsuranceCode { get; set; }
        public string Description { get; set; }
        public string ServiceProvider { get; set; }

        public virtual ICollection<InsuranceCategory> InsuranceCategories { get; set; }
    }
}