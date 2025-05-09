﻿using Microsoft.AspNetCore.Mvc;
using Common.View;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Services.Expenses;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace CallejoIncChildcareAPI.Controllers
{
    [RequireHttps]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }


        [HttpGet("children/count")]
        public async Task<IActionResult> GetChildrenCount()
        {
            int count = await _expenseService.GetChildrenCountAsync();
            return Ok(count); // count is a simple int, easily serializable
        }

        [HttpGet("expenses/total")]
        public async Task<IActionResult> GetTotalExpenses()
        {
            var total = await _expenseService.GetTotalExpensesAsync();
            return Ok(total);
        }



        // POST: api/Expenses/Upload
        [HttpPost("Upload")]
        public async Task<ActionResult<ExpenseDTO>> UploadExpense(
            IFormFile file,
            [FromForm] string category,
            [FromForm] string date,
            [FromForm] decimal amount,
            [FromForm] string? note)
        {
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

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var deleted = await _expenseService.DeleteExpenseAsync(id);

            if (!deleted)
            {
                return NotFound($"Expense with ID {id} not found."); // HTTP 404 -- not found
            }

            return NoContent(); // HTTP 204 -- deletion successful
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditExpense(
            int id,
            IFormFile? file,
            [FromForm] string category,
            [FromForm] string date,
            [FromForm] decimal amount,
            [FromForm] string? note)
        {
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

            var success = await _expenseService.EditExpenseAsync(updatedExpense);

            if (!success)
            {
                return NotFound($"Expense with ID {id} not found"); // HTTP 404 -- not found
            }

            return NoContent(); // HTTP 204 -- edit successful
        }

        [HttpGet("Download Receipt")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            // Retrieve file data
            var fileData = await _expenseService.DownloadExpenseAsync(id);

            // If file is not present, return NotFound
            if (fileData == null)
            {
                return NotFound("No file found");
            }

            // Return data as PDF
            return File(fileData, "application/pdf", "receipt.pdf");
        }

        [HttpGet("All Expenses")]
        public async Task<ActionResult<List<ExpenseDTO>>> GetAllExpenses()
        {
            var expenses = await _expenseService.GetAllExpensesAsync();
            return Ok(expenses);
        }
    }
}
