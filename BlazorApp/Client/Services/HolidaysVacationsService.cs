using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        // Get all holidays and vacations
        //EC public async Task<List<HolidaysVacations>> GetHolidaysVacationsAsync()
        //{
        //    var result = await _httpClient.GetFromJsonAsync<List<HolidaysVacations>>("api/HolidaysVacations");
        //    return result ?? new List<HolidaysVacations>(); // Avoid null reference errors
        //}

        // Create a new holiday/vacation
        //EC public async Task<bool> CreateHolidayVacationAsync(HolidaysVacations holidayVacation)
        //{
        //    var response = await _httpClient.PostAsJsonAsync("api/HolidaysVacations/admin-create", holidayVacation);
        //    return response.IsSuccessStatusCode;
        //}

        // Update an existing holiday/vacation
        //EC public async Task<bool> UpdateHolidayVacationAsync(long id, HolidaysVacations holidayVacation)
        //{
        //    var response = await _httpClient.PutAsJsonAsync($"api/HolidaysVacations/admin-update/{id}", holidayVacation);
        //    return response.IsSuccessStatusCode;
        //}

        // Delete a holiday/vacation
        public async Task<bool> DeleteHolidayVacationAsync(long id)
        {
            var response = await _httpClient.DeleteAsync($"api/HolidaysVacations/admin-delete/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
