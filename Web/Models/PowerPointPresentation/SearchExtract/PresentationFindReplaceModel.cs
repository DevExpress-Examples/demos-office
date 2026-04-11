using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.SearchExtract {
    public class PresentationFindReplaceModel : PresentationModelBase, IValidatableObject {
        [Required(ErrorMessage = "Search text is required")]
        public string SearchText { get; set; } = "";

        public string ReplaceText { get; set; } = "";

        [Required(ErrorMessage = "Operation type is required")]
        public FindReplaceActionType Operation { get; set; } = FindReplaceActionType.Replace;

        public string HighlightColor { get; set; }
        internal object Color { get; set; } = "#FFFF00"; // Default yellow color

        // Override to support more file types for find/replace operations
        public override string OutputName { get; set; } = "Modified_Presentation";

        internal readonly List<object> OperationTypes = new() {
            new { Text = "Replace Text", Value = FindReplaceActionType.Replace },
            new { Text = "Remove Text", Value = FindReplaceActionType.Remove },
            new { Text = "Highlight Text", Value = FindReplaceActionType.Highlight }
        };
        internal readonly List<FormatInfo> OutputFormats = new() {
            new FormatInfo { Text = "PowerPoint Presentation (.pptx)", Value = "pptx" },
            new FormatInfo { Text = "PowerPoint Macro-Enabled Presentation (.pptm)", Value = "pptm" },
            new FormatInfo { Text = "PowerPoint Template (.potx)", Value = "potx" },
            new FormatInfo { Text = "PowerPoint Macro-Enabled Template (.potm)", Value = "potm" }
        };
        internal bool IsReplaceOperation => Operation == FindReplaceActionType.Replace;
        internal bool IsHighlightOperation => Operation == FindReplaceActionType.Highlight;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            var results = new List<ValidationResult>();

            // Validate based on operation type
            switch(Operation) {
                case FindReplaceActionType.Replace:
                    if(string.IsNullOrWhiteSpace(ReplaceText)) {
                        results.Add(new ValidationResult(
                            "Replace text is required for replace operation",
                            new[] { nameof(ReplaceText) }));
                    }
                    break;

                case FindReplaceActionType.Highlight:
                    if(string.IsNullOrWhiteSpace(Color.ToString())) {
                        results.Add(new ValidationResult(
                            "Highlight color is required for highlight operation",
                            new[] { nameof(HighlightColor) }));
                    }
                    break;
            }

            return results;
        }
    }
}
