#region Code
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;
#endregion Code
using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.WordProcessing.DocumentConversion;

namespace BlazorDemo.OfficeFileApi.Controllers.WordProcessing.DocumentConversion {
    [ApiController]
    [Route("api/wordprocessing")]
    public class WordToPdfController : WordControllerBase {
        [HttpPost("word-to-pdf/convert")]
        public async Task<IActionResult> ConvertWordToPdf(IFormFile file, [FromForm] WordToPdfConversionModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            var pdfStream = ConvertToPdf(inputStream, model.PdfA, model.PdfUA,
                model.Author ?? string.Empty, model.Title ?? string.Empty, model.Subject ?? string.Empty, model.Keywords ?? string.Empty, model.ExportControlsAsAcroFormFields);

            pdfStream.Position = 0;

            return CreateOutputFile(pdfStream, model.OutputFormat, model.OutputName);
        }

        #region Code
        Stream ConvertToPdf(Stream inputStream, PdfACompatibility pdfAConformance, PdfUACompatibility pdfUACompatibility,
            string author, string title, string subject, string keywords, bool exportControlsAsAcroForm) {
            using var wordProcessor = new RichEditDocumentServer();

            wordProcessor.LoadDocument(inputStream);

            var outputStream = new MemoryStream();

            var exportOptions = new PdfExportOptions();
            exportOptions.PdfACompatibility = pdfAConformance;
            exportOptions.PdfUACompatibility = pdfUACompatibility;
            exportOptions.DocumentOptions.Author = author;
            exportOptions.DocumentOptions.Title = title;
            exportOptions.DocumentOptions.Subject = subject;
            exportOptions.DocumentOptions.Keywords = keywords;
            exportOptions.ExportEditingFieldsToAcroForms = exportControlsAsAcroForm;

            wordProcessor.ExportToPdf(outputStream, exportOptions);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
