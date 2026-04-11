using BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.DocumentConversion;
using Microsoft.AspNetCore.Mvc;
#region Code
using DevExpress.Docs.Presentation;
#endregion Code

namespace BlazorDemo.OfficeFileApi.Controllers.PowerPointPresentation.DocumentConversion {
    [ApiController]
    [Route("api/presentation")]
    public class PresentationToPresentationController : PresentationControllerBase {
        [HttpPost("presentation-to-presentation/convert")]
        public async Task<IActionResult> ConvertPresentationToFormat(IFormFile file, [FromForm] PresentationToPresentationConversionModel model) {
            if(file == null || file.Length == 0)
                return BadRequest(new { Success = false, Message = "No file uploaded" });

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;
            var outputStream = ConvertToFormat(ms, PresentationFormatHelper.GetDocumentFormat(model.OutputFormat));
            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }
        #region Code
        Stream ConvertToFormat(Stream inputStream, DocumentFormat targetFormat) {
            using var presentation = new Presentation(inputStream);

            var outputStream = new MemoryStream();

            presentation.SaveDocument(outputStream, targetFormat);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
