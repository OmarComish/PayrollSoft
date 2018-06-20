using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace PayrollSoft.Models
{
    public class Employee
    {
        
        [Key]
        public string EmpID { get; set; }  
        public string EmpName { get; set; }    
        public string DeptID { get; set; }      
        public string GradeID { get; set; }      
        public DateTime? HireDate { get; set; }     
        public DateTime? DateOfBirth { get; set; }    
        public string JobTitle { get; set; } 
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string MobileNumber1 { get; set; }
        public string MobileNumber2 { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmploymentTypeId { get; set; }
        public string PaySchedule { get; set; }
        public int onPayroll { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public int Voided { get; set; }
        public int VoidedBy { get; set; }
        public DateTime DateVoided { get; set; }
        public int RecordStatusId { get; set; }
        
        
        public virtual Department Departments { get; set; }
        public virtual EmployeeGrade EmployeeGrades { get; set; }
        public virtual ICollection<Payroll> Payroll { get; set; }
    }
}