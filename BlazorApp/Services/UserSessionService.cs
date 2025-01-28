public class UserSessionService
{
    public bool UserIsLoggedIn { get; private set; }
    public int? UserRole { get; private set; } // Store the user role

    public void SetUserLoggedIn(bool isLoggedIn)
    {
        UserIsLoggedIn = isLoggedIn;
    }
    public void SetUser(bool isLoggedIn, int? role = null)
    {
        UserIsLoggedIn = isLoggedIn;
        UserRole = role;
    }
}
