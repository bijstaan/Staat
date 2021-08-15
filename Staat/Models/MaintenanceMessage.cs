using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using Staat.Models.Users;

namespace Staat.Models
{
    public class MaintenanceMessage : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        [Required] public string Message { get; set; }
        [Required] public string MessageHtml { get; set; }
        // We do not display the author publicly
        [Required, Authorize] public User Author { get; set; }
        
        [UseSorting, UseFiltering] public ICollection<File> Attachments { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}