using iderkaInventorySystem_API.Service;
using Microsoft.AspNetCore.Mvc;

namespace iderkaInventorySystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : Controller
    {
        private readonly iNotification _noti;
        public NotificationsController(iNotification notificationService)
        {
            _noti = notificationService;
        }

        [HttpGet("{idLoc}")]
        public async Task<IActionResult> GetNotifications(string idLoc)
        {
            var notifications = await _noti.GetNotificationsForLocation(idLoc);
            return Ok(notifications);
        }
    }
}
