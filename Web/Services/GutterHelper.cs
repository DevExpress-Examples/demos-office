using DevExpress.XtraRichEdit.API.Native;

namespace BlazorDemo.OfficeFileApi
{
    public static class GutterHelper
    {
        public static GutterPosition GetGutterPosition(string gutterPosition)
        {
            return gutterPosition.ToLowerInvariant() switch
            {
                "left" => GutterPosition.Left,
                "right" => GutterPosition.Right,
                "top" => GutterPosition.Left,
                _ => GutterPosition.Left
            };
        }
        public static bool IsGutterAtTop(string gutterPosition)
        {
            return gutterPosition.ToLowerInvariant() == "top";
        }
    }
}
