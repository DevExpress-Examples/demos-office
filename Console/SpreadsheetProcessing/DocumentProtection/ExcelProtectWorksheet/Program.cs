using DevExpress.Spreadsheet;

class Program {
    const string InputFilePath = @"Documents\InvestmentPortfolio.xlsx";
    const string OutputFileName = "Protected_Document.xlsx";
    static readonly DocumentFormat OutputFormat = DocumentFormat.Xlsx;
    const string WorksheetPassword = "123";
    const bool ProtectActiveWorksheetOnly = false;
    static readonly WorksheetProtectionPermissions[] WorksheetProtectionPermissions = {
        DevExpress.Spreadsheet.WorksheetProtectionPermissions.SelectLockedCells,
        DevExpress.Spreadsheet.WorksheetProtectionPermissions.SelectUnlockedCells
    };

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var inputStream = File.OpenRead(inputPath);
            using var outputStream = ProtectWorksheetWithPassword(
                inputStream,
                OutputFormat,
                WorksheetPassword,
                ProtectActiveWorksheetOnly,
                WorksheetProtectionPermissions);
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

    static Stream ProtectWorksheetWithPassword(
        Stream inputStream,
        DocumentFormat outputFormat,
        string worksheetPassword,
        bool protectActiveWorksheetOnly,
        IEnumerable<WorksheetProtectionPermissions> worksheetProtectionPermissions) {
        using var workbook = new Workbook();

        workbook.LoadDocument(inputStream);

        WorksheetProtectionPermissions permissions = 0;
        foreach(var value in worksheetProtectionPermissions)
            permissions |= value;

        var worksheetsToProtect = new List<Worksheet>();
        if(protectActiveWorksheetOnly)
            worksheetsToProtect.Add(workbook.Worksheets.ActiveWorksheet);
        else
            worksheetsToProtect = workbook.Worksheets.ToList();

        foreach(var worksheet in worksheetsToProtect)
            worksheet.Protect(worksheetPassword ?? string.Empty, permissions);

        var outputStream = new MemoryStream();
        workbook.SaveDocument(outputStream, outputFormat);
        outputStream.Position = 0;
        return outputStream;
    }
}