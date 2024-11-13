using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Common.View;

public class CustomerInfoService
{
    private readonly HttpClient _httpClient;

    public CustomerInfoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ChildrenGuardianView>> GetChildrenGuardiansAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ListChildrenGuardianView>("https://localhost:7139/api/CustomerInfo/childrenguardian");
        if (response != null && response.listChildrenGuardian != null)
        {
            return response.listChildrenGuardian;
        }
        else
        {
            return new List<ChildrenGuardianView>();
        }
    }
    public async Task<HttpResponseMessage> CreateUserAsync(UserView newUser)
    {
        var apiUrl = "https://localhost:7139/api/CustomerInfo/create-user";
        return await _httpClient.PostAsJsonAsync(apiUrl, newUser);
    }
}
