using DevExpress.XtraPrinting;

namespace BlazorDemo.OfficeFileApi.Services
{
    public static class PdfCompatibilityHelper
    {
        public static PdfACompatibility GetPdfAConformance(string pdfaCompatibility)
        {
            return pdfaCompatibility.ToLowerInvariant() switch
            {
                "pdf/a-1a" => PdfACompatibility.PdfA1a,
                "pdf/a-1b" => PdfACompatibility.PdfA1b,
                "pdf/a-2a" => PdfACompatibility.PdfA2a,
                "pdf/a-2b" => PdfACompatibility.PdfA2b,
                "pdf/a-3a" => PdfACompatibility.PdfA3a,
                "pdf/a-3b" => PdfACompatibility.PdfA3b,
                _ => PdfACompatibility.None
            };
        }

        public static PdfUACompatibility GetPdfUACompatibility(string pdfuaCompatibility)
        {
            return pdfuaCompatibility.ToLowerInvariant() switch
            {
                "pdf/ua1" => PdfUACompatibility.PdfUA1,
                _ => PdfUACompatibility.None
            };
        }
    }
}
