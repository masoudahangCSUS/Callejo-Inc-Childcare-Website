/*using Microsoft.Extensions.Options;
using RestSharp;
using System.Threading.Tasks;
using Common.View;
using Org.BouncyCastle.Asn1.Crmf;
using System.Net.Http;

public class DailyScheduleService
{
    private readonly HttpClient _httpClient;

    public DailyScheduleService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
}*/
