
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

        [HttpGet]
        [Route("get-emergency-contact")]
        public async Task<IActionResult> GetEmergencyContact(Guid id)
        {
            // Retrieve the emergency contact record.
            var contact = await _userService.GetEmergencyContactAsync(id);
            if (contact == null)
            {
                return NotFound("Emergency contact not found.");
            }

            // Convert the EmergencyContact object to an EmergencyContactDTO
            var emergencyDTO = new EmergencyContactDTO
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Relationship = contact.Relationship,
                PrimaryPhoneNumber = (await _sqlServices.GetPhoneNumber(id, 3)).FirstOrDefault() is PhoneNumber emergencyPrimary
                    ? new PhoneNumberDTO
                    {
                        Id = emergencyPrimary.Id,
                        AreaCode = emergencyPrimary.AreaCode,
                        Prefix = emergencyPrimary.Prefix,
                        LastFour = emergencyPrimary.LastFour,
                        Fk_users = emergencyPrimary.FkUsers,
                        Type = emergencyPrimary.FkType
                    }
                    : null,
                SecondaryPhoneNumber = (await _sqlServices.GetPhoneNumber(id, 4)).FirstOrDefault() is PhoneNumber emergencySecondary
                    ? new PhoneNumberDTO
                    {
                        Id = emergencySecondary.Id,
                        AreaCode = emergencySecondary.AreaCode,
                        Prefix = emergencySecondary.Prefix,
                        LastFour = emergencySecondary.LastFour,
                        Fk_users = emergencySecondary.FkUsers,
                        Type = emergencySecondary.FkType
                    }
                    : null
            };

            return Ok(emergencyDTO);
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
            // Retrieve the user record (internal model)
            var user = await _userService.GetUserByID(id);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Convert the CallejoIncUser object to a DTO.
            var userDTO = new CustomerUserViewDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName ?? string.Empty,
                LastName = user.LastName,
                Address = user.Address,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode,
                Email = user.Email,
                // Convert phone numbers to DTOs
                PrimaryPhoneNumber = user.PhoneNumbers.FirstOrDefault(p => p.FkType == 1) is PhoneNumber primary
                    ? new PhoneNumberDTO
                    {
                        Id = primary.Id,
                        AreaCode = primary.AreaCode,
                        Prefix = primary.Prefix,
                        LastFour = primary.LastFour,
                        Fk_users = primary.FkUsers,
                        Type = primary.FkType
                    }
                    : null,
                SecondaryPhoneNumber = user.PhoneNumbers.FirstOrDefault(p => p.FkType == 2) is PhoneNumber secondary
                    ? new PhoneNumberDTO
                    {
                        Id = secondary.Id,
                        AreaCode = secondary.AreaCode,
                        Prefix = secondary.Prefix,
                        LastFour = secondary.LastFour,
                        Fk_users = secondary.FkUsers,
                        Type = secondary.FkType
                    }
                    : null,
                // Optionally, convert children if needed.
                Children = user.FkChildren.Select(child => new ChildView
                {
                    Id = child.Id,
                    FirstName = child.FirstName,
                    MiddleName = child.MiddleName ?? string.Empty,
                    LastName = child.LastName,
                    Age = child.Age
                }).ToList()
            };

            return Ok(userDTO);
        }


        //GET: api/customer/get-child-list
        [HttpGet]
        [Route("get-child-list")]
        public async Task<IActionResult> GetChildList(Guid? id)
        {
            // Assume _sqlServices.GetChildren(id) now returns a list of Child objects.
            var children = await _sqlServices.GetChildren(id);
            // Convert each raw Child to a ChildDTO:
            var childDTOList = children.Select(child => new ChildDTO
            {
                Id = child.Id,
                FirstName = child.FirstName,
                MiddleName = child.MiddleName ?? string.Empty,
                LastName = child.LastName,
                Age = child.Age
            }).ToList();

            return Ok(childDTOList);
        }

        [HttpGet]
        [Route("get-children-by-id")]
        public async Task<IActionResult> GetChildrenByID(long id)
        {
            var child = await _sqlServices.getChildById(id);
            if (child == null)
            {
                return NotFound("Child not found.");
            }

            var childDTO = new ChildDTO
            {
                Id = child.Id,
                FirstName = child.FirstName,
                MiddleName = child.MiddleName ?? string.Empty,
                LastName = child.LastName,
                Age = child.Age
            };

            return Ok(childDTO);
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
            bool updateStatus = await _sqlServices.updateUser(user, userDto);
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
