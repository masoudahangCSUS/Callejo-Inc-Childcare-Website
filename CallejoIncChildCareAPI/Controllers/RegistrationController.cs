using Common.Services.Registration;
using Common.View;
using Microsoft.AspNetCore.Mvc;

namespace CallejoIncChildcareAPI.Controllers
{
    [RequireHttps]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private IRegService _regService;

        public RegistrationController(IRegService regService)
        {
            _regService = regService;
        }

        //POST: api/Registration/Upload
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile(Guid userId, IFormFile file)
        {
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

            return Ok("File uploaded successfully.");
        }

        //GET: api/Registration/Download
        [HttpGet("Download")]
        public async Task<IActionResult> DownloadFile(Guid userId)
        {
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
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteFile(Guid userId)
        {
            var result = await _regService.DeleteFileAsync(userId);
            if (!result)
            {
                return NotFound("No file found/Deletion failed");
            }
            return Ok("File deleted successfully");
        }
    }
}
