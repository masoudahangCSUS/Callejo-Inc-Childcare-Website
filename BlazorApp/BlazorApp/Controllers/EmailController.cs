using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BlazorApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("send-reset-password")]
        public async Task<IActionResult> SendResetPasswordEmail([FromBody] PasswordResetRequest model)
        {
            var token = "123";
            var resetLink = $"{model.BaseUrl}reset-password?token={token}";
            var subject = "Reset Your Password";
            var body = $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";

            var smtpSettings = _configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
            smtpSettings.Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD");

            // Create and send the email
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(smtpSettings.SenderName, smtpSettings.SenderEmail));
            emailMessage.To.Add(MailboxAddress.Parse(model.Email));
            emailMessage.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            emailMessage.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpSettings.Server, smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpSettings.Username, smtpSettings.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);

            return Ok(new { Message = "Password reset email sent successfully.", Token = token });
        }
    }

    public class PasswordResetRequest
    {
        public string Email { get; set; }
        public String BaseUrl { get; set; }
    }

    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
