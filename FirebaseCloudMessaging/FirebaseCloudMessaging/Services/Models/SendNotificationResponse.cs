using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirebaseCloudMessaging.Services.Models
{
    public class SendNotificationResponse
    {
        public SendNotificationEnum SendNotificationStatus { get; set; }
        public string StatusMessage { get; set; }
    }

    public enum SendNotificationEnum
    {
        Success,
        NotificationPostFail,
        MissingToken,
        NotAllSuccess
    }
}
