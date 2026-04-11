using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.OfficeFileApi.Models.Spreadsheet.DocumentProtection {
    public class ExcelProtectWorkbookModel : ExcelFileModelBase {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(1, ErrorMessage = "Password cannot be empty")]
        public string WorkbookPassword { get; set; } = "123";

        [RegularExpression(@"^(xlsx|xls|xlsm|xltx|xlt|xltm|xlsb)$",
            ErrorMessage = "Invalid output format. Supported formats for protection: xlsx, xls, xlsm, xltx, xlt, xltm, xlsb")]
        public override string OutputFormat { get; set; } = "xlsx";
        public override string OutputName { get; set; } = "Protected_Document";
        public bool LockStructure { get; set; } = true;
        public bool LockWindows { get; set; } = true;

        public override List<FormatInfo> OutputFormats { get; set; } = new() {
            new FormatInfo { Text = "Excel Workbook (*.xlsx)", Value = "xlsx" },
            new FormatInfo { Text = "Excel 97-2003 Workbook (*.xls)", Value = "xls" },
            new FormatInfo { Text = "Excel Macro-Enabled Workbook (*.xlsm)", Value = "xlsm" },
            new FormatInfo { Text = "Excel Template (*.xltx)", Value = "xltx" },
            new FormatInfo { Text = "Excel 97-2003 Template (*.xlt)", Value = "xlt" },
            new FormatInfo { Text = "Excel Macro-Enabled Template (*.xltm)", Value = "xltm" },
            new FormatInfo { Text = "Excel Binary Workbook (*.xlsb)", Value = "xlsb" },
        };
    }
}
