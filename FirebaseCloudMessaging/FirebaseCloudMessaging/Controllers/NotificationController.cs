using System.Threading.Tasks;
using FirebaseCloudMessaging.Models;
using FirebaseCloudMessaging.Services.Interfaces;
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveToken(TokenViewModel token)
        {
            if (!ModelState.IsValid)
                return BadRequest("Token is not saved to Database!");

            await _notificationService.SaveTokenAsync(token.Value);
            return Ok("Token is saved to Database!");
        }
    }
}