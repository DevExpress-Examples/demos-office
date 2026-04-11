namespace BlazorDemo.OfficeFileApi.Models {
    public class FileModelBase {
        public virtual string OutputFormat { get; set; }
        public virtual string OutputName { get; set; } = "Document";
        public virtual List<string> AllowedFileExtensions { get; set; }
    }
}
