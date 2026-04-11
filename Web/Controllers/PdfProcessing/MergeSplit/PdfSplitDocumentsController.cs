using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.PdfProcessing.MergeSplit;
#region Code
using DevExpress.Pdf;
#endregion

namespace BlazorDemo.OfficeFileApi.Controllers.PdfProcessing.MergeSplit {
    [ApiController]
    [Route("api/pdfprocessing")]
    public class PdfSplitDocumentsController : PdfControllerBase {
        [HttpPost("pdf-merge-split/split")]
        public async Task<IActionResult> SplitPdfDocuments(IFormFile file, [FromForm] PdfSplitDocumentsModel model) {
            using var documentStream = new MemoryStream();
            await file.CopyToAsync(documentStream);
            documentStream.Position = 0;

            var pageRange = RangeHelper.GetIndexesByRange(model.ExportMode, model.PageRange);

            var outputStreams = SplitDocuments(documentStream, pageRange);

            return CreateZipOutputFile(outputStreams, "pdf", "document", model.OutputName);
        }

        #region Code
        IReadOnlyList<Stream> SplitDocuments(Stream documentStream, IEnumerable<int> pageRange) {
            using var processor = new PdfDocumentProcessor();
            processor.LoadDocument(documentStream);

            if(!pageRange.Any())
                pageRange = Enumerable.Range(0, processor.Document.Pages.Count);

            var result = new List<Stream>();
            foreach(var index in pageRange) {
                if(index < 0 || index >= processor.Document.Pages.Count)
                    continue;

                using var pageProcessor = new PdfDocumentProcessor();
                pageProcessor.CreateEmptyDocument();
                pageProcessor.Document.Pages.Add(processor.Document.Pages[index]);

                var outputStream = new MemoryStream();

                pageProcessor.SaveDocument(outputStream);

                outputStream.Position = 0;
                result.Add(outputStream);
            }

            return result;
        }
        #endregion
    }
}
