#region Code
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet.Export;
#endregion Code
using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Controllers.Spreadsheet;
using BlazorDemo.OfficeFileApi.Models.Spreadsheet.DocumentConversion;

namespace BlazorDemo.OfficeFileApi.Controllers.SpreadsheetProcessing.DocumentConversion {
    [ApiController]
    [Route("api/spreadsheetprocessing")]
    public class ExcelToHtmlController : SpreadsheetControllerBase {
        [HttpPost("excel-to-html/convert")]
        public async Task<IActionResult> ConvertExcelToHtml(IFormFile file, [FromForm] ExcelToHtmlConversionModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);

            inputStream.Position = 0;

            int sheetIndex = model.SheetIndex;
            bool exportImages = model.ExportImages;

            var convertedStream = ConvertToHtml(inputStream, sheetIndex, exportImages);
            return CreateOutputFile(convertedStream, model.OutputFormat, string.Format("{0} {1}", model.OutputName, sheetIndex));
        }
        #region Code
        Stream ConvertToHtml(Stream inputStream, int sheetIndex, bool exportImages) {
            using var workbook = new Workbook();

            workbook.LoadDocument(inputStream);

            var outputStream = new MemoryStream();

            var exportOptions = new HtmlDocumentExporterOptions() {
                SheetIndex = sheetIndex,
                EmbedImages = true,
                ExportImages = exportImages
            };

            workbook.ExportToHtml(outputStream, exportOptions);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }

}
