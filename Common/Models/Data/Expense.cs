using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.Data;

public partial class Expense
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } // Primary key -- auto-incremented


    public byte[] Receipt { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateOnly Date { get; set; }

    public string Category { get; set; } = null!;

    public string? Note { get; set; }
}
