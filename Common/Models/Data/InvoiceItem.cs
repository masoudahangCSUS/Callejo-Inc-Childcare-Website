using System;

namespace Common.Models.Data
{
    public class InvoiceItem
    {
        public Guid ItemId { get; set; }
        public Guid InvoiceId { get; set; }
        public long ChildId { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
