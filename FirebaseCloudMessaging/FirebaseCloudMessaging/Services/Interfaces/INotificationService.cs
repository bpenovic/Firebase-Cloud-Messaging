using System.Threading.Tasks;

namespace FirebaseCloudMessaging.Services.Interfaces
{
    public interface INotificationService
    {
        Task SaveTokenAsync(string tokenValue);
    }
}
