using CovidAPI.Models;

namespace CovidAPI.DTOs {
    public class CasesDTO {
        public DateTime Date { get; set; }
        public string? RegionName { get; set; }
        public int ActiveCases { get; set; }
        public int Deceased { get; set; }
        public int Vaccinated1st { get; set; }
        public int Vaccinated2nd { get; set; }
    }
}
