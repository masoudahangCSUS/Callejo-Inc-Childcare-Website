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

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[]? RegistrationDocument { get; set; }

    public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; } = new List<EmergencyContact>();

    public virtual Role FkRoleNavigation { get; set; } = null!;

    public virtual ICollection<Login> Logins { get; set; } = new List<Login>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();

    public virtual UserSecret? UserSecret { get; set; }

    public virtual ICollection<Child> FkChildren { get; set; } = new List<Child>();
}
