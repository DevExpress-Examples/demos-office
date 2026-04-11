using DevExpress.XtraRichEdit.API.Native;

namespace BlazorDemo.OfficeFileApi.Models.WordProcessing.MailMergeFields {
    public class WordMailMergeRegionsModel : WordFileModelBase {
        public override string OutputName { get; set; } = "MailMerge_Result";
        public int ActiveRecordIndex { get; set; } = 0;
        public MergeMode MergeMode { get; set; } = MergeMode.NewSection;
        public RecordExportMode RecordExportMode { get; set; } = RecordExportMode.AllRecords;
        public DocumentExportMode DocumentExportMode { get; set; } = DocumentExportMode.SingleFile;
        public override string OutputFormat { get; set; } = "pdf";
        public bool IsPdfOutput { get { return OutputFormat?.ToLowerInvariant() == "pdf"; } }
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
        internal readonly List<object> MergeModes = new()
        {
            new { Text = "Record Starts a New Section", Value = MergeMode.NewSection },
            new { Text = "Record Starts a New Paragraph", Value = MergeMode.NewParagraph }
        };
        internal readonly List<object> RecordExportModes = new()
        {
            new { Text = "Active Record", Value = RecordExportMode.ActiveRecord },
            new { Text = "All Records", Value = RecordExportMode.AllRecords }
        };
        internal readonly List<object> DocumentExportModes = new()
        {
            new { Text = "Separate Files", Value = DocumentExportMode.SeparateFiles },
            new { Text = "Single File", Value = DocumentExportMode.SingleFile }
        };
    }
    public enum RecordExportMode {
        AllRecords,
        ActiveRecord
    }
    public enum DocumentExportMode {
        SeparateFiles,
        SingleFile
    }
}
