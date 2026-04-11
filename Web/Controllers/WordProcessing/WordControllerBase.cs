namespace BlazorDemo.OfficeFileApi.Controllers.WordProcessing {
    public class WordControllerBase : FileStreamControllerBase {
        public override string GetMimeContentType(string format) {
            return format.ToLowerInvariant() switch {
                "doc" => "application/msword",
                "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "docm" => "application/vnd.ms-word.document.macroEnabled.12",
                "dotx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.template",
                "dotm" => "application/vnd.ms-word.template.macroEnabled.12",
                "rtf" => "application/rtf",
                "html" => "text/html",
                "mht" => "message/rfc822",
                "txt" => "text/plain",
                "odt" => "application/vnd.oasis.opendocument.text",
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
