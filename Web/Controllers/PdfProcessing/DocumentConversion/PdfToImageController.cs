using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.PdfProcessing.DocumentConversion;
#region Code
using DevExpress.Pdf;
#endregion

namespace BlazorDemo.OfficeFileApi.Controllers.PdfProcessing.DocumentConversion {
    [ApiController]
    [Route("api/pdfprocessing")]
    public class PdfToImageController : PdfControllerBase {
        [HttpPost("document-conversion/pdf-to-image")]
        public async Task<IActionResult> ConvertPdfToImage(IFormFile file, [FromForm] PdfToImageModel model) {
            using var documentStream = new MemoryStream();
            await file.CopyToAsync(documentStream);
            documentStream.Position = 0;

            var pageRange = RangeHelper.GetIndexesByRange(model.ExportMode, model.PageRange).Select(x => x + 1);

            var imageStreams = ConvertToImages(documentStream, model.ImageFormat, model.Resolution, pageRange);

            if(imageStreams?.Any() == false)
                return StatusCode(500, new { Success = false, Message = "Failed to convert document to images" });

            if(imageStreams?.Count() == 1) {
                var fileName = "page_image";
                return CreateOutputFile(imageStreams?.ElementAt(0), model.ImageFormat, fileName);
            } else {
                var zipFileName = $"{model.OutputName}.zip";
                return CreateZipOutputFile(imageStreams, model.ImageFormat, "page", zipFileName);
            }
        }

        #region Code
        IReadOnlyList<Stream> ConvertToImages(Stream documentStream, string imageFormat, int resolution, IEnumerable<int> pageRange) {
            using var processor = new PdfDocumentProcessor();
            processor.LoadDocument(documentStream);

            var pageCount = processor.Document.Pages.Count;
            if(!pageRange.Any())
                pageRange = Enumerable.Range(1, pageCount);

            var result = new List<Stream>();
            if(imageFormat == "tiff") {
                var tiffStream = new MemoryStream();
                processor.CreateTiff(tiffStream, pageRange.Where(x => x >= 1 && x <= processor.Document.Pages.Count), resolution);
                tiffStream.Position = 0;
                result.Add(tiffStream);
                return result;
            }

            var renderingParameters = PdfPageRenderingParameters.CreateWithResolution(resolution);
            foreach(var index in pageRange) {
                if(index < 1 || index > pageCount)
                    continue;

                var stream = new MemoryStream();

                switch(imageFormat) {
                    case "bmp":
                        var bmpBitmap = processor.CreateDXBitmap(index, renderingParameters);
                        bmpBitmap.Save(stream);
                        break;
                    case "png":
                        var pngBitmap = processor.CreateDXBitmap(index, renderingParameters);
                        pngBitmap.Save(stream, DevExpress.Drawing.DXImageFormat.Png);
                        break;
                    case "svg":
                        var svg = processor.CreateSvgImage(index, renderingParameters);
                        svg.Save(stream);
                        break;
                    default: throw new ArgumentException("Incorrect image format");
                }

                stream.Position = 0;
                result.Add(stream);
            }
            return result;
        }
        #endregion
    }
}
