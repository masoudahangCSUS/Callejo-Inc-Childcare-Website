using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Common.Models.Data;
using Common.View;
using BlazorApp.Client.Pages;

namespace BlazorApp.Client.Services
{
    public class NotificationService
    {
        private readonly HttpClient _httpClient;

        public NotificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Notification>> GetNotificationsByParentId(Guid parentId)
        {
            try
            {
                Console.WriteLine($"Fetching notifications for parentId: {parentId}");
                var notifications = await _httpClient.GetFromJsonAsync<List<Notification>>($"api/Notifications/{parentId}");
                return notifications ?? new List<Notification>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching notifications: {ex.Message}");
                return new List<Notification>();
            }
        }

        public async Task<bool> MarkAsRead(long id)
        {
            try
            {
                Console.WriteLine($"Calling API to mark notification {id} as read...");
                var response = await _httpClient.PutAsync($"api/Notifications/mark-as-read/{id}", null);
                Console.WriteLine($"API response status: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MarkAsRead: {ex.Message}");
                return false;
            }
        }
    }
}
