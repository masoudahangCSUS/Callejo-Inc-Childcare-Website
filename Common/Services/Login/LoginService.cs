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
        public APIResponse LoginCallejoIncUsers(string encEmail, string encPassword, string key)
        {
            APIResponse response = new APIResponse();

            try
            {
                string email = AesOperation.DecryptString(key, encEmail);
                string password = AesOperation.DecryptString(key, encPassword);

                var callejoIncUser = _context.CallejoIncUsers
                    .Where(u => u.Email == email && u.Password == password)
                    .FirstOrDefault();
                if (callejoIncUser != null)
                {
                    response.Success = true;
                    response.Message = "Login successful for user: " + encEmail;
                    // Update login with last login time along with GUID for authentication
                    Guid authenticationGuid = Guid.NewGuid();
                    callejoIncUser.LastLogin = DateTime.Now;
                    callejoIncUser.AuthenticationGuid = authenticationGuid;
                    _context.SaveChanges();
                    response.Token = authenticationGuid; // Return the authentication token

                    response.RoleId = callejoIncUser.FkRole;
                    GetRoleForUser(response);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Invalid email or password.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error during login: " + ex.Message;
            }
            return response;
        }
        private void GetRoleForUser(APIResponse response)
        {
            var roleRec = _context.Roles.Where(r => r.Id == response.RoleId).FirstOrDefault();

            if (roleRec == null)
            {
                throw new Exception("User has no assigned role");
            }
            response.Role = roleRec.Description;
        }

        public APIResponse LoginUserExample(string encUserName, string endPassword, string key)
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
        public bool IsUserAuthenticated(string email, Guid authenticationGuid)
        {
            var callejoIncUser = _context.CallejoIncUsers
                .Where(l => l.Email == email && l.AuthenticationGuid == authenticationGuid)
                .FirstOrDefault();
            if (callejoIncUser == null)
                return false;
            // Force user to log back in again if the last login is older than 24 hours
            return (DateTime.Now - callejoIncUser.LastLogin).TotalHours <= 24;
        }
        public bool IsUserAuthenticatedExample(string userName, Guid authenticationGuid)
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
