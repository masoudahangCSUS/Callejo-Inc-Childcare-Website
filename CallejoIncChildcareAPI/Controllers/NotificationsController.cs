using Common.Services.SQL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using Common.View;

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

        
        [HttpPost("send-custom-notif")]
        public IActionResult SendCustomNotification([FromBody] NotificationView newRequest)
        {
            if (newRequest == null || string.IsNullOrWhiteSpace(newRequest.Message))
            {
                return BadRequest("Invalid notification data.");
            }

            // Ensure default values
            newRequest.IsRead = false;
            newRequest.SentOn = DateTime.UtcNow;

            var success = _sqlServices.SendCustomNotification(newRequest);
            if (!success)
            {
                return StatusCode(500, "Failed to save the notification.");
            }
            return Ok("Notification sent successfully.");
        }

        
        [HttpPost("admin-create")]
        public IActionResult CreateNotification([FromBody] NotificationView notification)
        {
            if (notification == null || string.IsNullOrEmpty(notification.Title) || string.IsNullOrEmpty(notification.Message))
            {
                return BadRequest("Invalid notification data.");
            }

            // Ensure Id is not set since it's auto-generated
            notification.Id = 0;
            notification.IsRead = false;

            var success = _sqlServices.CreateNotification(notification);
            if (!success)
            {
                return StatusCode(500, "Failed to create notification.");
            }

            return Ok("Notification created successfully.");
        }

        
        [HttpPut("admin-update/{id}")]
        public IActionResult UpdateNotification(long id, [FromBody] NotificationView updatedNotification)
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
