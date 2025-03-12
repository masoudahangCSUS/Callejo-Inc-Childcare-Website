using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class DailySchedule
{
    public long Id { get; set; }

    public short Day { get; set; }

    public short Month { get; set; }

    public short Year { get; set; }

    public string Description { get; set; } = null!;

    public string? DescSpecial { get; set; }
}
