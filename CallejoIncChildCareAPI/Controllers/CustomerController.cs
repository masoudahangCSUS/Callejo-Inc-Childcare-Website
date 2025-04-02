
using Common.Services.SQL;
using Common.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.Models.Data;
using Common.View;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

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


        [HttpPut("update-user/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] CustomerUserViewDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is null.");
            }

            // Retrieve the user record along with associated phone numbers.
            var user = await _sqlServices.getUserWithNumber(userId);
            bool updateStatus =await _sqlServices.updateUser(user, userDto);
            if (updateStatus)
                return Ok("User Updated Succesfully");
            else
                return BadRequest("User Update Failed");

        }

        
        [HttpPut("update-emergency/{userId}")]
        public async Task<IActionResult> UpdateEmergencyContact(Guid userId, [FromBody] EmergencyContactDTO emergencyDto)
        {
            if (emergencyDto == null)
            {
                return BadRequest("Emergency contact data is null.");
            }

            // Retrieve the emergency contact record using the parent's userId.
            var emergencyContact = await _userService.GetEmergencyContactAsync(userId);

            if (emergencyContact == null)
            {
                return NotFound("Emergency contact not found.");
            }

            bool updateEmergency = await _sqlServices.updateEmergencyContact(emergencyContact, emergencyDto);
            if (updateEmergency)
                return Ok("Emergency Contact succesfully updated");
            else
                return BadRequest("Emergency Contact update failed");
        }

        // Endpoint to update a child's information.
        [HttpPut("update-child/{childId}")]
        public async Task<IActionResult> UpdateChild(long childId, [FromBody] ChildView childDto)
        {
            if (childDto == null)
            {
                return BadRequest("Child data is null.");
            }

            // Retrieve the existing child record by its ID.
            var child = await _sqlServices.getChildById(childId);
            if (child == null)
            {
                Console.WriteLine("Child not found");
                return NotFound("Child not found.");
            }
            // call sqlServiices to updare child
            bool updateStatus = await _sqlServices.updateChild(child, childDto);
            if (updateStatus)
            {
                return Ok("Child updated successfully.");
            }
            else
            {
                return BadRequest("Child update failed.");
            }
        }

        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] SettingsDTO settings)
        {
            // make sure the data is valid
            if (settings == null || settings.Id == Guid.Empty)
            {
                return BadRequest("Invalid settings data provided.");
            }

            // call sqlServices to update passowrd
            bool updateReults = await _sqlServices.updatePassowrd(settings);
            if (!updateReults)
            {
                return StatusCode(500, "An error has ocurred whule updating the password");
            }

            return Ok("Password updated successfully.");
        }

        [HttpPut("update-email")]
        public async Task<IActionResult> UpdateEmail([FromBody] SettingsDTO settings)
        {   
            // validate the data
            if (settings == null || settings.Id == Guid.Empty)
            {
                return BadRequest("Invalid settings data provided.");
            }

            // call sqlServices to update the email
            bool updateResults = await _sqlServices.updateEmail(settings);
            if (!updateResults)
            {
                return StatusCode(500, "An error has ocurred while updating the passowrd");
            }

            return Ok("Email updated successfully.");
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
