namespace BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.DocumentConversion {
    public class PresentationToPresentationConversionModel : PresentationModelBase {
        internal readonly List<FormatInfo> OutputFormats = new() {
            new FormatInfo() { Text = "PowerPoint Presentation (.pptx)", Value = "pptx" },
            new FormatInfo() { Text = "PowerPoint Macro-Enabled Presentation (.pptm)", Value = "pptm" },
            new FormatInfo() { Text = "PowerPoint Template (.potx)", Value = "potx" },
            new FormatInfo() { Text = "PowerPoint Macro-Enabled Template (.potm)", Value = "potm" }
        };
        public override string OutputName { get; set; } = "Converted_Presentation";
    }
}
