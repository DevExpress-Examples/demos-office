namespace BlazorDemo.OfficeFileApi.Models.Spreadsheet {
    public class ExcelFileModelBase : FileModelBase {
        public override string OutputFormat { get; set; } = "xlsx";
        public override List<string> AllowedFileExtensions { get; set; } =
            new List<string> { ".xls", ".xlsx", ".xlsm", ".xlsb", ".xltx", ".xltm", ".xlt", ".xml", ".csv", ".txt" };
        public virtual List<FormatInfo> OutputFormats { get; set; } =
            new() {
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
