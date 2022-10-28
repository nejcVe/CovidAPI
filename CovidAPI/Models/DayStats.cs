using Microsoft.EntityFrameworkCore;

namespace CovidAPI.Models
{
    public class DayStats
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public List<Region>? Regions { get; set; }
    }
}
