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
        APIResponse InsertChild(ChildView childInfo, UserView userInfo);
        //APIResponse UpdateUser(RoleView roleView);
        APIResponse DeleteUser(Guid userId);
        ListUsers GetAllUsers();
        //ListRoles GetUser(long id);
    }
}
