using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Common.Models.Data;

namespace BlazorApp.Client.Services
{
    public class HolidaysVacationsService
    {
        private readonly HttpClient _httpClient;

        public HolidaysVacationsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<HolidaysVacations>> GetHolidaysVacationsAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<HolidaysVacations>>("api/HolidaysVacations");

            return result ?? new List<HolidaysVacations>(); // Avoid null reference errors
        }
    }
}
