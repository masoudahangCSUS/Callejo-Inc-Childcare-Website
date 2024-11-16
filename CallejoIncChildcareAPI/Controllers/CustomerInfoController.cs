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
    public class CustomerInfoController : ControllerBase
    {
        private ISQLServices _sqlServices;
        private IUserService _userService;

        public CustomerInfoController(ISQLServices sqlServices, IUserService userService)
        {
            _sqlServices = sqlServices;
            _userService = userService;
        }

        [HttpGet]
        [Route("childrenguardian")]
        public IActionResult GetChildrenGuardian()
        {
            var result = _sqlServices.GetListOfAllChildrenAndGuardians();
            return Ok(result);
        }

        [HttpPost]
        [Route("create-user")]
        public ActionResult<APIResponse> InsertUser([FromBody] UserView userInfo)
        {
            var result = _userService.InsertUser(userInfo);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //[HttpPost]
        //[Route("create-child")]
        //public ActionResult<APIResponse> InsertChild([FromBody] ChildView childInfo, [FromBody] UserView userInfo)
        //{
        //    var result = _userService.InsertChild(childInfo, userInfo);
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result);
        //}

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

        // GET: api/Role
        [HttpGet]
        [Route("get-all-users")]
        public ActionResult<ListUsers> GetAllUsers()
        {
            var result = _userService.GetAllUsers();
            return Ok(result);
        }
    }
}
