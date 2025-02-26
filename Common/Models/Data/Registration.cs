using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class Registration
{
    // Unique ID for regstration tracking.
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserID { get; set; }
    // Name of uploader.
    public string Name { get; set; } = null!;

    // Default Status message
    public string Status { get; set; } = "Pending";
    // Date and time of submission.
    public DateTime Datetime { get; set; }
}
