using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.OfficeFileApi.Models.Spreadsheet.DocumentProtection {
    public class ExcelEncryptPasswordModel : ExcelFileModelBase {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(1, ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; } = "123";

        [RegularExpression(@"^(Compatible|Strong)$",
            ErrorMessage = "Invalid encryption type. Supported types: Compatible, Strong")]
        public string EncryptionType { get; set; } = "Strong";

        // Override OutputFormat validation to include encryption-compatible formats
        [RegularExpression(@"^(xlsx|xls|xlsm|xltx|xlt|xltm|xlsb)$",
            ErrorMessage = "Invalid output format. Supported formats for encryption: xlsx, xls, xlsm, xltx, xlt, xltm, xlsb")]
        public override string OutputFormat { get; set; } = "xlsx";
        public override string OutputName { get; set; } = "Encrypted_Document";

        internal readonly List<object> EncryptionTypes = new() {
            new { Text = "Compatible Encryption (RC4 128-bit)", Value = "Compatible" },
            new { Text = "Strong Encryption (AES 256-bit)", Value = "Strong" }
        };

        public override List<FormatInfo> OutputFormats { get; set; } = new() {
            new FormatInfo() { Text = "Excel Workbook (*.xlsx)", Value = "xlsx" },
            new FormatInfo() { Text = "Excel 97-2003 Workbook (*.xls)", Value = "xls" },
            new FormatInfo() { Text = "Excel Macro-Enabled Workbook (*.xlsm)", Value = "xlsm" },
            new FormatInfo() { Text = "Excel Template (*.xltx)", Value = "xltx" },
            new FormatInfo() { Text = "Excel 97-2003 Template (*.xlt)", Value = "xlt" },
            new FormatInfo() { Text = "Excel Macro-Enabled Template (*.xltm)", Value = "xltm" },
            new FormatInfo() { Text = "Excel Binary Workbook (*.xlsb)", Value = "xlsb" },
        };
    }
}
