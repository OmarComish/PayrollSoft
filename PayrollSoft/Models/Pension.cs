using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class Pension
    {
        [Key]
        public string PensionRefNum { get; set; }
        public Double EmployerContrRate { get; set; }
        public Double EmployeeContrRate { get; set; }
        public Double GroupLifeAssuranceRate { get; set; }
        public Double AdminiFeeRate { get; set; }
        public Double BrokerageFeeRate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public int RecordStatusId { get; set; }
        public int RecordStatusChangedBy { get; set; }
        public DateTime RecordStatusDateChanged { get; set; }

        public virtual ICollection<EmployeeGrade> EmployeeGrades { get; set; }
    }
}