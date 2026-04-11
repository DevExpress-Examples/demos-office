using DevExpress.XtraRichEdit.API.Native;
using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.OfficeFileApi.Models.WordProcessing.DocumentProtection {
    public class WordProtectPasswordModel : WordFileModelBase {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(1, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; } = "";

        [RegularExpression(@"^(ReadOnly|AllowComments|TrackedChanges|FillInForms)$",
            ErrorMessage = "Invalid protection type. Supported types: ReadOnly, AllowComments, TrackedChanges, FillInForms")]
        public DocumentProtectionType ProtectionType { get; set; } = DocumentProtectionType.ReadOnly;

        [RegularExpression(@"^(docx|doc|docm|dotx|dotm|rtf)$",
            ErrorMessage = "Invalid output format. Supported formats for protection: docx, doc, docm, dotx, dotm, rtf")]
        public override string OutputFormat { get; set; } = "docx";
        public override string OutputName { get; set; } = "Protected_Document";

        internal readonly List<object> ProtectionTypes = new()
    {
        new { Text = "Read Only", Value = DocumentProtectionType.ReadOnly },
        new { Text = "Allow Comments", Value = DocumentProtectionType.AllowComments },
        new { Text = "Allow Tracked Changes", Value = DocumentProtectionType.TrackedChanges },
        new { Text = "Allow Fill in Forms", Value = DocumentProtectionType.FillInForms }
    };

        internal readonly List<FormatInfo> OutputFormats = new()
    {
        new FormatInfo() { Text = "Word Document (.docx)", Value = "docx" },
        new FormatInfo() { Text = "Word 97-2003 Document (.doc)", Value = "doc" },
        new FormatInfo() { Text = "Word Macro-Enabled Document (.docm)", Value = "docm" },
        new FormatInfo() { Text = "Word Template (.dotx)", Value = "dotx" },
        new FormatInfo() { Text = "Word Macro-Enabled Template (.dotm)", Value = "dotm" },
        new FormatInfo() { Text = "Rich Text Format (.rtf)", Value = "rtf" }
    };
    }
}
