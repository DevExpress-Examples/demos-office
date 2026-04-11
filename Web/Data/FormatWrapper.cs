using Rich = DevExpress.XtraRichEdit;
using Spreadsheet = DevExpress.Spreadsheet;

namespace BlazorDemo.OfficeFileApi;

abstract class DocumentFormatWrapper {
    public string DisplayName { get; }
    public string Extension { get; }

    protected DocumentFormatWrapper(string displayName, string extension) {
        DisplayName = displayName;
        Extension = extension;
    }
}


sealed class RtfDocumentFormatWrapper: DocumentFormatWrapper {
    public Rich.DocumentFormat? Format { get; }

    public RtfDocumentFormatWrapper(string displayName, Rich.DocumentFormat? format, string extension)
        : base(displayName, extension) {
        Format = format;
    }
}

sealed class SpreadsheetDocumentFormatWrapper : DocumentFormatWrapper {
    public Spreadsheet.DocumentFormat? Format { get; }

    public SpreadsheetDocumentFormatWrapper(string displayName, Spreadsheet.DocumentFormat? format, string extension)
        : base(displayName, extension) {
        Format = format;
    }
}
