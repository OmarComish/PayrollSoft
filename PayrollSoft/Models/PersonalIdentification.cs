using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class PersonalIdentification
    {
        [Key]
        public string IdentityNumber { get; set; }
        public string IdentityType { get; set; }
        public string EmpID { get; set; }
        public DateTime ValidityDate { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
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