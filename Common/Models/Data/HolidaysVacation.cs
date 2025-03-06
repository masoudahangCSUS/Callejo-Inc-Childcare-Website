using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class HolidaysVacation
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string Type { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
