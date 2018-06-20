using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class Relation
    {
        [Key]
        public int Rid { get; set; }
        public string EmpID { get; set; }
        public string NextOfKinName { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public int Voided { get; set; }
        public int VoidedBy { get; set; }
        public DateTime DateVoided { get; set; }
        public int RecordStatusId { get; set; }
    }
}