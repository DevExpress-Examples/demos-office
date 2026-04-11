namespace BlazorDemo.OfficeFileApi.Controllers.PdfProcessing {
    public class PdfControllerBase : FileStreamControllerBase {
        public override string GetMimeContentType(string format) {
            return format.ToLowerInvariant() switch {
                "png" => "image/png",
                "jpeg" => "image/jpeg",
                "bmp" => "image/bmp",
                "tiff" => "image/tiff",
                "svg" => "image/svg+xml",
                "pdf" => "application/pdf",
                _ => "application/octet-stream"
            };
        }
    }
}
