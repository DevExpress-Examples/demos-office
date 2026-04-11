using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.OfficeFileApi.Models.Spreadsheet.MergeSplitDocuments {
    public class ExcelSplitWorkbooksModel : ExcelFileModelBase {
        public override string OutputName { get; set; } = "Split_Documents";
        [RegularExpression(@"^(VisibleOnly|All)$",
            ErrorMessage = "Invalid visibility mode. Supported modes: VisibleOnly, All")]
        public VisibilityMode SplitMode { get; set; } = VisibilityMode.VisibleOnly;

        internal readonly List<object> SplitModes = new() {
            new { Text = "Visible Only", Value = VisibilityMode.VisibleOnly },
            new { Text = "All", Value = VisibilityMode.All },
        };
    }

    public enum VisibilityMode {
        VisibleOnly,
        All
    }
}
