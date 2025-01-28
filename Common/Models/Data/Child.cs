using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class Child
{
    public long Id { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public short Age { get; set; }

    public virtual ICollection<CallejoIncUser> FkParents { get; set; } = new List<CallejoIncUser>();
}
