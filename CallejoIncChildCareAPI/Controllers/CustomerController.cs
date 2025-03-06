
using Common.Services.SQL;
using Common.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.Models.Data;
using Common.View;
using Microsoft.AspNetCore.Http.HttpResults;

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

       //GET: api/customer/get-emergency-contact
       [HttpGet]
       [Route("get-emergency-contact")]
       public async Task<IActionResult> GetEmergencyContact(Guid id)
        {
            var result = await _userService.GetEmergencyContactAsync(id);
            if (result == null)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        
        //GET: api/customer/get-phone-number
        [HttpGet]
        [Route("get-phone-number")]
        public async Task<IActionResult> GetPhoneNumber(Guid? id, long type)
        {
            var result = (await _sqlServices.GetPhoneNumber(id, type)).FirstOrDefault();
            if (result == null)
            {
                return BadRequest(result); ;
            }
            return Ok(result);
        }


        //GET: api/customer/get-user-by-id
        [HttpGet]
        [Route("get-user-by-id")]
        public async Task<IActionResult> GetUserByID(Guid? id)
        {
            var result = await _userService.GetUserByID(id);
            if (result == null)
            {
                return BadRequest(result); 
            }
            return Ok(result);
        }

        //GET: api/customer/get-child-list
        [HttpGet]
        [Route("get-child-list")]
        public async Task<IActionResult> GetChildList(Guid? id)
        {
            var result = await _sqlServices.GetChildren(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("get-children-by-id")]
        public async Task<IActionResult> GetChildrenByID(long id)
        {
            var result = await _sqlServices.getChildById(id);
            return Ok(result);
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
