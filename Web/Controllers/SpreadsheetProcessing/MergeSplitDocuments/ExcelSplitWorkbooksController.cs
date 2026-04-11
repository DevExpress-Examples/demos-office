#region Code
using DevExpress.Spreadsheet;
#endregion Code
using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Controllers.Spreadsheet;
using BlazorDemo.OfficeFileApi.Models.Spreadsheet.MergeSplitDocuments;
using BlazorDemo.OfficeFileApi.Services;

namespace BlazorDemo.OfficeFileApi.Controllers.SpreadsheetProcessing.MergeSplitDocuments {
    [ApiController]
    [Route("api/spreadsheetprocessing")]
    public class ExcelSplitWorkbooksController : SpreadsheetControllerBase {
        [HttpPost("split-workbooks/split")]
        public async Task<IActionResult> Split(IFormFile file, [FromForm] ExcelSplitWorkbooksModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;
            DocumentFormat outputFormatConverted = ExcelFormatHelper.GetDocumentFormat(model.OutputFormat);

            var documentStreams = SplitWorkbook(model.SplitMode == VisibilityMode.VisibleOnly, inputStream, outputFormatConverted);

            if(documentStreams == null || documentStreams.Count == 0)
                return StatusCode(500, new { Success = false, Message = "Failed to split workbook" });

            if(documentStreams.Count == 1) {
                var fileName = "document";
                return CreateOutputFile((Stream)documentStreams[0], model.OutputFormat, fileName);
            } else {
                var zipFileName = $"{model.OutputName}.zip";
                return CreateZipOutputFile(documentStreams, model.OutputFormat, "worksheet", zipFileName);
            }
        }
        #region Code
        IReadOnlyList<Stream> SplitWorkbook(bool splitVisibleOnly, Stream inputStream, DocumentFormat outputFormat) {
            using var sourceWorkbook = new Workbook();

            sourceWorkbook.LoadDocument(inputStream);

            var worksheetsToSplit = new List<Worksheet>();
            if(splitVisibleOnly)
                worksheetsToSplit = sourceWorkbook.Worksheets.Where(worksheet => worksheet.Visible).ToList();
            else
                worksheetsToSplit = sourceWorkbook.Worksheets.Where(worksheet => worksheet.VisibilityType != WorksheetVisibilityType.VeryHidden).ToList();

            var documentStreams = new List<Stream>();
            foreach(var sourceWorksheet in worksheetsToSplit) {
                using var targetWorkbook = new Workbook();
                var targetWorksheet = targetWorkbook.Worksheets[0];
                targetWorksheet.Name = sourceWorksheet.Name;
                targetWorksheet.CopyFrom(sourceWorksheet);

                targetWorksheet.VisibilityType = WorksheetVisibilityType.Visible;

                var outputStream = new MemoryStream();

                targetWorkbook.SaveDocument(outputStream, outputFormat);

                outputStream.Position = 0;
                documentStreams.Add(outputStream);
            }
            return documentStreams.AsReadOnly();
        }
        #endregion Code
    }
}
