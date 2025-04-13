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
        public async Task<PhoneNumberDTO> GetPhoneNumberAsync(Guid? id, long type)
        {
            try
            {
                var url = $"api/customer/get-phone-number?id={id}&type={type}";
                var phone = await _httpClient.GetFromJsonAsync<PhoneNumberDTO>(url);
                return phone;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetPhoneNumberAsync: {ex.Message}");
                return null;
            }
        }

        /// Retrieves a user DTO by ID.
        public async Task<CustomerUserViewDTO> GetUserByID(Guid? id)
        {
            try
            {
                var url = $"api/customer/get-user-by-id?id={id}";
                var userDTO = await _httpClient.GetFromJsonAsync<CustomerUserViewDTO>(url);
                return userDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetUserByID: {ex.Message}");
                return null;
            }
        }

        // Method to get a users Emergency Contact
        public async Task<EmergencyContactDTO> GetEmergencyContactAsync(Guid? id)
        {
            try
            {
                var url = $"api/customer/get-emergency-contact?id={id}";
                var emergencyDTO = await _httpClient.GetFromJsonAsync<EmergencyContactDTO>(url);
                return emergencyDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetEmergencyContactAsync: {ex.Message}");
                return null;
            }
        }

        // Method to get List of children from Guardians table
        public async Task<List<ChildDTO>> getChildrenAsync(Guid? id)
        {
            try
            {
                var url = $"api/customer/get-child-list?id={id}";
                var children = await _httpClient.GetFromJsonAsync<List<ChildDTO>>(url);
                return children ?? new List<ChildDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in getChildrenAsync: {ex.Message}");
                return new List<ChildDTO>();
            }
        }

        // Method to get child from Children table
        public async Task<ChildDTO> GetChildById(long id)
        {
            try
            {
                var url = $"api/customer/get-children-by-id?id={id}";
                var childDTO = await _httpClient.GetFromJsonAsync<ChildDTO>(url);
                return childDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetChildById: {ex.Message}");
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

        public async Task<bool> UpdatePassword(SettingsDTO settings)
        {
            var apiUrl = $"api/customer/update-password";
            var response = await _httpClient.PutAsJsonAsync(apiUrl, settings);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEmail(SettingsDTO settings)
        {
            var apiUrl = $"api/customer/update-email";
            var response = await _httpClient.PutAsJsonAsync(apiUrl, settings);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Validate(ValidationDTO request)
        {
            var apiUrl = $"api/MA/Validate";
            var response = await _httpClient.PostAsJsonAsync(apiUrl, request);
            return response.IsSuccessStatusCode;
        }




        // Simple method to convert a PhoneNumber to a string
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

        public async Task<List<InvoiceDTO>> GetInvoicesByGuardianId(Guid guardianId)
        {
            var response = await _httpClient.GetAsync($"api/invoices/guardian/{guardianId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<InvoiceDTO>>() ?? new();
            }
            return new List<InvoiceDTO>();

        }

    }
}
