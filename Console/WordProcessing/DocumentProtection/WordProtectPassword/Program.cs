using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

class Program {
    const string InputFilePath = @"Documents\DocumentForProtection.docx";
    const string OutputFileName = "Protected_Document.docx";
    const string Password = "demo-password";
    const DocumentProtectionType ProtectionType = DocumentProtectionType.ReadOnly;
    const string OutputFormat = "docx";

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var inputStream = File.OpenRead(inputPath);
            using var outputStream = ProtectDocumentWithPassword(inputStream, GetDocumentFormat(OutputFormat), Password, ProtectionType);
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

    static Stream ProtectDocumentWithPassword(Stream inputStream, DocumentFormat outputFormat, string password, DocumentProtectionType protectionType) {
        using var wordProcessor = new RichEditDocumentServer();

        wordProcessor.LoadDocument(inputStream);

        var document = wordProcessor.Document;

        if(document.IsDocumentProtected)
            document.Unprotect();

        if(protectionType == DocumentProtectionType.FillInForms) {
            foreach(Section section in document.Sections)
                section.ProtectedForForms = true;
        }

        document.Protect(password, protectionType);

        var outputStream = new MemoryStream();
        wordProcessor.SaveDocument(outputStream, outputFormat);
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
            "rtf" => DocumentFormat.Rtf,
            _ => DocumentFormat.Docx
        };
    }
}