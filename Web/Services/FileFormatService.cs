using DevExpress.XtraPrinting;
using Rich = DevExpress.XtraRichEdit;
using Spreadsheet = DevExpress.Spreadsheet;

namespace BlazorDemo.OfficeFileApi;

sealed class FileFormatService {
    readonly List<RtfDocumentFormatWrapper> rtfFormatWrappers = new() {
        new ("Rich Text Format (*.rtf)", Rich.DocumentFormat.Rtf, "rtf"),
        new ("Word 97-2003 Document (*.doc)", Rich.DocumentFormat.Doc, "doc"),
        new ("Word Document (*.docx)", Rich.DocumentFormat.OpenXml, "docx"),
        new ("Portable Document Format (*.pdf)", null, "pdf"),
        new ("Text Files (*.txt)", Rich.DocumentFormat.PlainText, "txt"),
        new ("Web Archive, single file (*.mht)", Rich.DocumentFormat.Mht, "mht"),
        new ("OpenDocument Text Document (*.odt)", Rich.DocumentFormat.OpenDocument, "odt"),
        new ("Word XML Document (*.xml)", Rich.DocumentFormat.WordML, "xml"),
        new ("Electronic Publication (*.epub)", Rich.DocumentFormat.ePub, "epub"),
        new ("HTML Page (*.html)", Rich.DocumentFormat.Html, "html")
    };
    readonly List<SpreadsheetDocumentFormatWrapper> spreadSheetFormatWrappers = new() {
        new ("Excel Workbook (*.xlsx)", Spreadsheet.DocumentFormat.Xlsx, "xlsx"),
        new ("Excel Macro-Enabled Workbook (*.xlsm)", Spreadsheet.DocumentFormat.Xlsm, "xlsm"),
        new ("Excel Binary Workbook (*.xlsb)", Spreadsheet.DocumentFormat.Xlsb, "xlsb"),
        new ("Excel 97-2003 Workbook (*.xls)", Spreadsheet.DocumentFormat.Xls, "xls"),
        new ("Excel Template (*.xltx)", Spreadsheet.DocumentFormat.Xltx, "xltx"),
        new ("Excel Macro-Enabled Template (*.xltm)", Spreadsheet.DocumentFormat.Xltm, "xltm"),
        new ("Excel 97-2003 Template (*.xlt)", Spreadsheet.DocumentFormat.Xlt, "xlt"),
        new ("XML Spreadsheet 2003 (*.xml)", Spreadsheet.DocumentFormat.XmlSpreadsheet2003, "xml"),
        new ("CSV (Comma delimited) (*.csv)", Spreadsheet.DocumentFormat.Csv, "csv"),
        new ("Text (Tab delimited) (*.txt)", Spreadsheet.DocumentFormat.Text, "txt"),
        new ("Portable Document Format (*.pdf)", null, "pdf"),
        new ("HTML Page (*.html)", null, "html"),
    };
    readonly List<PdfAccessibilityModeWrapper> accessibilityModeWrappers = new() {
        new ("PDF/A-1a", PdfACompatibility.PdfA1a),
        new ("PDF/A-2a", PdfACompatibility.PdfA2a),
        new ("PDF/A-3a", PdfACompatibility.PdfA3a),
        new ("PDF/UA", PdfUACompatibility.PdfUA1),
    };

    public IEnumerable<RtfDocumentFormatWrapper> RtfFormatWrappers => rtfFormatWrappers.AsReadOnly();
    public RtfDocumentFormatWrapper DefaultRtfFormatWrapper => rtfFormatWrappers[3];
    public IEnumerable<SpreadsheetDocumentFormatWrapper> SpreadsheetFormatWrappers => spreadSheetFormatWrappers.AsReadOnly();
    public SpreadsheetDocumentFormatWrapper Pdf => spreadSheetFormatWrappers[10];
    public SpreadsheetDocumentFormatWrapper Xlsx => spreadSheetFormatWrappers[0];
    public IEnumerable<PdfAccessibilityModeWrapper> AccessibilityModeWrappers => accessibilityModeWrappers.AsReadOnly();
    public PdfAccessibilityModeWrapper DefaultAccessibilityModeWrapper => accessibilityModeWrappers[2];
    public string SpreadsheetFileExtensions => ".xlsx, .xlsm, .xlsb, .xls, .xltx, .xltm, .xlt, .xml, .csv, .txt";
    public string RtfFileExtensions => ".rtf, .doc, .docx, .txt, .mht, .odt, .xml, .epub, .htm, .html";
}
