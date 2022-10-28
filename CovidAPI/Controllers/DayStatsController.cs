using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CovidAPI.Models;
using CovidAPI.DTOs;

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
        public async Task<ActionResult<IEnumerable<CasesDTO>>> GetRegionStatsFromTo([FromQuery] string region, [FromQuery] string from, [FromQuery] string to)
        {
            try {
                DateTime DateFrom = DateTime.ParseExact(from, "yyyy-MM-dd", null);
                DateTime DateTo = DateTime.ParseExact(to, "yyyy-MM-dd", null);
                region = region.ToUpper();

                var cases = await _context.DayStats
                    .Include(dayStats => dayStats.Regions)
                    .Where(d => d.Date >= DateFrom && d.Date <= DateTo)
                    .Select(ds => new DayStats {
                        Date = ds.Date,
                        Regions = ds.Regions.Where(r => r.Name == region).ToList()
                    })
                    .Select(dto => new CasesDTO {
                        Date = dto.Date,
                        RegionName = dto.Regions.First().Name,
                        ActiveCases = dto.Regions.First().Active,
                        Vaccinated1st = dto.Regions.First().Vaccinated1st,
                        Vaccinated2nd = dto.Regions.First().Vaccinated2nd,
                        Deceased = dto.Regions.First().ToDateDeceased
                    })
                    .ToListAsync();

                return cases;
            }
            catch (Exception ex) {
                return NotFound("Something went wrong.");
            }
        }

        [HttpGet("lastweek")]
        public async Task<ActionResult<IEnumerable<WeekAverageDTO>>> GetStatsLastweek() {
            try {
                DateTime DateNow = DateTime.Now;
                DateTime DateStart = DateNow.AddDays(-7);

                var last = _context.DayStats
                    .Include(dayStats => dayStats.Regions)
                    .Where(d => d.Date >= DateStart && d.Date <= DateNow)
                    .SelectMany(ds => ds.Regions).ToList()
                    .GroupBy(r => r.Name);

                List<WeekAverageDTO> averageWeek = new List<WeekAverageDTO>();

                foreach (var l in last) {
                    WeekAverageDTO weekAverage = new WeekAverageDTO();
                    weekAverage.RegionName = l.Key;
                    weekAverage.AverageNewCases = ComputeAverageNewCases(l.ToList());
                    averageWeek.Add(weekAverage);
                }

                return averageWeek;
            }
            catch (Exception ex) {
                return NotFound("Something went wrong.");
            }
        }

        private static double ComputeAverageNewCases(IEnumerable<Region> regions) {
            int sum = 0;
            int lastDay = -1;
            foreach (var region in regions) {
                if (lastDay == -1) {
                    lastDay = region.ToDateConfirmed;
                }
                else {
                    sum += (region.ToDateConfirmed - lastDay);
                    lastDay = region.ToDateConfirmed;
                }
            }

            return sum / 7;
        }
    }
}
