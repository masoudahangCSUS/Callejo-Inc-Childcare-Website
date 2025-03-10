using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.View;
using Common.Services.SQL;

namespace CallejoIncChildcareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidaysVacationsController : ControllerBase
    {
        private readonly ISQLServices _sqlServices;

        public HolidaysVacationsController(ISQLServices sqlServices)
        {
            _sqlServices = sqlServices;
        }

        [HttpGet]
        [Produces("application/json")] // Ensure JSON response
        public async Task<ActionResult<IEnumerable<HolidaysVacationView>>> GetHolidaysVacations()
        {
            var holidaysVacations = await Task.Run(() => _sqlServices.GetHolidaysVacations().ToList());

            return Ok(holidaysVacations);
        }

        // Create a new holiday/vacation (Admin)
        [HttpPost("admin-create")]
        public IActionResult CreateHolidayVacation([FromBody] HolidaysVacationView holidayVacationView)
        {
            if (holidayVacationView == null || string.IsNullOrWhiteSpace(holidayVacationView.Title))
                return BadRequest("Invalid holiday/vacation data.");

            var success = _sqlServices.CreateHolidayVacation(holidayVacationView);

            return success ? Ok("Holiday/Vacation created successfully.") : StatusCode(500, "Failed to create.");
        }

        // Update an existing holiday/vacation (Admin)
        [HttpPut("admin-update/{id}")]
        public IActionResult UpdateHolidayVacation(long id, [FromBody] HolidaysVacationView updatedHolidayVacation)
        {
            if (updatedHolidayVacation == null || string.IsNullOrWhiteSpace(updatedHolidayVacation.Title))
                return BadRequest("Invalid holiday/vacation data.");

            var success = _sqlServices.UpdateHolidayVacation(id, updatedHolidayVacation);

            return success ? Ok("Updated successfully.") : NotFound("Holiday/Vacation not found.");
        }

        // Delete a holiday/vacation (Admin)
        [HttpDelete("admin-delete/{id}")]
        public IActionResult DeleteHolidayVacation(long id)
        {
            var success = _sqlServices.DeleteHolidayVacation(id);
            return success ? Ok("Deleted successfully.") : NotFound("Holiday/Vacation not found.");
        }
    }
}
