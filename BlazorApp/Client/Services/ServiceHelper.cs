using BlazorApp;
using Common.AES;
using Common.View;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace ExampleBlazorAuthentication.Service
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