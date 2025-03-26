using Common.AES;
using Microsoft.AspNetCore.Mvc;

namespace CallejoIncChildCareAPI.Authorize
{
    public class AuthorizeAction : Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter
    {
        public static List<Guid> Applications { get; set; }
        public static string UserName { get; set; }
        public static Guid AuthorizationToken { get; set; }
        public static string Key { get; set; }

        public AuthorizeAction()
        {
            // Initialize any required properties or fields here if needed
        }

        public void OnAuthorization(Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext context)
        {
            bool isValid = false;
            try
            {
                string encAuthorization = context.HttpContext.Request.Headers["AppId"];
                string encToken = context.HttpContext.Request.Headers["AuthorizationToken"];
                string encUserName = context.HttpContext.Request.Headers["UserName"];

                string authHeader = AesOperation.DecryptString(AuthorizeAction.Key, encAuthorization);
                string[] authHeaderArry = authHeader.Split('|');
                string token = AesOperation.DecryptString(AuthorizeAction.Key, encToken);
                AuthorizeAction.UserName = AesOperation.DecryptString(AuthorizeAction.Key, encUserName);

                if (isValidApplication(authHeader) && isValidToken(token))
                {
                    isValid = true;
                }
            }
            catch
            {
                //Don't care about any exceptions
            }

            if (!isValid)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private bool isValidApplication(string authHeader)
        {
            if (string.IsNullOrEmpty(authHeader))
                return false;
            string[] authHeaderArry = authHeader.Split('|');
            if (authHeaderArry.Length != 2)
                return false;

            // First token is primarily padding
            Guid appIdGuid = Guid.Parse(authHeaderArry[1]);
            return Applications.Contains(appIdGuid);
        }

        private bool isValidToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            Guid tokenGuid = Guid.Empty;
            bool retValue = Guid.TryParse(token, out tokenGuid);
            AuthorizeAction.AuthorizationToken = tokenGuid;
            return retValue;
        }

    }
}
