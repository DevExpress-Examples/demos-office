using DevExpress.Spreadsheet;

class Program {
    const string InputFilePath = @"Documents\InvestmentPortfolio.xlsx";
    const string OutputFileName = "Protected_Document.xlsx";
    static readonly DocumentFormat OutputFormat = DocumentFormat.Xlsx;
    const string WorkbookPassword = "123";
    const bool LockStructure = true;
    const bool LockWindows = true;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var inputStream = File.OpenRead(inputPath);
            using var outputStream = ProtectWorkbookWithPassword(inputStream, OutputFormat, WorkbookPassword, LockStructure, LockWindows);
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

    static Stream ProtectWorkbookWithPassword(Stream inputStream, DocumentFormat outputFormat, string workbookPassword, bool lockStructure, bool lockWindows) {
        using var workbook = new Workbook();

        workbook.LoadDocument(inputStream);

        if(!workbook.IsProtected)
            workbook.Protect(workbookPassword ?? string.Empty, lockStructure, lockWindows);

        var outputStream = new MemoryStream();
        workbook.SaveDocument(outputStream, outputFormat);
        outputStream.Position = 0;
        return outputStream;
    }
}