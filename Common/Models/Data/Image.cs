using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class Image
{
    public int Id { get; set; }

    public string ImageUrl { get; set; } = null!;

    public bool? IsPublished { get; set; }

    public DateTime? UploadedAt { get; set; }
}
