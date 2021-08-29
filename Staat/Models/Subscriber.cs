using System;
using System.ComponentModel.DataAnnotations;

namespace Staat.Models
{
    public class Subscriber : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(256), StringLength(256), EmailAddress] public string Email { get; set; }
        [Required] public bool IsVerified { get; set; }
        [Required] public string VerificationString { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}