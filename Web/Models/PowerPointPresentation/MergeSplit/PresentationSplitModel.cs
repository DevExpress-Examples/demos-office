using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.MergeSplit
{
    public class PresentationSplitModel : PresentationModelBase, IValidatableObject
    {
        public override string OutputName { get; set; } = "Split_Presentation";
        public string SlideRange { get; set; } = string.Empty;
        public SlideExtractMode ExtractMode { get; set; } = SlideExtractMode.SeparateFiles;
        internal readonly List<object> ExtractModes = new()
        {
            new { Text = "As Separate Files", Value = SlideExtractMode.SeparateFiles },
            new { Text = "As Single File", Value = SlideExtractMode.SingleFile }
        };
        public bool IsPdfOutput { get { return OutputFormat?.ToLowerInvariant() == "pdf"; } }
        internal readonly List<FormatInfo> OutputFormats = new()
        {
            new FormatInfo { Text = "PDF Document (.pdf)", Value = "pdf" },
            new FormatInfo { Text = "PowerPoint Presentation (.pptx)", Value = "pptx" },
            new FormatInfo { Text = "PowerPoint Macro-Enabled Presentation (.pptm)", Value = "pptm" },
            new FormatInfo { Text = "PowerPoint Template (.potx)", Value = "potx" },
            new FormatInfo { Text = "PowerPoint Macro-Enabled Template (.potm)", Value = "potm" }
        };
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            const string pattern = @"^\s*\d+(\s*-\s*\d+)?(\s*,\s*\d+(\s*-\s*\d+)?)*\s*$";
            if (!string.IsNullOrWhiteSpace(SlideRange) && !Regex.IsMatch(SlideRange, pattern))
            {
                yield return new ValidationResult(
                    "SlideRange must be a comma-separated list of numbers or ranges (e.g. 1,3,5-7, 10-12).",
                    new[] { nameof(SlideRange) });
            }
        }
    }
    public enum SlideExtractMode
    {
        SeparateFiles,
        SingleFile
    }
}
