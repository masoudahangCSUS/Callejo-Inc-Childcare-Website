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

        /// <summary>
        /// Get all users in the user table
        /// </summary>
        /// <returns></returns>
        public ListUsers GetAllUsers()
        {
            ListUsers listUsers = new ListUsers();

            try
            {
                var userRecs = _context.CallejoIncUsers.ToList();
                UserView userViewRec = null;
                foreach (Models.Data.CallejoIncUser userRec in userRecs)
                {
                    userViewRec = new UserView();
                    userViewRec.userId = userRec.Id;
                    userViewRec.userFirstName = userRec.FirstName;
                    userViewRec.userMiddleName = userRec.MiddleName;
                    userViewRec.userLastName = userRec.LastName;
                    userViewRec.userAddress = userRec.Address;
                    userViewRec.userCity = userRec.City;
                    userViewRec.userState = userRec.State;
                    userViewRec.userZipCode = userRec.ZipCode;
                    userViewRec.userEmail = userRec.Email;

                    listUsers.users.Add(userViewRec);
                }

                listUsers.Success = true;
                listUsers.Message = "Retrieved " + listUsers.users.Count.ToString() + " user records";
            }
            catch (Exception ex)
            {
                listUsers.Success = false;
                listUsers.Message = "Problems retrieving all user record. Error: " + ex.Message + ". Inner Exception : " + ex.InnerException + ". Stack Trace : " + ex.StackTrace;
            }

            return listUsers;
        }

    }

}
