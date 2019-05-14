using System.ComponentModel.DataAnnotations;

namespace FirebaseCloudMessaging.Models
{
    public class TokenViewModel
    {
        [Required]
        public string Value { get; set; }
    }
}
