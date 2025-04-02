using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.View
{
    public class FileUploadDTO
    {
        public int Id { get; set; }

        public string FileName { get; set; } = null!;

        public string ContentType { get; set; } = null!;

        public byte[] FileData { get; set; } = null!;

        public string DocumentType { get; set; } = null!;

        public DateTime? UploadDate { get; set; }
    }
}
