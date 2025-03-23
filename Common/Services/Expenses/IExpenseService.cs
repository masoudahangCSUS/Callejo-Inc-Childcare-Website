using Common.View;
using System.Threading.Tasks;

namespace Common.Services.Expenses
{
    public interface IExpenseService
    {
        Task<ExpenseDTO> CreateExpenseAsync(ExpenseDTO expenseDto);
        Task<bool> DeleteExpenseAsync(int id);
        Task<bool> UpdateExpenseAsync(ExpenseDTO expenseDto);
        Task<int> GetChildrenCountAsync(); 
    }
}
