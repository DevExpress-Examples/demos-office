using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.OfficeFileApi.Models.PdfProcessing.DocumentConversion {
    public class PdfToImageModel : PdfPageRangeModelBase {
        internal readonly List<FormatInfo> ImageFormats = new() {
            new() { Text = "BMP (Bitmap)", Value = "bmp" },
            new() { Text = "PNG (Portable Network Graphics)", Value = "png" },
            new() { Text = "TIFF (Tagged Image File Format)", Value = "tiff" },
            new() { Text = "SVG (Scalable Vector Graphics)", Value = "svg" }
        };
        public string ImageFormat { get; set; } = "bmp";
        public override string OutputName { get => "Image"; }
        [Range(72, 1200, ErrorMessage = "Resolution must be between 72 and 1200 DPI")]
        public int Resolution { get; set; } = 300;

        internal readonly List<int> PredefinedResolutions = new() { 72, 96, 300 };
    }
}
