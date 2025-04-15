using Microsoft.AspNetCore.Mvc;
using Common.View;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Services.Expenses;
using Microsoft.Extensions.Configuration.UserSecrets;
using Common.Services.Login;
using CallejoIncChildCareAPI.Authorize;

namespace CallejoIncChildcareAPI.Controllers
{
    [RequireHttps]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private ILoginService _loginService;

        public ExpenseController(IExpenseService expenseService, ILoginService loginService)
        {
            _expenseService = expenseService;
            _loginService = loginService;
        }

        [AuthorizeAttribute()]
        [HttpGet("children/count")]
        public async Task<IActionResult> GetChildrenCount()
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            int count = await _expenseService.GetChildrenCountAsync();
            return Ok(count); // count is a simple int, easily serializable
        }

        [AuthorizeAttribute()]
        [HttpGet("expenses/total")]
        public async Task<IActionResult> GetTotalExpenses()
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var total = await _expenseService.GetTotalExpensesAsync();
            return Ok(total);
        }



        // POST: api/Expenses/Upload
        [AuthorizeAttribute()]
        [HttpPost("Upload")]
        public async Task<ActionResult<ExpenseDTO>> UploadExpense(
            IFormFile file,
            [FromForm] string category,
            [FromForm] string date,
            [FromForm] decimal amount,
            [FromForm] string? note)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            if (file == null || file.Length == 0)
            {
                return BadRequest("Receipt file is required.");
            }

            string fileType = file.ContentType;
            if (fileType != "application/pdf" || !file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Only PDF files are allowed");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();

            if (!DateOnly.TryParse(date, out DateOnly parsedDate))
            {
                return BadRequest("Invalid date format. Use MM/DD/YYYY");
            }

            if (amount <= 0)
            {
                return BadRequest("Amount must be greater than 0");
            }

            var expenseDto = new ExpenseDTO
            {
                Date = parsedDate,
                Category = category,
                Amount = amount,
                Note = note,
                Receipt = fileBytes
            };

            var createdExpense = await _expenseService.CreateExpenseAsync(expenseDto);

            return CreatedAtAction(nameof(UploadExpense), new {id =  createdExpense.Id}, createdExpense);
        }

        [AuthorizeAttribute()]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var deleted = await _expenseService.DeleteExpenseAsync(id);

            if (!deleted)
            {
                return NotFound($"Expense with ID {id} not found."); // HTTP 404 -- not found
            }

            return NoContent(); // HTTP 204 -- deletion successful
        }

        [AuthorizeAttribute()]
        [HttpPut("Edit")]
        public async Task<IActionResult> EditExpense(
            int id,
            IFormFile? file,
            [FromForm] string category,
            [FromForm] string date,
            [FromForm] decimal amount,
            [FromForm] string? note)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            if (!DateOnly.TryParse(date, out DateOnly parsedDate))
            {
                return BadRequest("Invalid date format. Use MM/DD/YYYY");
            }

            if (amount <= 0)
            {
                return BadRequest("Amount must be greater than 0");
            }

            byte[]? fileBytes = null;
            if (file != null && file.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            var updatedExpense = new ExpenseDTO
            {
                Id = id,
                Date = parsedDate,
                Category = category,
                Amount = amount,
                Note = note,
                Receipt = fileBytes
            };

            var success = await _expenseService.UpdateExpenseAsync(updatedExpense);

            if (!success)
            {
                return NotFound($"Expense with ID {id} not found"); // HTTP 404 -- not found
            }

            return NoContent(); // HTTP 204 -- edit successful
        }
    }
}
