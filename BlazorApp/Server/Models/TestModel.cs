using Microsoft.AspNetCore.Mvc;

// Basic Model template for login testing
namespace BlazorApp.Server.Models
{
    public class TestModel
    {
        [BindProperty]
        public string? userName { get; set; }
        [BindProperty]
        public string? passWord { get; set; }

    }
}
