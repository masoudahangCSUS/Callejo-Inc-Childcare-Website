using CallejoIncChildCareAPI.Authentication;
using Common.Services.Login;
using Common.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CallejoIncChildCareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        // POST: api/Login
        [AuthenticateAttribute()]
        [HttpPost]
        public ActionResult<APIResponse> Login([FromBody] LoginView loginInfo)
        {
            if (loginInfo == null || string.IsNullOrEmpty(loginInfo.UserName) || string.IsNullOrEmpty(loginInfo.Password))
            {
                return BadRequest(new APIResponse { Success = false, Message = "Username and password are required." });
            }
            var result = _loginService.LoginUser(loginInfo.UserName, loginInfo.Password, AuthenticateAction.Key);
            if (result.Success)
            {
                return Ok(result);
            }
            return Unauthorized(result); // Return 401 Unauthorized for failed login
        }
        // POST: api/Login/IsAuthenticated
        [AuthenticateAttribute()]
        [HttpPost("IsAuthenticated")]
        public ActionResult<bool> IsAuthenticated([FromBody] LoginView loginInfo)
        {
            if (loginInfo == null || string.IsNullOrEmpty(loginInfo.UserName) || loginInfo.AuthenticationCookie == Guid.Empty)
            {
                return BadRequest(false);
            }
            if (loginInfo.AuthenticationCookie != null && _loginService.IsUserAuthenticated(loginInfo.UserName, loginInfo.AuthenticationCookie.Value))
                return Ok(true);

            return Unauthorized(false); // Return 401 Unauthorized if not authenticated
        }
    }

}
