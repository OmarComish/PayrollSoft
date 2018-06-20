using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class UserPriviledge
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ModuleId { get; set; }
        public int CreateRole { get; set; }
        public int ReadRole { get; set; }
        public int UpdateRole { get; set; }
        public int DeleteRole { get; set; }
        public int AuthRole { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int ModifiedBy { get; set; }
        public int CreatedBy { get; set; }
        public int RecordStatusId { get; set; }
        public int RecordStatusChangedBy { get; set; }
        public DateTime RecordStatusDateChanged { get; set; }
    }
}