using Microsoft.EntityFrameworkCore;

namespace CovidAPI.Models {
    public class CovidContext : DbContext {
        public CovidContext(DbContextOptions<CovidContext> options) : base(options) { }
        public DbSet<DayStats> DayStats { get; set; }
    }
}
