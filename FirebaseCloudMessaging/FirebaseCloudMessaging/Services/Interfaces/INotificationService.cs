using System.Threading.Tasks;
using FirebaseCloudMessaging.Services.Models;

namespace FirebaseCloudMessaging.Services.Interfaces
{
    public interface INotificationService
    {
        Task SaveTokenAsync(string tokenValue);
        Task<SendNotificationResponse> SendNotificationAsync(NotificationModel notification);
    }
}
