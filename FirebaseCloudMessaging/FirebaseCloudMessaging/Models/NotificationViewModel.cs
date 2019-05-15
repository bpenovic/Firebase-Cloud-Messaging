
using System.ComponentModel.DataAnnotations;
using FirebaseCloudMessaging.Services.Models;

namespace FirebaseCloudMessaging.Models
{
    public class NotificationViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public SendNotificationResponse SendResponse { get; set; }
    }
}
