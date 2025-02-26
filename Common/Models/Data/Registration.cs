using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class Registration
{
    // Unique ID for reigstration tracking.
    public Guid Id { get; set; } = Guid.NewGuid();
    // Name of uploader.
    public string Name { get; set; } = null!;

    // Default Status message
    public string Status { get; set; } = "Pending";
    // Date and time of submission.
    public DateTime Datetime { get; set; }
}
