#region Code
using DevExpress.Spreadsheet;
using DevExpress.XtraPrinting;
#endregion Code
using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Controllers.Spreadsheet;
using BlazorDemo.OfficeFileApi.Models.Spreadsheet.DocumentConversion;

namespace BlazorDemo.OfficeFileApi.Controllers.SpreadsheetProcessing.DocumentConversion {
    [ApiController]
    [Route("api/spreadsheetprocessing")]
    public class ExcelToPdfController : SpreadsheetControllerBase {
        [HttpPost("excel-to-pdf/convert")]
        public async Task<IActionResult> ConvertExcelToPdf(IFormFile file, [FromForm] ExcelToPdfConversionModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);

            inputStream.Position = 0;

            var pdfStream = ConvertToPdf(
                inputStream,
                model.PdfA, model.PdfUA,
                model.Author ?? string.Empty, model.Title ?? string.Empty, model.Subject ?? string.Empty, model.Keywords ?? string.Empty);

            pdfStream.Position = 0;

            return CreateOutputFile(pdfStream, model.OutputFormat, model.OutputName);
        }

        #region Code
        Stream ConvertToPdf(Stream inputStream, PdfACompatibility pdfAConformance, PdfUACompatibility pdfUACompatibility,
            string author, string title, string subject, string keywords) {
            using var workbook = new Workbook();

            workbook.LoadDocument(inputStream);

            var outputStream = new MemoryStream();

            var exportOptions = new PdfExportOptions() {
                PdfACompatibility = pdfAConformance,
                PdfUACompatibility = pdfUACompatibility
            };

            exportOptions.DocumentOptions.Author = author;
            exportOptions.DocumentOptions.Title = title;
            exportOptions.DocumentOptions.Subject = subject;
            exportOptions.DocumentOptions.Keywords = keywords;

            workbook.ExportToPdf(outputStream, exportOptions);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
