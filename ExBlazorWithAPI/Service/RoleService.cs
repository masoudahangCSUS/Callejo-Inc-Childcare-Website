using Common.View;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Threading.Tasks;

namespace ExBlazorWithAPI.Service
{
    public class RoleService
    {
        private readonly RestClient _client;

        public RoleService(IOptions<AppSettings> apiSettings)
        {
            _client = new RestClient(apiSettings.Value.BaseUrl);
        }

        public async Task<ListRoles> GetAllRoles()
        {
            var request = new RestRequest("Role", Method.Get);
            var response = await _client.ExecuteAsync<ListRoles>(request);
            return response.Data;
        }

        public async Task<ListRoles> GetRole(long id)
        {
            var request = new RestRequest($"Role/{id}", Method.Get);
            var response = await _client.ExecuteAsync<ListRoles>(request);
            return response.Data;
        }

        public async Task<APIResponse> InsertRole(RoleView roleInfo)
        {
            var request = new RestRequest("Role", Method.Post);
            request.AddJsonBody(roleInfo);
            var response = await _client.ExecuteAsync<APIResponse>(request);
            return response.Data;
        }

        public async Task<APIResponse> UpdateRole(RoleView roleInfo)
        {
            var request = new RestRequest("Role", Method.Put);
            request.AddJsonBody(roleInfo);
            var response = await _client.ExecuteAsync<APIResponse>(request);
            return response.Data;
        }

        public async Task<APIResponse> DeleteRole(long id)
        {
            var request = new RestRequest($"Role/{id}", Method.Delete);
            var response = await _client.ExecuteAsync<APIResponse>(request);
            return response.Data;
        }
    }
}
