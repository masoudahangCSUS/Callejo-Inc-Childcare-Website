using Common.Services.SQL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Common.Models.Data;

namespace CallejoIncChildcareAPI.Controllers
{
    [RequireHttps]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ISQLServices _sqlServices;

        public NotificationsController(ISQLServices sqlServices)
        {
            _sqlServices = sqlServices;
        }

        //  Get notifications for a parent
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

        //  Mark notification as read
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

        //  Parent sending notification to the admin (DO NOT REMOVE)
        [HttpPost("send-custom-notif")]
        public IActionResult SendCustomNotification([FromBody] Notification newRequest)
        {
            if (newRequest == null || string.IsNullOrWhiteSpace(newRequest.Message))
            {
                return BadRequest("Invalid notification data.");
            }

            // Ensure IsRead is false for new requests
            newRequest.IsRead = false;
            newRequest.SentOn = DateTime.UtcNow;

            var success = _sqlServices.SendCustomNotification(newRequest);
            if (!success)
            {
                return StatusCode(500, "Failed to save the notification.");
            }
            return Ok("Notification sent successfully.");

            /*var success = _sqlServices.SendCustomNotification(newRequest);
            if (!success)
            {
                return NotFound("Parent ID not found.");
            }
            return Ok("Notification sent.");*/
        }

        // POST: api/Notifications/admin-create
        [HttpPost("admin-create")]
        public IActionResult CreateNotification([FromBody] Notification notification)
        {
            if (notification == null || string.IsNullOrEmpty(notification.Title) || string.IsNullOrEmpty(notification.Message))
            {
                return BadRequest("Invalid notification data.");
            }

            // Ensure Id is not set since it's auto-generated
            notification.Id = 0;

            // Force IsRead to be false when creating a new notification
            notification.IsRead = false;

            // Save the notification
            var success = _sqlServices.CreateNotification(notification);
            if (!success)
            {
                return StatusCode(500, "Failed to create notification.");
            }

            return Ok("Notification created successfully.");
        }


        //  Update notification (Admin)
        [HttpPut("admin-update/{id}")]
        public IActionResult UpdateNotification(long id, [FromBody] Notification updatedNotification)
        {
            if (updatedNotification == null || string.IsNullOrWhiteSpace(updatedNotification.Message))
            {
                return BadRequest("Invalid notification data.");
            }

            var success = _sqlServices.UpdateNotification(id, updatedNotification);
            if (!success)
            {
                return NotFound("Notification not found or could not be updated.");
            }

            return Ok("Notification updated successfully.");
        }

        //  Delete notification (Admin)
        [HttpDelete("admin-delete/{id}")]
        public IActionResult DeleteNotification(long id)
        {
            var success = _sqlServices.DeleteNotification(id);
            if (!success)
            {
                return NotFound("Notification not found or could not be deleted.");
            }

            return Ok("Notification deleted successfully.");
        }

        // Get all notifications (Admin)
        [HttpGet("get-all")]
        public IActionResult GetAllNotifications()
        {
            var notifications = _sqlServices.GetAllNotifications();
            if (notifications == null || !notifications.Any())
            {
                return NotFound("No notifications found.");
            }
            return Ok(notifications);
        }

    }
}
