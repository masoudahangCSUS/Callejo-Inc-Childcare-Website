using Common.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Login
{
    public interface ILoginService
    {
        APIResponse LoginUser(string encUserName, string endPassword, string key);
        bool IsUserAuthenticated(string userName, Guid authenticationGuid);
    }

}
