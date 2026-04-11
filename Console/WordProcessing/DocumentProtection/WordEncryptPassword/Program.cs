using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

class Program {
    const string InputFilePath = @"Documents\DocumentForProtection.docx";
    const string OutputFileName = "Encrypted_Document.docx";
    const string Password = "demo-password";
    const EncryptionType EncryptionType = DevExpress.XtraRichEdit.API.Native.EncryptionType.Strong;
    const string OutputFormat = "docx";

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var inputStream = File.OpenRead(inputPath);
            using var outputStream = EncryptDocumentWithPassword(inputStream, GetDocumentFormat(OutputFormat), Password, EncryptionType);
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

    static Stream EncryptDocumentWithPassword(Stream inputStream, DocumentFormat outputFormat, string password, EncryptionType encryptionType) {
        using var wordProcessor = new RichEditDocumentServer();

        wordProcessor.LoadDocument(inputStream);

        var encryptionSettings = new EncryptionSettings {
            Password = password,
            Type = encryptionType
        };

        var outputStream = new MemoryStream();
        wordProcessor.SaveDocument(outputStream, outputFormat, encryptionSettings);
        outputStream.Position = 0;
        return outputStream;
    }

    static DocumentFormat GetDocumentFormat(string format) {
        return format.ToLowerInvariant() switch {
            "doc" => DocumentFormat.Doc,
            "docx" => DocumentFormat.Docx,
            "docm" => DocumentFormat.Docm,
            "dotx" => DocumentFormat.Dotx,
            "dotm" => DocumentFormat.Dotm,
            _ => DocumentFormat.Docx
        };
    }
}