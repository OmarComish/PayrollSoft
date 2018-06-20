using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace PayrollSoft.Models
{
    public class ProfessionalQualification
    {
        [Key]
        public int Qid { get; set; }
        public string EmpID { get; set; }
        public string QualificationName { get; set; }
        public string Description { get; set; }
        public DateTime DateAttained { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int DateModified { get; set; }
        public DateTime ModifiedBy { get; set; }
        public int Voided { get; set; }
        public int VoidedBy { get; set; }
        public DateTime DateVoided { get; set; }
        public int RecordStatusId { get; set; }
    }
}