using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class PhoneNumber
{
    public long Id { get; set; }

    public Guid FkUsers { get; set; }

    public string AreaCode { get; set; } = null!;

    public string Prefix { get; set; } = null!;

    public string LastFour { get; set; } = null!;

    public long FkType { get; set; }

    public virtual PhoneNumbersType FkTypeNavigation { get; set; } = null!;

    public virtual CallejoIncUser FkUsersNavigation { get; set; } = null!;
}
