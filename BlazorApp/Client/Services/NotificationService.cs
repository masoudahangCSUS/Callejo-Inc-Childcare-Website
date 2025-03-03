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

        public async Task<bool> SendCustomNotification(Notification newRequest)
        {
            try
            {
                Console.WriteLine($"Sending new notification from: ");//{parentId}
                /*var notificationPayload = new
                {
                    parentId = parentId,
                    rqTitle = rqTitle,
                    Message = message,
                    TargetId = "F7DE2748-4FB0-4A78-8EF7-014C4D716A9B" // Hardcoded owner GUID -- change later
                };*/

                var response = await _httpClient.PostAsJsonAsync($"api/notifications/send-custom-notif", newRequest);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Notification sent successfully from ");//{parentId}
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to send notification: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex )
            {
                Console.WriteLine($"Error creating notification: {ex.Message}");
                return false;
            }

        }

        public async Task<bool> CreateNotification(Notification newNotification)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/notifications/admin-create", newNotification);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating notification: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateNotification(long id, Notification updatedNotification)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/notifications/admin-update/{id}", updatedNotification);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating notification: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteNotification(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/notifications/admin-delete/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting notification: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Notification>> GetAllNotifications()
        {
            try
            {
                Console.WriteLine("Fetching all notifications for admin...");
                var notifications = await _httpClient.GetFromJsonAsync<List<Notification>>("api/notifications/get-all");
                return notifications ?? new List<Notification>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching notifications: {ex.Message}");
                return new List<Notification>();
            }
        }



    }
}
