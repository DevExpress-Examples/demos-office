using DevExpress.Drawing;
using DevExpress.Spreadsheet;

namespace BlazorDemo.OfficeFileApi.Services
{
    public static class ImageFormatHelper
    {
        public static DXImageFormat GetDXImageFormat(string format)
        {
            return format.ToLowerInvariant() switch
            {
                "png" => DXImageFormat.Png,
                "jpeg" => DXImageFormat.Jpeg,
                "bmp" => DXImageFormat.Bmp,
                "tiff" => DXImageFormat.Tiff,
                "svg" => DXImageFormat.Svg,
                _ => DXImageFormat.Png
            };
        }
        public static ImageFileFormat GetImageFileFormat(string format) 
        {
            return format.ToLowerInvariant() switch 
            {
                "png" => ImageFileFormat.Png,
                "jpeg" => ImageFileFormat.Jpeg,
                "bmp" => ImageFileFormat.Bmp,
                "tiff" => ImageFileFormat.Tiff,
                "svg" => ImageFileFormat.Svg,
                _ => ImageFileFormat.Png
            };
        }
    }
}
