namespace BlazorDemo.OfficeFileApi.Models.PdfProcessing.DocumentProtection {
    public class PdfValidateSignatureModel : PdfModelBase {
        public override string OutputFormat { get => "txt"; }
        public bool ValidateCrlOcsp { get; set; } = true;
        public bool ValidateLtv { get; set; } = true;
    }
}
