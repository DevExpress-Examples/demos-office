#region Code
using DevExpress.Office.DigitalSignatures;
using System.Security.Cryptography.X509Certificates;
#endregion Code

using DevExpress.Spreadsheet;
using BlazorDemo.DemoData;
using BlazorDemo.OfficeFileApi.Controllers.Spreadsheet;
using BlazorDemo.OfficeFileApi.Models.Spreadsheet.DocumentProtection;
using BlazorDemo.OfficeFileApi.Services;
using DevExpress.Office.Tsp;
using Microsoft.AspNetCore.Mvc;

namespace BlazorDemo.OfficeFileApi.Controllers.SpreadsheetProcessing.DocumentProtection {
    [ApiController]
    [Route("api/spreadsheetprocessing")]
    public class ExcelDigitalSignatureController : SpreadsheetControllerBase {
        [HttpPost("digital-signature/sign")]
        public async Task<IActionResult> SignDocument(IFormFile file, [FromForm] ExcelDigitalSignatureModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            DocumentFormat outputFormatConverted = ExcelFormatHelper.GetDocumentFormat(model.OutputFormat);
            IConfiguration configuration = (IConfiguration)HttpContext.RequestServices.GetService(typeof(IConfiguration));
            string password = SignDemoUtils.GetSignDemoPassword(configuration);
            Stream outputStream = ApplyDigitalSignature(inputStream, model.TsaServer, DemoDocumentPaths.PdfSignCertificate, password, GetHashAlgorithm(model.HashAlgorithm), GetCommitmentType(model.Reason),
                model.Role, model.Country, model.City, model.State, model.Address1, model.Address2, model.PostalCode, model.Comments);

            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }
        HashAlgorithmType GetHashAlgorithm(string hashAlgorithm) {
            return hashAlgorithm switch {
                "SHA256" => HashAlgorithmType.SHA256,
                "SHA384" => HashAlgorithmType.SHA384,
                "SHA512" => HashAlgorithmType.SHA512,
                _ => HashAlgorithmType.SHA256
            };
        }
        CommitmentType GetCommitmentType(string reason) {
            return reason switch {
                "approved" => CommitmentType.ProofOfApproval,
                "created" => CommitmentType.ProofOfCreation,
                "created_approved" => CommitmentType.ProofOfOrigin,
                _ => CommitmentType.ProofOfApproval
            };
        }
        #region Code
        Stream ApplyDigitalSignature(Stream stream, string tsaServer, string certificateFile, string password, HashAlgorithmType hashAlgorithm,
            CommitmentType commitment, string role, string country, string city,
            string state, string address1, string address2, string postalCode, string comments) {
            var outputStream = new MemoryStream();
            var documentSigner = new DocumentSigner();

            var signatureOptions = CreateSignatureOptions(tsaServer, certificateFile, password, hashAlgorithm);

            var signatureInfo = CreateSignatureInfo(commitment, role, country, city, state, address1, address2, postalCode, comments);

            documentSigner.Sign(stream, outputStream, signatureOptions, signatureInfo);
            return outputStream;
        }

        SignatureOptions CreateSignatureOptions(string tsaServer, string certificateFile, string password, HashAlgorithmType hashAlgorithm) {
            var certificate = X509CertificateLoader.LoadPkcs12FromFile(certificateFile, password);
            var options = new SignatureOptions();
            options.Certificate = certificate;
            options.TsaClient = new TsaClient(new Uri(tsaServer), HashAlgorithmType.SHA256);
            var policy = new X509ChainPolicy();
            policy.RevocationMode = X509RevocationMode.NoCheck;
            policy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
            policy.VerificationFlags |= X509VerificationFlags.AllowUnknownCertificateAuthority | X509VerificationFlags.IgnoreCertificateAuthorityRevocationUnknown;
            options.CertificatePolicy = policy;
            options.TimestampCertificatePolicy = policy;
            options.SignatureFlags &= ~SignatureFlags.ValidateCertificate;
            options.CertificateKeyUsageFlags = X509KeyUsageFlags.None;
            options.DigestMethod = hashAlgorithm;
            return options;
        }

        SignatureInfo CreateSignatureInfo(CommitmentType commitment, string role, string country, string city,
            string state, string address1, string address2, string postalCode, string comments) {
            var signatureInfo = new SignatureInfo();
            signatureInfo.CommitmentType = commitment;
            signatureInfo.Time = DateTime.UtcNow;
            signatureInfo.ClaimedRoles.Clear();
            signatureInfo.ClaimedRoles.Add(role);
            signatureInfo.Country = country;
            signatureInfo.City = city;
            signatureInfo.StateOrProvince = state;
            signatureInfo.Address1 = address1;
            signatureInfo.Address2 = address2;
            signatureInfo.PostalCode = postalCode;
            signatureInfo.Comments = comments;
            return signatureInfo;
        }
        #endregion Code
    }
}
