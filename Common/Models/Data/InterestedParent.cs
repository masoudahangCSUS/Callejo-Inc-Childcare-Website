using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class InterestedParent
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string ReasonForInquiry { get; set; } = null!;
}
