using System.Net;
using System.IO;
using Microsoft.EntityFrameworkCore;
using CovidAPI.Models;
using CovidAPI.Enums;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using System;
using System.Drawing;
using Region = CovidAPI.Models.Region;

namespace CovidAPI.Services {
    public class DataRetrievalService {

        private readonly CovidContext _context;
        public DataRetrievalService(CovidContext context) {
            _context = context;
        }

        private async Task<int> DownloadData() {
            var client = new HttpClient();
            var response = await client.GetAsync(@"https://raw.githubusercontent.com/sledilnik/data/master/csv/region-cases.csv");

            using (var stream = await response.Content.ReadAsStreamAsync()) {
                var fileInfo = new FileInfo("Data/region-cases.csv");
                using (var fileStream = fileInfo.OpenWrite()) {
                    await stream.CopyToAsync(fileStream);
                }
            }
            return 0;
        }

        public async Task<int> CheckData() {
            if (!File.Exists("Data/region-cases.csv")) {
                await DownloadData();
                return 0;
            }
            else {
                DateTime LastChanged = File.GetLastWriteTime("Data/region-cases.csv");
                DateTime now = DateTime.Now;
                if(LastChanged.AddDays(1) < now) {
                    await DownloadData();
                    return 0;
                }
                else {
                    return 1;
                }
            }
        }

        public static Region ReadRegion(string[] data, int idx) {
            Region r = new Region();
            r.Active = Int32.Parse(data[idx]);
            r.ToDateConfirmed = Int32.Parse(data[idx+1]);
            r.ToDateDeceased = Int32.Parse(data[idx+2]);
            r.Vaccinated1st = Int32.Parse(data[idx+3]);
            r.Vaccinated2nd = Int32.Parse(data[idx+4]);
            r.Vaccinated3rd = Int32.Parse(data[idx+5]);

            return r;
        }

        public async Task<int> LoadDataToMemory() {
            try {
                using TextFieldParser csvReader = new TextFieldParser("D:/Files/Indigo/CovidAPI/CovidAPI/CovidAPI/Data/region-cases.csv");
                csvReader.SetDelimiters(new string[] { "," });
                csvReader.HasFieldsEnclosedInQuotes = false;
                string[] colFields = csvReader.ReadFields();
                Console.WriteLine(colFields.Length.ToString());

                while (!csvReader.EndOfData) {
                    string[] fieldData = csvReader.ReadFields();
                    Console.WriteLine(fieldData[0].ToString());

                    DateTime date = DateOnly.ParseExact(fieldData[0], "yyyy-MM-dd").ToDateTime(TimeOnly.Parse("00:00"));
                    DayStats day = new DayStats();
                    day.Date = date;
                    day.Regions = new List<Region>();

                    for (int i = 0; i < fieldData.Length; i++) {
                        if (fieldData[i] == "") {
                            fieldData[i] = "0";
                        }
                    }
                    day.Regions.Add(ReadRegion(fieldData, 1));
                    day.Regions.Add(ReadRegion(fieldData, 10));
                    day.Regions.Add(ReadRegion(fieldData, 16));
                    day.Regions.Add(ReadRegion(fieldData, 22));
                    day.Regions.Add(ReadRegion(fieldData, 28));
                    day.Regions.Add(ReadRegion(fieldData, 34));
                    day.Regions.Add(ReadRegion(fieldData, 40));
                    day.Regions.Add(ReadRegion(fieldData, 46));
                    day.Regions.Add(ReadRegion(fieldData, 52));
                    day.Regions.Add(ReadRegion(fieldData, 58));
                    day.Regions.Add(ReadRegion(fieldData, 64));
                    day.Regions.Add(ReadRegion(fieldData, 73));


                    _context.DayStats.Add(day);
                    await _context.SaveChangesAsync();
                }
                return 0;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return 1;
            }

        }
    }
}
