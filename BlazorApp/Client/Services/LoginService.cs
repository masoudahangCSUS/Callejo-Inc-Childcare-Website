using Common.AES;
using Common.View;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace BlazorApp.Client.Services
{
    public class LoginService
    {
        private readonly RestClient _client;
        private readonly AppSettings _appSettings;

        public LoginService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _client = new RestClient(_appSettings.BaseUrl);
        }

        public async Task<APIResponse> LoginUser(string email, string password)
        {
            var request = new RestRequest("Login/v2", Method.Post);

            string appId = ServiceHelper.BuildAppIdHeader(_appSettings);
            request.AddHeader("AppId", appId);

            LoginView loginView = new LoginView
            {
                Email = AesOperation.EncryptString(_appSettings.Key.ToString(), email),
                Password = AesOperation.EncryptString(_appSettings.Key.ToString(), password)
            };

            request.AddJsonBody(loginView);
            var response = await _client.ExecuteAsync<APIResponse>(request);
            APIResponse apiResponse = response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<APIResponse>(response.Content)
                : new APIResponse { Success = false, Message = "Login failed" };

            return apiResponse;
        }
    }
}
