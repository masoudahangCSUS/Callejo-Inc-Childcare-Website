using Common.Services.SQL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using Common.View;
using Common.Services.Login;
using CallejoIncChildCareAPI.Authorize;

namespace CallejoIncChildcareAPI.Controllers
{
    [RequireHttps]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ISQLServices _sqlServices;
        private ILoginService _loginService;

        public NotificationsController(ISQLServices sqlServices, ILoginService loginService)
        {
            _sqlServices = sqlServices;
            _loginService = loginService;
        }

        [AuthorizeAttribute()]
        [HttpGet("{parentId}")]
        public IActionResult GetNotificationsByParentId(Guid parentId)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var result = _sqlServices.GetNotificationsByParentId(parentId);
            if (result == null || !result.Any())
            {
                return NotFound("No notifications found for this parent.");
            }
            return Ok(result);
            return Ok(1);
        }

        [AuthorizeAttribute()]
        [HttpPut("mark-as-read/{id}")]
        public IActionResult MarkNotificationAsRead(long id)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var success = _sqlServices.MarkNotificationAsRead(id);
            if (!success)
            {
                return NotFound("Notification not found or could not be marked as read.");
            }
            return Ok("Notification marked as read.");
        }

        [AuthorizeAttribute()]
        [HttpPost("send-custom-notif")]
        public IActionResult SendCustomNotification([FromBody] NotificationView newRequest)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
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

        [AuthorizeAttribute()]
        [HttpPost("admin-create")]
        public IActionResult CreateNotification([FromBody] NotificationView notification)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
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

        [AuthorizeAttribute()]
        [HttpPut("admin-update/{id}")]
        public IActionResult UpdateNotification(long id, [FromBody] NotificationView updatedNotification)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
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

        [AuthorizeAttribute()]
        [HttpDelete("admin-delete/{id}")]
        public IActionResult DeleteNotification(long id)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var success = _sqlServices.DeleteNotification(id);
            if (!success)
            {
                return NotFound("Notification not found or could not be deleted.");
            }

            return Ok("Notification deleted successfully.");
        }

        [AuthorizeAttribute()]
        [HttpGet("get-all")]
        public IActionResult GetAllNotifications()
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var notifications = _sqlServices.GetAllNotifications();
            if (notifications == null || !notifications.Any())
            {
                return NotFound("No notifications found.");
            }
            return Ok(notifications);
        }
    }
}
