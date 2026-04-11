using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.PdfProcessing.DocumentProtection;
#region Code
using DevExpress.Pdf;
#endregion

namespace BlazorDemo.OfficeFileApi.Controllers.PdfProcessing.DocumentProtection {
    [ApiController]
    [Route("api/pdfprocessing")]
    public class PdfEncryptPasswordController : PdfControllerBase {
        [HttpPost("document-protection/encrypt-password")]
        public async Task<IActionResult> EncryptPdfWithPassword(
            IFormFile file,
            [FromForm] PdfEncryptPasswordModel model) {
            var documentStream = new MemoryStream();
            await file.CopyToAsync(documentStream);
            documentStream.Position = 0;

            var outputStream = Encrypt(
                documentStream,
                model.EncryptionAlgorithm,
                model.DocumentDataExtractionPermissions,
                model.DocumentInteractivityPermissions,
                model.DocumentModificationPermissions,
                model.DocumentPrintingPermissions,
                model.OwnerPassword,
                model.UserPassword);
            outputStream.Position = 0;

            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }

        #region Code
        Stream Encrypt(Stream documentStream, PdfEncryptionAlgorithm algorithm,
            PdfDocumentDataExtractionPermissions dataExtractionPermissions, PdfDocumentInteractivityPermissions interactivityPermissions,
            PdfDocumentModificationPermissions modificationPermissions, PdfDocumentPrintingPermissions printingPermissions,
            string ownerPassword, string userPassword) {
            using var processor = new PdfDocumentProcessor();

            processor.LoadDocument(documentStream);

            var outputStream = new MemoryStream();

            processor.SaveDocument(
                outputStream,
                new PdfSaveOptions() {
                    EncryptionOptions =
                        new PdfEncryptionOptions() {
                            Algorithm = algorithm,
                            DataExtractionPermissions = dataExtractionPermissions,
                            InteractivityPermissions = interactivityPermissions,
                            ModificationPermissions = modificationPermissions,
                            OwnerPasswordString = ownerPassword,
                            PrintingPermissions = printingPermissions,
                            UserPasswordString = userPassword
                        }
                });

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion
    }
}
