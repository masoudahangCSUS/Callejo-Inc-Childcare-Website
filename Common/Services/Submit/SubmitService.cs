﻿using Common.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Common.Services.Submit
{
    public class SubmitService : ISubmitService
    {
        private readonly HttpClient _http;

        public SubmitService(HttpClient http)
        {
            _http = http;
        }

        // Submit an inquiry via API
        public async Task AddInquiryAsync(InterestedParent inquiry)
        {
            await _http.PostAsJsonAsync("https://localhost:7139/api/Submit/submit", inquiry);
        }

        // Fetch inquiries from the API
        public async Task<List<InterestedParent>> GetInquiryAsync()
        {
            return await _http.GetFromJsonAsync<List<InterestedParent>>("https://localhost:7139/api/Submit/data");
        }

        // Delete an inquiry via API
        public async Task<bool> DeleteInquiryAsync(Guid id)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/Submit/delete/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to delete inquiry: {error}");
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in DeleteInquiryAsync: {ex.Message}");
                return false;
            }
        }
    }
}
