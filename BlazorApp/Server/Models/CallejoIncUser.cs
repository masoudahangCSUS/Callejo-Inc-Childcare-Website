using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp.Server.Models;

[Table("Callejo_Inc_Users", Schema = "dbo")]
public class CallejoIncUser
{
    public Guid Id { get; set; }

    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("middle_name")]
    public string? MiddleName { get; set; }

    [Column("last_name")]
    public string LastName { get; set; }

    [Column("address")]
    public string Address { get; set; }

    [Column("city")]
    public string City { get; set; }

    [Column("state")]
    public string State { get; set; }

    [Column("zip_code")]
    public string ZipCode { get; set; }

    [Column("fk_role")]
    public long FkRole { get; set; }

    // Navigation Properties
    public Role? Role { get; set; }
    public ICollection<Guardian>? Guardians { get; set; } = new List<Guardian>();
    public ICollection<PhoneNumber>? PhoneNumbers { get; set; } = new List<PhoneNumber>();
}
