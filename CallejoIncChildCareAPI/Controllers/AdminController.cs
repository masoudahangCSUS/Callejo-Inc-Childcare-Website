﻿using Common.Services.SQL;
using Common.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.Models.Data;
using Common.View;

namespace CallejoIncChildcareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/admin/create-user
        [HttpPost]
        [Route("create-user")]
        public ActionResult<APIResponse> InsertUser([FromBody] AdminUserCreationDTO userInfo)
        {
            var result = _userService.InsertUser(userInfo);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/admin/get-all-users
        [HttpGet]
        [Route("get-all-users")]
        public ActionResult<ListUsers> GetAllUsers()
        {
            var result = _userService.GetAllUsers();
            return Ok(result);
        }

        // DELETE: api/admin/delete-user
        [HttpDelete("delete-user")]
        public ActionResult<APIResponse> DeleteUser([FromQuery] Guid userId)
        {
            var result = _userService.DeleteUser(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}