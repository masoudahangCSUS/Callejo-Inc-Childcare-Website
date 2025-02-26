    using Common.Models.Data;
    using Microsoft.AspNetCore.Mvc;

    namespace BlazorApp.Client.Services
    {
        public class ProfileService
        {
            private readonly HttpClient _httpClient;

            public ProfileService(HttpClient httpClient)
            {
                _httpClient = httpClient;
            }

            // Method for retreiving phone numbers
            public async Task<PhoneNumber> GetPhoneNumberAsync(Guid? id, long type)
            {

                try
                {
                    var url = $"api/customer/get-phone-number?id={id}&type={type}";
                    var PhoneNumber = await _httpClient.GetFromJsonAsync<PhoneNumber>(url);
                    return PhoneNumber;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Exception in GetPhoneNumberAsync: {ex.Message}");
                    return null;
                }
            }
            
            // Method for getting user by ID
            public async Task<CallejoIncUser> GetUserByID(Guid? id)
            {
                try
                {
                    var url = $"api/customer/get-user-by-id?id={id}";
                    var user = await _httpClient.GetFromJsonAsync<CallejoIncUser>(url);
                    return user;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in GetUserView: {ex.Message}");
                    return null;
                }
            }

            // Method to get a users Emergency Contact
            public async Task<EmergencyContact> GetEmergencyContactAsync(Guid? id)
            {
                try
                {
                    var url = $"api/customer/get-emergency-contact?id={id}";
                    var EmergencyContact = await _httpClient.GetFromJsonAsync<EmergencyContact>(url);
                    return EmergencyContact;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in GetPhoneNumberAsync: {ex.Message}");
                    return null;
                }
            }
        
            // Method to get List of children from Guardians table
            public async Task<List<long>> getChildrenAsync(Guid? id)
            {
                try
                {
                    var url = $"api/customer/get-child-list?id={id}";
                    var Children = await _httpClient.GetFromJsonAsync<List<long>>(url);
                    return Children;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in GetPhoneNumberAsync: {ex.Message}");
                    return null;
                }
            }

            // Method to get child from Children table
            public async Task<Child> GetChildById(long id)
            {
                try
                {
                    var url = $"api/customer/get-children-by-id?id={id}";
                    var Child = await _httpClient.GetFromJsonAsync<Child>(url);
                    return Child;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in GetPhoneNumberAsync: {ex.Message}");
                    return null;
                }
            }
        }
    }
