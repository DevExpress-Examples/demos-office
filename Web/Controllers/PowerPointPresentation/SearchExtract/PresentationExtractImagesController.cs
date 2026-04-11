using BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.SearchExtract;
#region Code
using DevExpress.Docs;
using DevExpress.Docs.Presentation;
using DevExpress.Drawing;
#endregion Code
using Microsoft.AspNetCore.Mvc;

namespace BlazorDemo.OfficeFileApi.Controllers.PowerPointPresentation.SearchExtract {
    [ApiController]
    [Route("api/presentation")]
    public class PresentationExtractImagesController : PresentationControllerBase {
        [HttpPost("extract-images/process")]
        public async Task<IActionResult> ExtractDocumentImages(IFormFile file, [FromForm] PresentationExtractImagesModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;
            var slideRange = RangeHelper.GetIndexesByRange(model.ImageExtractMode, model.SlideRange);
            var imageStreams = ExtractImages(inputStream, model.ImageFormat, slideRange);

            if(imageStreams == null || imageStreams.Count == 0) {
                return StatusCode(500, new { Success = false, Message = "Failed to extract document images" });
            }
            var zipFileName = $"{model.OutputName}.zip";
            return CreateZipOutputFile(imageStreams, model.ImageFormat.ToString(), "image", zipFileName);
        }
        #region Code
        IReadOnlyList<Stream> ExtractImages(Stream inputStream, DXImageFormat imageFormat, List<int> slideRange) {
            using var presentation = new Presentation(inputStream);
            var imageStreams = new List<Stream>();
            bool shouldExportFromAllSlides = slideRange.Count == 0;
            for(int i = 0; i < presentation.Slides.Count; i++) {
                if(shouldExportFromAllSlides || slideRange.Contains(i)) {
                    var slide = presentation.Slides[i];
                    foreach(var shape in slide.Shapes)
                        ExtractShapeImages(shape, imageStreams, imageFormat);
                }
            }
            return imageStreams.AsReadOnly();
        }
        void ExtractShapeImages(ShapeBase shape, IList<Stream> imageStreams, DXImageFormat imageFormat) {
            switch(shape) {
                case PictureShape pictureShape:
                    var officeImage = pictureShape.Image as OfficeImage;
                    var dxImage = officeImage?.DXImage;
                    if(dxImage != null) {
                        var imgStream = new MemoryStream();
                        dxImage.Save(imgStream, imageFormat);
                        imgStream.Position = 0;
                        imageStreams.Add(imgStream);
                    }
                    break;
                case GroupShape groupShape:
                    foreach(var innerShape in groupShape.Shapes)
                        ExtractShapeImages(innerShape, imageStreams, imageFormat);
                    break;
                default:
                    break;
            }
        }
        #endregion Code
    }
}
