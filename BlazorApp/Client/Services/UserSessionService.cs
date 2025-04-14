using System;

public class UserSessionService
{
    public bool UserIsLoggedIn { get;  set; }
    public long UserRole { get; private set; }
    public Guid AuthToken { get; private set; }
    public string UserRoleName { get; private set; }

    public void SetUser(bool isLoggedIn, long role, Guid authToken, string userRoleName)
    {
        UserIsLoggedIn = isLoggedIn;
        UserRole = role;
        AuthToken = authToken;
        UserRoleName = userRoleName;

        Console.WriteLine($"DEBUG: User logged in → {isLoggedIn}");
    }
}
