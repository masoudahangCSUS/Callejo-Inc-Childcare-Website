using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class Registration
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime Datetime { get; set; }

    public Guid UserId { get; set; }
}
