using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class UserSecret
{
    public Guid FkUser { get; set; }

    public string Secret { get; set; } = null!;

    public virtual CallejoIncUser FkUserNavigation { get; set; } = null!;
}
