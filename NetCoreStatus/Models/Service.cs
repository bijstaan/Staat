using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace NetCoreStatus.Models
{
    public class Service : ITimeStampedModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Url]
        public string Link { get; set; }
        
        public Status Status { get; set; }
        public ServiceGroup Group { get; set; }
        public ICollection<Incident> Incidents { get; set; }
        public Service Parent { get; set; }
        public ICollection<Service> Children { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}