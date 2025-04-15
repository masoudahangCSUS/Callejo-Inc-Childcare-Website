
using Common.Services.Registration;
using Common.View;
using Microsoft.AspNetCore.Mvc;
using Common.Models.Data;
using System.Collections.Generic;
using Common.Services.Login;
using CallejoIncChildCareAPI.Authorize;

namespace CallejoIncChildcareAPI.Controllers
{
    [RequireHttps]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private IRegService _regService;
        private static List<Registration> reg = new List<Registration>();
        private ILoginService _loginService;

        public RegistrationController(IRegService regService, ILoginService loginService)
        {
            _regService = regService;
            _loginService = loginService;
        }

        //POST: api/Registration/Upload
        [AuthorizeAttribute()]
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile(Guid userId, IFormFile file)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

            // Exit if file has no content
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            // Extract type and size
            string fileType = file.ContentType;
            long fileSize = file.Length;

            // Only allow PDF files
            if (fileType != "application/pdf" || !file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Only PDF files are allowed");
            }

            // Hard limit in Swagger is 30MB. Only allow files < 5MB.
            const long maxFileSize = 5242880; // 5MB
            if (fileSize > maxFileSize)
            {
                return BadRequest("File size exceeds 5MB limit");
            }

            // Convert file to byte array
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();

            // Save file
            var result = await _regService.UploadFileAsync(userId, fileBytes, fileType, fileSize);

            if (!result)
            {
                return StatusCode(500, "File upload failed.");
            }
            var existingRegistration = reg.FirstOrDefault(r => r.UserId == userId);
            if (existingRegistration == null)
            {
                reg.Add(new Registration
                {
                    Id = Guid.NewGuid(),
                    UserId = userId, //  Store User ID properly
                    Name = $"User_{userId}",
                    Status = "Pending",
                    Datetime = DateTime.UtcNow
                });
            }
            else
            {
                existingRegistration.Status = "Pending";
                existingRegistration.Datetime = DateTime.UtcNow;
            }

            return Ok("File uploaded successfully.");
        }

        //GET: api/Registration/Download
        [AuthorizeAttribute()]
        [HttpGet("Download")]
        public async Task<IActionResult> DownloadFile(Guid userId)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

            // Retrieve file data
            var fileData = await _regService.GetFileAsync(userId);

            // If file is not present, return NotFound
            if (fileData == null)
            {
                return NotFound("No file found");
            }

            // Return data as PDF
            return File(fileData, "application/pdf", "registration.pdf");
        }

        //DELETE: api/Registration/Delete
        [AuthorizeAttribute()]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteFile(Guid userId)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var result = await _regService.DeleteFileAsync(userId);
            if (!result)
            {
                return NotFound("No file found/Deletion failed");
            }
            //  Update registration status if deleted
            var registration = reg.FirstOrDefault(r => r.UserId == userId);
            if (registration != null)
            {
                registration.Status = "Deleted";
                registration.Datetime = DateTime.UtcNow;
            }

            return Ok("File deleted successfully");
        }

        //GET api/Registration/status/{userId}
        [AuthorizeAttribute()]
        [HttpGet("status/{userId}")]
        public async Task<IActionResult> GetRegistrationStatus(Guid userId)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var registration = reg.FirstOrDefault(r => r.UserId == userId);
            if (registration == null)
                return NotFound("No registration found for this user.");

            var fileData = await _regService.GetFileAsync(userId);
            bool fileExists = fileData != null;

            var dto = new RegistrationDTO
            {
                Id = registration.Id,
                Name = registration.Name,
                Status = registration.Status == "Pending" && !fileExists ? "No File Submitted" : registration.Status,
                DateTime = registration.Datetime
            };

            return Ok(dto);
        }
    }
}
