using BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.MergeSplit;
#region Code
using DevExpress.Docs.Presentation;
#endregion Code
using Microsoft.AspNetCore.Mvc;

namespace BlazorDemo.OfficeFileApi.Controllers.PowerPointPresentation.MergeSplit {
    [ApiController]
    [Route("api/presentation")]
    public class PresentationMergeController : PresentationControllerBase {
        [HttpPost("merge/process")]
        public async Task<IActionResult> MergePresentationDocuments(List<IFormFile> files, [FromForm] PresentationMergeModel model) {
            var inputStreams = new List<Stream>();
            foreach(var file in files) {
                var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                ms.Position = 0;
                inputStreams.Add(ms);
            }

            var targetFormat = PresentationFormatHelper.GetDocumentFormat(model.OutputFormat);
            var outputStream = MergePresentations(inputStreams, targetFormat, model.IsPdfOutput);
            outputStream.Position = 0;
            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }
        #region Code
        Stream MergePresentations(List<Stream> inputStreams, DocumentFormat targetFormat, bool pdfOutput) {
            var outputStream = new MemoryStream();

            if(inputStreams.Count > 0) {
                using var presentation = new Presentation(inputStreams[0]);
                for(int i = 1; i < inputStreams.Count; i++) {
                    var inputStream = inputStreams[i];
                    using var tempPresentation = new Presentation(inputStream);
                    foreach(var slide in tempPresentation.Slides)
                        presentation.Slides.Add(slide);
                }
                if(pdfOutput)
                    presentation.ExportToPdf(outputStream);
                else
                    presentation.SaveDocument(outputStream, targetFormat);
            }

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
