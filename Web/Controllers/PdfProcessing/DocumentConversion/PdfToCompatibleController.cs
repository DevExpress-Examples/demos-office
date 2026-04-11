#region Code
using DevExpress.Pdf;
#endregion Code
using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.PdfProcessing.DocumentConversion;

namespace BlazorDemo.OfficeFileApi.Controllers.PdfProcessing.DocumentConversion {
    [ApiController]
    [Route("api/pdfprocessing")]
    public class PdfToCompatibleController : PdfControllerBase {
        [HttpPost("document-conversion/pdf-to-compatible")]
        public async Task<IActionResult> ConvertPdfToCompatible(
            IFormFile file,
            [FromForm] PdfToCompatibleModel model) {
            using var documentStream = new MemoryStream();
            await file.CopyToAsync(documentStream);
            documentStream.Position = 0;

            var outputStream = ConvertToCompatible(documentStream, model.CompatibilityLevel);
            outputStream.Position = 0;

            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }

        #region Code
        Stream ConvertToCompatible(Stream documentStream, PdfCompatibility compatibilityLevel) {
            using var converter = new PdfDocumentConverter(documentStream);

            var outputStream = new MemoryStream();

            converter.Convert(compatibilityLevel);
            converter.SaveDocument(outputStream);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
