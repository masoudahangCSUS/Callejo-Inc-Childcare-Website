using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;


namespace BlazorApp.Controllers
{
    // Implements Backend Logic on login...
    // Doesn't work with login page yet
    public class LoginController : Controller
    {
        // Hardcoded credentials for testing
        // DB will be connected to fill these values at later time
        private const string validUser = "admin";
        private const string validPass = "password";

        [HttpPost]
        public IActionResult HandleLogin(string userName, string passWord, string action)
        {
            if (action == "login")
            {
                if (userName == validUser && passWord == validPass)
                {
                    
                    return Content("Login was successful!");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Username or Password.";
                    return View();
                }
            }
            return View();
        }
    }
}