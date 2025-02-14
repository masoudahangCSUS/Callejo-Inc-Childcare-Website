using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models.Data;

[Route("api/[controller]")]
[ApiController]
public class HolidaysVacationsController : ControllerBase
{
    private readonly CallejoSystemDbContext _context;

    public HolidaysVacationsController(CallejoSystemDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Produces("application/json")] // Ensure JSON response
    public async Task<ActionResult<IEnumerable<HolidaysVacations>>> GetHolidaysVacations()
    {
        var holidaysVacations = await _context.HolidaysVacations
            .OrderBy(h => h.StartDate) // Order by start date for better display
            .ToListAsync();

        return Ok(holidaysVacations);
    }
}
