public class InvoiceDTO
{
    public Guid InvoiceId { get; set; }
    public Guid GuardianId { get; set; }
    public string GuardianName { get; set; }
    public string ChildNames { get; set; }

    public DateOnly? DueDate { get; set; }
    public string Status { get; set; }
    public string? Notes { get; set; }

    public decimal? TotalAmount { get; set; }
    public decimal? AmountPaid { get; set; }

    public string? PaymentMethod { get; set; }
    public string? TransactionReference { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? LastPaymentDate { get; set; }
}
