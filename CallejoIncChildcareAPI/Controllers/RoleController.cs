using Common.Services.Role;
using Common.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CallejoIncChildcareAPI.Controllers
{
    [RequireHttps]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRoleService _roleService;

        // Constructor to inject the IRoleService
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        // GET: api/Role
        [HttpGet]
        public ActionResult<ListRoles> GetAllRoles()
        {
            var result = _roleService.GetAllRoles();
            return Ok(result);
        }

        // GET: api/Role/{id}
        [HttpGet("{id}")]
        public ActionResult<ListRoles> GetRole(long id)
        {
            var result = _roleService.GetRole(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // POST: api/Role
        [HttpPost]
        public ActionResult<APIResponse> InsertRole([FromBody] DailyScheduleView roleInfo)
        {
            var result = _roleService.InsertRole(roleInfo);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // PUT: api/Role/{id}
        [HttpPut]
        public ActionResult<APIResponse> UpdateRole([FromBody] DailyScheduleView roleInfo)
        {
            var result = _roleService.UpdateRole(roleInfo);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // DELETE: api/Role/{id}
        [HttpDelete("{id}")]
        public ActionResult<APIResponse> DeleteRole(long id)
        {
            var result = _roleService.DeleteRole(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }

}
