using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Common.Models.Data;

namespace CallejoIncChildcareAPI.Controllers
{
    [Route("api/parent/documents")]
    [ApiController]
    public class ParentDocumentsController : ControllerBase
    {
        private readonly CallejoSystemDbContext _context;

        public ParentDocumentsController(CallejoSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/parent/documents
        [HttpGet]
        public IActionResult GetDocuments()
        {
            List<FileUpload> files = _context.FileUploads.ToList();
            return Ok(files);
        }

        // GET: api/parent/documents/preview/{documentId}
        [HttpGet("preview/{documentId}")]
        public IActionResult PreviewDocument(int documentId)
        {
            var file = _context.FileUploads.FirstOrDefault(f => f.Id == documentId);
            if (file == null)
                return NotFound();

            // Return the file content so it can be displayed inline (e.g., in an iframe)
            return File(file.FileData, file.ContentType);
        }

        // GET: api/parent/documents/download/{documentId}
        [HttpGet("download/{documentId}")]
        public IActionResult DownloadDocument(int documentId)
        {
            var file = _context.FileUploads.FirstOrDefault(f => f.Id == documentId);
            if (file == null)
                return NotFound();

            // Return the file with proper content-disposition for downloading
            return File(file.FileData, file.ContentType, file.FileName);
        }
    }
}