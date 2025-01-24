using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Common.Models.Data
{
    public class Notification
    {
        public long Id { get; set; } // Maps to `id`

        public Guid FkParentId { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }


        public DateTime SentOn { get; set; }

        public bool IsRead { get; set; }
    }


}
