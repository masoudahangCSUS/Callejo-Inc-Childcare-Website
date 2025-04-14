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

        // POST: api/v2/Login
        [AuthenticateAttribute()]
        [HttpPost("v2")]
        public ActionResult<APIResponse> Login([FromBody] LoginView loginInfo)
        {
            if (loginInfo == null || string.IsNullOrEmpty(loginInfo.Email) || string.IsNullOrEmpty(loginInfo.Password))
            {
                return BadRequest(new APIResponse { Success = false, Message = "Email and password are required." });
            }
            var result = _loginService.LoginCallejoIncUsers(loginInfo.Email, loginInfo.Password, AuthenticateAction.Key);
            if (result.Success)
            {
                return Ok(result);
            }
            return Unauthorized(result); // Return 401 Unauthorized for failed login
        }
        // POST: api/v2/Login/IsAuthenticated
        [AuthenticateAttribute()]
        [HttpPost("v2/IsAuthenticated")]
        public ActionResult<bool> IsAuthenticated([FromBody] LoginView loginInfo)
        {
            if (loginInfo == null || string.IsNullOrEmpty(loginInfo.Email) || loginInfo.AuthenticationCookie == Guid.Empty)
            {
                return BadRequest(false);
            }
            if (loginInfo.AuthenticationCookie != null && _loginService.IsUserAuthenticatedExample(loginInfo.Email, loginInfo.AuthenticationCookie.Value))
                return Ok(true);

            return Unauthorized(false); // Return 401 Unauthorized if not authenticated
        }

        // POST: api/v1/Login
        // This is demo endpoint and not to be used for production
        [AuthenticateAttribute()]
        [HttpPost("v1")]
        public ActionResult<APIResponse> LoginExample([FromBody] LoginView loginInfo)
        {
            if (loginInfo == null || string.IsNullOrEmpty(loginInfo.UserName) || string.IsNullOrEmpty(loginInfo.Password))
            {
                return BadRequest(new APIResponse { Success = false, Message = "Username and password are required." });
            }
            var result = _loginService.LoginUserExample(loginInfo.UserName, loginInfo.Password, AuthenticateAction.Key);
            if (result.Success)
            {
                return Ok(result);
            }
            return Unauthorized(result); // Return 401 Unauthorized for failed login
        }
        // POST: api/v1/Login/IsAuthenticated
        [AuthenticateAttribute()]
        [HttpPost("v1/IsAuthenticated")]
        public ActionResult<bool> IsAuthenticatedExample([FromBody] LoginView loginInfo)
        {
            if (loginInfo == null || string.IsNullOrEmpty(loginInfo.UserName) || loginInfo.AuthenticationCookie == Guid.Empty)
            {
                return BadRequest(false);
            }
            if (loginInfo.AuthenticationCookie != null && _loginService.IsUserAuthenticatedExample(loginInfo.UserName, loginInfo.AuthenticationCookie.Value))
                return Ok(true);

            return Unauthorized(false); // Return 401 Unauthorized if not authenticated
        }
    }

}
