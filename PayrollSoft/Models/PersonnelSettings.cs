using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class PersonnelSettings
    {
        [Key]
        public int Id { get; set; }
        public int AutogenerateEmpNums { get; set; }
        public int MaxEmploymentAge { get; set; }
        public int MinEmploymentAge { get; set; }
        public int MaxBackDateDays { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public int Voided { get; set; }
        public int VoidedBy { get; set; }
        public DateTime DateVoided { get; set; }
    }
}