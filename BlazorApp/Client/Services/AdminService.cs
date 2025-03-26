using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Common.View;
using Microsoft.AspNetCore.Mvc;

public class AdminService
{
    private readonly HttpClient _httpClient;

    public AdminService(HttpClient httpClient)
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

    public async Task<HttpResponseMessage> CreateUserAsync(AdminUserCreationDTO newUser)
    {
        var apiUrl = "https://localhost:7139/api/admin/create-user";
        return await _httpClient.PostAsJsonAsync(apiUrl, newUser);
    }

    public async Task<HttpResponseMessage> UpdateUserAsync(AdminUserUpdateDTO userInfo)
    {
        var apiUrl = "https://localhost:7139/api/admin/update-user";
        return await _httpClient.PutAsJsonAsync(apiUrl, userInfo);
    }

    public async Task<List<AdminUserCreationDTO>> GetAllUsersAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ListUsers>("https://localhost:7139/api/admin/get-all-users");
        Console.WriteLine(response);
        if (response != null)
        {
            return response.users;
        }
        else
        {
            return new List<AdminUserCreationDTO>();
        }
    }

    public async Task<HttpResponseMessage> DeleteUserAsync(Guid userId)
    {
        var response = await _httpClient.DeleteAsync($"https://localhost:7139/api/admin/delete-user?userId={userId}");
        return response;
    }

    public async Task<List<InvoiceDTO>> GetAllInvoicesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<InvoiceDTO>>("https://localhost:7139/api/invoices/all");
        return response ?? new List<InvoiceDTO>();
    }

    public async Task<HttpResponseMessage> SaveInvoiceAsync(InvoiceDTO invoice)
    {
        var apiUrl = "https://localhost:7139/api/invoices/save";
        return await _httpClient.PostAsJsonAsync(apiUrl, invoice);
    }

    public async Task<HttpResponseMessage> UpdateInvoiceAsync(InvoiceDTO invoice)
    {
        var apiUrl = "https://localhost:7139/api/invoices/update";
        return await _httpClient.PutAsJsonAsync(apiUrl, invoice);
    }
    public async Task<HttpResponseMessage> DeleteInvoiceAsync(Guid invoiceId)
    {
        return await _httpClient.DeleteAsync($"https://localhost:7139/api/invoices/delete/{invoiceId}");
    }


}
