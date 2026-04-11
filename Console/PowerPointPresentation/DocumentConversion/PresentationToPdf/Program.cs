using DevExpress.Docs.Presentation;

class Program {
    const string InputFilePath = @"Documents\Sample.pptx";
    const string OutputFileName = "Converted_Presentation.pdf";

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var inputStream = File.OpenRead(inputPath);
            using var outputStream = ConvertToPdf(inputStream);
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

    static Stream ConvertToPdf(Stream inputStream) {
        using var presentation = new Presentation(inputStream);

        var outputStream = new MemoryStream();
        presentation.ExportToPdf(outputStream);
        outputStream.Position = 0;
        return outputStream;
    }
}