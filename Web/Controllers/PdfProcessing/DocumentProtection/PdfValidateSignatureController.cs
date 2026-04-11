using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.PdfProcessing.DocumentProtection;
#region Code
using System.Text;
using DevExpress.Pdf;
#endregion

namespace BlazorDemo.OfficeFileApi.Controllers.PdfProcessing.DocumentProtection {
    [ApiController]
    [Route("api/pdfprocessing")]
    public class PdfValidateSignatureController : PdfControllerBase {
        [HttpPost("document-protection/validate")]
        public async Task<IActionResult> ValidatePdfSignatures(
            IFormFile file,
            [FromForm] PdfValidateSignatureModel model) {
            var documentStream = new MemoryStream();
            await file.CopyToAsync(documentStream);
            documentStream.Position = 0;

            var outputStream = ValidateSignatures(documentStream, model.ValidateCrlOcsp, model.ValidateLtv);
            outputStream.Position = 0;

            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }

        #region Code
        Stream ValidateSignatures(Stream documentStream, bool validateClrOcsp, bool validateLtv) {
            var outputStream = new MemoryStream();
            var streamWriter = new StreamWriter(outputStream);

            using var documentSigner = new PdfDocumentSigner(documentStream);

            var signatureInfo = documentSigner.GetSignatureInfo();

            streamWriter.WriteLine($"Found {signatureInfo.Count} signatures");
            streamWriter.WriteLine();

            foreach(var signature in signatureInfo)
                ValidateSignature(documentSigner, signature, signatureInfo.IndexOf(signature), validateClrOcsp, streamWriter);

            if(validateLtv)
                ValidateLtv(documentSigner, streamWriter);

            streamWriter.Flush();
            outputStream.Position = 0;
            return outputStream;
        }

        void ValidateSignature(PdfDocumentSigner documentSigner, PdfSignatureInfo signature, int signatureIndex, bool validateClrOcsp, StreamWriter streamWriter) {
            streamWriter.WriteLine($"Signature #{signatureIndex}");
            streamWriter.WriteLine($"===");
            streamWriter.WriteLine($"Field Name : {signature.FieldName}");
            streamWriter.WriteLine($"Signer name : {signature.SignerName}");

            var pkcs7 = documentSigner.GetPdfPkcs7Signature(signature.FieldName);
            streamWriter.WriteLine($"Valid : {pkcs7.VerifySignature()}");

            var certificate = pkcs7.GetSignatureCertificate();
            streamWriter.WriteLine($"Certificate Issuer : {certificate.IssuerName.Name}");
            streamWriter.WriteLine($"Certificate Valid : {certificate.Verify()}");

            var timestampDate = pkcs7.GetTimeStampDate();
            streamWriter.WriteLine($"Has Timestamp : {timestampDate.HasValue}");
            if(timestampDate.HasValue) {
                streamWriter.WriteLine($"Timestamp : {timestampDate}");
                streamWriter.WriteLine($"Timestamp Valid : {pkcs7.VerifyTimeStamp()}");
                streamWriter.WriteLine($"Document Timestamp : {pkcs7.IsDocumentTimeStamp}");
            }

            if(validateClrOcsp)
                ValidateClrOscp(pkcs7, streamWriter);

            streamWriter.WriteLine();
        }

        void ValidateClrOscp(PdfPkcs7Signature pkcs7, StreamWriter streamWriter) {
            streamWriter.WriteLine();
            streamWriter.WriteLine($"CRL/OCSP Information");
            streamWriter.WriteLine($"CRL Embedded : {pkcs7.CrlList != null}");
            streamWriter.WriteLine($"OCSP Embedded : {pkcs7.OcspList != null}");

            var crlOcspOptions = new PdfRevocationVerificationOptions() {
                TryFetchCrlOnline = pkcs7.CrlList == null,
                TryFetchOcspOnline = pkcs7.OcspList == null,
            };

            var crlOcspResult = pkcs7.CheckCertificateRevocation(DateTime.UtcNow, crlOcspOptions);

            streamWriter.WriteLine($"Certificate Revoked : {crlOcspResult.IsCrlRevoked}");
            if(crlOcspResult.IsCrlRevoked)
                streamWriter.WriteLine($"CRL Found Online : {crlOcspResult.IsCrlFoundOnline}");
            streamWriter.WriteLine($"OCSP Response Status : {crlOcspResult.OcspRevocationStatus}");
            if(crlOcspResult.OcspRevocationStatus != PdfOcspRevocationStatus.None)
                streamWriter.WriteLine($"OCSP found online : {crlOcspResult.IsOcspFoundOnline}");
        }

        void ValidateLtv(PdfDocumentSigner documentSigner, StreamWriter streamWriter) {
            streamWriter.WriteLine($"===");
            streamWriter.WriteLine($"LTV Information");
            streamWriter.WriteLine($"===");
            streamWriter.WriteLine();

            var ltvOptions = new PdfLtvOptions() {
                TryFetchCrlOnline = true,
                TryFetchOcspOnline = true,
                VerifyEntireCertificateChain = true,
            };

            var ltvResult = documentSigner.VerifyLtv(ltvOptions);
            foreach(var entry in ltvResult) {
                streamWriter.WriteLine($"Signature Name : {entry.SignatureFieldName}");
                streamWriter.WriteLine($"Certificates In Chain : {entry.CertificateRevocationResults.Count}");

                foreach(var revocation in entry.CertificateRevocationResults) {
                    streamWriter.WriteLine($"Certificate {revocation.Key.Subject}");
                    streamWriter.WriteLine($"Certificate revoked : {revocation.Value.IsCrlRevoked}");
                    if(revocation.Value.IsCrlRevoked)
                        streamWriter.WriteLine($"CRL found online : {revocation.Value.IsCrlFoundOnline}");
                    streamWriter.WriteLine($"OCSP Response Status : {revocation.Value.OcspRevocationStatus}");
                    if(revocation.Value.OcspRevocationStatus != PdfOcspRevocationStatus.None)
                        streamWriter.WriteLine($"OCSP found online : {revocation.Value.IsOcspFoundOnline}");
                    streamWriter.WriteLine();
                }
            }

            streamWriter.WriteLine();
        }
        #endregion
    }
}
