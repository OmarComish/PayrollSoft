using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace PayrollSoft.Models
{
    public class Taxation
    {
        [Key]
        public string TaxRefCode { get; set; }
        public string Description { get; set; }
        public int Active { get; set; }
        public int RecordStatusId { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateModified { get; set; }
        public int ModifiedBy { get; set; }
        

        //public virtual TaxationThreshold Thresholds { get; set; }
        public virtual ICollection<TaxationThreshold> TaxationThresholds { get; set; }
    }
}