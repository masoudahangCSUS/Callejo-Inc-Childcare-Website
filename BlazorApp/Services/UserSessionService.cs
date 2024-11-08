public class UserSessionService
{
    public bool UserIsLoggedIn { get; private set; }

    public void SetUserLoggedIn(bool isLoggedIn)
    {
        UserIsLoggedIn = isLoggedIn;
    }
}
