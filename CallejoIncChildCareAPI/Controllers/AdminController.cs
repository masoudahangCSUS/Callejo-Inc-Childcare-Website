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
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly ImageService _imageService;

        public AdminController(IUserService userService, ImageService imageService)
        {
            _userService = userService;
            _imageService = imageService;
        }

        // POST: api/admin/create-user
        [HttpPost("create-user")]
        public ActionResult<APIResponse> InsertUser([FromBody] AdminUserCreationDTO userInfo)
        {
            var result = _userService.InsertUser(userInfo);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // GET: api/admin/get-all-users
        [HttpGet("get-all-users")]
        public ActionResult<ListUsers> GetAllUsers()
        {
            var result = _userService.GetAllUsers();
            return Ok(result);
        }

        // DELETE: api/admin/delete-user
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

        // PUT: api/admin/update-user
        [HttpPut("update-user")]
        public ActionResult<APIResponse> UpdateUser([FromBody] AdminUserUpdateDTO userDTO)
        {
            var result = _userService.UpdateUser(userDTO);
            Console.WriteLine("Does it reach here?");
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        // POST: api/admin/login
        [HttpPost("login")]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginDTO loginInfo)
        {
            var user = await _userService.GetUserByEmailAsync(loginInfo.Email);
            if (user == null)
            {
                return Unauthorized(new APIResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
            }

            // Verify password (in production, use hashing + salted storage)
            if (user.Password != loginInfo.Password)
            {
                return Unauthorized(new APIResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
            }

            // Create a DTO so we don't expose fields like Password, RegistrationDocument, etc.
            var userDTO = new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                // Cast from long to int if needed:
                Role = (int)user.FkRole
            };

            return Ok(new APIResponse
            {
                Success = true,
                Message = "Login successful.",
                Data = userDTO
            });
        }
        [HttpPost("upload-image")]
        public async Task<ActionResult<APIResponse>> UploadImage([FromBody] ImageUploadDTO imageData)
        {
            try
            {
                await _imageService.SaveImageUrlAsync(imageData.ImageUrl);
                return Ok(new APIResponse { Success = true, Message = "Image URL stored successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse { Success = false, Message = ex.Message });
            }
        }

    }
}