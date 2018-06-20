using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class ApplicationModule
    {
        [Key]
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public DateTime LastUsed { get; set; }
    }
}