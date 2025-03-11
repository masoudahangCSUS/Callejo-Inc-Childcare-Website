using Common.Models.Data;
using Common.Services.User;
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

        // Inject DB context in the constructor for DB access
        public SubmitController(CallejoSystemDbContext context)
        {
            _context = context;
        }


        // Post Form Data to DB
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitForm([FromBody] InterestedParent inquiry)
        {
            _context.InterestedParents.Add(inquiry);
            await _context.SaveChangesAsync();
            return Ok(inquiry);
        }

        // Retrieve data from DB
        [HttpGet("data")]
        public async Task<IActionResult> GetFormData()
        {
            var data = await _context.InterestedParents  
                .OrderByDescending(i => i.Datetime)
                .ToListAsync();

            return Ok(data);
        }
        // DELETE: api/Submit/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteInquiry(Guid id)
        {
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