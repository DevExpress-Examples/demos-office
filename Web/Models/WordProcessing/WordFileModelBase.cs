namespace BlazorDemo.OfficeFileApi.Models.WordProcessing {
    public class WordFileModelBase : FileModelBase {
        public override string OutputFormat { get; set; } = "docx";
        public override List<string> AllowedFileExtensions { get; set; } =
            new List<string> { ".doc", ".docx", ".docm", ".dotx", ".dotm", ".dot", ".rtf", ".txt", ".html", ".mht", ".odt", ".xml" };
    }
}
