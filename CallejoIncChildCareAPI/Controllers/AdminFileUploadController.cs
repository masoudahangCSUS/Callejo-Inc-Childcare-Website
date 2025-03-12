namespace CallejoIncChildcareAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using System.IO;
    using System.Linq;
    using Common.Models.Data;
    using System;

    namespace CallejoIncChildcareAPI.Controllers
    {
        [Route("api/admin/file-upload")]
        [ApiController]
        public class AdminFileUploadController : ControllerBase
        {
            private readonly CallejoSystemDbContext _context;

            public AdminFileUploadController(CallejoSystemDbContext context)
            {
                _context = context;
            }

            [HttpPost]
            [Consumes("multipart/form-data")]
            public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string documentType)
            {
                // Check if file is null or empty
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Invalid file.");
                }

                // Validate allowed file types
                var allowedTypes = new[] { "application/pdf", "image/jpeg", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };
                if (!allowedTypes.Contains(file.ContentType))
                {
                    return BadRequest("Invalid file type. Only PDF, JPG, DOC are allowed.");
                }

                // Store file in memory before saving to DB
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var fileUpload = new FileUpload
                {
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    FileData = memoryStream.ToArray(),
                    DocumentType = documentType,
                    UploadDate = DateTime.UtcNow // Add the upload date if needed
                };

                _context.FileUploads.Add(fileUpload);
                await _context.SaveChangesAsync();

                return Ok(new { message = "File uploaded successfully!" });
            }
        }
    }
}
