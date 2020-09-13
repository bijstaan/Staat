using System;

namespace NetCoreStatus.Models
{
    public class IncidentMessages : ITimeStampedModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public Status Status { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}