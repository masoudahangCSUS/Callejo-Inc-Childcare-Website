using System;

public class UserSessionService
{
    public bool UserIsLoggedIn { get;  set; }
    public int? UserRole { get; private set; }
    public Guid? UserId { get; private set; }

    public void SetUser(bool isLoggedIn, int? role = null, Guid? userId = null)
    {
        UserIsLoggedIn = isLoggedIn;
        UserRole = role;
        UserId = userId;

        Console.WriteLine($"DEBUG: User logged in → {isLoggedIn}, Role: {role}, User ID: {UserId?.ToString()}");
    }
}
