using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.View
{
    public class ExpenseDTO
    {
        public int Id { get; set; }
        public byte[] Receipt { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public string Category { get; set; } = null!;
        public string? Note { get; set; }
    }
}
