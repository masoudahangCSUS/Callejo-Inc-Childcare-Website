using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using Common.Models.Data;
using System;
using System.Diagnostics; // Added for Debug.WriteLine

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

        private string GetActualContentType(IFormFile file)
        {
            // Use the ContentType if it's not empty.
            if (!string.IsNullOrWhiteSpace(file.ContentType))
            {
                return file.ContentType;
            }

            // Infer the MIME type from the file extension if ContentType is empty.
            var fileName = file.FileName;
            if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                return "application/pdf";
            else if (fileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            else if (fileName.EndsWith(".doc", StringComparison.OrdinalIgnoreCase))
                return "application/msword";
            else if (fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                     fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                return "image/jpeg";
            else
                return string.Empty; // Otherwise return an empty string.
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string documentType)
        {
            try
            {
                Debug.WriteLine($"Received upload request. Document Type: {documentType}");

                if (file == null || file.Length == 0)
                {
                    Debug.WriteLine("File is null or empty.");
                    return BadRequest("Invalid file.");
                }

                // Enforce a 10 MB maximum file size limit
                if (file.Length > 10_000_000)
                {
                    Debug.WriteLine($"File size exceeds limit: {file.Length} bytes.");
                    return BadRequest("File size exceeds the maximum allowed limit of 10 MB.");
                }

                // Get the actual MIME type (inferred if necessary)
                var actualContentType = GetActualContentType(file);
                Debug.WriteLine($"File Name: {file.FileName}, Inferred ContentType: '{actualContentType}', Size: {file.Length} bytes");

                var allowedTypes = new[] {
            "application/pdf",
            "image/jpeg",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };

                if (!allowedTypes.Contains(actualContentType))
                {
                    Debug.WriteLine($"Invalid file type: '{actualContentType}'");
                    return BadRequest("Invalid file type. Only PDF, JPG, and DOC are allowed.");
                }

                // Delete any existing file for the given document type.
                var existingFile = _context.FileUploads.FirstOrDefault(f => f.DocumentType == documentType);
                if (existingFile != null)
                {
                    _context.FileUploads.Remove(existingFile);
                    Debug.WriteLine($"Deleted existing file for document type: {documentType}");
                }

                // Copy the file into a memory stream.
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var newFileUpload = new FileUpload
                {
                    FileName = file.FileName,
                    ContentType = actualContentType,
                    FileData = memoryStream.ToArray(),
                    DocumentType = documentType,
                    UploadDate = DateTime.UtcNow
                };

                _context.FileUploads.Add(newFileUpload);
                await _context.SaveChangesAsync();

                Debug.WriteLine($"File uploaded successfully: {file.FileName} for document type: {documentType}");

                return Ok(new
                {
                    message = "File uploaded and replaced successfully!",
                    fileName = newFileUpload.FileName,
                    documentType = newFileUpload.DocumentType,
                    uploadDate = newFileUpload.UploadDate
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during file upload: {ex.Message}");
                return StatusCode(500, "An internal server error occurred. Please try again.");
            }
        }
    }
}