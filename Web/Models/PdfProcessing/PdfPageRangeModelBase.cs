using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BlazorDemo.OfficeFileApi.Models.PdfProcessing {
    public class PdfPageRangeModelBase : PdfModelBase, IValidatableObject {
        internal readonly List<object> ExportModes = new() {
            new { Text = "All Pages", Value = RangeExportMode.All },
            new { Text = "First Page Only", Value = RangeExportMode.First },
            new { Text = "Custom Page Range", Value = RangeExportMode.CustomRange }
        };
        internal bool IsPageRangeEnabled => ExportMode == RangeExportMode.CustomRange;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if(IsPageRangeEnabled) {
                if(string.IsNullOrWhiteSpace(PageRange)) {
                    yield return new ValidationResult(
                        "The range is required when Export Mode is set to 'Custom Page Range'.",
                        new[] { nameof(PageRange) });
                    yield break;
                }

                // Accept patterns like: 1,2,4-6, 10-12,15
                const string pattern = @"^\s*\d+(\s*-\s*\d+)?(\s*,\s*\d+(\s*-\s*\d+)?)*\s*$";
                if(!Regex.IsMatch(PageRange, pattern)) {
                    yield return new ValidationResult(
                        "The range must be a comma-separated list of numbers or ranges (e.g. 1,3,5-7; 10-12).",
                        new[] { nameof(PageRange) });
                }
            }
        }
        public RangeExportMode ExportMode { get; set; } = RangeExportMode.All;
        public string PageRange { get; set; } = string.Empty;
    }
}
