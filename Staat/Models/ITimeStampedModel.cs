using System;

namespace Staat.Models
{
    public interface ITimeStampedModel
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}