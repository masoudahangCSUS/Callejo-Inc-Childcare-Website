using Microsoft.AspNetCore.Mvc;

namespace CallejoIncChildCareAPI.Authorize
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute() : base(typeof(AuthorizeAction))
        {
            Arguments = new object[] { }; // Pass any required arguments to the constructor of AuthorizeAction if needed
        }
    }
}
