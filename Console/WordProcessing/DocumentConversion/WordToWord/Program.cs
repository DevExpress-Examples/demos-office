using DevExpress.XtraRichEdit;

class Program {
    const string InputFilePath = @"Documents\Sample.docx";
    const string OutputFileName = "ConvertedDocument.rtf";
    const string OutputFormat = "rtf";
    const bool EmbedFonts = false;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var inputStream = File.OpenRead(inputPath);
            using var outputStream = ConvertDocument(inputStream, EmbedFonts, GetDocumentFormat(OutputFormat));
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

    static Stream ConvertDocument(Stream inputStream, bool embedFonts, DocumentFormat outputFormat) {
        using var wordProcessor = new RichEditDocumentServer();

        wordProcessor.Options.Export.Html.EmbedImages = true;
        wordProcessor.Options.Export.Mht.EmbedImages = true;

        wordProcessor.LoadDocument(inputStream);
        wordProcessor.Document.EmbedFonts = embedFonts;

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
            "html" => DocumentFormat.Html,
            "mht" => DocumentFormat.Mht,
            "txt" => DocumentFormat.PlainText,
            "odt" => DocumentFormat.OpenDocument,
            _ => DocumentFormat.Docx
        };
    }
}