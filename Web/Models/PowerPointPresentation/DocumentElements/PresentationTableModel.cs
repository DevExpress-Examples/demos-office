using System.ComponentModel.DataAnnotations;
using DevExpress.Docs.Presentation;

namespace BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.DocumentElements {
    public class PresentationTableModel : PresentationModelBase {
        // Table structure
        [Range(1, 50, ErrorMessage = "Number of rows must be between 1 and 50")]
        public int NumberOfRows { get; set; } = 5;

        [Range(1, 20, ErrorMessage = "Number of columns must be between 1 and 20")]
        public int NumberOfColumns { get; set; } = 4;

        [Range(0.1, 10.0, ErrorMessage = "Column width must be between 0.1 and 10.0 inches")]
        public float ColumnWidth { get; set; } = 1.5f; // inches
        public override string OutputName { get; set; } = "Table_Result";
        public TableStyleType TableStyle { get; set; } = TableStyleType.DarkStyle1Accent1;
        public IEnumerable<TableStyleOption> StyleOption { get; set; } = new List<TableStyleOption>() { TableStyleOption.HeaderRow, TableStyleOption.BandedRows };

        internal readonly List<object> TableStyles = new() {
            new { Text = "Dark Style 1 Accent 1", Value = TableStyleType.DarkStyle1Accent1 },
            new { Text = "Dark Style 2", Value = TableStyleType.DarkStyle2 },
            new { Text = "Light Style 1", Value = TableStyleType.LightStyle1 },
            new { Text = "Light Style 2", Value = TableStyleType.LightStyle2 },
            new { Text = "Light Style 3", Value = TableStyleType.LightStyle3 },
            new { Text = "Medium Style 1", Value = TableStyleType.MediumStyle1 },
            new { Text = "Medium Style 2", Value = TableStyleType.MediumStyle2 },
            new { Text = "Medium Style 3", Value = TableStyleType.MediumStyle3 }
        };
        internal readonly List<FormatInfo> OutputFormats = new() {
            new FormatInfo { Text = "PowerPoint Presentation (.pptx)", Value = "pptx" },
            new FormatInfo { Text = "PowerPoint Macro-Enabled Presentation (.pptm)", Value = "pptm" },
            new FormatInfo { Text = "PowerPoint Template (.potx)", Value = "potx" },
            new FormatInfo { Text = "PowerPoint Macro-Enabled Template (.potm)", Value = "potm" },
            new FormatInfo { Text = "PDF Document (.pdf)", Value = "pdf" }
        };
        internal readonly List<EnumWrapper<TableStyleOption>> StyleOptions = new() {
            new EnumWrapper<TableStyleOption>(TableStyleOption.HeaderRow, "Header Row"),
            new EnumWrapper<TableStyleOption>(TableStyleOption.BandedRows, "Banded Rows"),
            new EnumWrapper<TableStyleOption>(TableStyleOption.BandedColumns, "Banded Columns")
        };
        public bool IsPdfOutput => OutputFormat?.Equals("pdf", StringComparison.OrdinalIgnoreCase) == true;
    }
    public enum TableStyleOption {
        HeaderRow,
        BandedRows,
        BandedColumns
    }
}
