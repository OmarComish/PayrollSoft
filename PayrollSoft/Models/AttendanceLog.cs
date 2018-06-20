using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class AttendanceLog
    {
        [Key]
        public string logNum { get; set; }
        public string EmpId { get; set; }
        public string payCode { get; set; }
        public string Date { get; set; }
        public double overtimeHours { get; set; }
    }
}