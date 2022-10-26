using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CovidAPI.Models;
using CovidAPI.Enums;

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

        [HttpGet("cases/{region}/{from}/{to}")]
        public async Task<ActionResult<IEnumerable<Region>>> GetRegionStatsFromTo(RegionName region, DateOnly from, DateOnly to)
        {
            return NotFound();
        }

        [HttpGet("cases/lastweek")]
        public async Task<ActionResult<DayStats>> GetDayStats(long id)
        {
            var dayStats = await _context.DayStats.FindAsync(id);

            if (dayStats == null)
            {
                return NotFound();
            }

            return dayStats;
        }

        private bool DayStatsExists(long id)
        {
            return _context.DayStats.Any(e => e.Id == id);
        }
    }
}
