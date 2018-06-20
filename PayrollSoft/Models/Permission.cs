using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ModuleId {get;set;}
        public string AccessLevelId { get; set; }
    }
}