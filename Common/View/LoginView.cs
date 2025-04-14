namespace Common.View
{
    public class LoginView
    {
        public string Email { get; set; }
        public string? UserName { get; set; }
        public string Password { get; set; }
        public Guid? AuthenticationCookie { get; set; }
    }
}
