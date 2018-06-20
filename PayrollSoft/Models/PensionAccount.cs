using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayrollSoft.Models;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class PensionAccount
    {
        [Key]
        public int Id { get; set; }
        public string EmpId { get; set; }
        public double BasicSalary { get; set; }
        public double EmployerContribution { get; set; }
        public double EmployeeContribution { get; set; }
        public double AdministrationFee { get; set; }
        public double BrokerageFee { get; set; }
        public double GroupLifeAssurance { get; set; }
        public DateTime EntryDate { get; set; }

    }
}