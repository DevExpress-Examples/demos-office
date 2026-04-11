namespace BlazorDemo.OfficeFileApi.Models.PdfProcessing.DocumentProtection {
    public class PdfApplyWatermarkModel : PdfModelBase {
        public string WatermarkText { get; set; } = "WATERMARK";
        public bool Rotate { get; set; } = false;
    }
}
