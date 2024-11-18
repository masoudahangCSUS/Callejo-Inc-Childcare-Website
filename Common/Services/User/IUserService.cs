using Common.Models.Data;
using Common.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.User
{
    public interface IUserService
    {
        APIResponse InsertUser(object userDTO);
        //APIResponse InsertChild(ChildView childInfo, CustomerUserView userInfo);
        APIResponse UpdateUser(object userDTO);
        APIResponse DeleteUser(Guid userId);
        ListUsers GetAllUsers();
        //ListRoles GetUser(long id);
        Task<CallejoIncUser?> GetUserByEmailAsync(string email);

    }
}
