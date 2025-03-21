namespace Common.View
{
    public class LoginView
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid FkCallejoIncUser { get; set; }
        public Guid? AuthenticationCookie { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
