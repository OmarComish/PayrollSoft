using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace PayrollSoft.Models
{
    public class Department
    {
        [Key]
        public string DeptID { get; set; }
        [Required]
        public string DeptName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}