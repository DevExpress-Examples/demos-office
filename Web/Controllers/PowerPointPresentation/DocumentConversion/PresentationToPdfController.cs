using BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.DocumentConversion;
using Microsoft.AspNetCore.Mvc;
#region Code
using DevExpress.Docs.Presentation;
#endregion Code

namespace BlazorDemo.OfficeFileApi.Controllers.PowerPointPresentation.DocumentConversion {
    [ApiController]
    [Route("api/presentation")]
    public class PresentationToPdfController : PresentationControllerBase {
        [HttpPost("presentation-to-pdf/convert")]
        public async Task<IActionResult> ConvertPresentationToPdf(IFormFile file, [FromForm] PresentationToPdfConversionModel model) {
            if(file == null || file.Length == 0)
                return BadRequest(new { Success = false, Message = "No file uploaded" });

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;
            var outputStream = ConvertToPdf(ms);
            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }
        #region Code
        Stream ConvertToPdf(Stream inputStream) {
            using var presentation = new Presentation(inputStream);

            var outputStream = new MemoryStream();

            presentation.ExportToPdf(outputStream);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
