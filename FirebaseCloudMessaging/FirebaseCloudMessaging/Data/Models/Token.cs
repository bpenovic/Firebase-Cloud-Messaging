using System;
using System.ComponentModel.DataAnnotations;

namespace FirebaseCloudMessaging.Data.Models
{
    public class Token
    {
        [Key]
        public Guid TokenId { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public DateTime CreatedUtc { get; set; }
        public DateTime? ModifiedUtc { get; set; }
    }
}
