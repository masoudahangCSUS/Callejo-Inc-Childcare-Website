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
        APIResponse LoginCallejoIncUsers(string encEmail, string encPassword, string key);
        bool IsUserAuthenticated(string userName, Guid authenticationGuid);
        /// <summary>
        /// Method was used as example on how to authenticate do not use for production
        /// </summary>
        /// <param name="encUserName"></param>
        /// <param name="endPassword"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        APIResponse LoginUserExample(string encUserName, string endPassword, string key);
        /// <summary>
        /// Method was used as example on how to authenticate do not use for production
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="authenticationGuid"></param>
        /// <returns></returns>
        bool IsUserAuthenticatedExample(string userName, Guid authenticationGuid);
    }

}
