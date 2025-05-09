﻿using Common.Services.SQL;
using Common.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.Models.Data;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common.View;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


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
        private readonly ISQLServices _sqlService;
        private readonly CallejoSystemDbContext _context;
        private readonly PasswordService _passwordService;




        public AdminController(IUserService userService, ImageService imageService, IConfiguration configuration, CallejoSystemDbContext context)
        {
            _userService = userService;
            _imageService = imageService;
            _configuration = configuration;
            _context = context;
        }

        //  POST: api/admin/create-user
        [HttpPost("create-user")]
        public ActionResult<APIResponse> InsertUser([FromBody] AdminUserCreationDTO userInfo)
        {
            userInfo.Password = PasswordService.HashPassword(userInfo.Password);
            var result = _userService.InsertUser(userInfo);
            return result.Success ? Ok(result) : BadRequest(result);
        }



        //  GET: api/admin/get-all-users
        [HttpGet("get-all-users")]
        public ActionResult<ListUsers> GetAllUsers()
        {
            var result = _userService.GetAllUsers();
            return Ok(result);
        }

        //  GET: api/admin/get-all-users
        [HttpGet("children")]
        public ActionResult<ListChildren> GetAllChildren()
        {
            var result = _userService.GetAllChildren();
            return Ok(result);
        }




        //  PUT: api/admin/update-user
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
                new Claim(ClaimTypes.Role, (int)user.FkRole == 1 ? "Admin" : "User"),
                new Claim(IdClaim.UserId, user.Id.ToString(), ClaimValueTypes.String),
            };

            // Create the identity and principal
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            //Configures some properties for the cookies
            // Allows the Cookie to be persistent across browser sessions
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
            };
            // Sign in the user and issue the cookie
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authProperties);

            return Ok(new APIResponse
            {
                Success = true,
                Message = "Login successful.",
                Data = userDTO
            });
        }

        //Post: api/admin/logout
        [HttpPost("logout")]
        public async Task<IActionResult> logoout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("MyAppAuthCookie");
            return Ok(new { Success = true, Message = "Logged Out Successfully" });
        }

        [HttpDelete("delete-user")]
        public async Task<ActionResult<APIResponse>> DeleteUser([FromQuery] Guid userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Load the user and include related children relationships
                var parent = await _context.CallejoIncUsers
                    .Include(u => u.FkChildren) // Include many-to-many relationship (Guardians)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (parent != null && parent.FkChildren.Any())
                {
                    // Remove the child relationships first (EF Core will handle the Guardians join table)
                    parent.FkChildren.Clear();
                    await _context.SaveChangesAsync();
                }

                // 2. Delete emergency contacts linked to the user
                var emergencyContacts = await _context.EmergencyContacts
                    .Where(ec => ec.FkUsers == userId)
                    .ToListAsync();

                if (emergencyContacts.Any())
                {
                    _context.EmergencyContacts.RemoveRange(emergencyContacts);
                    await _context.SaveChangesAsync();
                }

                // 3. Delete phone numbers linked to the user
                var phoneNumbers = await _context.PhoneNumbers
                    .Where(pn => pn.FkUsers == userId)
                    .ToListAsync();

                if (phoneNumbers.Any())
                {
                    _context.PhoneNumbers.RemoveRange(phoneNumbers);
                    await _context.SaveChangesAsync();
                }

                // 4. Delete notifications linked to the user
                var notifications = await _context.Notifications
                    .Where(n => n.FkParentId == userId)
                    .ToListAsync();

                if (notifications.Any())
                {
                    _context.Notifications.RemoveRange(notifications);
                    await _context.SaveChangesAsync();
                }

                // 5. Fetch and delete the user
                var user = await _context.CallejoIncUsers.FindAsync(userId);
                if (user == null)
                {
                    return NotFound(new APIResponse
                    {
                        Success = false,
                        Message = "User not found."
                    });
                }

                _context.CallejoIncUsers.Remove(user);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new APIResponse
                {
                    Success = true,
                    Message = "User and all related records deleted successfully."
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new APIResponse
                {
                    Success = false,
                    Message = $"Error deleting user: {ex.Message}"
                });
            }
        }

        // POST: api/admin/upload-photo
        [HttpPost("upload-photo")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new APIResponse { Success = false, Message = "No file uploaded." });

            try
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "photos");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                string fullPath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    Console.WriteLine($"Saving file into {fullPath}");
                    await file.CopyToAsync(stream);
                }

                // Insert into database
                using (var conn = new SqlConnection(_configuration.GetConnectionString("DataContext")))
                {
                    await conn.OpenAsync();
                    var query = "INSERT INTO Images (file_name, is_published, uploaded_at) VALUES (@FileName, 0, GETUTCDATE())";

                    using var cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FileName", uniqueFileName);
                    await cmd.ExecuteNonQueryAsync();
                }

                return Ok(new APIResponse { Success = true, Message = "Image uploaded successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                return StatusCode(500, new APIResponse { Success = false, Message = "Server error while uploading image." });
            }
        }

        [HttpGet("get-all-photos")]
        public async Task<ActionResult<List<Common.View.Image>>> GetAllPhotos()
        {
            var images = await _imageService.GetAllImagesAsync();
            Console.WriteLine("returning all images");
            Console.WriteLine(images);
            return Ok(images);
        }


        [HttpGet("/api/photos/featured")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Common.View.Image>>> GetPublishedPhotos()
        {
            var images = await _imageService.GetPublishedImagesAsync();
            Console.WriteLine("returning all featured images");
            Console.WriteLine($"Count: {images.Count}");

            foreach (var img in images)
            {
                Console.WriteLine($"✔️ File: {img.FileName}, Published: {img.IsPublished}");
            }
            return Ok(images);
        }

        [HttpPost("publish-images")]
        public async Task<IActionResult> PublishSelectedImages([FromBody] List<string> selectedFileNames)
        {
            try
            {

                await _context.Database.ExecuteSqlRawAsync("UPDATE Images SET is_published = 0");

                if (selectedFileNames.Any())
                {
                    foreach (var name in selectedFileNames)
                    {
                        await _context.Database.ExecuteSqlRawAsync(
                            "UPDATE Images SET is_published = 1 WHERE file_name = {0}", name);
                    }
                }

                return Ok(new APIResponse { Success = true, Message = "Featured images updated." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse { Success = false, Message = ex.Message });
            }
        }



    }

    public static class IdClaim
    {
        public const string UserId = "http://schemas.yourapp.com/claims/userid";

    }
}