using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayrollSoft.Models;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class RotativaTest
    {
        [Key]
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
    }
}