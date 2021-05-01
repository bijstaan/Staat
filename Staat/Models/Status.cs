using System;
using System.ComponentModel.DataAnnotations;

namespace Staat.Models
{
    public class Status : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        [Required] [StringLength(60)] public string Name { get; set; }
        public string Description { get; set; }

        [Required] public string Color { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}