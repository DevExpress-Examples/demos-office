using BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.MergeSplit;
#region Code
using DevExpress.Docs.Presentation;
#endregion Code
using Microsoft.AspNetCore.Mvc;

namespace BlazorDemo.OfficeFileApi.Controllers.PowerPointPresentation.MergeSplit {
    [ApiController]
    [Route("api/presentation")]
    public class PresentationSplitController : PresentationControllerBase {
        [HttpPost("split/process")]
        public async Task<IActionResult> ConvertPresentationToPdf(IFormFile file, [FromForm] PresentationSplitModel model) {
            if(file == null || file.Length == 0)
                return BadRequest(new { Success = false, Message = "No file uploaded" });

            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            var slideIndexes = ParseSlideRange(model.SlideRange);
            var slideStreams = ExtractSlides(inputStream, slideIndexes, model.ExtractMode == SlideExtractMode.SingleFile, model.IsPdfOutput, PresentationFormatHelper.GetDocumentFormat(model.OutputFormat));

            if(slideStreams == null || slideStreams.Count == 0)
                return StatusCode(500, new { Success = false, Message = "Failed to split presentation" });

            if(slideStreams.Count == 1) {
                var fileName = "slide";
                return CreateOutputFile((Stream)slideStreams[0], model.OutputFormat.ToString(), fileName);
            } else {
                var zipFileName = $"{model.OutputName}.zip";
                return CreateZipOutputFile(slideStreams, model.OutputFormat.ToString(), "slide", zipFileName);
            }
        }

        IEnumerable<int> ParseSlideRange(string slideRange) {
            if(string.IsNullOrWhiteSpace(slideRange))
                return Enumerable.Empty<int>();
            var result = new SortedSet<int>();
            var tokens = slideRange.Split(new[] { ',', ';', ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var raw in tokens) {
                var part = raw.Trim();
                if(part.Length == 0) continue;
                var dashIndex = part.IndexOf('-');
                if(dashIndex > 0) {
                    var startStr = part.Substring(0, dashIndex).Trim();
                    var endStr = part.Substring(dashIndex + 1).Trim();
                    if(int.TryParse(startStr, out int start) && int.TryParse(endStr, out int end)) {
                        if(start <= 0 || end <= 0) continue;
                        if(end < start) (start, end) = (end, start);
                        for(int v = start; v <= end; v++) result.Add(v - 1);
                    }
                    continue;
                }
                if(int.TryParse(part, out int single)) {
                    if(single > 0) result.Add(single - 1);
                }
            }
            return result;
        }
        #region Code
        IReadOnlyList<Stream> ExtractSlides(Stream inputStream, IEnumerable<int> slideIndexes, bool isSingleFileOutput,
            bool isPdfOutput, DocumentFormat documentFormat) {
            using var presentation = new Presentation(inputStream);
            return isSingleFileOutput
                ? ExtractSlidesSingleFile(slideIndexes, isPdfOutput, documentFormat, presentation)
                : ExtractSlidesDifferentFiles(slideIndexes, isPdfOutput, documentFormat, presentation);
        }

        IReadOnlyList<Stream> ExtractSlidesDifferentFiles(IEnumerable<int> slideIndexes, bool isPdfOutput, DocumentFormat documentFormat, Presentation presentation) {
            var outputStreams = new List<Stream>();

            SlideCollection slides = presentation.Slides;
            if(!slideIndexes.Any())
                slideIndexes = Enumerable.Range(0, slides.Count);

            foreach(var slideIndex in slideIndexes) {
                if(slideIndex < 0 || slideIndex >= slides.Count)
                    continue;
                using var singleSlidePresentation = new Presentation();
                singleSlidePresentation.Slides.Clear();
                singleSlidePresentation.SlideSize = presentation.SlideSize;
                singleSlidePresentation.Slides.Add(slides[slideIndex]);

                var outputStream = new MemoryStream();

                if(isPdfOutput)
                    singleSlidePresentation.ExportToPdf(outputStream);
                else
                    singleSlidePresentation.SaveDocument(outputStream, documentFormat);

                outputStream.Position = 0;
                outputStreams.Add(outputStream);
            }
            return outputStreams.AsReadOnly();
        }
        IReadOnlyList<Stream> ExtractSlidesSingleFile(IEnumerable<int> slideIndexes, bool isPdfOutput, DocumentFormat documentFormat, Presentation presentation) {
            var outputStreams = new List<Stream>();

            SlideCollection slides = presentation.Slides;
            if(!slideIndexes.Any())
                slideIndexes = Enumerable.Range(0, slides.Count);

            for(int i = slides.Count - 1; i >= 0; i--) {
                if(!slideIndexes.Contains(i))
                    slides.RemoveAt(i);
            }

            var outputStream = new MemoryStream();

            if(isPdfOutput)
                presentation.ExportToPdf(outputStream);
            else
                presentation.SaveDocument(outputStream, documentFormat);

            outputStream.Position = 0;
            outputStreams.Add(outputStream);
            return outputStreams.AsReadOnly();
        }
        #endregion Code
    }
}
