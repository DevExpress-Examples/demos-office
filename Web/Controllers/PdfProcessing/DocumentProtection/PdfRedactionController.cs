using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.PdfProcessing.DocumentProtection;
#region Code
using DevExpress.Pdf;
#endregion

namespace BlazorDemo.OfficeFileApi.Controllers.PdfProcessing.DocumentProtection {
    [ApiController]
    [Route("api/pdfprocessing")]
    public class PdfRedactionController : PdfControllerBase {
        [HttpPost("document-protection/add-redaction")]
        public async Task<IActionResult> AddRedactionToPdf(IFormFile file, [FromForm] PdfRedactionModel model) {
            var documentStream = new MemoryStream();
            await file.CopyToAsync(documentStream);
            documentStream.Position = 0;

            var outputStream = AddRedaction(documentStream, model.SearchText, model.RedactionText, model.ApplyRedaction);
            outputStream.Position = 0;

            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }

        #region Code
        Stream AddRedaction(Stream documentStream, string searchText, string redactionText, bool applyRedaction) {
            using var processor = new PdfDocumentProcessor();

            processor.LoadDocument(documentStream);

            PdfTextSearchResults search;

            do {
                search = processor.FindText(
                    searchText,
                    new PdfTextSearchParameters() {
                        CaseSensitive = false,
                        WholeWords = false
                    });
                if(search.Status == PdfTextSearchStatus.Found) {
                    var annotation = processor.DocumentFacade.Pages[search.Page.GetPageIndex()].AddRedactAnnotation(search.Rectangles);

                    annotation.Author = "DX Demo";
                    annotation.FillColor = new PdfRGBColor(0, 0, 0);
                    annotation.FontColor = new PdfRGBColor(1, 1, 1);
                    if(!string.IsNullOrEmpty(redactionText))
                        annotation.OverlayText = redactionText;
                    annotation.CreationDate = DateTime.UtcNow;
                    annotation.TextJustification = PdfTextJustification.LeftJustified;
                    annotation.RepeatText = true;
                    annotation.FontSize = 0;
                }
            } while(search.Status == PdfTextSearchStatus.Found);

            if(applyRedaction)
                processor.DocumentFacade.ApplyRedactAnnotations();

            var outputStream = new MemoryStream();
            processor.SaveDocument(outputStream);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion
    }
}
