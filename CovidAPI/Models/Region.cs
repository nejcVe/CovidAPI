using CovidAPI.Enums;

namespace CovidAPI.Models
{
    public class Region {

        public long Id { get; set; }
        public RegionName Name { get; set; }
        public int CasesActive { get; set; }
        public int CasesTodateConfirmed { get; set; }
        public int CasesTodateDeceased { get; set; }
        public int CasesTodateVaccinated1 { get; set; }
        public int CasesTodateVaccinated2 { get; set; }
        public int CasesTodateVaccinated3 { get; set; }

    }
}
