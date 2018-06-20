using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class GroupLoanPortifolios
    {
        [Key]
        public string RefNumber { get; set; }
        //public IEnumerable<Employee> EmpName { get; set; }
        //public IEnumerable<LoanType> LoanType { get; set; }
        public IEnumerable<loanMaster> PortifolioDetails { get; set; }
        public IEnumerable<loanMaster> Total { get; set; }
    }
}