using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class CallejoIncUser
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public long FkRole { get; set; }

    public string? Email { get; set; }

    public virtual Role FkRoleNavigation { get; set; } = null!;

    public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();

    public virtual ICollection<Child> FkChildren { get; set; } = new List<Child>();
}
