using System;
using System.Collections.Generic;

namespace Common.Models.Data;

public class Payment
{
    public Guid PaymentId { get; set; }
    public Guid InvoiceId { get; set; }
    public string PaymentMethod { get; set; } = "";
    public string? TransactionReference { get; set; }
    public decimal AmountPaid { get; set; }
    public string PaymentStatus { get; set; } = "Completed";
    public DateTime PaymentDate { get; set; }
    public Invoice Invoice { get; set; }
}
