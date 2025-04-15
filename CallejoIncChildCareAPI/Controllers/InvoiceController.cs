using Common.Services.SQL;
using Common.Services.Invoice;
using Microsoft.AspNetCore.Mvc;
using Common.Models.Data;
using Common.View;
using Common.Services.Login;
using CallejoIncChildCareAPI.Authorize;


namespace CallejoIncChildcareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IConfiguration _configuration;
        private readonly CallejoSystemDbContext _context;
        private ILoginService _loginService;

        public InvoicesController(IInvoiceService invoiceService, IConfiguration configuration, CallejoSystemDbContext context, ILoginService loginService)
        {
            _invoiceService = invoiceService;
            _configuration = configuration;
            _context = context;
            _loginService = loginService;
        }

        [AuthorizeAttribute()]
        [HttpGet("all")]
        public ActionResult<List<InvoiceDTO>> GetAllInvoices()
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var result = _invoiceService.GetAllInvoices();
            return Ok(result);
        }

        [AuthorizeAttribute()]
        [HttpPost("save")]
        public ActionResult<APIResponse> SaveInvoice([FromBody] InvoiceDTO invoice)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var result = _invoiceService.InsertInvoice(invoice);
            return Ok(result);
        }

        [AuthorizeAttribute()]
        [HttpPut("update")]
        public ActionResult<APIResponse> UpdateInvoice([FromBody] InvoiceDTO invoice)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var result = _invoiceService.UpdateInvoice(invoice);
            return Ok(result);
        }

        [AuthorizeAttribute()]
        [HttpDelete("delete/{invoiceId}")]
        public ActionResult<APIResponse> DeleteInvoice(Guid invoiceId)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var result = _invoiceService.DeleteInvoice(invoiceId);
            return Ok(result);
        }

        [AuthorizeAttribute()]
        [HttpGet("guardian/{guardianId}")]
        public IActionResult GetInvoicesByGuardianId(Guid guardianId)
        {
            if (!_loginService.IsUserAuthenticated(AuthorizeAction.UserName, AuthorizeAction.AuthorizationToken))
            {
                return Unauthorized(new APIResponse { Success = false, Message = "User is not authenticated." });
            }
            var invoices = _invoiceService.GetInvoicesByGuardianId(guardianId);
            return Ok(invoices);
        }


    }
}
