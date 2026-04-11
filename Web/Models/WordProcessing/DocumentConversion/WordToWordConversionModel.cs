namespace BlazorDemo.OfficeFileApi.Models.WordProcessing.DocumentConversion {
    public class WordToWordConversionModel : WordFileModelBase {
        internal readonly List<FormatInfo> OutputFormats = new()
        {
            new FormatInfo() { Text = "Word Document (.docx)", Value = "docx" },
            new FormatInfo() { Text = "Word 97-2003 Document (.doc)", Value = "doc" },
            new FormatInfo() { Text = "Word Macro-Enabled Document (.docm)", Value = "docm" },
            new FormatInfo() { Text = "Word Template (.dotx)", Value = "dotx" },
            new FormatInfo() { Text = "Word Macro-Enabled Template (.dotm)", Value = "dotm" },
            new FormatInfo() { Text = "Rich Text Format (.rtf)", Value = "rtf" },
            new FormatInfo() { Text = "Web Archive (.mht)", Value = "mht" },
            new FormatInfo() { Text = "Web Page (.html)", Value = "html" },
            new FormatInfo() { Text = "Plain Text (.txt)", Value = "txt" },
            new FormatInfo() { Text = "OpenDocument Text (.odt)", Value = "odt" }
        };
        public override string OutputName { get; set; } = "ConvertedDocument";
        public bool EmbedFonts { get; set; } = false;
        public bool IsFontEmbeddingSupported {
            get {
                return OutputFormat?.ToLowerInvariant() == "docx" || OutputFormat?.ToLowerInvariant() == "docm"
                    || OutputFormat?.ToLowerInvariant() == "dotx" || OutputFormat?.ToLowerInvariant() == "dotm"
                    || OutputFormat?.ToLowerInvariant() == "rtf" || OutputFormat?.ToLowerInvariant() == "doc";
            }
        }
    }
}
