using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Staat.Models.Users
{
    public class User : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Email { get; set; }
        [Required] [JsonIgnore] public byte[] PasswordHash { get; set; }
        [Required] [JsonIgnore] public byte[] PasswordSalt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}