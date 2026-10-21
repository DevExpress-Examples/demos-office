using System.Drawing;
using BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.SearchExtract;
#region Code
using DevExpress.Docs;
using DevExpress.Docs.Presentation;
#endregion Code
using Microsoft.AspNetCore.Mvc;
using DevExpress.Docs.Office;

namespace BlazorDemo.OfficeFileApi.Controllers.PowerPointPresentation.SearchExtract {
    [ApiController]
    [Route("api/presentation/find-replace")]
    public class PresentationFindReplaceController : PresentationControllerBase {
        [HttpPost("process")]
        public async Task<IActionResult> ProcessFindReplace([FromForm] IFormFile file, [FromForm] PresentationFindReplaceModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            var targetFormat = PresentationFormatHelper.GetDocumentFormat(model.OutputFormat);
            Stream outputStream = FindReplacePresentationText(inputStream, model.Operation, model.SearchText, model.ReplaceText, ColorTranslator.FromHtml(model.HighlightColor ?? string.Empty), targetFormat);

            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }
        #region Code
        Stream FindReplacePresentationText(Stream inputStream, FindReplaceActionType actionType, string findText, string replaceText, Color highlightColor, DocumentFormat outputFormat) {
            var presentation = new Presentation(inputStream);

            var searchResult = presentation.FindText(findText);
            switch(actionType) {
                case FindReplaceActionType.Replace:
                    presentation.ReplaceText(searchResult, replaceText);
                    break;
                case FindReplaceActionType.Remove:
                    presentation.RemoveText(searchResult);
                    break;
                case FindReplaceActionType.Highlight:
                    presentation.ModifyTextProperties(searchResult, new DevExpress.Docs.Office.TextProperties() { HighlightColor = new OfficeColor(highlightColor) });
                    break;
            }
            var outputStream = new MemoryStream();

            presentation.SaveDocument(outputStream, outputFormat);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
