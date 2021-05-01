using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Staat.Models
{
    public class Monitor : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        [Required] public string Type { get; set; }
        [Required] public string Host { get; set; }
        public int? Port { get; set; }
        public bool? ValidateSsl { get; set; }

        public Incident CurrentIncident { get; set; }
        [Required] public Service Service { get; set; }
        public ICollection<MonitorData> Data { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}