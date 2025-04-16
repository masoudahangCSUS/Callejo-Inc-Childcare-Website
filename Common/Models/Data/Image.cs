using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class Image
{
    public int Id { get; set; }

    public string FileName { get; set; }

    public bool IsPublished { get; set; } = false;

    public DateTime? UploadedAt { get; set; } = DateTime.UtcNow;
}
