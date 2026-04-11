namespace BlazorDemo.OfficeFileApi.Models.PdfProcessing {
    public class PdfModelBase : FileModelBase {
        public override string OutputFormat { get; set; } = "pdf";
        public override List<string> AllowedFileExtensions { get; set; } = new List<string> { ".pdf" };
    }
}
