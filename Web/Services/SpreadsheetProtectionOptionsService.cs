using System.Collections.ObjectModel;
using DevExpress.Spreadsheet;

namespace BlazorDemo.OfficeFileApi {
    sealed class SpreadsheetProtectionOptionsService {

        readonly List<EnumWrapper<WorksheetProtectionPermissions>> worksheetPermissions = new() {
            new(WorksheetProtectionPermissions.AutoFilters, "Use Auto Filter"),
            new(WorksheetProtectionPermissions.DeleteColumns, "Delete columns"),
            new(WorksheetProtectionPermissions.DeleteRows, "Delete rows"),
            new(WorksheetProtectionPermissions.FormatCells, "Format cells"),
            new(WorksheetProtectionPermissions.FormatColumns, "Format columns"),
            new(WorksheetProtectionPermissions.FormatRows, "Format rows"),
            new(WorksheetProtectionPermissions.InsertColumns, "Insert columns"),
            new(WorksheetProtectionPermissions.InsertHyperlinks, "Insert hyperlinks"),
            new(WorksheetProtectionPermissions.InsertRows, "Insert rows"),
            new(WorksheetProtectionPermissions.Objects, "Edit objects"),
            new(WorksheetProtectionPermissions.PivotTables, "Use Pivot Table reports"),
            new(WorksheetProtectionPermissions.Scenarios, "Edit scenarios"),
            new(WorksheetProtectionPermissions.SelectLockedCells, "Select locked cells"),
            new(WorksheetProtectionPermissions.SelectUnlockedCells, "Select unlocked cells"),
            new(WorksheetProtectionPermissions.Sort, "Sort"),
        };

        public ReadOnlyCollection<EnumWrapper<WorksheetProtectionPermissions>> WorksheetPermissions => worksheetPermissions.AsReadOnly();

    }
}
