using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Common.Models.Data;
using Common.Services.Login;
using CallejoIncChildCareAPI.Authorize;
using Common.View;

namespace CallejoIncChildcareAPI.Controllers
{
    [Route("api/parent/documents")]
    [ApiController]
    public class ParentDocumentsController : ControllerBase
    {
        private readonly CallejoSystemDbContext _context;
        private ILoginService _loginService;

        public ParentDocumentsController(CallejoSystemDbContext context, ILoginService loginService)
        {
            _context = context;
            _loginService = loginService;
        }

        // GET: api/parent/documents
        [AuthorizeAttribute()]
        [HttpGet]
        public IActionResult GetDocuments()
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            List<FileUpload> files = _context.FileUploads.ToList();
            return Ok(files);
        }

        // GET: api/parent/documents/preview/{documentId}
        [AuthorizeAttribute()]
        [HttpGet("preview/{documentId}")]
        public IActionResult PreviewDocument(int documentId)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var file = _context.FileUploads.FirstOrDefault(f => f.Id == documentId);
            if (file == null)
                return NotFound();

            // Return the file content so it can be displayed inline (e.g., in an iframe)
            return File(file.FileData, file.ContentType);
        }

        // GET: api/parent/documents/download/{documentId}
        [AuthorizeAttribute()]
        [HttpGet("download/{documentId}")]
        public IActionResult DownloadDocument(int documentId)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var file = _context.FileUploads.FirstOrDefault(f => f.Id == documentId);
            if (file == null)
                return NotFound();

            // Return the file with proper content-disposition for downloading
            return File(file.FileData, file.ContentType, file.FileName);
        }
    }
}