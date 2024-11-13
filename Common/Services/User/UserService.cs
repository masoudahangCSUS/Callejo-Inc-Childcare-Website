using Common.Models.Data;
using Common.View;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.User
{
    public class UserService : IUserService
    {
        // DbContext class provides access to database
        private CallejoSystemDbContext _context;

        // Constructor uses dependency injection
        public UserService(CallejoSystemDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a user record in user database
        /// </summary>
        /// <param name="userInfo">Data to be saved.  Since the id field is an auto increment field we will not retrieve that value</param>
        /// <returns></returns>
        public APIResponse InsertUser(UserView userInfo)
        {
            APIResponse response = new APIResponse();
            string userName = userInfo.userFirstName + " " + userInfo.userMiddleName + " " + userInfo.userLastName;

            try
            {
                Models.Data.CallejoIncUser user = new Models.Data.CallejoIncUser();
                user.Id = Guid.NewGuid();
                user.FirstName = userInfo.userFirstName;
                user.MiddleName = userInfo.userMiddleName;
                user.LastName = userInfo.userLastName;
                user.Address = userInfo.userAddress;
                user.City = userInfo.userCity;
                user.State = userInfo.userState;
                user.ZipCode = userInfo.userZipCode;
                user.FkRole = userInfo.userFkRole;
                user.Email = userInfo.userEmail;
                user.Password = userInfo.userPassword;

                _context.CallejoIncUsers.Add(user);
                _context.SaveChanges();

                response.Message = "User record for  " + userName + " was saved to database";

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Problems saving user record " + userName + ". Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }

            return response;
        }
    
    }

}
