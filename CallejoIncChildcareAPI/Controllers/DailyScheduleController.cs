using CallejoIncChildCareAPI.Authorize;
using Common.Services.DailySchedule;
using Common.Services.Login;
using Common.Services.Role;
using Common.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CallejoIncChildcareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyScheduleController : ControllerBase
    {
        private IDailyScheduleService _dailyScheduleService;
        private ILoginService _loginService;

        // Constructor to inject the IDailyScheduleService
        public DailyScheduleController(IDailyScheduleService dailyScheduleService, ILoginService loginService)
        {
            _dailyScheduleService = dailyScheduleService;
            //_loginService = loginService;
        }

        //[AuthorizeAttribute()]
        [HttpGet("{date}")]
        public ActionResult<ListDailySchedule> GetDailyScheduleByDate(DateOnly date)
        {
            //if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            //{
            //    return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            //}

            var result = _dailyScheduleService.GetDailyScheduleByDate(date);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/DailySchedule/{id}
        [AuthorizeAttribute()]
        [HttpGet("ById/{id}")]
        public ActionResult<ListDailySchedule> GetDailyScheduleById(long id)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

            var result = _dailyScheduleService.GetDailyScheduleById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [AuthorizeAttribute()]
        [HttpGet]
        public ActionResult<ListDailySchedule> GetAllDailySchedules()
        {
            //if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            //{
            //    return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            //}
            var result = _dailyScheduleService.GetAllDailySchedules();
            return Ok(result);
        }

        // POST: api/DailySchedule
        [AuthorizeAttribute()]
        [HttpPost]
        public ActionResult<APIResponse> InsertDailySchedule([FromBody] DailyScheduleView dailyScheduleView)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

            var result = _dailyScheduleService.InsertDailySchedule(dailyScheduleView);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT: 
        [AuthorizeAttribute()]
        [HttpPut]
        public ActionResult<APIResponse> UpdateDailySchedule([FromBody] DailyScheduleView dailyScheduleView)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

            var result = _dailyScheduleService.UpdateDailySchedule(dailyScheduleView);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [AuthorizeAttribute()]
        [HttpDelete("{id}")]
        public ActionResult<APIResponse> DeleteDailySchedule(long id)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

            var result = _dailyScheduleService.DeleteDailySchedule(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}