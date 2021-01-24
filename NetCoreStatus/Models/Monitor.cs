using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreStatus.Models
{
    public class Monitor : ITimeStampedModel
    {
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Host { get; set; }
        public int Port { get; set; }
        public bool ValidateSsl { get; set; }

        public Incident CurrentIncident { get; set; }
        
        [Required]
        public Service Service { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}