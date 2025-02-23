using Azure;
using Common.Models.Data;
using Common.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
        /// <param name="userDTO">Data to be saved.</param>
        /// <returns></returns>
        public APIResponse InsertUser(object userDTO)
        {
            APIResponse response = new APIResponse();

            try
            {
                Models.Data.CallejoIncUser newUser = new Models.Data.CallejoIncUser();
                newUser.Id = Guid.NewGuid();
                if (userDTO is AdminUserCreationDTO adminDTO)
                {
                    Console.WriteLine("Admin is creating an account");
                    newUser.FirstName = adminDTO.FirstName;
                    newUser.MiddleName = adminDTO.MiddleName;
                    newUser.LastName = adminDTO.LastName;
                    newUser.Address = adminDTO.Address;
                    newUser.City = adminDTO.City;
                    newUser.State = adminDTO.State;
                    newUser.ZipCode = adminDTO.ZipCode;
                    newUser.FkRole = adminDTO.FkRole;
                    newUser.Email = adminDTO.Email;
                    newUser.Password = adminDTO.Password;
                    AddChildrenToUser(newUser, adminDTO.Children);
                }
                else if (userDTO is CustomerUserCreationDTO customerDTO)
                {
                    Console.WriteLine("customer is creating an account");
                    newUser.FirstName = customerDTO.FirstName;
                    newUser.MiddleName = customerDTO.MiddleName;
                    newUser.LastName = customerDTO.LastName;
                    newUser.Address = customerDTO.Address;
                    newUser.City = customerDTO.City;
                    newUser.State = customerDTO.State;
                    newUser.ZipCode = customerDTO.ZipCode;
                    newUser.FkRole = 3;
                    newUser.Email = customerDTO.Email;
                    newUser.Password = customerDTO.Password;
                    AddChildrenToUser(newUser, customerDTO.Children);
                }

                _context.CallejoIncUsers.Add(newUser);
                _context.SaveChanges();

                response.Message = $"User record for {newUser.FirstName} was saved to database";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Problems saving user record. Error: {ex.Message}. Inner Exception: {ex.InnerException}. Stack Trace: {ex.StackTrace}";
            }

            return response;
        }

        public void AddChildrenToUser(CallejoIncUser newUser, List<ChildView> children)
        {
            if (children != null && children.Any())
            {
                foreach (var childView in children)
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
        }

        public APIResponse UpdateUser(object userDTO)
        {
            APIResponse response = new APIResponse();

            try
            {
                if (userDTO is AdminUserUpdateDTO adminDTO)
                {
                    var user = _context.CallejoIncUsers.Include(u => u.FkChildren).FirstOrDefault(u => u.Id == adminDTO.Id);

                    if (user == null)
                    {
                        response.Success = false;
                        response.Message = "User not found.";
                        return response;
                    }
                    user.FirstName = adminDTO.FirstName;
                    user.MiddleName = adminDTO.MiddleName;
                    user.LastName = adminDTO.LastName;
                    user.Address = adminDTO.Address;
                    user.City = adminDTO.City;
                    user.State = adminDTO.State;
                    user.ZipCode = adminDTO.ZipCode;
                    user.Email = adminDTO.Email;
                    if (!string.IsNullOrEmpty(adminDTO.Password))
                    {
                        user.Password = adminDTO.Password;
                    }
                    user.FkRole = adminDTO.FkRole;
                    if (adminDTO.Children != null)
                    {
                        var existingChildren = user.FkChildren.ToList();

                        // Remove children not in the new list
                        var childrenToRemove = existingChildren
                            .Where(ec => !adminDTO.Children.Any(nc => nc.Id == ec.Id))
                            .ToList();
                        foreach (var childToRemove in childrenToRemove)
                        {
                            user.FkChildren.Remove(childToRemove);
                        }

                        // Add or update children in the new list
                        foreach (var child in adminDTO.Children)
                        {
                            // new child
                            if (child.Id == 0)
                            {
                                var checkIfChildExists = _context.Children.FirstOrDefault(c => c.FirstName == child.FirstName && c.MiddleName == child.MiddleName && c.LastName == child.LastName && c.Age == child.Age);
                                
                                if (checkIfChildExists == null)
                                {
                                    var newChild = new Child
                                    {
                                        FirstName = child.FirstName,
                                        MiddleName = child.MiddleName,
                                        LastName = child.LastName,
                                        Age = child.Age
                                    };
                                    user.FkChildren.Add(newChild);
                                    _context.Children.Add(newChild);
                                }
                                else
                                {
                                    user.FkChildren.Add(checkIfChildExists);
                                }
                            }
                            else
                            {
                                // existing child, update it
                                var childToUpdate = existingChildren.FirstOrDefault(c => c.Id == child.Id);
                                if (childToUpdate != null)
                                {
                                    childToUpdate.FirstName = child.FirstName;
                                    childToUpdate.MiddleName = child.MiddleName;
                                    childToUpdate.LastName = child.LastName;
                                    childToUpdate.Age = child.Age;
                                }
                            }
                        }
                    }

                    _context.SaveChanges();
                    response.Success = true;
                    response.Message = $"Successfully updated info for {user.FirstName}";

                }
                else if (userDTO is CustomerUserUpdateDTO customerDTO)
                {

                }
                else
                {
                    response.Success = false;
                    response.Message = "Invalid DTO type.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error updating user: {ex.Message}";
            }
            return response;
        }

        public async Task<CallejoIncUser?> GetUserByEmailAsync(string email)
        {
            return await _context.CallejoIncUsers
                .FirstOrDefaultAsync(user => user.Email == email);
        }


        public APIResponse InsertChild(ChildView childInfo, CustomerUserViewDTO userInfo)
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
                var userRecs = _context.CallejoIncUsers.Include(u => u.FkChildren).ToList();
                AdminUserCreationDTO userViewRec = null;
                foreach (CallejoIncUser userRec in userRecs)
                {
                    userViewRec = new AdminUserCreationDTO();
                    userViewRec.Id = userRec.Id;
                    userViewRec.FirstName = userRec.FirstName;
                    userViewRec.MiddleName = userRec.MiddleName;
                    userViewRec.LastName = userRec.LastName;
                    userViewRec.Address = userRec.Address;
                    userViewRec.City = userRec.City;
                    userViewRec.State = userRec.State;
                    userViewRec.ZipCode = userRec.ZipCode;
                    userViewRec.Email = userRec.Email;
                    userViewRec.FkRole = userRec.FkRole;
                    userViewRec.RegistrationDocument = userRec.RegistrationDocument;

                    userViewRec.Children = new List<ChildView>();

                    foreach (var child in userRec.FkChildren)
                    {
                        var childRec = new ChildView();
                        childRec.Id = child.Id;
                        childRec.FirstName = child.FirstName;
                        childRec.MiddleName = child.MiddleName;
                        childRec.LastName = child.LastName;
                        childRec.Age = child.Age;
                        userViewRec.Children.Add(childRec);
                    }

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
                        //var otherGuardian = _context.CallejoIncUsers.Include(u => u.FkChildren).Where(u => u.FkChildren.Contains(child)).ToList();
                        //if (!otherGuardian.Any())
                        //{
                        //    Console.WriteLine($"Child record {child.Id} has no other guardian on record, deleting record now.");
                        //    _context.Children.Remove(child);
                        //}
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
