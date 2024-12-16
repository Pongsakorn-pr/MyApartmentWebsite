using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using Microsoft.AspNetCore.Mvc;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;
using webapi.Model;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;

namespace webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApartmentController : ControllerBase
{
    const string SPREADSHEET_ID = "1CpeyKGh-l8BsvyTTsJALb8Im0jTt91uW-9KT9328T2M";
    const string SHEET_NAME = "Bill";

    SpreadsheetsResource.ValuesResource _googleSheetValues;

    public ApartmentController(GoogleSheetsHelper googleSheetsHelper)
    {
        _googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var range = $"{SHEET_NAME}!A:O";
        var request = _googleSheetValues.Get(SPREADSHEET_ID, range);
        var response = request.Execute();
        var values = response.Values;
        var returnValue = ItemsMapper.MapFromRangeData(values);
        int countReturn = returnValue.Count();
        ArrayList arraylist = new ArrayList();
        arraylist.Add(returnValue);
        arraylist.Add(countReturn);
        return Ok(arraylist);
    }
    [HttpGet("{rowId}")]
    public IActionResult Get(int rowId)
    {
        var range = $"{SHEET_NAME}!A{rowId}:O{rowId}";
        var request = _googleSheetValues.Get(SPREADSHEET_ID, range);
        var response = request.Execute();
        var values = response.Values;

        return Ok(ItemsMapper.MapFromRangeData(values).FirstOrDefault());
    }
    [HttpPost("oldMeter")]
    public IActionResult Post([FromBody] MeterRequest reqData)
    {
        var range = $"{SHEET_NAME}!A:O";
        var request = _googleSheetValues.Get(SPREADSHEET_ID, range);
        var response = request.Execute();
        var values = response.Values;
        var allData = ItemsMapper.MapFromRangeData(values);
        var filter = allData.Where
            (x => x.room_number == reqData.room_number && x.Month == reqData.Month && x.Year == reqData.Year).Select(a => new { a.water_reading_meter }).ToList();
        return Ok(filter);
    }
    [HttpPost]
    public IActionResult Post([FromBody] Apartment item)
    {
        var range = $"{SHEET_NAME}!A:O";
        /*
        Apartment item = new Apartment
        {
            bill_id = jsonDynamic.bill_id.Value,
            room_number = jsonDynamic.room_number.Value,
            bill_month_year = DateTime.Now.ToString("dd/MM/yyyy"),
            room_rent = jsonDynamic.room_rent.Value,
            water_reading_meter = jsonDynamic.water_reading_meter.Value,
            water_unit_fees = jsonDynamic.water_unit_fees.Value,
            garbage_fees = jsonDynamic.garbage_fees.Value,
            other_fees = jsonDynamic.other_fees.Value,
            previous_meter_month = jsonDynamic.previous_meter_month.Value,
            water_diff = jsonDynamic.water_diff.Value,
            total_amount = jsonDynamic.total_amount.Value,
            Month = jsonDynamic.Month.Value,
            Year = jsonDynamic.Year.Value
        };*/
        // item.BAHT = BahtTextConverter.ConvertToBahtText(Convert.ToDecimal(item.BAHT));
        var valueRange = new ValueRange
        {
            Values = ItemsMapper.MapToRangeData(item)
        };

        var appendRequest = _googleSheetValues.Append(valueRange, SPREADSHEET_ID, range);
        appendRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.USERENTERED;
        appendRequest.Execute();

        return CreatedAtAction(nameof(Get), item);
    }
    public class rowRequest
    {
        public int rowIndex { get; set; }
    }
    [HttpPost("PdfBill")]
    public IActionResult Print([FromBody] rowRequest rowIndex)
    {
        int rowId = rowIndex.rowIndex + 1 ;
        var range = $"{SHEET_NAME}!A{rowId}:O{rowId}";
        var request = _googleSheetValues.Get(SPREADSHEET_ID, range);
        var response = request.Execute();
        var values = response.Values;
        Apartment item = ItemsMapper.MapFromRangeData(values).FirstOrDefault();
        // Create a MemoryStream to hold the generated PDF data
        try
        {
            BillAparment billAparment = new BillAparment();
            byte[] pdfBytes = billAparment.PrintBillPdf(item);

            // Return the PDF file as a response
            return File(pdfBytes, "application/pdf", $"Bill_{item.bill_id}.pdf");
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
[HttpPut("{rowId}")]
    public IActionResult Put(int rowId, Apartment item)
    {
        var range = $"{SHEET_NAME}!A{rowId}:O{rowId}";
        var valueRange = new ValueRange
        {
            Values = ItemsMapper.MapToRangeData(item)
        };

        var updateRequest = _googleSheetValues.Update(valueRange, SPREADSHEET_ID, range);
        updateRequest.ValueInputOption = UpdateRequest.ValueInputOptionEnum.USERENTERED;
        updateRequest.Execute();

        return NoContent();
    }

    [HttpDelete("{rowId}")]
    public IActionResult Delete(int rowId)
    {
        var range = $"{SHEET_NAME}!A{rowId}:O{rowId}";
        var requestBody = new ClearValuesRequest();

        var deleteRequest = _googleSheetValues.Clear(requestBody, SPREADSHEET_ID, range);
        deleteRequest.Execute();

        return NoContent();
    }
    public class MeterRequest
    {
        public string? room_number { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
