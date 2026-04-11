namespace BlazorDemo.OfficeFileApi.Services {
    static class ExcelCellFormatHelper {
        const string shortDateFormat = "mm-dd-yy";
        const string longDateFormat = @"[$-F800]dddd\, mmmm dd\, yyyy";

        const string general = "";
        const string number = "0.00";
        const string currency = "[$$-409]#,##0.00";
        const string accounting = "_([$$-409]* #,##0.00_);_([$$-409]* (#,##0.00);_([$$-409]* \"-\"??_);_(@_)";
        const string percentage = "0.00%";
        const string scientific = "0.00E+00";

        public static string GetDateFormatString(ExcelNumberFormat dateFormat) {
            return dateFormat switch {
                ExcelNumberFormat.LongDate => longDateFormat,
                ExcelNumberFormat.ShortDate => shortDateFormat,
                _ => shortDateFormat // Default value
            };
        }

        public static string GetNumberFormatString(ExcelNumberFormat numberFormat) {
            return numberFormat switch {
                ExcelNumberFormat.General => general,
                ExcelNumberFormat.Number => number,
                ExcelNumberFormat.Currency => currency,
                ExcelNumberFormat.Accounting => accounting,
                ExcelNumberFormat.Percentage => percentage,
                ExcelNumberFormat.Scientific => scientific,
                _ => general // Default value
            };
        }
    }

    public enum ExcelNumberFormat {
        General,

        Number,
        Currency,
        Accounting,
        Percentage,
        Scientific,

        ShortDate,
        LongDate
    }
}
