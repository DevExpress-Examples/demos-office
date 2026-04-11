using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.OfficeFileApi.Models.Spreadsheet.DocumentConversion {
    public class ExcelToHtmlConversionModel : ExcelFileModelBase {
        // Specify the sheet index to convert, defaulting to the first sheet (index 0)
        [Range(0, int.MaxValue, ErrorMessage = "SheetIndex must be 0 or greater.")]
        public int SheetIndex { get; set; } = 0;

        // Specify whether images should be exported to the resulting HTML document, true by default
        public bool ExportImages { get; set; } = true;

        // Override OutputFormat to always be HTML for this conversion type
        public override string OutputFormat { get; set; } = "html";

        public override string OutputName { get; set; } = "ConvertedSheet";
    }
}
