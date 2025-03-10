using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class EmergencyContact
{
    public Guid Id { get; set; }

    public Guid FkUsers { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Relationship { get; set; }

    public virtual CallejoIncUser FkUsersNavigation { get; set; } = null!;
}
