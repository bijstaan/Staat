using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreStatus.Models
{
    public class Incident : ITimeStampedModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public Service Service { get; set; }
        public ICollection<IncidentMessage> Messages { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}