using Common.Models.Data;
using Common.View;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Services.Expenses
{
    public class ExpenseService : IExpenseService
    {
        // SBC
        // private readonly CallejoSystemDbContext _dbContext;

        // sBC public ExpenseService(CallejoSystemDbContext dbContext)
        public ExpenseService()
        {
            // SBC _dbContext = dbContext;
        }

        public async Task<ExpenseDTO> CreateExpenseAsync(ExpenseDTO expenseDto)
        {
            return null;
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
            return false;
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
            return false;

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
            return 0;
            // SBC return await _dbContext.Children.CountAsync();
        }

        public async Task<decimal> GetTotalExpensesAsync()
        {
            return 0;
            // SBC return await _dbContext.Expenses.SumAsync(e => e.Amount);
        }




    }
}