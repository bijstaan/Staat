using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using Staat.Models.Users;

namespace Staat.Models
{
    public class Maintenance : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(100), StringLength(100)] public string Title { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string DescriptionHtml { get; set; }
        [Required] public DateTime StartedAt { get; set; }
        [Required] public DateTime? EndedAt { get; set; }

        [UseFiltering, UseSorting] public ICollection<MaintenanceMessage> Messages { get; set; }
        [UseFiltering, UseSorting] public ICollection<Service> Services { get; set; }
        
        // We do not display the author publicly
        [Required, Authorize] public User Author { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}