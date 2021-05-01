using System;
using System.ComponentModel.DataAnnotations;

namespace Staat.Models
{
    public class MonitorData : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        public int PingTime { get; set; }
        public bool Available { get; set; }
        public bool? SslValid { get; set; }
        public string FailureReason { get; set; }

        [Required] public Monitor Monitor { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}