﻿using Common.Services.DailySchedule;
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

        // Constructor to inject the IRoleService
        public DailyScheduleController(IDailyScheduleService dailyScheduleService)
        {
            _dailyScheduleService = dailyScheduleService;
        }

        // GET: api/DailySchedule/{id}
        [HttpGet("{id}")]
        public ActionResult<ListDailySchedule> GetDailySchedule(long id)
        {
            var result = _dailyScheduleService.GetDailySchedule(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        // POST: api/Role
        [HttpPost]
        public ActionResult<APIResponse> InsertDailySchedule([FromBody] DailyScheduleView dailyScheduleView)
        {
            var result = _dailyScheduleService.InsertDailySchedule(dailyScheduleView);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
