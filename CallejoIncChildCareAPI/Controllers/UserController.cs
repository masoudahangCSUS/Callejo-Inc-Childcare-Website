using Common.Services.User;
using Microsoft.AspNetCore.Mvc;
using Common.Models.Data;
using Common.View;

namespace CallejoIncChildcareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/login
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginInfo)
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

            // Check password (assumes plain text - be sure to hash in production!)
            if (user.Password != loginInfo.Password)
            {
                return Unauthorized(new APIResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
            }

            // If role == 1, redirect to user home page:
            if (user.FkRole == 3)
            {
                // This server-side redirect instructs the browser to go to /user/home
                return Redirect("/user/home");
            }

            // Otherwise, perhaps do something else (like redirect to an admin page),
            // or just return a JSON response. For example:
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
