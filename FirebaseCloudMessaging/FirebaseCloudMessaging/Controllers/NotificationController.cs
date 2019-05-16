using System.Threading.Tasks;
using FirebaseCloudMessaging.Models;
using FirebaseCloudMessaging.Services.Interfaces;
using FirebaseCloudMessaging.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirebaseCloudMessaging.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            return View(new NotificationViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(NotificationViewModel notification)
        {
            if (ModelState.IsValid)
            {
                var response = await _notificationService.SendNotificationAsync(new NotificationModel
                {
                    Title = notification.Title,
                    Message = notification.Message,
                    Icon = notification.Icon,
                    Link = notification.Link
                });

                if (response.SendNotificationStatus == SendNotificationEnum.NotificationPostFail || response.SendNotificationStatus == SendNotificationEnum.MissingToken)
                    ModelState.AddModelError("", response.StatusMessage);
            }

            return View(notification);
        }

        [HttpPost]
        public async Task<IActionResult> SaveToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is not saved to Database!");

            await _notificationService.SaveTokenAsync(token);
            return Ok("Token is saved to Database!");
        }
    }
}