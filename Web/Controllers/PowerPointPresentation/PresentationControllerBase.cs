namespace BlazorDemo.OfficeFileApi.Controllers.PowerPointPresentation {
    public class PresentationControllerBase : FileStreamControllerBase {
        public override string GetMimeContentType(string format) {
            return format.ToLowerInvariant() switch {
                "pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                "pptm" => "application/vnd.ms-powerpoint.presentation.macroEnabled.12",
                "potx" => "application/vnd.openxmlformats-officedocument.presentationml.template",
                "potm" => "application/vnd.ms-powerpoint.template.macroEnabled.12",
                _ => "application/octet-stream"
            };
        }
    }
}
