using System;
using System.ComponentModel.DataAnnotations;

namespace Staat.Models
{
    public class IncidentMessage : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        [Required] public string Message { get; set; }
        [Required] public Status Status { get; set; }
        [Required] public Incident Incident { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}