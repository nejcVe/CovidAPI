# CovidAPI

The API exposes two endpoints:

### `/api/region/cases`

Returns a `CasesDTO` for the selected region in the range of given dates.

The request should be made with 3 query parameters:
* `region` : string (with possible values of: LJ, CE, KR, NM, KK, KP, MB, MS, NG, PO, SG, ZA)
* `from` : string (with date in format of `YYYY-MM-DD`)
* `to` : string (with date in format of `YYYY-MM-DD`)

The response is a JSON object with a list of `CasesDTO` for each day.

`CasesDTO`:
* `Date` : string (in datetime format, the date of gathered data)
* `RegionName` : string (selected region)
* `ActiveCases` : int (active cases for a given day)
* `Vaccinated1st` : int (amount of people vaccinated with first shot to that date)
* `Vaccinated2st` : int (amount of people vaccinated with second shot to that date)
* `Deceased` : int (amount of deceased people to date)

### `api/region/lastweek`

Returns a `WeekAverageDTO` for each region for last week.

Request is made without any parameters.

Response is a JSON object with a list of `WeekAverageDTO` sorted in descending order based on average amount of new daily cases in that week.

`WeekAverageDTO`:
* `RegionName` : string (name of the region)
* `AverageNewCases` : double (calculated average of daily new cases for each region)

Data is retrieved daily from https://github.com/sledilnik/data/blob/master/csv/region-cases.csv