using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PayrollSoft.Models
{
    public class Overtime
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
        public double Rate { get; set; }
        public double AllowableWorkDays { get; set; }
        public double AllowableWorkHours { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public int RecordStatusId { get; set; }
        public int RecordStatusChangedBy { get; set; }
        public DateTime RecordStatusDateChanged { get; set; }
    }
}