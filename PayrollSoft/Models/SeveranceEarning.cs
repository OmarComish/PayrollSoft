using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    public class SeveranceEarning
    {
        [Key]
        public string RefNumber { get; set; }
        public string Description { get; set; }
        public double FirstThresholdRate { get; set; }
        public double SecondThresholdRate { get; set; }
        public double ThirdThresholdRate { get; set; }
        public int FirstThresholdMinPeriod { get; set; }
        public int SecondThresholdMinPeriod { get; set; }
        public int ThirdThresholdMinPeriod { get; set; }
        public int FirstThresholdMaxPeriod { get; set; }
        public int SecondThresholdMaxPeriod { get; set; }
        public int ThirdThresholdMaxPeriod { get; set; }
        public string FirstThresholdMinPeriodIn { get; set; }
        public string FirstThresholdMaxPeriodIn { get; set; }
        public string SecondThresholdMinPeriodIn { get; set; }
        public string SecondThresholdMaxPeriodIn { get; set; }
        public string ThirdThresholdMinPeriodIn { get; set; }
        public string ThirdThresholdMaxPeriodIn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime DateModified { get; set; }

    }
}