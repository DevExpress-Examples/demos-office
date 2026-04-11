namespace BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.DocumentConversion {
    public class PresentationToPdfConversionModel : PresentationModelBase {
        public override string OutputFormat { get; set; } = "pdf";
        public override string OutputName { get; set; } = "Converted_Presentation";
    }
}
