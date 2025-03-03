using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models.Data;
using Common.Services.SQL;


namespace CallejoIncChildcareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidaysVacationsController : ControllerBase
    {
        private readonly CallejoSystemDbContext _context;
        private readonly ISQLServices _sqlServices;

        public HolidaysVacationsController(CallejoSystemDbContext context, ISQLServices sqlServices)
        {
            _context = context;
            _sqlServices = sqlServices;
        }

        //[HttpGet]
        //[Produces("application/json")] // Ensure JSON response
        //EC public async Task<ActionResult<IEnumerable<HolidaysVacations>>> GetHolidaysVacations()
        //{
        //    var holidaysVacations = await _context.HolidaysVacations
        //        .OrderBy(h => h.StartDate) // Order by start date for better display
        //        .ToListAsync();

        //    return Ok(holidaysVacations);
        //}

        // Create a new holiday/vacation (Admin)
        //[HttpPost("admin-create")]
        //EC public IActionResult CreateHolidayVacation([FromBody] HolidaysVacations holidayVacation)
        //{
        //    if (holidayVacation == null || string.IsNullOrWhiteSpace(holidayVacation.Title))
        //        return BadRequest("Invalid holiday/vacation data.");

        //    var success = _sqlServices.CreateHolidayVacation(holidayVacation);
        //    return success ? Ok("Holiday/Vacation created successfully.") : StatusCode(500, "Failed to create.");
        //}

        // Update an existing holiday/vacation (Admin)
        //[HttpPut("admin-update/{id}")]
        //EC public IActionResult UpdateHolidayVacation(long id, [FromBody] HolidaysVacations updatedHolidayVacation)
        //{
        //    if (updatedHolidayVacation == null || string.IsNullOrWhiteSpace(updatedHolidayVacation.Title))
        //        return BadRequest("Invalid holiday/vacation data.");

        //    var success = _sqlServices.UpdateHolidayVacation(id, updatedHolidayVacation);
        //    return success ? Ok("Updated successfully.") : NotFound("Holiday/Vacation not found.");
        //}

        // Delete a holiday/vacation (Admin)
        //[HttpDelete("admin-delete/{id}")]
        //EC public IActionResult DeleteHolidayVacation(long id)
        //{
        //    var success = _sqlServices.DeleteHolidayVacation(id);
        //    return success ? Ok("Deleted successfully.") : NotFound("Holiday/Vacation not found.");
        //}



    }
}
