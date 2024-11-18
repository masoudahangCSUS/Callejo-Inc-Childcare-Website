using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.View
{
    public class RoleView
    {
        public long Id { get; set; }
        public string Description { get; set; }
    }
    public class ListRoles : APIResponse
    {
        public List<RoleView> roles { get; set; }

        public ListRoles()
        {
            roles = new List<RoleView>();
        }
    }
}