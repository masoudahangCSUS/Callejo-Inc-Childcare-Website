using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class DailySchedule
{
    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public string? DescSpecial { get; set; }

    public DateOnly? CreatedAt { get; set; }
}
