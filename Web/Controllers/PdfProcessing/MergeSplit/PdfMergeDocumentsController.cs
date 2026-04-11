using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.PdfProcessing.MergeSplit;
#region Code
using DevExpress.Pdf;
#endregion

namespace BlazorDemo.OfficeFileApi.Controllers.PdfProcessing.MergeSplit {
    [ApiController]
    [Route("api/pdfprocessing")]
    public class PdfMergeDocumentsController : PdfControllerBase {
        [HttpPost("pdf-merge-split/merge")]
        public async Task<IActionResult> MergePdfDocuments(IList<IFormFile> files, [FromForm] PdfMergeDocumentsModel model) {
            var documentStreams = await Task.WhenAll(files.Select(async x => {
                var stream = new MemoryStream();
                await x.CopyToAsync(stream);
                stream.Position = 0;
                return stream;
            }));

            var outputStream = MergeDocuments(documentStreams);
            outputStream.Position = 0;

            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }

        #region Code
        Stream MergeDocuments(IEnumerable<Stream> documentStreams) {
            using var processor = new PdfDocumentProcessor();

            processor.CreateEmptyDocument();

            foreach(var stream in documentStreams)
                processor.AppendDocument(stream);

            var outputStream = new MemoryStream();

            processor.SaveDocument(outputStream);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion
    }
}
