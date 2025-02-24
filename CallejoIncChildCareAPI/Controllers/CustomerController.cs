using Common.Services.SQL;
using Common.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.Models.Data;
using Common.View;

namespace CallejoIncChildcareAPI.Controllers
{
    [RequireHttps]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ISQLServices _sqlServices;
        private IUserService _userService;

        public CustomerController(ISQLServices sqlServices, IUserService userService)
        {
            _sqlServices = sqlServices;
            _userService = userService;
        }

        // GET: api/customer/childrenguardian
        [HttpGet]
        [Route("childrenguardian")]
        public IActionResult GetChildrenGuardian()
        {
            var result = _sqlServices.GetListOfAllChildrenAndGuardians();
            return Ok(result);
        }

        // POST: api/customer/create-user
        [HttpPost]
        [Route("create-user")]
        public ActionResult<APIResponse> InsertUser([FromBody] CustomerUserCreationDTO userInfo)
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

    }
}
