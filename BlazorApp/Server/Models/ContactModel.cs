using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace BlazorApp.Client.Pages;
[BindProperties]
public class ContactModel : PageModel
{
    public string? Name { get; set; }


    public string? Phone { get; set; }


    public string? Email { get; set; }
    public string? Reason { get; set; }

    public void OnPost()
    {
        // Process the data
        // Save to database or perform an operation
        ViewData["Message"] = $"Name: {Name}, Phone Number: {Phone}, Email: {Email},\n Reason: {Reason}";
    }
}
