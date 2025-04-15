
using Common.Services.SQL;
using Common.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.Models.Data;
using Common.View;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Common.Services.Login;
using CallejoIncChildCareAPI.Authorize;

namespace CallejoIncChildcareAPI.Controllers
{
    [RequireHttps]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ISQLServices _sqlServices;
        private IUserService _userService;
        private ILoginService _loginService;

        public CustomerController(ISQLServices sqlServices, IUserService userService, ILoginService loginService)
        {
            _sqlServices = sqlServices;
            _userService = userService;
            _loginService = loginService;
        }

        // GET: api/customer/childrenguardian
        [AuthorizeAttribute()]
        [HttpGet]
        [Route("childrenguardian")]
        public IActionResult GetChildrenGuardian()
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var result = _sqlServices.GetListOfAllChildrenAndGuardians();
            return Ok(result);
        }

        // POST: api/customer/create-user
        [AuthorizeAttribute()]
        [HttpPost]
        [Route("create-user")]
        public ActionResult<APIResponse> InsertUser([FromBody] CustomerUserCreationDTO userInfo)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var result = _userService.InsertUser(userInfo);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [AuthorizeAttribute()]
        [HttpGet]
        [Route("get-emergency-contact")]
        public async Task<IActionResult> GetEmergencyContact(Guid id)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

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
        [AuthorizeAttribute()]
        [HttpGet]
        [Route("get-phone-number")]
        public async Task<IActionResult> GetPhoneNumber(Guid? id, long type)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var result = (await _sqlServices.GetPhoneNumber(id, type)).FirstOrDefault();
            if (result == null)
            {
                return BadRequest(result); ;
            }
            return Ok(result);
        }


        //GET: api/customer/get-user-by-id
        [AuthorizeAttribute()]
        [HttpGet]
        [Route("get-user-by-id")]
        public async Task<IActionResult> GetUserByID(Guid? id)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

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
        [AuthorizeAttribute()]
        [HttpGet]
        [Route("get-child-list")]
        public async Task<IActionResult> GetChildList(Guid? id)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

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

        [AuthorizeAttribute()]
        [HttpGet]
        [Route("get-children-by-id")]
        public async Task<IActionResult> GetChildrenByID(long id)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
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

        [AuthorizeAttribute()]
        [HttpPut("update-user/{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] CustomerUserViewDTO userDto)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
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

        [AuthorizeAttribute()]
        [HttpPut("update-emergency/{userId}")]
        public async Task<IActionResult> UpdateEmergencyContact(Guid userId, [FromBody] EmergencyContactDTO emergencyDto)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
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
        [AuthorizeAttribute()]
        [HttpPut("update-child/{childId}")]
        public async Task<IActionResult> UpdateChild(long childId, [FromBody] ChildView childDto)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
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

        [AuthorizeAttribute()]
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] SettingsDTO settings)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

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

        [AuthorizeAttribute()]
        [HttpPut("update-email")]
        public async Task<IActionResult> UpdateEmail([FromBody] SettingsDTO settings)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }

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
