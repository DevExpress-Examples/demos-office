namespace BlazorDemo.OfficeFileApi.Models.WordProcessing.MergeSplit {
    public class WordSplitDocumentModel : WordFileModelBase {
        public DocumentSplitMode SplitMode { get; set; } = DocumentSplitMode.BySection;
        public bool IsPdfOutput { get { return OutputFormat?.ToLowerInvariant() == "pdf"; } }
        public override string OutputFormat { get; set; } = "pdf";
        public override string OutputName { get; set; } = "Split_Document";
        internal readonly List<FormatInfo> OutputFormats = new()
        {
            new FormatInfo() { Text = "PDF Document (.pdf)", Value = "pdf" },
            new FormatInfo() { Text = "Word Document (.docx)", Value = "docx" },
            new FormatInfo() { Text = "Word 97-2003 Document (.doc)", Value = "doc" },
            new FormatInfo() { Text = "Word Macro-Enabled Document (.docm)", Value = "docm" },
            new FormatInfo() { Text = "Word Template (.dotx)", Value = "dotx" },
            new FormatInfo() { Text = "Word Macro-Enabled Template (.dotm)", Value = "dotm" },
            new FormatInfo() { Text = "Rich Text Format (.rtf)", Value = "rtf" }
        };
        internal readonly List<object> SplitModes = new()
        {
            new { Text = "By Section", Value = DocumentSplitMode.BySection },
            new { Text = "By Page", Value = DocumentSplitMode.ByPage },
            new { Text = "By Style (Heading 1)", Value = DocumentSplitMode.ByHeading }
        };
    }
    public enum DocumentSplitMode {
        BySection,
        ByPage,
        ByHeading
    }
}
