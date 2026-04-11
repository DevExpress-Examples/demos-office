#region Code
using DevExpress.XtraRichEdit;
#endregion Code
using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.WordProcessing.DocumentConversion;

namespace BlazorDemo.OfficeFileApi.Controllers.WordProcessing.DocumentConversion {
    [ApiController]
    [Route("api/wordprocessing")]
    public class WordToWordController : WordControllerBase {
        [HttpPost("word-to-word/convert")]
        public async Task<IActionResult> ConvertWordToWord(IFormFile file, [FromForm] WordToWordConversionModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            var targetFormat = WordFormatHelper.GetDocumentFormat(model.OutputFormat);
            var convertedStream = ConvertDocument(inputStream, model.EmbedFonts, targetFormat);

            return CreateOutputFile(convertedStream, model.OutputFormat, model.OutputName);
        }
        #region Code
        Stream ConvertDocument(Stream inputStream, bool embedFonts, DocumentFormat outputFormat) {
            using var wordProcessor = new RichEditDocumentServer();

            wordProcessor.Options.Export.Html.EmbedImages = true;
            wordProcessor.Options.Export.Mht.EmbedImages = true;

            wordProcessor.LoadDocument(inputStream);

            wordProcessor.Document.EmbedFonts = embedFonts;

            var outputStream = new MemoryStream();

            wordProcessor.SaveDocument(outputStream, outputFormat);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
