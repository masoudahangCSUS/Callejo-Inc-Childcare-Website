using Common.AES;
using Common.View;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace ExampleBlazorAuthentication.Service
{
    public class LoginService
    {
        private readonly RestClient _client;
        private AppSettings _appSettings;

        public LoginService(IOptions<AppSettings> apiSettings)
        {
            _client = new RestClient(apiSettings.Value.BaseUrl);
            _appSettings = apiSettings.Value;
        }

        public async Task<APIResponse> LoginUser(string userName, string password)
        {
            var request = new RestRequest("Login", Method.Post);

            string appId = ServiceHelper.BuildAppIdHeader(_appSettings);
            request.AddHeader("AppId", appId);

            LoginView loginView = new LoginView();
            loginView.UserName = AesOperation.EncryptString(_appSettings.Key.ToString(), userName);
            loginView.Password = AesOperation.EncryptString(_appSettings.Key.ToString(), password);

            request.AddJsonBody(loginView);
            var response = await _client.ExecuteAsync<APIResponse>(request);
            APIResponse apiResponse = null;
            if (response.IsSuccessStatusCode)
            {
                apiResponse = JsonConvert.DeserializeObject<APIResponse>(response.Content);
            }
            else
            {
                apiResponse = new APIResponse();
                apiResponse.Success = false;
                apiResponse.Message = "Login failed";
            }
            return apiResponse;
        }
    }

}
