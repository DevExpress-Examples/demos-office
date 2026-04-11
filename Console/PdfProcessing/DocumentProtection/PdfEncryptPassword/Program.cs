using DevExpress.Pdf;

internal static class Program {
    const string InputFilePath = @"Documents\PDF\Sample_Main.pdf";
    const string OutputFileName = "Document.pdf";
    const PdfEncryptionAlgorithm EncryptionAlgorithm = PdfEncryptionAlgorithm.AES256;
    const PdfDocumentDataExtractionPermissions DataExtractionPermissions = PdfDocumentDataExtractionPermissions.Allowed;
    const PdfDocumentInteractivityPermissions InteractivityPermissions = PdfDocumentInteractivityPermissions.Allowed;
    const PdfDocumentModificationPermissions ModificationPermissions = PdfDocumentModificationPermissions.Allowed;
    const PdfDocumentPrintingPermissions PrintingPermissions = PdfDocumentPrintingPermissions.Allowed;
    const string OwnerPassword = "demo-owner-password";
    const string UserPassword = "demo-user-password";

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var documentStream = File.OpenRead(inputPath);
            using var outputStream = Encrypt(
                documentStream,
                EncryptionAlgorithm,
                DataExtractionPermissions,
                InteractivityPermissions,
                ModificationPermissions,
                PrintingPermissions,
                OwnerPassword,
                UserPassword);
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

    static Stream Encrypt(
        Stream documentStream,
        PdfEncryptionAlgorithm algorithm,
        PdfDocumentDataExtractionPermissions dataExtractionPermissions,
        PdfDocumentInteractivityPermissions interactivityPermissions,
        PdfDocumentModificationPermissions modificationPermissions,
        PdfDocumentPrintingPermissions printingPermissions,
        string ownerPassword,
        string userPassword) {
        using var processor = new PdfDocumentProcessor();
        processor.LoadDocument(documentStream);

        var outputStream = new MemoryStream();
        processor.SaveDocument(
            outputStream,
            new PdfSaveOptions {
                EncryptionOptions = new PdfEncryptionOptions {
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
}