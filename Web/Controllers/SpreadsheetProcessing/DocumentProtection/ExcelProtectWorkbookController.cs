#region Code
using DevExpress.Spreadsheet;
#endregion Code
using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Controllers.Spreadsheet;
using BlazorDemo.OfficeFileApi.Models.Spreadsheet.DocumentProtection;
using BlazorDemo.OfficeFileApi.Services;

namespace BlazorDemo.OfficeFileApi.Controllers.SpreadsheetProcessing.DocumentProtection {
    [ApiController]
    [Route("api/spreadsheetprocessing")]
    public class ExcelProtectWorkbookController : SpreadsheetControllerBase {
        [HttpPost("protect-password/protect-workbook")]
        public async Task<IActionResult> ProtectDocument(IFormFile file, [FromForm] ExcelProtectWorkbookModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            DocumentFormat outputFormatConverted = ExcelFormatHelper.GetDocumentFormat(model.OutputFormat);

            Stream outputStream = ProtectWorkbookWithPassword(
                inputStream,
                outputFormatConverted,
                model.WorkbookPassword,
                model.LockStructure,
                model.LockWindows
                );
            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }

        #region Code
        Stream ProtectWorkbookWithPassword(Stream inputStream, DocumentFormat outputFormat, string workbookPassword, bool lockStructure, bool lockWindows) {
            using var workbook = new Workbook();

            workbook.LoadDocument(inputStream);

            if(!workbook.IsProtected) {
                workbook.Protect(workbookPassword ?? string.Empty, lockStructure, lockWindows);
            }

            var outputStream = new MemoryStream();

            workbook.SaveDocument(outputStream, outputFormat);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
