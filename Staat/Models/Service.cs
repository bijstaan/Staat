using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate.Data;

namespace Staat.Models
{
    public class Service : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        [Required] [StringLength(255)] public string Name { get; set; }
        [StringLength(255)] public string Description { get; set; }
        [Url] public string Url { get; set; }

        [Required] public Status Status { get; set; }
        public ICollection<Incident> Incidents { get; set; }
        public ServiceGroup Group { get; set; }
        public Service Parent { get; set; }
        public ICollection<Service> Children { get; set; }
        public ICollection<Monitor> Monitors { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}