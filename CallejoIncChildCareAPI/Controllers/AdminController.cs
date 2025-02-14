using Common.Services.SQL;
using Common.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.Models.Data;
using Common.View;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace CallejoIncChildcareAPI.Controllers
{
    [RequireHttps]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ImageService _imageService;
        private readonly IConfiguration _configuration;

        public AdminController(IUserService userService, ImageService imageService, IConfiguration configuration)
        {
            _userService = userService;
            _imageService = imageService;
            _configuration = configuration;
        }

        // ✅ POST: api/admin/create-user
        [HttpPost("create-user")]
        public ActionResult<APIResponse> InsertUser([FromBody] AdminUserCreationDTO userInfo)
        {
            var result = _userService.InsertUser(userInfo);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ✅ GET: api/admin/get-all-users
        [HttpGet("get-all-users")]
        public ActionResult<ListUsers> GetAllUsers()
        {
            var result = _userService.GetAllUsers();
            return Ok(result);
        }

        // ✅ DELETE: api/admin/delete-user
        [HttpDelete("delete-user")]
        public ActionResult<APIResponse> DeleteUser([FromQuery] Guid userId)
        {
            var result = _userService.DeleteUser(userId);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        // ✅ PUT: api/admin/update-user
        [HttpPut("update-user")]
        public ActionResult<APIResponse> UpdateUser([FromBody] AdminUserUpdateDTO userDTO)
        {
            var result = _userService.UpdateUser(userDTO);
            Console.WriteLine("DEBUG: Reached UpdateUser method");
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        // ✅ POST: api/admin/login
        [HttpPost("login")]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginDTO loginInfo)
        {
            var user = await _userService.GetUserByEmailAsync(loginInfo.Email);
            if (user == null || user.Password != loginInfo.Password)
            {
                return Unauthorized(new APIResponse
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
            }

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = (int)user.FkRole
            };

            // Create the user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.FkRole == 1 ? "Admin" : "User")
            };

            // Create the identity and principal
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Sign in the user and issue the cookie
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          claimsPrincipal);

            return Ok(new APIResponse
            {
                Success = true,
                Message = "Login successful.",
                Data = userDTO
            });
        }

        // ✅ POST: api/admin/upload-image
        [HttpPost("upload-image")]
        public async Task<ActionResult<APIResponse>> UploadImage([FromBody] ImageUploadDTO imageData)
        {
            try
            {
                Console.WriteLine($"DEBUG: Received image upload request: {imageData?.ImageUrl}");

                if (string.IsNullOrWhiteSpace(imageData?.ImageUrl))
                {
                    Console.WriteLine("DEBUG: [FAILURE] Empty image URL received.");
                    return BadRequest(new APIResponse { Success = false, Message = "Image URL is empty." });
                }

                // Store Image URL using ImageService
                await _imageService.SaveImageUrlAsync(imageData.ImageUrl);

                Console.WriteLine($"DEBUG: Image URL {imageData.ImageUrl} successfully saved to DB.");
                return Ok(new APIResponse { Success = true, Message = "Image URL stored successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DEBUG: [EXCEPTION] Error saving image URL. {ex.Message}");
                return StatusCode(500, new APIResponse { Success = false, Message = ex.Message });
            }
        }

        // ✅ GET: api/admin/get-latest-image
        [HttpGet("get-latest-image")]
        public async Task<ActionResult<APIResponse>> GetLatestImage()
        {
            try
            {
                var imageUrl = await _imageService.GetLatestImageUrlAsync();

                if (string.IsNullOrEmpty(imageUrl))
                {
                    return NotFound(new APIResponse { Success = false, Message = "No image found." });
                }

                return Ok(new APIResponse { Success = true, Data = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse { Success = false, Message = ex.Message });
            }
        }

        // ✅ Direct SQL insert method (if _imageService is unavailable)
        private async Task SaveImageUrlAsync(string imageUrl)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var query = "INSERT INTO Images (image_url, uploaded_at) VALUES (@ImageUrl, GETUTCDATE())";

                using var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
