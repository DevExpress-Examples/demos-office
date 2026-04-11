using DevExpress.Office.DigitalSignatures;
using DevExpress.Pdf;

namespace BlazorDemo.OfficeFileApi.Models.PdfProcessing.DocumentProtection {
    public class PdfDigitalSignatureModel : PdfModelBase {
        public PdfSignatureAppearanceType AppearanceType { get; set; } = PdfSignatureAppearanceType.None;
        public string DateTimeFormat { get; set; } = "MM/dd/yyyy hh:mm";
        public List<object> HashAlgorithms { get; } = new() {
            new { Text = "SHA256", Value = HashAlgorithmType.SHA256},
            new { Text = "SHA384", Value = HashAlgorithmType.SHA384},
            new { Text = "SHA512", Value = HashAlgorithmType.SHA512}
        };
        public bool RightToLeftTextDirection { get; set; }
        public bool ShowDate { get; set; } = true;
        public bool ShowDistinguishedName { get; set; } = true;
        public bool ShowLabels { get; set; } = true;
        public bool ShowLocation { get; set; } = true;
        public bool ShowName { get; set; } = true;
        public bool ShowReason { get; set; } = true;
        public List<object> SignatureAppearanceTypes { get; } = new() {
            new { Text = "None", Value = PdfSignatureAppearanceType.None },
            new { Text = "Image", Value = PdfSignatureAppearanceType.Image },
            new { Text = "Name", Value = PdfSignatureAppearanceType.Name }
        };
        public HashAlgorithmType TsaHashAlgorithm { get; set; } = HashAlgorithmType.SHA256;
        public HashAlgorithmType CertificateHashAlgorithm { get; set; } = HashAlgorithmType.SHA256;
        public bool UseCustomAppearance { get; set; }
    }
}
