using DevExpress.Spreadsheet;

namespace BlazorDemo.OfficeFileApi.Services {
    public static class ExcelFormatHelper {
        public static DocumentFormat GetDocumentFormat(string format) {
            return format.ToLowerInvariant() switch {
                "xls" => DocumentFormat.Xls,
                "xlsx" => DocumentFormat.Xlsx,
                "xlt" => DocumentFormat.Xlt,
                "xlsm" => DocumentFormat.Xlsm,
                "xltx" => DocumentFormat.Xltx,
                "xltm" => DocumentFormat.Xltm,
                "xlsb" => DocumentFormat.Xlsb,
                "xml" => DocumentFormat.XmlSpreadsheet2003,
                "csv" => DocumentFormat.Csv,
                "txt" => DocumentFormat.Text,
                _ => DocumentFormat.Xlsx // Default to Xlsx if unknown
            };
        }
    }
}
