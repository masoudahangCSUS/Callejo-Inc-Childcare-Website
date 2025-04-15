using Common.Models.Data;
using Common.View;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Common.Services.Expenses
{
    public class ExpenseService : IExpenseService
    {
        private readonly CallejoSystemDbContext _dbContext;

        public ExpenseService(CallejoSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ExpenseDTO> CreateExpenseAsync(ExpenseDTO expenseDto)
        {
            string[] validCategories = { "Groceries", "Supplies", "Other" };
            if (!Array.Exists(validCategories, c => c.Equals(expenseDto.Category, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Invalid category. Select Groceries, Supplies, or Other.");
            }

            if (expenseDto.Amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than 0");
            }

            var expense = new Expense
            {
                Date = expenseDto.Date,
                Category = expenseDto.Category,
                Amount = expenseDto.Amount,
                Note = expenseDto.Note,
                Receipt = expenseDto.Receipt
            };

            _dbContext.Expenses.Add(expense);
            await _dbContext.SaveChangesAsync();

            return new ExpenseDTO
            {
                Id = expense.Id,
                Date = expenseDto.Date,
                Category = expenseDto.Category,
                Amount = expenseDto.Amount,
                Note = expenseDto.Note,
                Receipt = expenseDto.Receipt
            };
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var expense = await _dbContext.Expenses.FindAsync(id);

            if (expense == null)
            {
                return false; // Expense not found
            }

            _dbContext.Expenses.Remove(expense);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditExpenseAsync(ExpenseDTO expenseDto)
        {
            var expense = await _dbContext.Expenses.FindAsync(expenseDto.Id);

            if (expense == null)
            {
                return false; // Expense not found
            }

            string[] validCategories = { "Groceries", "Supplies", "Other" };
            if (!Array.Exists(validCategories, c => c.Equals(expenseDto.Category, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Invalid category. Select Groceries, Supplies, or Other.");
            }

            if (expenseDto.Amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than 0");
            }

            expense.Date = expenseDto.Date;
            expense.Category = expenseDto.Category;
            expense.Amount = expenseDto.Amount;
            expense.Note = expenseDto.Note;

            if (expenseDto.Receipt != null)
            {
                expense.Receipt = expenseDto.Receipt;
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<byte[]?> DownloadExpenseAsync(int id)
        {
            var expense = await _dbContext.Expenses.FindAsync(id);
            return expense?.Receipt;
        }

        public async Task<int> GetChildrenCountAsync()
        {
            return await _dbContext.Children.CountAsync();
        }

        public async Task<decimal> GetTotalExpensesAsync()
        {
            return await _dbContext.Expenses.SumAsync(e => e.Amount);
        }

        public async Task<List<ExpenseDTO>> GetAllExpensesAsync()
        {
            return await _dbContext.Expenses
                .OrderByDescending(e => e.Date) // Sort newest to oldest
                .Select(e => new ExpenseDTO
                {
                    Id = e.Id,
                    Date = e.Date,
                    Category = e.Category,
                    Amount = e.Amount,
                    Note = e.Note,
                    Receipt = e.Receipt
                })
                .ToListAsync();
        }
    }
}