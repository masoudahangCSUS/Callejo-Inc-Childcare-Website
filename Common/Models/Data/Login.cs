using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class Login
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid FkCallejoIncUser { get; set; }

    public DateTime LastLogin { get; set; }

    public Guid? AuthenticationToken { get; set; }

    public virtual CallejoIncUser FkCallejoIncUserNavigation { get; set; } = null!;
}
