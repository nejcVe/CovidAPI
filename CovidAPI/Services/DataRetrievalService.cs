using System.Net;
using System.IO;
using Microsoft.EntityFrameworkCore;
using CovidAPI.Models;
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
                using var fileStream = fileInfo.OpenWrite();
                await stream.CopyToAsync(fileStream);
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
            Region r = new() {
                Active = Int32.Parse(data[idx]),
                ToDateConfirmed = Int32.Parse(data[idx + 1]),
                ToDateDeceased = Int32.Parse(data[idx + 2]),
                Vaccinated1st = Int32.Parse(data[idx + 3]),
                Vaccinated2nd = Int32.Parse(data[idx + 4]),
                Vaccinated3rd = Int32.Parse(data[idx + 5])
            };

            return r;
        }

        public async Task<int> LoadDataToMemory() {
            try {
                using TextFieldParser csvReader = new TextFieldParser("D:/Files/Indigo/CovidAPI/CovidAPI/CovidAPI/Data/region-cases.csv");
                csvReader.SetDelimiters(new string[] { "," });
                csvReader.HasFieldsEnclosedInQuotes = false;
                string[] colFields = csvReader.ReadFields();
                Console.WriteLine(colFields.Length.ToString());
                int[] indices = new int[] { 1, 10, 16, 22, 28, 34, 40, 46, 52, 58, 64, 73 };
                string[] regionNames = new string[] {"CE", "KK", "KP", "KR", "LJ", "MB", "MS", "NG", "NM", "PO", "SG", "ZA"};
                Region r = new Region();

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

                    for (int i = 0; i < indices.Length; i++) {
                        r = ReadRegion(fieldData, indices[i]);
                        r.Name = regionNames[i];
                        day.Regions.Add(r);
                    }

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
