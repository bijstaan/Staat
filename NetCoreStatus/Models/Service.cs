using System;
using System.Collections.Generic;

namespace NetCoreStatus.Models
{
    public class Services : ITimeStampedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        
        public Status Status { get; set; }
        public Services Parent { get; set; }
        public List<Services> Children { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}