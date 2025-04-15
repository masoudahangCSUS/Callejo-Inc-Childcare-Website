using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.View
{
    public class Image
    {
        public int Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public bool IsPublished { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
