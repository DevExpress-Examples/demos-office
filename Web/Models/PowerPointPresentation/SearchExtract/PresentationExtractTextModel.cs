using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.SearchExtract {
    public class PresentationExtractTextModel : PresentationModelBase, IValidatableObject {
        public override string OutputName { get; set; } = "Presentation_Text";
        public override string OutputFormat { get; set; } = ".txt";
        public RangeExportMode ImageExtractMode { get; set; } = RangeExportMode.First;
        public IEnumerable<SourceType> ExtractionSource { get; set; } = new List<SourceType>() { SourceType.Shapes, SourceType.Tables };
        public string SlideRange { get; set; } = string.Empty;
        internal readonly List<object> ImageExtractModes = new() {
            new { Text = "First Slide Only", Value = RangeExportMode.First },
            new { Text = "All Slides", Value = RangeExportMode.All },
            new { Text = "Custom Slide Range", Value = RangeExportMode.CustomRange }
        };
        internal readonly List<EnumWrapper<SourceType>> ExtractionSources = new() {
            new EnumWrapper<SourceType>(SourceType.Shapes, "Shapes"),
            new EnumWrapper<SourceType>(SourceType.Tables, "Tables"),
            new EnumWrapper<SourceType>(SourceType.SpeakerNotes, "Speaker Notes")
        };
        public bool IsSlideRangeEnabled => ImageExtractMode == RangeExportMode.CustomRange;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if(IsSlideRangeEnabled) {
                if(string.IsNullOrWhiteSpace(SlideRange)) {
                    yield return new ValidationResult(
                        "SlideRange is required when Export Mode is 'custom'.",
                        new[] { nameof(SlideRange) });
                    yield break;
                }

                // Accept patterns like: 1,2,4-6, 10-12,15
                const string pattern = @"^\s*\d+(\s*-\s*\d+)?(\s*,\s*\d+(\s*-\s*\d+)?)*\s*$";
                if(!Regex.IsMatch(SlideRange, pattern)) {
                    yield return new ValidationResult(
                        "SlideRange must be a comma-separated list of numbers or ranges (e.g. 1,3,5-7, 10-12).",
                        new[] { nameof(SlideRange) });
                }
            }
        }
    }
    public enum SourceType {
        Shapes,
        Tables,
        SpeakerNotes
    }
}
