using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Staat.Models
{
    public class ServiceGroup : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool DefaultOpen { get; set; }
        public ICollection<Service> Services { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}