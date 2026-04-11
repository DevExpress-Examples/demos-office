namespace BlazorDemo.OfficeFileApi.Models.Spreadsheet.DocumentConversion {
    public class ExcelToExcelConversionModel : ExcelFileModelBase {
        public override string OutputName { get; set; } = "ConvertedDocument";
        public override List<FormatInfo> OutputFormats { get; set; } = new()
        {
            new FormatInfo() { Text = "Excel Workbook (*.xlsx)", Value = "xlsx" },
            new FormatInfo() { Text = "Excel 97-2003 Workbook (*.xls)", Value = "xls" },
            new FormatInfo() { Text = "Excel Macro-Enabled Workbook (*.xlsm)", Value = "xlsm" },
            new FormatInfo() { Text = "Excel Template (*.xltx)", Value = "xltx" },
            new FormatInfo() { Text = "Excel 97-2003 Template (*.xlt)", Value = "xlt" },
            new FormatInfo() { Text = "Excel Macro-Enabled Template (*.xltm)", Value = "xltm" },
            new FormatInfo() { Text = "Excel Binary Workbook (*.xlsb)", Value = "xlsb" },
            new FormatInfo() { Text = "XML Spreadsheet 2003 (*.xml)", Value = "xml" },
            new FormatInfo() { Text = "CSV (Comma delimited) (*.csv)", Value = "csv" }
        };
    }
}
