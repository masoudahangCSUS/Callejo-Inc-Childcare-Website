using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Server.Data;
using BlazorApp.Server.Models;

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CallejoSystemDbContext _context;

        public UsersController(CallejoSystemDbContext context)
        {
            _context = context;
        }

        // GET
        // gets all users
        // /api/users
        [HttpGet]
        public async Task<ActionResult<List<CallejoIncUser>>> GetUsers()
        {
            try
            {
                var users = await _context.CallejoIncUsers.ToListAsync();
                Console.WriteLine("fetch working");
                return Ok(users);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error fetching users: {error}");
                return StatusCode(500, $"Internal server error: {error.Message}");
            }
        }

        // POST
        // creates a user
        // api/users/create-user
        [HttpPost("create-user")]
        public async Task<ActionResult<CallejoIncUser>> CreateUser(CallejoIncUser newUser)
        {
            Console.WriteLine("trying to create new user");
            try
            {
                _context.CallejoIncUsers.Add(newUser);
                await _context.SaveChangesAsync();
                return Ok(newUser);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error creating user: {error}");
                return StatusCode(500, $"Internal server error: {error.Message}");
            }
        }
    }
}
