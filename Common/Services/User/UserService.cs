using Azure;
using Common.Models.Data;
using Common.View;
using Microsoft.EntityFrameworkCore;
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
            string userName = $"{userInfo.FirstName} {userInfo.MiddleName} {userInfo.LastName}";

            try
            {
                Models.Data.CallejoIncUser newUser = new Models.Data.CallejoIncUser();
                newUser.Id = Guid.NewGuid();
                newUser.FirstName = userInfo.FirstName;
                newUser.MiddleName = userInfo.MiddleName;
                newUser.LastName = userInfo.LastName;
                newUser.Address = userInfo.Address;
                newUser.City = userInfo.City;
                newUser.State = userInfo.State;
                newUser.ZipCode = userInfo.ZipCode;
                newUser.FkRole = userInfo.FkRole;
                newUser.Email = userInfo.Email;
                newUser.Password = userInfo.Password;

                if (userInfo.Children != null && userInfo.Children.Any())
                {
                    foreach (var childView in userInfo.Children)
                    {
                        // Checks if child exists in database or not
                        var existingChild = _context.Children.FirstOrDefault(c => c.FirstName == childView.FirstName &&
                                         c.LastName == childView.LastName &&
                                         c.Age == childView.Age);

                        if (existingChild == null)
                        {
                            Models.Data.Child newChild = new Models.Data.Child();
                            newChild.FirstName = childView.FirstName;
                            newChild.MiddleName = childView.MiddleName;
                            newChild.LastName = childView.LastName;
                            newChild.Age = childView.Age;

                            newUser.FkChildren.Add(newChild);
                            _context.Children.Add(newChild);
                            newChild.FkParents.Add(newUser);
                        }
                        else
                        {
                            newUser.FkChildren.Add(existingChild);
                            existingChild.FkParents.Add(newUser);
                        }
                    }
                }

                _context.CallejoIncUsers.Add(newUser);
                _context.SaveChanges();

                response.Message = $"User record for {userName} was saved to database";

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Problems saving user record {userName}. Error: {ex.Message}. Inner Exception: {ex.InnerException}. Stack Trace: {ex.StackTrace}";
            }

            return response;
        }

        public APIResponse InsertChild(ChildView childInfo, UserView userInfo)
        {
            APIResponse response = new APIResponse();

            try
            {
                // Check if the parent exists
                var parent = _context.CallejoIncUsers
                    .Include(u => u.FkChildren)
                    .FirstOrDefault(u => u.Id == userInfo.Id);

                if (parent == null)
                {
                    response.Success = false;
                    response.Message = "Parent user not found.";
                    return response;
                }

                // Check if the child already exists in the database
                var existingChild = _context.Children
                    .FirstOrDefault(c => c.FirstName == childInfo.FirstName &&
                                         c.LastName == childInfo.LastName &&
                                         c.Age == childInfo.Age);

                if (existingChild == null)
                {
                    // Create a new child if it doesn't exist
                    Models.Data.Child newChild = new Models.Data.Child();
                    newChild.FirstName = childInfo.FirstName;
                    newChild.MiddleName = childInfo.MiddleName;
                    newChild.LastName = childInfo.LastName;
                    newChild.Age = childInfo.Age;

                    _context.Children.Add(newChild);

                    parent.FkChildren.Add(newChild);
                    newChild.FkParents.Add(parent);
                }
                else
                {
                    if (!parent.FkChildren.Contains(existingChild))
                    {
                        parent.FkChildren.Add(existingChild);
                        existingChild.FkParents.Add(parent);
                    }
                }

                // Save changes
                _context.SaveChanges();

                response.Success = true;
                response.Message = $"{childInfo.FirstName} was successfully added to the {userInfo.FirstName}.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error adding {childInfo.FirstName} to {userInfo.FirstName}: {ex.Message}. Inner Exception: {ex.InnerException}";
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
                    userViewRec.Id = userRec.Id;
                    userViewRec.FirstName = userRec.FirstName;
                    userViewRec.MiddleName = userRec.MiddleName;
                    userViewRec.LastName = userRec.LastName;
                    userViewRec.Address = userRec.Address;
                    userViewRec.City = userRec.City;
                    userViewRec.State = userRec.State;
                    userViewRec.ZipCode = userRec.ZipCode;
                    userViewRec.Email = userRec.Email;

                    listUsers.users.Add(userViewRec);
                }
                listUsers.Success = true;
                listUsers.Message = $"Retrieved {listUsers.users.Count.ToString()} user records";
            }
            catch (Exception ex)
            {
                listUsers.Success = false;
                listUsers.Message = $"Problems retrieving all user record. Error: {ex.Message}. Inner Exception: {ex.InnerException}. Stack Trace: {ex.StackTrace}";
            }

            return listUsers;
        }

        /// <summary>
        /// Deletes user record from database
        /// NOTE: Deletion will fail if user is referred to in any other table
        /// </summary>
        /// <param object of UserView>Primary key of the record to delete</param>
        /// <returns></returns>
        public APIResponse DeleteUser(Guid userId)
        {
            Console.WriteLine("Delete USER API CALLED");
            APIResponse response = new APIResponse();

            try
            {
                var userRecord = _context.CallejoIncUsers.Include(user => user.FkChildren).Where(user => user.Id == userId).FirstOrDefault();

                if (userRecord != null)
                {

                    _context.Entry(userRecord).Collection(u => u.FkChildren).Load();

                    // Removes all the children from user record, essentially removing any Guardian entries linking them together
                    var linkedChildren = userRecord.FkChildren.ToList();
                    foreach (var child in linkedChildren)
                    {
                        userRecord.FkChildren.Remove(child);
                        _context.SaveChanges();

                        // Checks if child belongs to any other guardians, if not then delete child record.
                        var otherGuardian = _context.CallejoIncUsers.Include(u => u.FkChildren).Where(u => u.FkChildren.Contains(child)).ToList();
                        if (!otherGuardian.Any())
                        {
                            Console.WriteLine($"Child record {child.Id} has no other guardian on record, deleting record now.");
                            _context.Children.Remove(child);
                        }
                    }

                    response.Message = $"User record {userRecord.Id.ToString()} : {userRecord.FirstName} has been deleted.";
                    _context.CallejoIncUsers.Remove(userRecord);

                    _context.SaveChanges();
                }
                else
                {
                    response.Message = $"No record with the id of {userId.ToString()} was not found.";
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Problems deleting user record. Error: {ex.Message}. Inner Exception: {ex.InnerException}. Stack Trace: {ex.StackTrace}";
            }

            return response;
        }

    }

}
