using QRCoder;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtpNet;
using Common.View;
using Microsoft.EntityFrameworkCore;
using Common.Services.Login;
using CallejoIncChildCareAPI.Authorize;

[RequireHttps]
[ApiController]
[Route("api/[controller]")]
public class MAController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private ILoginService _loginService;

    public MAController(HttpClient httpClient, ILoginService loginService)
    {
        _httpClient = httpClient;
        _loginService = loginService;
    }

    // GET: api/MA/QRGenerate/{id}?email={email}
    [AuthorizeAttribute()]
    [HttpGet("QRGenerate/{id}")]
    public async Task<IActionResult> QRGenerate(Guid id, [FromQuery] string email)
    {
        if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
        {
            return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
        }
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email parameter is required.");
        }

        // First, try to retrieve an existing secret for this user.
        SecretDTO secretDTO = null;
        var getResponse = await _httpClient.GetAsync($"https://localhost:7139/api/Secrets/{id}");
        if (getResponse.IsSuccessStatusCode)
        {
            secretDTO = await getResponse.Content.ReadFromJsonAsync<SecretDTO>();
        }

        // If no secret exists, generate a new one and store it.
        if (secretDTO == null)
        {
            string secret = GenerateSecret();
            secretDTO = new SecretDTO
            {
                FkUser = id,
                Secret = secret
            };

            var createResult = await CreateUserSecret(secretDTO);
            // Accept either Ok or CreatedAtAction
            if (!(createResult is OkObjectResult || createResult is CreatedAtActionResult))
            {
                return StatusCode(500, "Error creating the secret.");
            }
        }

        string issuer = "CallejoInc";
        // Create the TOTP URI using the provided email.
        var totpUri = $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(email)}?secret={secretDTO.Secret}&issuer={Uri.EscapeDataString(issuer)}";

        // Generate the QR code
        using (var qrGenerator = new QRCodeGenerator())
        {
            var qrCodeData = qrGenerator.CreateQrCode(totpUri, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new Base64QRCode(qrCodeData);
            var qrCodeImageAsBase64 = qrCode.GetGraphic(20);
            return Ok(new { QrCodeImage = "data:image/png;base64," + qrCodeImageAsBase64 });
        }
    }

    // POST api/MA/Validate
    [AuthorizeAttribute()]
    [HttpPost("Validate")]
    public async Task<IActionResult> ValidateTotp([FromBody] ValidationDTO request, Guid id)
    {
        if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
        {
            return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
        }

        // Build the URL to retrieve the secret.
        var secretUrl = $"https://localhost:7139/api/Secrets/{request.UserId}";

        // First, get the response.
        var secretResponse = await _httpClient.GetAsync(secretUrl);
        if (!secretResponse.IsSuccessStatusCode)
        {
            return NotFound("User secret not found.");
        }


        // Now safely read the content as JSON.
        var userSecret = await secretResponse.Content.ReadFromJsonAsync<SecretDTO>();
        if (userSecret == null)
        {
            return NotFound("User secret not found.");
        }
     

        // Convert the Base32 secret back to bytes.
        byte[] secretBytes = Base32Encoding.ToBytes(userSecret.Secret);

        // Create a Totp instance using the secret.
        var totp = new Totp(secretBytes);

        // Verify the provided TOTP code. Allow one previous and one future time step.
        bool isValid = totp.VerifyTotp(request.TotpCode, out long timeStepMatched, new VerificationWindow(previous: 1, future: 1));

        if (isValid)
        {
            return Ok(new { Message = "TOTP code is valid." });
        }
        else
        {
            return BadRequest(new { Message = "Invalid TOTP code." });
        }
    }

    private string GenerateSecret()
    {
        byte[] secretKey = KeyGeneration.GenerateRandomKey(20);
        return Base32Encoding.ToString(secretKey);
    }

    // This method calls the external Secrets API to store the user secret.
    [AuthorizeAttribute()]
    [NonAction]
    public async Task<IActionResult> CreateUserSecret(SecretDTO model)
    {
        if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
        {
            return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
        }
        if (model == null)
        {
            return BadRequest("Secret model cannot be null.");
        }

        // Use an absolute URL for the Secrets API.
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7139/api/Secrets", model);

        if (response.IsSuccessStatusCode)
        {
            var createdSecret = await response.Content.ReadFromJsonAsync<SecretDTO>();
            // You might get either a CreatedAtActionResult or OkObjectResult.
            return Ok(createdSecret);
        }
        else
        {
            return StatusCode((int)response.StatusCode, "Error creating the secret.");
        }
    }
}
