using DevExpress.XtraPrinting;

namespace BlazorDemo.OfficeFileApi;

sealed class PdfAccessibilityModeWrapper {
    public string DisplayName { get; }
    public PdfACompatibility? PdfACompatibility { get; }
    public PdfUACompatibility? PdfUACompatibility { get; }

    public PdfAccessibilityModeWrapper(string displayName, PdfACompatibility? pdfACompatibility) {
        DisplayName = displayName;
        PdfACompatibility = pdfACompatibility;
    }
    public PdfAccessibilityModeWrapper(string displayName, PdfUACompatibility? pdfUACompatibility) {
        DisplayName = displayName;
        PdfUACompatibility = pdfUACompatibility;
    }
}

