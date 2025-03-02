/*using Microsoft.Extensions.Options;
using RestSharp;
using System.Threading.Tasks;
using Common.View;
using BlazorApp;
using Org.BouncyCastle.Asn1.Crmf;

public class DailyScheduleService
{
    private readonly RestClient _client;

    public DailyScheduleService(IOptions<AppSettings> apiSettings)
    {
        _client = new RestClient(apiSettings.Value.BaseUrl);
    }

    public async Task<ListDailySchedule> GetAllRoles()
    {
        var request = new RestRequest("Role", Method.Get);
        var response = await _client.ExecuteAsync<ListDailySchedule>(request);
        return response.Data;
    }

    public async Task<ListDailySchedule> GetRole(long id)
    {
        var request = new RestRequest($"Role/{id}", Method.Get);
        var response = await _client.ExecuteAsync<ListDailySchedule>(request);
        return response.Data;
    }

    public async Task<APIResponse> InsertRole(DailyScheduleView DailyScheduleInfo)
    {
        var request = new RestRequest("Role", Method.Post);
        request.AddJsonBody(DailyScheduleInfo);
        var response = await _client.ExecuteAsync<APIResponse>(request);
        return response.Data;
    }

    public async Task<APIResponse> UpdateRole(DailyScheduleView DailyScheduleInfo)
    {
        var request = new RestRequest("Role", Method.Put);
        request.AddJsonBody(DailyScheduleInfo);
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
