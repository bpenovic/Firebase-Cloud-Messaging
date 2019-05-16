using System;
using System.ComponentModel.DataAnnotations;

namespace FirebaseCloudMessaging.Data.Models
{
    public class Token
    {
        [Key]
        public Guid TokenId { get; set; }
        public bool IsActive { get; set; }
        public string Value { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? ModifiedUtc { get; set; }
    }
}
