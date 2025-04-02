using Common.View;
using System.Threading.Tasks;

namespace Services.Expenses
{
    public interface IExpenseService
    {
        Task<ExpenseDTO> CreateExpenseAsync(ExpenseDTO expenseDto);
        Task<bool> DeleteExpenseAsync(int id);
        Task<bool> UpdateExpenseAsync(ExpenseDTO expenseDto);
        Task<int> GetChildrenCountAsync();
        Task<decimal> GetTotalExpensesAsync();

    }
}
