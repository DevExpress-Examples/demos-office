using DevExpress.XtraRichEdit;

namespace BlazorDemo.OfficeFileApi
{
    public static class WordFormatHelper
    {
        public static DocumentFormat GetDocumentFormat(string format)
        {
            return format.ToLowerInvariant() switch
            {
                "doc" => DocumentFormat.Doc,
                "docx" => DocumentFormat.Docx,
                "docm" => DocumentFormat.Docm,
                "dotx" => DocumentFormat.Dotx,
                "dotm" => DocumentFormat.Dotm,
                "rtf" => DocumentFormat.Rtf,
                "html" => DocumentFormat.Html,
                "mht" => DocumentFormat.Mht,
                "txt" => DocumentFormat.PlainText,
                "odt" => DocumentFormat.OpenDocument,
                _ => DocumentFormat.Docx // Default to Docx if unknown
            };
        }
        
    }
}
