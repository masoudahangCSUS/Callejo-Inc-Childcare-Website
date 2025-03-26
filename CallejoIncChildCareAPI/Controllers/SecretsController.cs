using Common.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Common.View;

namespace CallejoIncChildcareAPI.Controllers
{
    [RequireHttps]
    [ApiController]
    [Route("api/[controller]")]
    public class SecretsController : ControllerBase
    {
        private readonly CallejoSystemDbContext _context;

        public SecretsController(CallejoSystemDbContext context)
        {
            _context = context;
        }

        // POST api/Secrets
        [HttpPost]
        public async Task<IActionResult> CreateSecret([FromBody] SecretDTO model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Secret))
            {
                return BadRequest("A valid secret is required.");
            }

            // Check if a secret already exists for this user.
            var existingSecret = await _context.UserSecrets.FindAsync(model.FkUser);
            if (existingSecret != null)
            {
                return Conflict("A secret record already exists for this user.");
            }

            // Create a new UserSecret entity with the provided values.
            var userSecret = new UserSecret
            {
                FkUser = model.FkUser,
                Secret = model.Secret
            };

            _context.UserSecrets.Add(userSecret);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSecret),
                new { fkUser = model.FkUser, secret = model.Secret }, userSecret);
        }

        // GET api/Secrets/{fkUser}
        [HttpGet("{fkUser}")]
        public async Task<SecretDTO> GetSecret(Guid fkUser)
        {
            var userSecret = await _context.UserSecrets.FindAsync(fkUser);
            if (userSecret == null)
            {
                return null;
            }

            SecretDTO secretDto = new SecretDTO
            {
                FkUser = userSecret.FkUser,
                Secret = userSecret.Secret

            };

            return secretDto;
        }
    }

}

