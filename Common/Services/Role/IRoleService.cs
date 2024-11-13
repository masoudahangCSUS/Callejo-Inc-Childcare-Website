using Common.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Role
{
    public interface IRoleService
    {
        APIResponse InsertRole(RoleView roleInfo);
        APIResponse UpdateRole(RoleView roleView);
        APIResponse DeleteRole(long id);
        ListRoles GetAllRoles();
        ListRoles GetRole(long id);
    }
}
