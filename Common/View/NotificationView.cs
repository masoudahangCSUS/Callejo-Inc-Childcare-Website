using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.View
{
    public class NotificationView
    {
        public long Id { get; set; }
        public Guid FkParentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime SentOn { get; set; }
        public bool IsRead { get; set; }
        public bool IsExpanded { get; set; }
    }
}
