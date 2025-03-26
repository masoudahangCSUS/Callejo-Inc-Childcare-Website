using Common.Services.SQL;
using Common.Services.Invoice;
using Microsoft.AspNetCore.Mvc;
using Common.Models.Data;
using Common.View;


namespace CallejoIncChildcareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IConfiguration _configuration;
        private readonly CallejoSystemDbContext _context;

        public InvoicesController(IInvoiceService invoiceService, IConfiguration configuration, CallejoSystemDbContext context)
        {
            _invoiceService = invoiceService;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("all")]
        public ActionResult<List<InvoiceDTO>> GetAllInvoices()
        {
            var result = _invoiceService.GetAllInvoices();
            return Ok(result);
        }

        [HttpPost("save")]
        public ActionResult<APIResponse> SaveInvoice([FromBody] InvoiceDTO invoice)
        {
            var result = _invoiceService.InsertInvoice(invoice);
            return Ok(result);
        }

        [HttpPut("update")]
        public ActionResult<APIResponse> UpdateInvoice([FromBody] InvoiceDTO invoice)
        {
            var result = _invoiceService.UpdateInvoice(invoice);
            return Ok(result);
        }

        [HttpDelete("delete/{invoiceId}")]
        public ActionResult<APIResponse> DeleteInvoice(Guid invoiceId)
        {
            var result = _invoiceService.DeleteInvoice(invoiceId);
            return Ok(result);
        }

        [HttpGet("guardian/{guardianId}")]
        public IActionResult GetInvoicesByGuardianId(Guid guardianId)
        {
            var invoices = _invoiceService.GetInvoicesByGuardianId(guardianId);
            return Ok(invoices);
        }


    }
}
