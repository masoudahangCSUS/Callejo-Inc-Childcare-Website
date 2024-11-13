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
        APIResponse InsertUser(UserView userInfo);
        //APIResponse UpdateUser(RoleView roleView);
        //APIResponse DeleteUser(long id);
        ListUsers GetAllUsers();
        //ListRoles GetUser(long id);
    }
}
