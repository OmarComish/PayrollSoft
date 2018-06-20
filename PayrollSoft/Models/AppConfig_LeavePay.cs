using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class AppConfig_LeavePay
    {
        [Key]
        public  String ID {get; set;}
        public String LeaveFrequency { get; set; }
        public Double RateOfLeavePay { get; set; }
        public int IncludeLeavePayAfter { get; set; }
        public String RateBasedOn { get; set; }
    }
}