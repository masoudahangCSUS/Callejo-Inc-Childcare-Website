using Common.Services.SQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CallejoIncChildcareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerInfoController : ControllerBase
    {
        private ISQLServices _sqlServices;

        public CustomerInfoController(ISQLServices sqlServices)
        {
            _sqlServices = sqlServices;
        }

        [HttpGet]
        [Route("childrenguardian")]
        public IActionResult GetChildrentGuardian()
        {
            var result = _sqlServices.GetListOfAllChildrenAndGuardians();
            return Ok(result);
        }
    }
}
