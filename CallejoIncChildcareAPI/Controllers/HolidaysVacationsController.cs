using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.View;
using Common.Services.SQL;
using Common.Services.Login;
using CallejoIncChildCareAPI.Authorize;

namespace CallejoIncChildcareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidaysVacationsController : ControllerBase
    {
        private readonly ISQLServices _sqlServices;
        private ILoginService _loginService;

        public HolidaysVacationsController(ISQLServices sqlServices, ILoginService loginService)
        {
            _sqlServices = sqlServices;
            _loginService = loginService;
        }

        [AuthorizeAttribute()]
        [HttpGet]
        [Produces("application/json")] // Ensure JSON response
        public async Task<ActionResult<IEnumerable<HolidaysVacationView>>> GetHolidaysVacations()
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var holidaysVacations = await Task.Run(() => _sqlServices.GetHolidaysVacations().ToList());

            return Ok(holidaysVacations);
        }

        // Create a new holiday/vacation (Admin)
        [AuthorizeAttribute()]
        [HttpPost("admin-create")]
        public IActionResult CreateHolidayVacation([FromBody] HolidaysVacationView holidayVacationView)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            if (holidayVacationView == null || string.IsNullOrWhiteSpace(holidayVacationView.Title))
                return BadRequest("Invalid holiday/vacation data.");

            var success = _sqlServices.CreateHolidayVacation(holidayVacationView);

            return success ? Ok("Holiday/Vacation created successfully.") : StatusCode(500, "Failed to create.");
        }

        // Update an existing holiday/vacation (Admin)
        [AuthorizeAttribute()]
        [HttpPut("admin-update/{id}")]
        public IActionResult UpdateHolidayVacation(long id, [FromBody] HolidaysVacationView updatedHolidayVacation)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            if (updatedHolidayVacation == null || string.IsNullOrWhiteSpace(updatedHolidayVacation.Title))
                return BadRequest("Invalid holiday/vacation data.");

            var success = _sqlServices.UpdateHolidayVacation(id, updatedHolidayVacation);

            return success ? Ok("Updated successfully.") : NotFound("Holiday/Vacation not found.");
        }

        // Delete a holiday/vacation (Admin)
        [AuthorizeAttribute()]
        [HttpDelete("admin-delete/{id}")]
        public IActionResult DeleteHolidayVacation(long id)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var success = _sqlServices.DeleteHolidayVacation(id);
            return success ? Ok("Deleted successfully.") : NotFound("Holiday/Vacation not found.");
        }
    }
}
