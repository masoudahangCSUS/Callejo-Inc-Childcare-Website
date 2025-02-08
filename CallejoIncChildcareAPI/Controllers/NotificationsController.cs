using Common.Services.SQL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CallejoIncChildcareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ISQLServices _sqlServices;

        public NotificationsController(ISQLServices sqlServices)
        {
            _sqlServices = sqlServices;
        }

        // GET: api/notifications/{parentId}
        [HttpGet("{parentId}")]
        public IActionResult GetNotificationsByParentId(Guid parentId)
        {
            var result = _sqlServices.GetNotificationsByParentId(parentId);
            if (result == null || !result.Any())
            {
                return NotFound("No notifications found for this parent.");
            }
            return Ok(result);
        }

        // PUT: api/notifications/mark-as-read/{id}
        [HttpPut("mark-as-read/{id}")]
        public IActionResult MarkNotificationAsRead(long id)
        {
            var success = _sqlServices.MarkNotificationAsRead(id);
            if (!success)
            {
                return NotFound("Notification not found or could not be marked as read.");
            }
            return Ok("Notification marked as read.");
        }

        // POST: api/notifications/send-custom-notif/{parentId, message}
        [HttpPost("send-custom-notif/{parentId}/{message}")]
        public IActionResult SendCustomNotification(string parentId, string message)
        {
            var success = _sqlServices.SendCustomNotification(parentId, message);
            if (!success)
            {
                return NotFound("Parent ID not found.");
            }
            return Ok("Notification sent.");
        }
    }
}
