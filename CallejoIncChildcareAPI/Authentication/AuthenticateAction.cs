using Common.AES;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CallejoIncChildCareAPI.Authentication
{
    public class AuthenticateAction : Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter
    {
        public static List<Guid> Applications { get; set; }
        public static string Key { get; set; }

        public AuthenticateAction()
        {

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isValid = false;

            try
            {
                string encAuthorization = context.HttpContext.Request.Headers["AppId"];
                string authHeader = AesOperation.DecryptString(AuthenticateAction.Key, encAuthorization);
                string[] authHeaderArry = authHeader.Split('|');

                if (!string.IsNullOrEmpty(authHeader) && authHeaderArry.Length == 2)
                {
                    // First value in the array should be date string.  Make sure date is current date
                    // If not equal,  invalidate and force them to send auth identifier again
                    DateTime authHeaderDate = DateTime.Parse(DateTime.Parse(authHeaderArry[0]).ToString("MM/dd/yyyy"));

                    if (authHeaderDate.Equals(DateTime.Today))
                    {
                        Guid appIdGuid = Guid.Parse(authHeaderArry[1]);

                        foreach (Guid applications in Applications)
                        {
                            if (appIdGuid.Equals(applications))
                            {
                                isValid = true;
                                break;
                            }
                        }
                    }

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
    }
}
