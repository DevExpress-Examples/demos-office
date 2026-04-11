using DevExpress.Office.DigitalSignatures;
using DevExpress.Office.Tsp;
using DevExpress.Pdf;

internal static class Program {
    const string InputFilePath = @"Documents\PDF\Sample_Alternative.pdf";
    const string CertificatePath = @"Documents\PDF\Sample_Certificate.pfx";
    const string SignatureBackgroundImagePath = @"Documents\PDF\Signature_Background.png";
    const string SignatureImagePath = @"Documents\DevExpress.png";
    const string OutputFileName = "Document.pdf";
    const string CertificatePassword = "dxdemo";
    const string TsaUrl = "https://www.freetsa.org/tsr";
    const HashAlgorithmType CertificateHashAlgorithm = HashAlgorithmType.SHA256;
    const HashAlgorithmType TsaHashAlgorithm = HashAlgorithmType.SHA256;
    const PdfSignatureAppearanceType AppearanceType = PdfSignatureAppearanceType.None;
    const string DateTimeFormat = "MM/dd/yyyy hh:mm";
    const bool RightToLeftTextDirection = false;
    const bool ShowDate = true;
    const bool ShowDistinguishedName = true;
    const bool ShowLabels = true;
    const bool ShowLocation = true;
    const bool ShowName = true;
    const bool ShowReason = true;
    const bool UseCustomAppearance = false;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var certificatePath = Path.Combine(AppContext.BaseDirectory, CertificatePath);
            var backgroundImagePath = Path.Combine(AppContext.BaseDirectory, SignatureBackgroundImagePath);
            var signatureImagePath = Path.Combine(AppContext.BaseDirectory, SignatureImagePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var documentStream = File.OpenRead(inputPath);
            using var certificateStream = File.OpenRead(certificatePath);
            using var signatureImageStream = File.Exists(signatureImagePath) ? File.OpenRead(signatureImagePath) : null;
            using var signatureBackgroundImageStream = UseCustomAppearance && File.Exists(backgroundImagePath) ? File.OpenRead(backgroundImagePath) : null;

            var signatureAppearance = UseCustomAppearance
                ? BuildSignatureAppearance(
                    AppearanceType,
                    DateTimeFormat,
                    RightToLeftTextDirection,
                    ShowDate,
                    ShowDistinguishedName,
                    ShowLabels,
                    ShowLocation,
                    ShowName,
                    ShowReason,
                    signatureBackgroundImageStream,
                    signatureImageStream)
                : null;

            using var outputStream = Sign(
                documentStream,
                certificateStream,
                signatureImageStream,
                CertificateHashAlgorithm,
                TsaHashAlgorithm,
                signatureAppearance);
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

    static PdfSignatureAppearance BuildSignatureAppearance(
        PdfSignatureAppearanceType signatureAppearanceType,
        string dateTimeFormat,
        bool rightToLeftTextDirection,
        bool showDate,
        bool showDistinguishedName,
        bool showLabels,
        bool showLocation,
        bool showName,
        bool showReason,
        Stream? signatureBackgroundImageStream = null,
        Stream? signatureImageStream = null) {
        var appearance = new PdfSignatureAppearance {
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

    static PdfSignatureFieldInfo CreateSignatureField(PdfDocumentProcessor processor) {
        var page = processor.Document.Pages.Last();
        var cropBox = page.CropBox;
        var signatureFieldBounds = new PdfRectangle(
            cropBox.Width / 3,
            cropBox.Height / 10,
            cropBox.Width / 3 * 2,
            cropBox.Height / 10 * 2);

        return new PdfSignatureFieldInfo(processor.Document.Pages.Count) {
            Name = "SignatureField",
            SignatureBounds = signatureFieldBounds
        };
    }

    static MemoryStream Sign(
        Stream documentStream,
        Stream certificateStream,
        Stream? signatureImageStream,
        HashAlgorithmType certificateHashAlgorithm,
        HashAlgorithmType tsaHashAlgorithm,
        PdfSignatureAppearance? signatureAppearance = null) {
        using var processor = new PdfDocumentProcessor();
        processor.LoadDocument(documentStream);
        var signatureFieldInfo = CreateSignatureField(processor);

        documentStream.Position = 0;
        certificateStream.Position = 0;
        if(signatureImageStream != null)
            signatureImageStream.Position = 0;

        using var documentSigner = new PdfDocumentSigner(documentStream);

        var tsaClient = new TsaClient(new Uri(TsaUrl, UriKind.Absolute), tsaHashAlgorithm);
        var pkcs7Signer = new Pkcs7Signer(certificateStream, CertificatePassword, certificateHashAlgorithm, tsaClient);

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
}