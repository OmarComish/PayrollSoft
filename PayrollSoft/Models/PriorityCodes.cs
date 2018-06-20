using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class PriorityCodes
    {
        [Key]
        public int PriorityCode { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int Voided { get; set; }
        public int VoidedBy { get; set; }
        public DateTime DateVoided { get; set; }

        public virtual ICollection<Deductions> Deductions { get; set; }
    }
}