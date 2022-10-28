namespace CovidAPI.Models {
    public class Region {
        public long Id { get; set; }
        public int Active { get; set; }
        public int ToDateConfirmed { get; set; }
        public int ToDateDeceased { get; set; }
        public int Vaccinated1st { get; set; }
        public int Vaccinated2nd { get; set; }
        public int Vaccinated3rd { get; set; }
    }
}
