using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CovidAPI.Models;

namespace CovidAPI.Controllers
{
    [Route("api/region")]
    [ApiController]
    public class DayStatsController : ControllerBase
    {
        private readonly CovidContext _context;

        public DayStatsController(CovidContext context)
        {
            _context = context;
        }

        [HttpGet("cases")]
        public async Task<ActionResult<IEnumerable<DayStats>>> GetRegionStatsFromTo([FromQuery] string region, [FromQuery] DateOnly from, [FromQuery] DateOnly to)
        {
            try {
                DateTime DateFrom = from.ToDateTime(TimeOnly.Parse("00:00"));
                DateTime DateTo = to.ToDateTime(TimeOnly.Parse("00:00"));

            }
            catch (Exception ex) { 
                
            }
            return NotFound();
        }

        [HttpGet("lastweek")]
        public async Task<ActionResult<DayStats>> GetDayStats()
        {
            return NotFound();
        }
        [HttpGet("all")]
        public async Task<ActionResult<List<DayStats>>> GetStats() {
            var stats = await _context.DayStats.Include(dayStats => dayStats.Regions).ToListAsync();

            if (stats == null) {
                return NotFound();
            }

            return stats;
        }

        private bool DayStatsExists(long id)
        {
            return _context.DayStats.Any(e => e.Id == id);
        }
    }
}
