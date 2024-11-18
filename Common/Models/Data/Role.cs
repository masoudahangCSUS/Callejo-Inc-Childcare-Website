using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class Role
{
    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<CallejoIncUser> CallejoIncUsers { get; set; } = new List<CallejoIncUser>();
}
