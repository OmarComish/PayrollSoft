using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class TaskTracker
    {
        [Key]
        public int TrackerId { get; set; }
        public string Activity { get; set; }
        public TimeSpan  Duration { get; set; }
        public TimeSpan BeginTime { get; set; }
        public TimeSpan FinishTime { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }
        public int RecordStatusId { get; set; }
        public DateTime RecordStatusDateChanged { get; set; }
        public int RecordStatusChangedBy { get; set; }
    }
}