using Microsoft.Extensions.Options;
using RestSharp;
using System.Threading.Tasks;
using Common.View;
using BlazorApp;
using Org.BouncyCastle.Asn1.Crmf;
using CallejoIncChildCareAPI;
using Common.AES;

namespace BlazorApp.Client.Services
{
    public class DailyScheduleService
    {
        private readonly RestClient _client;
        private AppSettings _appSettings;

        public DailyScheduleService(IOptions<AppSettings> apiSettings)
        {
            _client = new RestClient(apiSettings.Value.BaseUrl);
        }
        //public async Task<ListDailySchedule> GetDailySchedule(long id)
        //{
        //    var request = new RestRequest($"DailySchedule/{id}", Method.Get);
        //    var response = await _client.ExecuteAsync<ListDailySchedule>(request);
        //    return response.Data;
        //}

        private void SetHeaders(RestRequest request, string userName, string authToken)
        {
            string appId = ServiceHelper.BuildAppIdHeader(_appSettings);
            request.AddHeader("AppId", appId);
            request.AddHeader("AuthorizationToken", AesOperation.EncryptString(_appSettings.Key.ToString(), authToken));
            request.AddHeader("UserName", AesOperation.EncryptString(_appSettings.Key.ToString(), userName));
        }


        public async Task<APIResponse> InsertDailySchedule(DailyScheduleView dailyScheduleView)
        {
            //https://localhost:7139/api/DailySchedule
            //https://localhost:7139/api
            //https://localhost:7139/api/DailySchedule

            var request = new RestRequest("DailySchedule", Method.Post);
            request.AddJsonBody(dailyScheduleView);
            var response = await _client.ExecuteAsync<APIResponse>(request);
            return response.Data;
        }

        public async Task<ListDailySchedule> GetDailyScheduleByDate(DateTime date)
        {
            var request = new RestRequest($"DailySchedule/{date.ToString("yyyy-MM-dd")}", Method.Get);
            var response = await _client.ExecuteAsync<ListDailySchedule>(request);
            return response.Data;
        }

        public async Task<APIResponse> UpdateDailySchedule(DailyScheduleView DailyScheduleInfo)
        {
            var request = new RestRequest("DailySchedule", Method.Put);
            request.AddJsonBody(DailyScheduleInfo);
            var response = await _client.ExecuteAsync<APIResponse>(request);
            return response.Data;
        }

        public async Task<APIResponse> DeleteDailySchedule(long id)
        {
            var request = new RestRequest($"DailySchedule/{id}", Method.Delete);
            var response = await _client.ExecuteAsync<APIResponse>(request);
            return response.Data;
        }

        /*public async Task<ListDailySchedule> GetAllRoles()
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
        }*/
    }
}
