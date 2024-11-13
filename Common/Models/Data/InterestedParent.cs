using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class InterestedParent
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string ReasonForInquiry { get; set; } = null!;
}
