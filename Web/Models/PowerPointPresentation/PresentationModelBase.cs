namespace BlazorDemo.OfficeFileApi.Models.PowerPointPresentation {
    public class PresentationModelBase : FileModelBase {
        public override string OutputFormat { get; set; } = "pptx";
        public override List<string> AllowedFileExtensions { get; set; } = new List<string> { ".pptx", ".pptm", ".potx", ".potm" };
    }
}
