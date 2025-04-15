using CallejoIncChildCareAPI.Authorize;
using Common.Models.Data;
using Common.Services.Login;
using Common.Services.User;
using Common.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System;
namespace CallejoIncChildcareAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SubmitController : ControllerBase
    {
        // DB Context declaration
        private readonly CallejoSystemDbContext _context;
        private ILoginService _loginService;

        // Inject DB context in the constructor for DB access
        public SubmitController(CallejoSystemDbContext context, ILoginService loginService)
        {
            _context = context;
            _loginService = loginService;
        }


        // Post Form Data to DB
        [AuthorizeAttribute()]
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitForm([FromBody] InterestedParent inquiry)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            _context.InterestedParents.Add(inquiry);
            await _context.SaveChangesAsync();
            return Ok(inquiry);
        }

        // Retrieve data from DB
        [AuthorizeAttribute()]
        [HttpGet("data")]
        public async Task<IActionResult> GetFormData()
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var data = await _context.InterestedParents  
                .OrderByDescending(i => i.Datetime)
                .ToListAsync();

            return Ok(data);
        }
        // DELETE: api/Submit/delete/{id}
        [AuthorizeAttribute()]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteInquiry(Guid id)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var inquiry = await _context.InterestedParents.FindAsync(id);

            if (inquiry == null)
            {
                return NotFound("Inquiry not found.");
            }

            _context.InterestedParents.Remove(inquiry);
            await _context.SaveChangesAsync();

            return Ok("Inquiry deleted successfully.");
        }

    }
}