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
    public class ExcelEncryptPasswordController : SpreadsheetControllerBase {
        [HttpPost("encrypt-password/encrypt")]
        public async Task<IActionResult> EncryptDocument(IFormFile file, [FromForm] ExcelEncryptPasswordModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            EncryptionType encryptionTypeConverted = GetEncryptionType(model.EncryptionType);
            DocumentFormat outputFormatConverted = ExcelFormatHelper.GetDocumentFormat(model.OutputFormat);

            var documentStream = EncryptDocumentWithPassword(inputStream, outputFormatConverted, model.Password, encryptionTypeConverted);
            return CreateOutputFile(documentStream, model.OutputFormat, model.OutputName);
        }

        EncryptionType GetEncryptionType(string encryptionType) {
            return encryptionType.ToLowerInvariant() switch {
                "compatible" => EncryptionType.Compatible,
                "strong" => EncryptionType.Strong,
                _ => EncryptionType.Strong
            };
        }

        #region Code
        Stream EncryptDocumentWithPassword(Stream inputStream, DocumentFormat outputFormat, string password, EncryptionType encryptionType) {
            using var workboook = new Workbook();

            workboook.LoadDocument(inputStream);

            var encryptionSettings = new EncryptionSettings {
                Password = password,
                Type = encryptionType
            };

            var outputStream = new MemoryStream();

            workboook.SaveDocument(outputStream, outputFormat, encryptionSettings);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
