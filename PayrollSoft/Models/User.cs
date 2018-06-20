using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please provide user name", AllowEmptyStrings = false)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password cannot be empty", AllowEmptyStrings = false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]

        public string Password { get; set; }
        public DateTime LastSignedOn { get; set; }
        public string SignedInTo { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }
        public int RecordStatusId { get; set; }
        public int RecordStatusChangedBy { get; set; }
        public DateTime RecordStatusDateChanged { get; set; }
    }
    public enum appname
    {
        ESS,
        Payroll
    }
}