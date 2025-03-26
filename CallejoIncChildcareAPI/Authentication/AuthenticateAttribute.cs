using Microsoft.AspNetCore.Mvc;

namespace CallejoIncChildCareAPI.Authentication
{
    public class AuthenticateAttribute : TypeFilterAttribute
    {
        public AuthenticateAttribute() : base(typeof(AuthenticateAction))
        {
            Arguments = new object[] { }; // Pass any required arguments to the constructor of AuthenticateAction if needed
        }
    }
}
