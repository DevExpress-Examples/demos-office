using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.OfficeFileApi.Models.Spreadsheet.DocumentConversion {
    public class ExcelToImageConversionModel : ExcelFileModelBase {
        [RegularExpression(@"^(png|jpeg|bmp|tiff|svg)$",
        ErrorMessage = "Invalid image format. Supported formats: png, jpeg, bmp, tiff, svg")]
        public string ImageFormat { get; set; } = "png";

        [Range(72, 1200, ErrorMessage = "Resolution must be between 72 and 1200 DPI")]
        public int Resolution { get; set; } = 300;

        internal readonly List<int> PredefinedResolutions = new() { 72, 96, 300 };

        [RegularExpression(@"^(AllSheets|ActiveSheet)$",
            ErrorMessage = "Invalid export mode. Supported modes: AllSheets, ActiveSheet")]

        public ImageExportMode ExportMode { get; set; } = ImageExportMode.AllSheets;

        public bool ExportGridlines { get; set; } = false;

        // Override OutputFormat to always be the image format for this conversion type
        public override string OutputFormat {
            get => ImageFormat;
            set => ImageFormat = value; // keep binder happy if it posts OutputFormat
        }
        public override string OutputName { get; set; } = "Document_Images";
        internal readonly List<FormatInfo> ImageFormats = new()
        {
            new FormatInfo() { Text = "PNG (Portable Network Graphics)", Value = "png" },
            new FormatInfo() { Text = "JPEG (Joint Photographic Experts Group)", Value = "jpeg" },
            new FormatInfo() { Text = "BMP (Bitmap)", Value = "bmp" },
            new FormatInfo() { Text = "TIFF (Tagged Image File Format)", Value = "tiff" },
            new FormatInfo() { Text = "SVG (Scalable Vector Graphics)", Value = "svg" }
        };

        internal readonly List<object> ExportModes = new()
        {
            new { Text = "All Sheets", Value = ImageExportMode.AllSheets },
            new { Text = "Active Sheet", Value = ImageExportMode.ActiveSheet }
        };
    }
    public enum ImageExportMode {
        AllSheets,
        ActiveSheet
    }
}
