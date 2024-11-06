using Microsoft.AspNetCore.Mvc;

// Basic Model template for login testing
namespace BlazorApp.Models
{
    public class TestModel
    {
        [BindProperty]
        public string? userName { get; set; }
        [BindProperty]
        public string? passWord { get; set; }

    }
}
