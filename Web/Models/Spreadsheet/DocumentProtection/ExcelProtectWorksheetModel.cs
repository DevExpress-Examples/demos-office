using DevExpress.Spreadsheet;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.OfficeFileApi.Models.Spreadsheet.DocumentProtection {
    public class ExcelProtectWorksheetModel : ExcelFileModelBase {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(1, ErrorMessage = "Password cannot be empty")]
        public string WorksheetPassword { get; set; } = "123";

        public override string OutputName { get; set; } = "Protected_Document";

        [RegularExpression(@"^(AllSheets|ActiveSheet)$",
            ErrorMessage = "Invalid sheet protection mode. Supported modes: AllSheets, ActiveSheet")]
        public SheetProtectionMode SheetProtectionMode { get; set; } = SheetProtectionMode.AllSheets;

        internal readonly List<object> SheetProtectionModes = new() {
            new { Text = "All Sheets", Value = SheetProtectionMode.AllSheets },
            new { Text = "Active Sheet", Value = SheetProtectionMode.ActiveSheet }
        };

        public IEnumerable<WorksheetProtectionPermissions> SelectedWorksheetProtectionPermissions { get; set; } = new List<WorksheetProtectionPermissions>{
            WorksheetProtectionPermissions.SelectLockedCells,
            WorksheetProtectionPermissions.SelectUnlockedCells
        };
        internal ReadOnlyCollection<EnumWrapper<WorksheetProtectionPermissions>> WorksheetPermissions => new SpreadsheetProtectionOptionsService().WorksheetPermissions;
    }
    public enum SheetProtectionMode {
        AllSheets,
        ActiveSheet
    }
}
