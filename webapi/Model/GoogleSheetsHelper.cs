using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System;
using System.Globalization;

namespace webapi.Model
{
    public class GoogleSheetsHelper
    {
        public SheetsService Service { get; set; }
        const string APPLICATION_NAME = "BillDatabase";
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };

        public GoogleSheetsHelper()
        {
            InitializeService();
        }

        private void InitializeService()
        {
            var credential = GetCredentialsFromFile();
            Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME
            });
        }

        private GoogleCredential GetCredentialsFromFile()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("billapartment-13fe4d7ba0a1.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            return credential;
        }
    }
    public static class ItemsMapper
    {
        public static List<Apartment> MapFromRangeData(IList<IList<object>> values)
        {
            CultureInfo thaiCulture = new CultureInfo("th-TH");
            var items = new List<Apartment>();

            if (values.Count == 0)
            {
                return items; 
            }

            foreach (var value in values)
            {
                if (value.Count == 0)
                {
                    continue;
                }
                Apartment item = new Apartment();
                if (!int.TryParse(value[0]?.ToString(), out int billId))
                {
                    continue; 
                }
                item.bill_id = billId;
                item.bill_month_year = !string.IsNullOrEmpty(value[2].ToString()) ? value[2].ToString() : string.Empty;
                item.room_number = value[1]?.ToString() ?? string.Empty; // Handle null or empty values
                item.room_rent = !string.IsNullOrEmpty(value[3]?.ToString())? Decimal.Parse(value[3].ToString()) : 0;
                item.water_reading_meter = !string.IsNullOrEmpty(value[4]?.ToString()) ? int.Parse(value[4].ToString()) : 0;
                item.water_unit_fees = !string.IsNullOrEmpty(value[5]?.ToString()) ? int.Parse(value[5].ToString()) : 0;
                item.garbage_fees = !string.IsNullOrEmpty(value[6]?.ToString()) ? int.Parse(value[6].ToString()) : 0;
                item.other_fees = !string.IsNullOrEmpty(value[7]?.ToString()) ? Decimal.Parse(value[7].ToString()) : 0;
                item.previous_meter_month = !string.IsNullOrEmpty(value[8]?.ToString()) ? int.Parse(value[8].ToString()) : 0;
                item.water_diff = !string.IsNullOrEmpty(value[9]?.ToString()) ? int.Parse(value[9].ToString()) : 0;
                item.total_amount = !string.IsNullOrEmpty(value[10]?.ToString()) ? Decimal.Parse(value[10].ToString())  : 0;
                item.Month = !string.IsNullOrEmpty(value[11]?.ToString()) ? int.Parse(value[11].ToString()) : 0;
                item.Year = !string.IsNullOrEmpty(value[12]?.ToString()) ? int.Parse(value[12].ToString()) : 0;
                item.BAHT = value[13].ToString();
                item.Month_TH = value[14].ToString();
                items.Add(item);
            }

            return items;
        }
        public static IList<IList<object>> MapToRangeData(Apartment item)
        {
            var objectList = new List<object>() { item.bill_id, item.room_number, item.bill_month_year, item.room_rent, item.water_reading_meter, item.water_unit_fees, item.garbage_fees,item.other_fees,item.previous_meter_month,item.water_diff,item.total_amount,item.Month,item.Year,item.BAHT,item.Month_TH };
            var rangeData = new List<IList<object>> { objectList };
            return rangeData;
        }
    }
}
