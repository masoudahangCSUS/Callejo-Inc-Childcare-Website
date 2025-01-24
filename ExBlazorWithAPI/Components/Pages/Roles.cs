using Microsoft.AspNetCore.Components;

namespace ExBlazorWithAPI.Components.Pages
{
    public partial class Roles : ComponentBase
    {
        private Common.View.ListRoles listRoles;
        private Common.View.RoleView role;
        private int roleId;
        private string newRoleName;
        private int updateRoleId;
        private string updateRoleName;
        private int deleteRoleId;
        private string roleInfo;
        private string allroles;

        protected override async Task OnInitializedAsync()
        {
            listRoles = await RoleService.GetAllRoles(); //Accesses static method
            BuildHtmlTable();
        }
        private void BuildHtmlTable()
        {
            allroles = "<table>";
            allroles += "<tr><th>ID</th><th>Role Description</th></tr>";

            foreach (var role in listRoles.roles)
            {
                allroles += "<tr>";
                allroles += "<td>" + role.Id.ToString() + "</td>";
                allroles += "<td>" + role.Description + "</td>";
                allroles += "</tr>";
            }

            allroles += "</table>";
        }
        private async Task GetRoleById()
        {
            roleInfo = string.Empty;
            listRoles = await RoleService.GetRole(roleId);

            foreach (var role in listRoles.roles)
            {
                roleInfo += @"<br/><br/><b>Role Description</b>: " + role.Description + "<br/><br/><br/>";
            }
        }

        private async Task AddRole()
        {
            var roleInfo = new Common.View.RoleView { Description = newRoleName };
            var response = await RoleService.InsertRole(roleInfo);
            if (response.Success)
            {
                listRoles = await RoleService.GetAllRoles();
                newRoleName = string.Empty;
            }
            BuildHtmlTable();
        }

        private async Task UpdateRole()
        {
            var roleInfo = new Common.View.RoleView { Id = updateRoleId, Description = updateRoleName };
            var response = await RoleService.UpdateRole(roleInfo);
            if (response.Success)
            {
                listRoles = await RoleService.GetAllRoles();
                updateRoleId = 0;
                updateRoleName = string.Empty;
            }
            BuildHtmlTable();
        }

        private async Task DeleteRole()
        {
            var response = await RoleService.DeleteRole(deleteRoleId);
            if (response.Success)
            {
                listRoles = await RoleService.GetAllRoles();
                deleteRoleId = 0;
            }
            BuildHtmlTable();
        }

    }
}
