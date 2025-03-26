using Common.AES;
using Common.Models.Data;
using Common.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Login
{
    public class LoginService : ILoginService
    {
        private CallejoSystemDbContext _context;

        public LoginService(CallejoSystemDbContext context)
        {
            _context = context;
        }

        public APIResponse LoginUser(string encUserName, string endPassword, string key)
        {
            APIResponse response = new APIResponse();
            try
            {
                string userName = AesOperation.DecryptString(key, encUserName);
                string password = AesOperation.DecryptString(key, endPassword);
                var loginRecord = _context.Logins
                    .Where(l => l.Username == userName && l.Password == password)
                    .FirstOrDefault();
                if (loginRecord != null)
                {
                    response.Success = true;
                    response.Message = "Login successful for user: " + encUserName;

                    // Update login with last login time along with GUID for authentication
                    Guid authenticationGuid = Guid.NewGuid();
                    loginRecord.LastLogin = DateTime.Now;
                    loginRecord.AuthenticationToken = authenticationGuid;
                    _context.SaveChanges();

                    response.Token = authenticationGuid; // Return the authentication token
                }
                else
                {
                    response.Success = false;
                    response.Message = "Invalid username or endPassword.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error during login: " + ex.Message;
            }
            return response;

        }
        public bool IsUserAuthenticated(string userName, Guid authenticationGuid)
        {
            var loginRecord = _context.Logins
                .Where(l => l.Username == userName && l.AuthenticationToken == authenticationGuid)
                .FirstOrDefault();

            if (loginRecord == null)
                return false;

            // Force user to log back in again if the last login is older than 24 hours
            return (DateTime.Now - loginRecord.LastLogin).TotalHours <= 24;
        }
    }

}
