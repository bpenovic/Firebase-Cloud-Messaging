using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirebaseCloudMessaging.Controllers
{
    public class NotificationController : Controller
    {
        //[Authorize(Roles="Administrator")]
        public IActionResult Index()
        {
            return View();
        }
    }
}