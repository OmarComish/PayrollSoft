using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{

    /// <summary>
    /// Id holds the autoincrement integers generated at the database level
    /// ProcessDate is the date the service and/or Process is executed
    /// ProcessMethod specifies whether the process is manual or Automated
    /// </summary>
    public class ServiceDefinition
    {
        [Key]
        public int id { get; set; } 
        public DateTime ProcessDate { get; set; }  
        public string ProcessMethod { get; set; }  
        public string Description { get; set; }
    }
}