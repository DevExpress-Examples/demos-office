#region Code
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Layout;
using DevExpress.XtraRichEdit.API.Native;
#endregion Code
using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.WordProcessing.MergeSplit;

namespace BlazorDemo.OfficeFileApi.Controllers.WordProcessing.MergeSplit {
    [ApiController]
    [Route("api/wordprocessing")]
    public class SplitController : WordControllerBase {
        [HttpPost("word-split/process")]
        public async Task<IActionResult> SplitWordDocuments(IFormFile file, [FromForm] WordSplitDocumentModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            var targetFormat = WordFormatHelper.GetDocumentFormat(model.OutputFormat);
            IReadOnlyList<Stream> outputStreams;
            string entryName;
            switch(model.SplitMode) {
                case DocumentSplitMode.ByPage:
                    outputStreams = SplitByPage(inputStream, targetFormat, model.IsPdfOutput);
                    entryName = "page";
                    break;
                case DocumentSplitMode.ByHeading:
                    outputStreams = SplitByHeading(inputStream, targetFormat, model.IsPdfOutput);
                    entryName = "heading_section";
                    break;
                case DocumentSplitMode.BySection:
                default:
                    outputStreams = SplitBySection(inputStream, targetFormat, model.IsPdfOutput);
                    entryName = "section";
                    break;
            }
            return CreateZipOutputFile(outputStreams, model.OutputFormat, entryName, model.OutputName);
        }
        #region Code
        IReadOnlyList<Stream> SplitBySection(Stream inputStream, DocumentFormat targetFormat, bool pdfOutput) {
            using var wordProcessor = new RichEditDocumentServer();
            wordProcessor.LoadDocument(inputStream);

            var document = wordProcessor.Document;
            using var sectionProcessor = new RichEditDocumentServer();
            var outputStreams = new List<Stream>();

            foreach(Section section in document.Sections) {
                sectionProcessor.DocxBytes = document.GetDocxBytes(section.Range);
                var sectionStream = new MemoryStream();
                if(pdfOutput)
                    sectionProcessor.ExportToPdf(sectionStream);
                else
                    sectionProcessor.SaveDocument(sectionStream, targetFormat);
                sectionStream.Position = 0;
                outputStreams.Add(sectionStream);
            }

            return outputStreams;
        }

        IReadOnlyList<Stream> SplitByHeading(Stream inputStream, DocumentFormat targetFormat, bool pdfOutput) {
            const string styleName = "Heading 1";

            using var wordProcessor = new RichEditDocumentServer();
            wordProcessor.LoadDocument(inputStream);

            var document = wordProcessor.Document;
            if(document.ParagraphStyles[styleName] == null)
                return null;

            var headingParagraphs = document.Paragraphs.Where(p => p.Style.Name == styleName).ToList();
            using var headingProcessor = new RichEditDocumentServer();
            var outputStreams = new List<Stream>();
            for(int i = 0; i < headingParagraphs.Count; i++) {
                var startPosition = headingParagraphs[i].Range.Start;
                DocumentPosition endPosition;
                if(i == headingParagraphs.Count - 1)
                    endPosition = document.Range.End;
                else
                    endPosition = headingParagraphs[i + 1].Range.Start;
                var headingRange = document.CreateRange(startPosition, endPosition.ToInt() - startPosition.ToInt());

                headingProcessor.DocxBytes = document.GetDocxBytes(headingRange);
                var headingStream = new MemoryStream();
                if(pdfOutput)
                    headingProcessor.ExportToPdf(headingStream);
                else
                    headingProcessor.SaveDocument(headingStream, targetFormat);
                headingStream.Position = 0;
                outputStreams.Add(headingStream);
            }
            return outputStreams;
        }

        IReadOnlyList<Stream> SplitByPage(Stream inputStream, DocumentFormat targetFormat, bool pdfOutput) {
            using var wordProcessor = new RichEditDocumentServer();
            wordProcessor.LoadDocument(inputStream);

            var pageCount = wordProcessor.DocumentLayout.GetPageCount();
            var document = wordProcessor.Document;
            using var pageProcessor = new RichEditDocumentServer();
            var outputStreams = new List<Stream>();
            for(int i = 0; i < pageCount; i++) {
                var page = wordProcessor.DocumentLayout.GetPage(i);
                var pageRange = document.CreateRange(page.MainContentRange.Start, page.MainContentRange.Length);
                pageProcessor.DocxBytes = document.GetDocxBytes(pageRange);
                var pageStream = new MemoryStream();
                if(pdfOutput)
                    pageProcessor.ExportToPdf(pageStream);
                else
                    pageProcessor.SaveDocument(pageStream, targetFormat);
                pageStream.Position = 0;
                outputStreams.Add(pageStream);
            }
            return outputStreams;
        }
        #endregion Code
    }
}
