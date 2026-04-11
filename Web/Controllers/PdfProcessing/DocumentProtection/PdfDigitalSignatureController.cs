using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.PdfProcessing.DocumentProtection;
#region Code
using DevExpress.Pdf;
using DevExpress.Office.DigitalSignatures;
using DevExpress.Office.Tsp;
#endregion
namespace BlazorDemo.OfficeFileApi.Controllers.PdfProcessing.DocumentProtection {
    [ApiController]
    [Route("api/pdfprocessing")]
    public class PdfDigitalSignatureController : PdfControllerBase {
        [HttpPost("document-protection/sign")]
        public async Task<IActionResult> SignPdfWithCertificate(
            IFormFile file,
            IFormFile certificate,
            IFormFile signatureBackgroundImage,
            IFormFile signatureImage,
            [FromForm] PdfDigitalSignatureModel model) {
            PdfSignatureAppearance signatureAppearance = null;
            MemoryStream signatureAppearanceImageStream = null;

            if(signatureImage != null) {
                signatureAppearanceImageStream = new MemoryStream();
                await signatureImage.CopyToAsync(signatureAppearanceImageStream);
                signatureAppearanceImageStream.Position = 0;
            }

            if(model.UseCustomAppearance) {
                MemoryStream signatureAppearanceBackgroundImageStream = null;

                if(signatureBackgroundImage != null) {
                    signatureAppearanceBackgroundImageStream = new MemoryStream();
                    await signatureBackgroundImage.CopyToAsync(signatureAppearanceBackgroundImageStream);
                    signatureAppearanceBackgroundImageStream.Position = 0;
                }

                signatureAppearance = await BuildSignatureAppearance(
                    model.AppearanceType,
                    model.DateTimeFormat,
                    model.RightToLeftTextDirection,
                    model.ShowDate,
                    model.ShowDistinguishedName,
                    model.ShowLabels,
                    model.ShowLocation,
                    model.ShowName,
                    model.ShowReason,
                    signatureAppearanceBackgroundImageStream,
                    signatureAppearanceImageStream);
            }

            var documentStream = new MemoryStream();
            await file.CopyToAsync(documentStream);
            documentStream.Position = 0;

            var certificateStream = new MemoryStream();
            await certificate.CopyToAsync(certificateStream);
            certificateStream.Position = 0;

            var outputStream = await Sign(documentStream, certificateStream, signatureAppearanceImageStream, model.CertificateHashAlgorithm, model.TsaHashAlgorithm, signatureAppearance);
            outputStream.Position = 0;

            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }

        #region Code
        async Task<PdfSignatureAppearance> BuildSignatureAppearance(PdfSignatureAppearanceType signatureAppearanceType, string dateTimeFormat,
            bool rightToLeftTextDirection, bool showDate, bool showDistinguishedName, bool showLabels, bool showLocation, bool showName,
            bool showReason, Stream signatureBackgroundImageStream = null, Stream signatureImageStream = null) {
            var appearance = new PdfSignatureAppearance() {
                AppearanceType = signatureAppearanceType,
                ApplicationVersion = "v1.0",
                DateTimeFormat = dateTimeFormat,
                RightToLeftTextDirection = rightToLeftTextDirection,
                ShowDate = showDate,
                ShowDistinguishedName = showDistinguishedName,
                ShowLabels = showLabels,
                ShowLocation = showLocation,
                ShowName = showName,
                ShowReason = showReason,
                SignatureDetailsFont = new PdfSignatureFontSettings("Calibri"),
                SignatureNameFont = new PdfSignatureFontSettings("Calibri")
            };

            if(signatureBackgroundImageStream != null)
                appearance.SetBackgroundImageData(signatureBackgroundImageStream);
            if(signatureAppearanceType == PdfSignatureAppearanceType.Image && signatureImageStream != null)
                appearance.SetImageData(signatureImageStream);

            return appearance;
        }

        async Task<PdfSignatureFieldInfo> CreateSignatureField(PdfDocumentProcessor processor) {
            var page = processor.Document.Pages.Last();

            var cropBox = page.CropBox;
            var signatureFieldBounds = new PdfRectangle(
                cropBox.Width / 3,
                cropBox.Height / 10,
                cropBox.Width / 3 * 2,
                cropBox.Height / 10 * 2);

            var signatureFieldInfo = new PdfSignatureFieldInfo(processor.Document.Pages.Count);
            signatureFieldInfo.Name = "SignatureField";
            signatureFieldInfo.SignatureBounds = signatureFieldBounds;

            return signatureFieldInfo;
        }

        async Task<MemoryStream> Sign(Stream documentStream, Stream certificateStream, Stream signatureImageStream,
            HashAlgorithmType certificateHashAlgorithm, HashAlgorithmType tsaHashAlgorithm, PdfSignatureAppearance signatureAppearance = null) {
            using var processor = new PdfDocumentProcessor();

            processor.LoadDocument(documentStream);
            var signatureFieldInfo = await CreateSignatureField(processor);

            using var documentSigner = new PdfDocumentSigner(documentStream);

            var tsaClient = new TsaClient(
                new Uri("https://www.freetsa.org/tsr", UriKind.Absolute),
                tsaHashAlgorithm);
            var pkcs7Signer = new Pkcs7Signer(certificateStream, "dxdemo", certificateHashAlgorithm, tsaClient);

            var signatureBuilder = new PdfSignatureBuilder(pkcs7Signer, signatureFieldInfo) {
                ApplicationName = "DevExpress Office File API Demo",
                CertificationLevel = PdfCertificationLevel.NoCertification,
                ContactInfo = "support@devexpress.com",
                Location = "Area 51",
                Name = "John Doe",
                Reason = "Demo",
                SigningTime = DateTime.UtcNow
            };

            if(signatureAppearance != null)
                signatureBuilder.SetSignatureAppearance(signatureAppearance);
            else if(signatureImageStream != null)
                signatureBuilder.SetImageData(signatureImageStream);

            var outputStream = new MemoryStream();
            documentSigner.SaveDocument(outputStream, signatureBuilder);
            outputStream.Position = 0;

            return outputStream;
        }
        #endregion
    }
}
