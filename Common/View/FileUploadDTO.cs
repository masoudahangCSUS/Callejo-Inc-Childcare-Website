using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.View
{
    public class FileUploadDTO
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public string FileUrl { get; set; }  // Can be the path to the file in your storage
        public byte[] FileContent { get; set; }  // Store the actual content (bytes)
    }
}
