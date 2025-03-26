using CallejoIncChildCareAPI.Authorize;
using Common.Services.Login;
using Common.Services.Role;
using Common.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CallejoIncChildcareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRoleService _roleService;
        private ILoginService _loginService;

        // Constructor to inject the IRoleService
        public RoleController(IRoleService roleService,
                              ILoginService loginService)
        {
            _roleService = roleService;
            _loginService = loginService;
        }


        // GET: api/Role
        [AuthorizeAttribute()]
        [HttpGet]
        public ActionResult<ListRoles> GetAllRoles()
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var result = _roleService.GetAllRoles();
            return Ok(result);
        }

        // GET: api/Role/{id}
        [AuthorizeAttribute()]
        [HttpGet("{id}")]
        public ActionResult<ListRoles> GetRole(long id)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

            var result = _roleService.GetRole(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // POST: api/Role
        [AuthorizeAttribute()]
        [HttpPost]
        public ActionResult<APIResponse> InsertRole([FromBody] RoleView roleInfo)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

            var result = _roleService.InsertRole(roleInfo);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT: api/Role/{id}
        [AuthorizeAttribute()]
        [HttpPut]
        public ActionResult<APIResponse> UpdateRole([FromBody] RoleView roleInfo)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

            var result = _roleService.UpdateRole(roleInfo);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // DELETE: api/Role/{id}
        [AuthorizeAttribute()]
        [HttpDelete("{id}")]
        public ActionResult<APIResponse> DeleteRole(long id)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

            var result = _roleService.DeleteRole(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }

}
