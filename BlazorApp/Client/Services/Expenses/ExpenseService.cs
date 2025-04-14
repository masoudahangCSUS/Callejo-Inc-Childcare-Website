using BlazorApp;
using Common.Models.Data;
using Common.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Services.Expenses
{
    public class ExpenseService : IExpenseService
    {
        private AppSettings _appSettings;
        private readonly RestClient _client;
        // SBC
        // private readonly CallejoSystemDbContext _dbContext;

        // sBC public ExpenseService(CallejoSystemDbContext dbContext)
        public ExpenseService(IOptions<AppSettings> apiSettings)
        {
            // SBC _dbContext = dbContext;
            _appSettings = apiSettings.Value;
            _client = new RestClient(apiSettings.Value.BaseUrl + "/Expense");
        }

        public async Task<ExpenseDTO> CreateExpenseAsync(ExpenseDTO expenseDto)
        {
            var request = new RestRequest("Upload", Method.Post);

            // Add file
            request.AddFile("file", expenseDto.Receipt, "receipt.pdf", "application/pdf");

            // Add other form data
            request.AddParameter("category", expenseDto.Category);
            request.AddParameter("date", expenseDto.Date.ToString("MM/dd/yyyy"));
            request.AddParameter("amount", expenseDto.Amount);
            request.AddParameter("note", expenseDto.Note);

            // Execute the request
            var response = await _client.ExecuteAsync<ExpenseDTO>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }            
            //SBC string[] validCategories = { "Groceries", "Supplies", "Other" };
            //if (!Array.Exists(validCategories, c => c.Equals(expenseDto.Category, StringComparison.OrdinalIgnoreCase)))
            //{
            //    throw new ArgumentException("Invalid category. Select Groceries, Supplies, or Other.");
            //}

            //if (expenseDto.Amount <= 0)
            //{
            //    throw new ArgumentException("Amount must be greater than 0");
            //}

            //var expense = new Expense
            //{
            //    Date = expenseDto.Date,
            //    Category = expenseDto.Category,
            //    Amount = expenseDto.Amount,
            //    Note = expenseDto.Note,
            //    Receipt = expenseDto.Receipt
            //};

            //_dbContext.Expenses.Add(expense);
            //await _dbContext.SaveChangesAsync();

            //return new ExpenseDTO
            //{
            //    Id = expense.Id,
            //    Date = expenseDto.Date,
            //    Category = expenseDto.Category,
            //    Amount = expenseDto.Amount,
            //    Note = expenseDto.Note,
            //    Receipt = expenseDto.Receipt
            //};
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var request = new RestRequest($"Delete?id={id}", Method.Delete);

            // Execute the request
            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Expense with ID {id} not found.");
                return false;
            }
            else
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }
            //var SBC expense = await _dbContext.Expenses.FindAsync(id);

            //if (expense == null)
            //{
            //    return false; // Expense not found
            //}

            //_dbContext.Expenses.Remove(expense);
            //await _dbContext.SaveChangesAsync();
            //return true;
        }

        public async Task<bool> UpdateExpenseAsync(ExpenseDTO expenseDto)
        {
            var request = new RestRequest($"Edit?id={expenseDto.Id}", Method.Put);

            // Add file if present
            if (expenseDto.Receipt != null)
            {
                request.AddFile("file", expenseDto.Receipt, "receipt.pdf", "application/pdf");
            }

            // Add other form data
            request.AddParameter("category", expenseDto.Category);
            request.AddParameter("date", expenseDto.Date.ToString("MM/dd/yyyy"));
            request.AddParameter("amount", expenseDto.Amount);
            request.AddParameter("note", expenseDto.Note);

            // Execute the request
            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Expense with ID {expenseDto.Id} not found.");
                return false;
            }
            else
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }
            //SBC var expense = await _dbContext.Expenses.FindAsync(expenseDto.Id);

            //if (expense == null)
            //{
            //    return false; // Expense not found
            //}

            //string[] validCategories = { "Groceries", "Supplies", "Other" };
            //if (!Array.Exists(validCategories, c => c.Equals(expenseDto.Category, StringComparison.OrdinalIgnoreCase)))
            //{
            //    throw new ArgumentException("Invalid category. Select Groceries, Supplies, or Other.");
            //}

            //if (expenseDto.Amount <= 0)
            //{
            //    throw new ArgumentException("Amount must be greater than 0");
            //}

            //expense.Date = expenseDto.Date;
            //expense.Category = expenseDto.Category;
            //expense.Amount = expenseDto.Amount;
            //expense.Note = expenseDto.Note;

            //if (expenseDto.Receipt != null)
            //{
            //    expense.Receipt = expenseDto.Receipt;
            //}

            //await _dbContext.SaveChangesAsync();
            //return true;
        }

        public async Task<int> GetChildrenCountAsync()
        {
            var request = new RestRequest("children/count", Method.Get);

            // Execute the request
            var response = await _client.ExecuteAsync<int>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }
            // SBC return await _dbContext.Children.CountAsync();
        }

        public async Task<decimal> GetTotalExpensesAsync()
        {
            var request = new RestRequest("expenses/total", Method.Get);

            // Execute the request
            var response = await _client.ExecuteAsync<decimal>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error: {response.ErrorMessage}");
            }            
            // SBC return await _dbContext.Expenses.SumAsync(e => e.Amount);
        }




    }
}