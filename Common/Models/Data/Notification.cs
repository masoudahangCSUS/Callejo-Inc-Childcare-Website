using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class Notification
{
    public long Id { get; set; }

    public Guid FkParentId { get; set; }

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime SentOn { get; set; }

    public bool IsRead { get; set; }

    public bool IsExpanded { get; set; }

    public virtual CallejoIncUser FkParent { get; set; } = null!;
}
