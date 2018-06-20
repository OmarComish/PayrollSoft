using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class Settings
    {
        [Key]
        public string ObjectID { get; set; }
        public string ObjectName { get; set; }
        public string ActionInvoked { get; set; }
        public DateTime TimeStamp { get; set; }
        public string User { get; set; }
    }
}