#region Code
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
#endregion Code
using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.WordProcessing.DocumentProtection;

namespace BlazorDemo.OfficeFileApi.Controllers.WordProcessing.DocumentProtection {
    [ApiController]
    [Route("api/wordprocessing")]
    public class WordEncryptPasswordController : WordControllerBase {
        [HttpPost("encrypt-password/encrypt")]
        public async Task<IActionResult> EncryptDocument(IFormFile file, [FromForm] WordEncryptPasswordModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            DocumentFormat outputFormatConverted = WordFormatHelper.GetDocumentFormat(model.OutputFormat);
            var documentStream = EncryptDocumentWithPassword(inputStream, outputFormatConverted, model.Password, model.EncryptionType);

            return CreateOutputFile(documentStream, model.OutputFormat, model.OutputName);
        }

        #region Code
        Stream EncryptDocumentWithPassword(Stream inputStream, DocumentFormat outputFormat, string password, EncryptionType encryptionType) {
            using var wordProcessor = new RichEditDocumentServer();

            wordProcessor.LoadDocument(inputStream);

            var encryptionSettings = new EncryptionSettings {
                Password = password,
                Type = encryptionType
            };

            var outputStream = new MemoryStream();

            wordProcessor.SaveDocument(outputStream, outputFormat, encryptionSettings);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
