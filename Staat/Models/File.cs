using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate.Data;

namespace Staat.Models
{
    public class File : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Namespace { get; set; }
        [Required] public string Hash { get; set; }
        [Required] public string MimeType { get; set; }
        
        // Models that can attach files
        [UseSorting, UseFiltering] public ICollection<Incident> Incidents { get; set; }
        [UseSorting, UseFiltering] public ICollection<IncidentMessage> IncidentMessages { get; set; }
        [UseSorting, UseFiltering] public ICollection<Maintenance> Maintenances { get; set; }
        [UseSorting, UseFiltering] public ICollection<MaintenanceMessage> MaintenanceMessages { get; set; }
        
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}