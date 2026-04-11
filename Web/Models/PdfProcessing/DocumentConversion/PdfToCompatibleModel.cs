using DevExpress.Pdf;

namespace BlazorDemo.OfficeFileApi.Models.PdfProcessing.DocumentConversion {
    public class PdfToCompatibleModel : PdfModelBase {
        internal readonly List<FormatInfo> CompatibilityLevels = new() {
            new() { Text = "PDF/A-1b", Value = PdfCompatibility.PdfA1b },
            new() { Text = "PDF/A-2b", Value = PdfCompatibility.PdfA2b },
            new() { Text = "PDF/A-3b", Value = PdfCompatibility.PdfA3b }
        };
        public PdfCompatibility CompatibilityLevel { get; set; } = PdfCompatibility.PdfA2b;
    }
}
