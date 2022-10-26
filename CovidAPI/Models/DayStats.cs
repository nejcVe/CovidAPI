namespace CovidAPI.Models
{
    public class DayStats
    {
        public long Id { get; set; }
        public DateOnly Date { get; set; }
        public List<Region>? Regions { get; set; }

    }
}
