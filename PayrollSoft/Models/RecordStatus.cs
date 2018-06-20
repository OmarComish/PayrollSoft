﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class RecordStatus
    {
        [Key]
        public int RecordStatusId { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public int? Voided { get; set; }
        public int? VoidedBy { get; set; }
        public DateTime? DateVoided { get; set; }
        public int Status { get; set; }
    }
}