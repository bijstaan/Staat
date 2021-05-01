using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Staat.Models
{
    [Index(nameof(Key))]
    public class Settings : ITimeStampedModel
    {
        [Key] public string Key { get; set; }

        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}