using DevExpress.Pdf;
using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.OfficeFileApi.Models.PdfProcessing.DocumentProtection {
    public class PdfEncryptPasswordModel : PdfModelBase {
        public PdfDocumentDataExtractionPermissions DocumentDataExtractionPermissions { get; set; } = PdfDocumentDataExtractionPermissions.Allowed;
        public List<object> DocumentDataExtractionPermissionsCollection { get; } = new() {
            new { Text = "Not Allowed", Value = PdfDocumentDataExtractionPermissions.NotAllowed },
            new { Text = "Only Accessibility", Value = PdfDocumentDataExtractionPermissions.Accessibility },
            new { Text = "Allowed", Value = PdfDocumentDataExtractionPermissions.Allowed }
        };
        public PdfDocumentInteractivityPermissions DocumentInteractivityPermissions { get; set; } = PdfDocumentInteractivityPermissions.Allowed;
        public List<object> DocumentInteractivityPermissionsCollection { get; } = new() {
            new { Text = "Not Allowed", Value = PdfDocumentInteractivityPermissions.NotAllowed },
            new { Text = "Only Form Filling and Signing", Value = PdfDocumentInteractivityPermissions.FormFillingAndSigning },
            new { Text = "Allowed", Value = PdfDocumentInteractivityPermissions.Allowed }
        };
        public PdfDocumentModificationPermissions DocumentModificationPermissions { get; set; } = PdfDocumentModificationPermissions.Allowed;
        public List<object> DocumentModificationPermissionsCollection { get; } = new() {
            new { Text = "Not Allowed", Value = PdfDocumentModificationPermissions.NotAllowed },
            new { Text = "Only Document Assembling", Value = PdfDocumentModificationPermissions.DocumentAssembling },
            new { Text = "Allowed", Value = PdfDocumentModificationPermissions.Allowed }
        };
        public PdfDocumentPrintingPermissions DocumentPrintingPermissions { get; set; } = PdfDocumentPrintingPermissions.Allowed;
        public List<object> DocumentPrintingPermissionsCollection { get; } = new() {
            new { Text = "Not Allowed", Value = PdfDocumentPrintingPermissions.NotAllowed },
            new { Text = "Only Low Quality", Value = PdfDocumentPrintingPermissions.LowQuality },
            new { Text = "Allowed", Value = PdfDocumentPrintingPermissions.Allowed },
        };
        [Required(ErrorMessage = "An encryption algorithm is required")]
        public PdfEncryptionAlgorithm EncryptionAlgorithm { get; set; } = PdfEncryptionAlgorithm.AES256;
        public List<object> EncryptionAlgorithms { get; } = new() {
            new { Text = "ARC4", Value = PdfEncryptionAlgorithm.ARC4 },
            new { Text = "AES128", Value = PdfEncryptionAlgorithm.AES128 },
            new { Text = "AES256", Value = PdfEncryptionAlgorithm.AES256}
        };
        [Required(ErrorMessage = "Password is required")]
        [MinLength(1, ErrorMessage = "Password cannot be empty")]
        public string OwnerPassword { get; set; }
        [MinLength(1, ErrorMessage = "Password cannot be empty")]
        public string UserPassword { get; set; }
    }
}
