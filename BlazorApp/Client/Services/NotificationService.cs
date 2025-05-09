﻿using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Common.View;

namespace BlazorApp.Client.Services
{
    public class NotificationService
    {
        private readonly HttpClient _httpClient;

        public NotificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NotificationView>> GetNotificationsByParentId(Guid parentId)
        {
            try
            {
                Console.WriteLine($"Fetching notifications for parentId: {parentId}");
                var notifications = await _httpClient.GetFromJsonAsync<List<NotificationView>>($"api/Notifications/{parentId}");
                return notifications ?? new List<NotificationView>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching notifications: {ex.Message}");
                return new List<NotificationView>();
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

        public async Task<bool> SendCustomNotification(NotificationView newRequest)
        {
            try
            {
                Console.WriteLine($"Sending new notification from: {newRequest.FkParentId}");
                var response = await _httpClient.PostAsJsonAsync("api/notifications/send-custom-notif", newRequest);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Notification sent successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to send notification: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating notification: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CreateNotification(NotificationView newNotification)
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

        public async Task<bool> UpdateNotification(long id, NotificationView updatedNotification)
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

        public async Task<List<NotificationView>> GetAllNotifications()
        {
            try
            {
                Console.WriteLine("Fetching all notifications for admin...");
                var notifications = await _httpClient.GetFromJsonAsync<List<NotificationView>>("api/notifications/get-all");
                return notifications ?? new List<NotificationView>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching notifications: {ex.Message}");
                return new List<NotificationView>();
            }
        }
    }
}
