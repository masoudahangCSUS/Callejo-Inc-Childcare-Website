using System;
using System.Collections.Generic;

namespace Common.Models.Data
{
    public class Invoice
    {
        public Guid InvoiceId { get; set; }
        public Guid GuardianId { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; }
        public string? Notes { get; set; }
        public virtual ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
