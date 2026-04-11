using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.PdfProcessing.DocumentProtection;
#region Code
using DevExpress.Drawing;
using DevExpress.Pdf;
#endregion
namespace BlazorDemo.OfficeFileApi.Controllers.PdfProcessing.DocumentProtection {
    [ApiController]
    [Route("api/pdfprocessing")]
    public class PdfApplyWatermarkController : PdfControllerBase {
        [HttpPost("document-protection/apply-watermark")]
        public async Task<IActionResult> ApplyWatermarkToPdf(
            IFormFile file,
            [FromForm] PdfApplyWatermarkModel model) {
            var documentStream = new MemoryStream();
            await file.CopyToAsync(documentStream);
            documentStream.Position = 0;

            var outputStream = ApplyWatermark(documentStream, model.WatermarkText, model.Rotate);
            outputStream.Position = 0;

            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }

        #region Code
        Stream ApplyWatermark(Stream documentStream, string text, bool rotate) {
            using var processor = new PdfDocumentProcessor();

            processor.LoadDocument(documentStream);

            using var brush = new DXSolidBrush(System.Drawing.Color.FromArgb(50, System.Drawing.Color.LightSkyBlue));

            var fontName = "Arial Black";
            var fontSize = 12;
            var font = new DXFont(fontName, fontSize);

            var stringFormat = PdfStringFormat.GenericTypographic;
            stringFormat.Alignment = PdfStringAlignment.Center;
            stringFormat.LineAlignment = PdfStringAlignment.Center;

            foreach(var page in processor.Document.Pages) {
                using var graphics = processor.CreateGraphicsPageSystem();

                var watermarkSize = page.CropBox.Width * 0.75;

                var stringSize = graphics.MeasureString(text, font);
                float scale = (float)(watermarkSize / (double)stringSize.Width);

                graphics.TranslateTransform((float)(page.CropBox.Width * 0.5), (float)(page.CropBox.Height * 0.5));
                if(rotate)
                    graphics.RotateTransform(-45f);
                graphics.TranslateTransform(
                    (float)(-stringSize.Width * scale * 0.5),
                    (float)(-stringSize.Height * scale * 0.5));

                var actualFont = new DXFont(fontName, fontSize * scale);
                var rect = new System.Drawing.RectangleF(0, 0, stringSize.Width * scale, stringSize.Height * scale);
                graphics.DrawString(text, actualFont, brush, rect, stringFormat);

                graphics.AddToPageForeground(page);
            }

            var outputStream = new MemoryStream();
            processor.SaveDocument(outputStream);

            return outputStream;
        }
        #endregion
    }
}
