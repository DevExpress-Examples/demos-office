using DevExpress.XtraPrinting;

namespace BlazorDemo.OfficeFileApi.Models.WordProcessing.DocumentConversion
{
    public class WordToPdfConversionModel : WordFileModelBase
    {
        public PdfACompatibility PdfA { get; set; } = PdfACompatibility.None;

        public PdfUACompatibility PdfUA { get; set; } = PdfUACompatibility.None;
        public string Title { get; set; } = "Custom Title";
        public string Author { get; set; } = "Custom Author";
        public string Subject { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public bool ExportControlsAsAcroFormFields { get; set; } = false;
        public override string OutputFormat { get; set; } = "pdf";
        public override string OutputName { get; set; } = "ConvertedDocument";
        public bool CanExportAsAcroFormFields => PdfA != PdfACompatibility.PdfA1a && PdfA != PdfACompatibility.PdfA2a && PdfA != PdfACompatibility.PdfA3a;

        public readonly List<FormatInfo> PdfAConformanceLevels = new()
    {
        new FormatInfo() { Text = "None", Value = PdfACompatibility.None },
        new FormatInfo() { Text = "PDF/A-1a", Value = PdfACompatibility.PdfA1a },
        new FormatInfo() { Text = "PDF/A-1b", Value = PdfACompatibility.PdfA1b },
        new FormatInfo() { Text = "PDF/A-2a", Value = PdfACompatibility.PdfA2a },
        new FormatInfo() { Text = "PDF/A-2b", Value = PdfACompatibility.PdfA2b },
        new FormatInfo() { Text = "PDF/A-3a", Value = PdfACompatibility.PdfA3a },
        new FormatInfo() { Text = "PDF/A-3b", Value = PdfACompatibility.PdfA3b }
    };

        public readonly List<object> PdfUACompatibilityLevels = new()
    {
        new { Text = "None", Value = PdfUACompatibility.None },
        new { Text = "PDF/UA-1", Value = PdfUACompatibility.PdfUA1 }
    };
    }
}
