using Common.Models.Data;
using Common.View;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using System.Text.RegularExpressions;


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

            public async Task<bool> UpdateUserProfileAsync(Guid? userId, CustomerUserViewDTO userDto)
            {
                try
                {
                    // Route: /api/customer/update-user/{userId}
                    var apiUrl = $"api/Customer/update-user/{userId}";
                    var response = await _httpClient.PutAsJsonAsync(apiUrl, userDto);
                    return response.IsSuccessStatusCode;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in UpdateUserProfileAsync: {ex.Message}");
                    return false;
                }
            
            }

            public async Task<bool> UpdateEmergencyContactAsync(Guid? userId, EmergencyContactDTO emergencyDto)
            {   
                // Route: /api/customer/update-emergency/{userId}
                var apiUrl = $"api/customer/update-emergency/{userId}";
                var response = await _httpClient.PutAsJsonAsync(apiUrl, emergencyDto);
                return response.IsSuccessStatusCode;
            }

            public async Task<bool> updateChild(long childID, ChildDTO childDTO)
            {
                var apiUrl = $"api/customer/update-child/{childID}";
                var response = await _httpClient.PutAsJsonAsync(apiUrl, childDTO);
                return response.IsSuccessStatusCode;
            
            }


            // Simple method to conver a PhoneNumber to a string
            public String numberToString(PhoneNumberDTO number)
            {
                if (number == null)
                {
                    return "No phone number available.";
                }
                return $"({number.AreaCode}) {number.Prefix} {number.LastFour}";
            }

            // Simple method to decode a string into the components of a Phone Number
            public (string AreaCode, string Prefix, string LastFour) PhoneNumberDecode(string number)
            {
                string digits = Regex.Replace(number, @"\D", "");    // Remove all non-number characters
                Console.WriteLine($"[Debug] Input number: '{number}', digits: '{digits}', Length: {digits.Length}");

                string areaCode = digits.Substring(0, 3);
                string prefix = digits.Substring(3, 3);
                string lastFour = digits.Substring(6, 4);

                return (areaCode, prefix, lastFour);    
            }
        }
    }
