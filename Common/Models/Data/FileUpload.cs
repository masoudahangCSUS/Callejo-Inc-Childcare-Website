using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public partial class FileUpload
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public byte[] FileData { get; set; } = null!;

    public string DocumentType { get; set; } = null!;

    public DateTime? UploadDate { get; set; }
}
