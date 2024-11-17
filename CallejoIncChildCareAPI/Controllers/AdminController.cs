using Common.Services.SQL;
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


        // POST: api/admin/login
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginDTO loginInfo)
        {
            var user = await _userService.GetUserByEmailAsync(loginInfo.Email);

            if (user == null)
            {
                return Unauthorized(new APIResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
            }

            // Verify password (you should hash passwords in production)
            if (user.Password != loginInfo.Password)
            {
                return Unauthorized(new APIResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
            }

            return Ok(new APIResponse
            {
                Success = true,
                Message = "Login successful.",
                Data = new
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.FkRole
                }
            });
        }
    }
}
