#region Code
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
#endregion Code
using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.WordProcessing.DocumentProtection;

namespace BlazorDemo.OfficeFileApi.Controllers.WordProcessing.DocumentProtection {
    [ApiController]
    [Route("api/wordprocessing")]
    public class WordProtectPasswordController : WordControllerBase {
        [HttpPost("protect-password/protect")]
        public async Task<IActionResult> ProtectDocument(IFormFile file, [FromForm] WordProtectPasswordModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            DocumentFormat outputFormatConverted = WordFormatHelper.GetDocumentFormat(model.OutputFormat);
            Stream outputStream = ProtectDocumentWithPassword(inputStream, outputFormatConverted, model.Password, model.ProtectionType);

            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }

        #region Code
        Stream ProtectDocumentWithPassword(Stream inputStream, DocumentFormat outputFormat, string password, DocumentProtectionType protectionType) {
            using var wordProcessor = new RichEditDocumentServer();

            wordProcessor.LoadDocument(inputStream);

            var document = wordProcessor.Document;

            if(document.IsDocumentProtected)
                document.Unprotect();

            if(protectionType == DocumentProtectionType.FillInForms)
                foreach(Section section in document.Sections)
                    section.ProtectedForForms = true;

            document.Protect(password, protectionType);

            var outputStream = new MemoryStream();

            wordProcessor.SaveDocument(outputStream, outputFormat);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
