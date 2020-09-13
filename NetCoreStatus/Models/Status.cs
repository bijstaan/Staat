using System;
using System.ComponentModel.DataAnnotations;

namespace NetCoreStatus.Models
{
    public class Status : ITimeStampedModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Color { get; set; }
        public bool IsError { get; set; }
        public bool IsDegraded { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}