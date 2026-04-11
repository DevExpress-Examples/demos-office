using DevExpress.Office.DigitalSignatures;
using DevExpress.Office.Tsp;
using DevExpress.Spreadsheet;
using System.Security.Cryptography.X509Certificates;

class Program {
    const string InputFilePath = @"Documents\InvestmentPortfolio.xlsx";
    const string CertificatePath = @"Documents\PDF\Sample_Certificate.pfx";
    const string OutputFileName = "Signed_Document.xlsx";
    const string CertificatePassword = "dxdemo";
    const string TsaServer = "https://freetsa.org/tsr";
    const HashAlgorithmType HashAlgorithm = HashAlgorithmType.SHA256;
    const string Role = "Sales Representative";
    const string Comments = "Demo Digital Signature";
    const string Country = "USA";
    const string State = "WA";
    const string City = "Seattle";
    const string Address1 = "507 - 20th Ave. E.";
    const string Address2 = "Apt. 2A";
    const string PostalCode = "98122";
    static readonly CommitmentType Reason = CommitmentType.ProofOfApproval;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var certificatePath = Path.Combine(AppContext.BaseDirectory, CertificatePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var inputStream = File.OpenRead(inputPath);
            using var outputStream = ApplyDigitalSignature(
                inputStream,
                TsaServer,
                certificatePath,
                CertificatePassword,
                HashAlgorithm,
                Reason,
                Role,
                Country,
                City,
                State,
                Address1,
                Address2,
                PostalCode,
                Comments);
            using var fileStream = File.Create(outputPath);
            outputStream.CopyTo(fileStream);

            Console.WriteLine($"Created {outputPath}");
            return 0;
        }
        catch(Exception exception) {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    static Stream ApplyDigitalSignature(
        Stream stream,
        string tsaServer,
        string certificateFile,
        string password,
        HashAlgorithmType hashAlgorithm,
        CommitmentType commitment,
        string role,
        string country,
        string city,
        string state,
        string address1,
        string address2,
        string postalCode,
        string comments) {
        var outputStream = new MemoryStream();
        var documentSigner = new DocumentSigner();
        var signatureOptions = CreateSignatureOptions(tsaServer, certificateFile, password, hashAlgorithm);
        var signatureInfo = CreateSignatureInfo(commitment, role, country, city, state, address1, address2, postalCode, comments);
        documentSigner.Sign(stream, outputStream, signatureOptions, signatureInfo);
        outputStream.Position = 0;
        return outputStream;
    }

    static SignatureOptions CreateSignatureOptions(string tsaServer, string certificateFile, string password, HashAlgorithmType hashAlgorithm) {
        var certificate = X509CertificateLoader.LoadPkcs12FromFile(certificateFile, password);
        var options = new SignatureOptions {
            Certificate = certificate,
            TsaClient = new TsaClient(new Uri(tsaServer), HashAlgorithmType.SHA256),
            DigestMethod = hashAlgorithm,
            CertificateKeyUsageFlags = X509KeyUsageFlags.None
        };

        var policy = new X509ChainPolicy {
            RevocationMode = X509RevocationMode.NoCheck,
            RevocationFlag = X509RevocationFlag.ExcludeRoot
        };
        policy.VerificationFlags |= X509VerificationFlags.AllowUnknownCertificateAuthority | X509VerificationFlags.IgnoreCertificateAuthorityRevocationUnknown;

        options.CertificatePolicy = policy;
        options.TimestampCertificatePolicy = policy;
        options.SignatureFlags &= ~SignatureFlags.ValidateCertificate;

        return options;
    }

    static SignatureInfo CreateSignatureInfo(
        CommitmentType commitment,
        string role,
        string country,
        string city,
        string state,
        string address1,
        string address2,
        string postalCode,
        string comments) {
        var signatureInfo = new SignatureInfo {
            CommitmentType = commitment,
            Time = DateTime.UtcNow,
            Country = country,
            City = city,
            StateOrProvince = state,
            Address1 = address1,
            Address2 = address2,
            PostalCode = postalCode,
            Comments = comments
        };
        signatureInfo.ClaimedRoles.Clear();
        signatureInfo.ClaimedRoles.Add(role);
        return signatureInfo;
    }
}