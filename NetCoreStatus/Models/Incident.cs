using System;

namespace NetCoreStatus.Models
{
    public class Incidents : ITimeStampedModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}