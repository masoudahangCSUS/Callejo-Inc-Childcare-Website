using Common.View;
using System.Threading.Tasks;

namespace Common.Services.Expenses
{
    public interface IExpenseService
    {
        Task<ExpenseDTO> CreateExpenseAsync(ExpenseDTO expenseDto);
        Task<bool> DeleteExpenseAsync(int id);
        Task<bool> EditExpenseAsync(ExpenseDTO expenseDto);
        Task<byte[]?> DownloadExpenseAsync(int id);
        Task<int> GetChildrenCountAsync();
        Task<decimal> GetTotalExpensesAsync();
        Task<List<ExpenseDTO>> GetAllExpensesAsync();
    }
}
