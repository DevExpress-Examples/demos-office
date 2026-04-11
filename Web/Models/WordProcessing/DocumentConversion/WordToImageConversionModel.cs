using DevExpress.Drawing;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BlazorDemo.OfficeFileApi.Models.WordProcessing.DocumentConversion {
    public class WordToImageConversionModel : WordFileModelBase, IValidatableObject {
        public DXImageFormat ImageFormat { get; set; } = DXImageFormat.Png;

        [Range(72, 1200, ErrorMessage = "Resolution must be between 72 and 1200 DPI")]
        public int Resolution { get; set; } = 300;

        internal readonly List<int> PredefinedResolutions = new() { 72, 96, 300 };

        public RangeExportMode ExportMode { get; set; } = RangeExportMode.All;

        public string PageRange { get; set; } = string.Empty;

        public override string OutputName { get; set; } = "Document_Images";
        internal readonly List<FormatInfo> ImageFormats = new()
    {
        new FormatInfo() { Text = "PNG (Portable Network Graphics)", Value = DXImageFormat.Png },
        new FormatInfo() { Text = "JPEG (Joint Photographic Experts Group)", Value = DXImageFormat.Jpeg },
        new FormatInfo() { Text = "BMP (Bitmap)", Value = DXImageFormat.Bmp },
        new FormatInfo() { Text = "SVG (Scalable Vector Graphics)", Value = DXImageFormat.Svg }
    };

        internal readonly List<object> ExportModes = new()
    {
        new { Text = "First Page Only", Value = RangeExportMode.First },
        new { Text = "All Pages", Value = RangeExportMode.All },
        new { Text = "Custom Page Range", Value = RangeExportMode.CustomRange }
    };

        public bool IsPageRangeEnabled => ExportMode == RangeExportMode.CustomRange;

        // Conditional validation for PageRange
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if(IsPageRangeEnabled) {
                if(string.IsNullOrWhiteSpace(PageRange)) {
                    yield return new ValidationResult(
                        "PageRange is required when Export Mode is 'custom'.",
                        new[] { nameof(PageRange) });
                    yield break;
                }

                // Accept patterns like: 1,2,4-6, 10-12,15
                const string pattern = @"^\s*\d+(\s*-\s*\d+)?(\s*,\s*\d+(\s*-\s*\d+)?)*\s*$";
                if(!Regex.IsMatch(PageRange, pattern)) {
                    yield return new ValidationResult(
                        "PageRange must be a comma-separated list of numbers or ranges (e.g. 1,3,5-7, 10-12).",
                        new[] { nameof(PageRange) });
                }
            }
        }
    }
}
