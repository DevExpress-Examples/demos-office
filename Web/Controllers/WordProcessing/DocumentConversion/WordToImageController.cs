#region Code
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Export.Image;
using DevExpress.Drawing;
#endregion Code
using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.WordProcessing.DocumentConversion;

namespace BlazorDemo.OfficeFileApi.Controllers.WordProcessing.DocumentConversion {
    [ApiController]
    [Route("api/wordprocessing")]
    public class WordToImageController : WordControllerBase {
        [HttpPost("word-to-image/convert")]
        public async Task<IActionResult> ConvertWordToImage(IFormFile file, [FromForm] WordToImageConversionModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            var correctedPageRange = GetPagesToExport(model.ExportMode, model.PageRange);
            var imageStreams = ConvertToImages(inputStream, model.ImageFormat, model.Resolution, correctedPageRange);

            if(imageStreams == null || imageStreams.Count == 0)
                return StatusCode(500, new { Success = false, Message = "Failed to convert document to images" });

            if(imageStreams.Count == 1) {
                var fileName = "page_image";
                return CreateOutputFile((Stream)imageStreams[0], model.ImageFormat.ToString(), fileName);
            } else {
                var zipFileName = $"{model.OutputName}.zip";
                return CreateZipOutputFile(imageStreams, model.ImageFormat.ToString(), "page", zipFileName);
            }
        }

        string GetPagesToExport(RangeExportMode exportMode, string pageRange) {
            return exportMode switch {
                RangeExportMode.First => "1",
                RangeExportMode.All => String.Empty,
                RangeExportMode.CustomRange => pageRange,
                _ => "1"
            };
        }

        #region Code
        IReadOnlyList<Stream> ConvertToImages(Stream inputStream, DXImageFormat imageFormat, int resolution, string pageRange) {
            using var wordProcessor = new RichEditDocumentServer();

            wordProcessor.LoadDocument(inputStream);

            var imageExportOptions = new RichEditImageExportOptions {
                ExportMode = RichEditImageExportMode.DifferentFiles,
                Resolution = resolution,
                PageRange = pageRange,
                Format = imageFormat
            };

            var imageStreams = wordProcessor.Document.ExportToImage(imageExportOptions);
            return imageStreams;
        }
        #endregion Code
    }
}
