using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class DailySchedule
{
    public long Id { get; set; }

    public short Day { get; set; }

    public string Month { get; set; } = null!;

    public string Year { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? DescSpecial { get; set; }
}
