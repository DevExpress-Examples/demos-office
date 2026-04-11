using BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.SearchExtract;
#region Code
using System.Text;
using DevExpress.Docs.Presentation;
#endregion Code
using Microsoft.AspNetCore.Mvc;

namespace BlazorDemo.OfficeFileApi.Controllers.PowerPointPresentation.SearchExtract {
    [ApiController]
    [Route("api/presentation")]
    public class PresentationExtractTextController : PresentationControllerBase {
        [HttpPost("extract-text/process")]
        public async Task<IActionResult> ExtractDocumentText(IFormFile file, [FromForm] PresentationExtractTextModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;
            var slideRange = RangeHelper.GetIndexesByRange(model.ImageExtractMode, model.SlideRange);
            var presentationText = ExtractText(inputStream, slideRange, model.ExtractionSource.Contains(SourceType.Shapes), model.ExtractionSource.Contains(SourceType.Tables), model.ExtractionSource.Contains(SourceType.SpeakerNotes));
            var outputStream = new MemoryStream(Encoding.UTF8.GetBytes(presentationText));
            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }
        #region Code
        string ExtractText(Stream inputStream, List<int> slideRange, bool extractFromShapes, bool extractFromTables, bool extractNotes) {
            using var presentation = new Presentation(inputStream);
            bool shouldExportFromAllSlides = slideRange.Count == 0;
            var sb = new StringBuilder();
            for(int i = 0; i < presentation.Slides.Count; i++) {
                if(shouldExportFromAllSlides || slideRange.Contains(i)) {
                    var slide = presentation.Slides[i];
                    sb.AppendLine($"## Slide {i + 1}");

                    ExtractShapeText(sb, slide.Shapes, extractFromShapes, extractFromTables);
                    if(extractNotes) {
                        var notesText = ExtractTextFromNotes(slide);
                        if(!string.IsNullOrWhiteSpace(notesText)) {
                            sb.AppendLine("### Notes");
                            sb.AppendLine(notesText);
                        }
                    }
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
        void ExtractShapeText(StringBuilder sb, ShapeCollection shapes, bool extractFromShapes, bool extractFromTables) {
            var sortedShapes = shapes.OrderBy(shape => shape.Y).ThenBy(shape => shape.X);
            foreach(ShapeBase shape in sortedShapes) {
                switch(shape) {
                    case Shape textShape:
                        if(extractFromShapes && textShape.TextArea != null)
                            sb.AppendLine(textShape.TextArea.Text);
                        break;
                    case Table table:
                        if(extractFromTables)
                            sb.Append(ExtractTextFromTable(table));
                        break;
                    case GroupShape groupShape:

                        ExtractShapeText(sb, groupShape.Shapes, extractFromShapes, extractFromTables);
                        break;
                }
            }
        }
        public string ExtractTextFromNotes(Slide slide) {
            var notesText = string.Empty;
            if(slide.Notes != null) {
                var notesSlide = slide.Notes;
                var notesBody = notesSlide.Shapes.Where(shape => shape is Shape noteShape && noteShape.PlaceholderSettings.Type == PlaceholderType.Body).FirstOrDefault();
                notesText += notesBody != null ? ((Shape)notesBody).TextArea.Text : string.Empty;
            }
            return notesText;
        }
        public string ExtractTextFromTable(Table table) {
            var sb = new StringBuilder();
            for(int r = 0; r < table.Rows.Count; r++) {
                for(int c = 0; c < table.Columns.Count; c++) {
                    var cell = table[r, c];
                    var cellText = cell.TextArea?.Text ?? string.Empty;
                    sb.Append(cellText);
                    if(c != table.Columns.Count - 1)
                        sb.Append('\t');
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        #endregion Code
    }
}
