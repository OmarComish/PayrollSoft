using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class PendingItem
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ReferenceNumber { get; set; }
        public int Initiator { get; set; }
        public string Source { get; set; }
        public string Controller { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}