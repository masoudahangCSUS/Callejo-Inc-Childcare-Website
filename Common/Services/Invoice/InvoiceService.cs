using Azure;
using Common.Models.Data;
using Common.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Invoice
{
    public class InvoiceService : IInvoiceService
    {
        private readonly CallejoSystemDbContext _context;

        public InvoiceService(CallejoSystemDbContext context)
        {
            _context = context;
        }

        public List<InvoiceDTO> GetAllInvoices()
        {
            return _context.Invoices
                .Select(i => new InvoiceDTO
                {
                    InvoiceId = i.InvoiceId,
                    GuardianId = i.GuardianId,
                    GuardianName = i.GuardianName,
                    ChildNames = i.ChildNames,
                    DueDate = i.DueDate,
                    Status = i.Status,
                    Notes = i.Notes,
                    TotalAmount = i.TotalAmount,
                    AmountPaid = i.AmountPaid,
                    PaymentMethod = i.PaymentMethod,
                    TransactionReference = i.TransactionReference,
                    CreatedAt = i.CreatedAt,
                    LastPaymentDate = i.LastPaymentDate
                })
                .ToList();
        }

        public APIResponse InsertInvoice(InvoiceDTO dto)
        {
            var invoice = new Common.Models.Data.Invoice
            {
                InvoiceId = Guid.NewGuid(),
                GuardianId = dto.GuardianId,
                GuardianName = dto.GuardianName,
                ChildNames = dto.ChildNames,
                DueDate = dto.DueDate,
                Status = dto.Status ?? "Pending",
                Notes = dto.Notes,
                TotalAmount = dto.TotalAmount,
                AmountPaid = dto.AmountPaid,
                PaymentMethod = dto.PaymentMethod,
                TransactionReference = dto.TransactionReference,
                CreatedAt = DateTime.UtcNow,
                LastPaymentDate = dto.LastPaymentDate
            };

            _context.Invoices.Add(invoice);
            _context.SaveChanges();

            return new APIResponse { Success = true, Message = "Invoice saved successfully." };
        }

        public APIResponse UpdateInvoice(InvoiceDTO dto)
        {
            var existing = _context.Invoices.FirstOrDefault(i => i.InvoiceId == dto.InvoiceId);
            if (existing == null)
            {
                return new APIResponse { Success = false, Message = "Invoice not found." };
            }

            existing.GuardianId = dto.GuardianId;
            existing.GuardianName = dto.GuardianName;
            existing.ChildNames = dto.ChildNames;
            existing.DueDate = dto.DueDate;
            existing.Status = dto.Status;
            existing.Notes = dto.Notes;
            existing.TotalAmount = dto.TotalAmount;
            existing.AmountPaid = dto.AmountPaid;
            existing.PaymentMethod = dto.PaymentMethod;
            existing.TransactionReference = dto.TransactionReference;
            existing.LastPaymentDate = dto.LastPaymentDate;

            _context.SaveChanges();

            return new APIResponse { Success = true, Message = "Invoice updated successfully." };
        }
        public APIResponse DeleteInvoice(Guid invoiceId)
        {
            var invoice = _context.Invoices.FirstOrDefault(i => i.InvoiceId == invoiceId);
            if (invoice == null)
            {
                return new APIResponse { Success = false, Message = "Invoice not found." };
            }

            _context.Invoices.Remove(invoice);
            _context.SaveChanges();

            return new APIResponse { Success = true, Message = "Invoice deleted successfully." };
        }

        public List<InvoiceDTO> GetInvoicesByGuardianId(Guid guardianId)
        {
            return _context.Invoices
                .Where(i => i.GuardianId == guardianId)
                .Select(i => new InvoiceDTO
                {
                    InvoiceId = i.InvoiceId,
                    GuardianId = i.GuardianId,
                    GuardianName = i.GuardianName,
                    ChildNames = i.ChildNames,
                    DueDate = i.DueDate,
                    Status = i.Status,
                    Notes = i.Notes,
                    TotalAmount = i.TotalAmount,
                    AmountPaid = i.AmountPaid,
                    PaymentMethod = i.PaymentMethod,
                    TransactionReference = i.TransactionReference,
                    CreatedAt = i.CreatedAt,
                    LastPaymentDate = i.LastPaymentDate
                })
                .ToList();
        }


    }
}
