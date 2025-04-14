using Common.AES;

namespace BlazorApp.Client.Services
{
    public class ServiceHelper
    {
        public static string BuildAppIdHeader(AppSettings apiSettings)
        {
            string appId = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff") + "|" + apiSettings.Application.ToString();
            string encAppId = AesOperation.EncryptString(apiSettings.Key.ToString(), appId);

            return encAppId;
        }

    }
}
