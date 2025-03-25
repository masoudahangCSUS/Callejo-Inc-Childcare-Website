using Common.Models.Data;
using Common.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Invoice
{
    public interface IInvoiceService
    {
        List<InvoiceDTO> GetAllInvoices();
        APIResponse InsertInvoice(InvoiceDTO invoice);
        APIResponse UpdateInvoice(InvoiceDTO invoice);
        APIResponse DeleteInvoice(Guid invoiceId);
    }

}
