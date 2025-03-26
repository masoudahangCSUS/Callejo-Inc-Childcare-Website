using Common.AES;
using Common.View;
using Microsoft.Extensions.Options;
using RestSharp;

namespace ExampleBlazorAuthentication.Service
{
    public class RoleService
    {
        private readonly RestClient _client;
        private AppSettings _appSettings;

        public RoleService(IOptions<AppSettings> apiSettings)
        {
            _client = new RestClient(apiSettings.Value.BaseUrl);
            _appSettings = apiSettings.Value;
        }

        private void SetHeaders(RestRequest request, string userName, string authToken)
        {
            string appId = ServiceHelper.BuildAppIdHeader(_appSettings);
            request.AddHeader("AppId", appId);
            request.AddHeader("AuthorizationToken", AesOperation.EncryptString(_appSettings.Key.ToString(), authToken));
            request.AddHeader("UserName", AesOperation.EncryptString(_appSettings.Key.ToString(), userName));
        }
        public async Task<ListRoles> GetAllRoles(string userName, string authToken)
        {
            RestRequest request = new RestRequest("Role", Method.Get);
            SetHeaders(request, userName, authToken);

            var response = await _client.ExecuteAsync<ListRoles>(request);
            return response.Data;
        }

        public async Task<ListRoles> GetRole(long id, string userName, string authToken)
        {
            RestRequest request = new RestRequest($"Role/{id}", Method.Get);
            SetHeaders(request, userName, authToken);
            var response = await _client.ExecuteAsync<ListRoles>(request);
            return response.Data;
        }

        public async Task<APIResponse> InsertRole(RoleView roleInfo, string userName, string authToken)
        {
            RestRequest request = new RestRequest("Role", Method.Post);
            SetHeaders(request, userName, authToken);
            request.AddJsonBody(roleInfo);
            var response = await _client.ExecuteAsync<APIResponse>(request);
            return response.Data;
        }

        public async Task<APIResponse> UpdateRole(RoleView roleInfo, string userName, string authToken)
        {
            RestRequest request = new RestRequest("Role", Method.Put);
            SetHeaders(request, userName, authToken);
            request.AddJsonBody(roleInfo);
            var response = await _client.ExecuteAsync<APIResponse>(request);
            return response.Data;
        }

        public async Task<APIResponse> DeleteRole(long id, string userName, string authToken)
        {
            RestRequest request = new RestRequest($"Role/{id}", Method.Delete);
            SetHeaders(request, userName, authToken);
            var response = await _client.ExecuteAsync<APIResponse>(request);
            return response.Data;
        }
    }
}
