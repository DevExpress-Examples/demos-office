namespace BlazorDemo.OfficeFileApi.Controllers.Spreadsheet {
    public class SpreadsheetControllerBase : FileStreamControllerBase {
        public override string GetMimeContentType(string format) {
            return format.ToLowerInvariant() switch {
                "xls" => "application/vnd.ms-excel",
                "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "xlsm" => "application/vnd.ms-excel.sheet.macroEnabled.12",
                "xlsb" => "application/vnd.ms-excel.sheet.binary.macroEnabled.12",
                "xltx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.template",
                "xltm" => "application/vnd.ms-excel.template.macroEnabled.12",
                "xlt" => "application/vnd.ms-excel",
                "xml" => "application/xml",
                "csv" => "text/csv",
                "txt" => "text/plain",
                "html" => "text/html",
                "png" => "image/png",
                "jpeg" => "image/jpeg",
                "bmp" => "image/bmp",
                "tiff" => "image/tiff",
                "svg" => "image/svg+xml",
                "pdf" => "application/pdf",
                _ => "application/octet-stream"
            };
        }
    }
}
