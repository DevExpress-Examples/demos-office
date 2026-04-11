using BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.DocumentElements;
#region Code
using DevExpress.Docs.Presentation;
using DevExpress.Office.Utils;
#endregion Code
using Microsoft.AspNetCore.Mvc;

namespace BlazorDemo.OfficeFileApi.Controllers.PowerPointPresentation.DocumentElements {
    [ApiController]
    [Route("api/presentation/tables")]
    public class PresentationTablesController : PresentationControllerBase {
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateTable([FromForm] PresentationTableModel model) {
            var outputStream = CreatePresentationTable(
                model.NumberOfRows,
                model.NumberOfColumns,
                model.ColumnWidth,
                model.TableStyle,
                model.StyleOption.Contains(TableStyleOption.HeaderRow),
                model.StyleOption.Contains(TableStyleOption.BandedRows),
                model.StyleOption.Contains(TableStyleOption.BandedColumns),
                model.IsPdfOutput,
                PresentationFormatHelper.GetDocumentFormat(model.OutputFormat)
            );
            return CreateOutputFile(outputStream, model.IsPdfOutput ? "pdf" : model.OutputFormat, model.OutputName);
        }
        #region Code
        Stream CreatePresentationTable(int rows, int columns, float columnWidthInInches, TableStyleType styleType,
            bool hasHeader, bool hasBandedRows, bool hasBandedColumns, bool isPdfOutput, DocumentFormat format) {
            using var presentation = new Presentation();

            presentation.Slides.Clear();
            var slide = new Slide(SlideLayoutType.Blank);
            presentation.Slides.Add(slide);
            var tableWidth = Units.InchesToDocumentsF(columns * columnWidthInInches);
            var table = new Table(rows, columns, 100, 100, tableWidth) {
                Style = new ThemedTableStyle(styleType),
                HasHeaderRow = hasHeader,
                HasBandedRows = hasBandedRows,
                HasBandedColumns = hasBandedColumns
            };
            slide.Shapes.Add(table);

            var outputStream = new MemoryStream();

            if(isPdfOutput)
                presentation.ExportToPdf(outputStream);
            else
                presentation.SaveDocument(outputStream, format);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
