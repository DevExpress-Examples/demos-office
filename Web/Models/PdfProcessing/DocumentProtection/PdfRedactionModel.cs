using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.OfficeFileApi.Models.PdfProcessing.DocumentProtection {
    public class PdfRedactionModel : PdfModelBase {
        public bool ApplyRedaction { get; set; } = true;
        [Required(ErrorMessage = "Add a search term for sensitive data")]
        public string SearchText { get; set; } = "DevExpress";
        public string RedactionText { get; set; } = "REDACTED";
    }
}
