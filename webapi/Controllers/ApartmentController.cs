using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using Microsoft.AspNetCore.Mvc;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;
using webapi.Model;

namespace webapi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApartmentController : ControllerBase
{
    const string SPREADSHEET_ID = "1jtM2ZAhEo8SGTQEKIRJbNbPF49JYDXriiQll-2Sx4Bg";
    const string SHEET_NAME = "Bill";

    SpreadsheetsResource.ValuesResource _googleSheetValues;

    public ApartmentController(GoogleSheetsHelper googleSheetsHelper)
    {
        _googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var range = $"{SHEET_NAME}!A:M";

        var request = _googleSheetValues.Get(SPREADSHEET_ID, range);
        var response = request.Execute();
        var values = response.Values;

        return Ok(ItemsMapper.MapFromRangeData(values));
    }

    [HttpGet("{rowId}")]
    public IActionResult Get(int rowId)
    {
        var range = $"{SHEET_NAME}!A{rowId}:D{rowId}";
        var request = _googleSheetValues.Get(SPREADSHEET_ID, range);
        var response = request.Execute();
        var values = response.Values;

        return Ok(ItemsMapper.MapFromRangeData(values).FirstOrDefault());
    }

    [HttpPost]
    public IActionResult Post(Apartment item)
    {
        var range = $"{SHEET_NAME}!A:D";
        var valueRange = new ValueRange
        {
            Values = ItemsMapper.MapToRangeData(item)
        };

        var appendRequest = _googleSheetValues.Append(valueRange, SPREADSHEET_ID, range);
        appendRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.USERENTERED;
        appendRequest.Execute();

        return CreatedAtAction(nameof(Get), item);
    }

    [HttpPut("{rowId}")]
    public IActionResult Put(int rowId, Apartment item)
    {
        var range = $"{SHEET_NAME}!A{rowId}:D{rowId}";
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
        var range = $"{SHEET_NAME}!A{rowId}:D{rowId}";
        var requestBody = new ClearValuesRequest();

        var deleteRequest = _googleSheetValues.Clear(requestBody, SPREADSHEET_ID, range);
        deleteRequest.Execute();

        return NoContent();
    }
}