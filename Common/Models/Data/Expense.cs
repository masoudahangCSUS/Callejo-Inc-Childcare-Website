using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class Expense
{
    public int Id { get; set; }

    public byte[] Receipt { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateOnly Date { get; set; }

    public string Category { get; set; } = null!;

    public string? Note { get; set; }
}
