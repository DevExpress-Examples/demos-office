using DevExpress.Docs.Presentation;

namespace BlazorDemo.OfficeFileApi
{
    public static class PresentationFormatHelper
    {
        public static DocumentFormat GetDocumentFormat(string format)
        {
            return format.ToLowerInvariant() switch
            {
                "potm" => DocumentFormat.Potm,
                "potx" => DocumentFormat.Potx,
                "pptm" => DocumentFormat.Pptm,
                "pptx" => DocumentFormat.Pptx,
                _ => DocumentFormat.Pptx
            };
        }
    }
}
