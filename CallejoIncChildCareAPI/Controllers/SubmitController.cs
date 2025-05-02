using Common.Models.Data;
using Common.Services.Submit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CallejoIncChildcareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubmitController : ControllerBase
    {
        private readonly ISubmitService _submitService;

        public SubmitController(ISubmitService submitService)
        {
            _submitService = submitService;
        }

        // POST: api/Submit/submit
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitForm([FromBody] InterestedParent inquiry)
        {
            try
            {
                await _submitService.AddInquiryAsync(inquiry);
                return Ok(inquiry);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error submitting inquiry: {ex.Message}");
            }
        }

        // GET: api/Submit/data
        [HttpGet("data")]
        public async Task<IActionResult> GetFormData()
        {
            try
            {
                var data = await _submitService.GetInquiryAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving inquiries: {ex.Message}");
            }
        }

        // DELETE: api/Submit/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteInquiry(Guid id)
        {
            try
            {
                var success = await _submitService.DeleteInquiryAsync(id);
                if (!success)
                {
                    return NotFound("Inquiry not found or could not be deleted.");
                }
                return Ok("Inquiry deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting inquiry: {ex.Message}");
            }
        }
    }
}
