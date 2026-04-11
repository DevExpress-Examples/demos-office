using DevExpress.Spreadsheet;
using DevExpress.XtraPrinting;

class Program {
    const string InputFilePath = @"Documents\InvestmentPortfolio.xlsx";
    const string OutputFileName = "ConvertedDocument.pdf";
    const PdfACompatibility PdfA = PdfACompatibility.None;
    const PdfUACompatibility PdfUA = PdfUACompatibility.None;
    const string Title = "Custom Title";
    const string Author = "Custom Author";
    const string Subject = "";
    const string Keywords = "";

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var inputStream = File.OpenRead(inputPath);
            using var outputStream = ConvertToPdf(inputStream, PdfA, PdfUA, Author, Title, Subject, Keywords);
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

    static Stream ConvertToPdf(
        Stream inputStream,
        PdfACompatibility pdfAConformance,
        PdfUACompatibility pdfUACompatibility,
        string author,
        string title,
        string subject,
        string keywords) {
        using var workbook = new Workbook();

        workbook.LoadDocument(inputStream);

        var outputStream = new MemoryStream();
        var exportOptions = new PdfExportOptions {
            PdfACompatibility = pdfAConformance,
            PdfUACompatibility = pdfUACompatibility
        };
        exportOptions.DocumentOptions.Author = author;
        exportOptions.DocumentOptions.Title = title;
        exportOptions.DocumentOptions.Subject = subject;
        exportOptions.DocumentOptions.Keywords = keywords;

        workbook.ExportToPdf(outputStream, exportOptions);
        outputStream.Position = 0;
        return outputStream;
    }
}