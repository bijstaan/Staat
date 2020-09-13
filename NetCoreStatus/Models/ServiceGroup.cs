using System;
using System.Collections.Generic;

namespace NetCoreStatus.Models
{
    public class ServiceGroup : ITimeStampedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public ICollection<Service> Services { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}