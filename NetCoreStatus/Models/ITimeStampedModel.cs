using System;

namespace NetCoreStatus.Models
{
    public interface ITimeStampedModel
    {
        public DateTime CreatedAt {get; set;}
        public DateTime LastModified {get; set;}
    }
}