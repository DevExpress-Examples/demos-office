using DevExpress.Spreadsheet;

class Program {
    const string InputFilePath = @"Documents\InvestmentPortfolio.xlsx";
    const string OutputDirectoryName = "Split_Documents";
    static readonly DocumentFormat OutputFormat = DocumentFormat.Xlsx;
    const bool SplitVisibleOnly = true;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputDirectory = Path.Combine(AppContext.BaseDirectory, OutputDirectoryName);

            if(Directory.Exists(outputDirectory))
                Directory.Delete(outputDirectory, recursive: true);
            Directory.CreateDirectory(outputDirectory);

            using var inputStream = File.OpenRead(inputPath);
            var documentStreams = SplitWorkbook(SplitVisibleOnly, inputStream, OutputFormat);

            if(documentStreams.Count == 0)
                throw new InvalidOperationException("Failed to split workbook.");

            for(var index = 0; index < documentStreams.Count; index++) {
                var outputPath = Path.Combine(outputDirectory, $"worksheet_{index + 1}.xlsx");
                using var fileStream = File.Create(outputPath);
                using var outputStream = documentStreams[index];
                outputStream.CopyTo(fileStream);
            }

            Console.WriteLine($"Created {outputDirectory}");
            return 0;
        }
        catch(Exception exception) {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    static IReadOnlyList<Stream> SplitWorkbook(bool splitVisibleOnly, Stream inputStream, DocumentFormat outputFormat) {
        using var sourceWorkbook = new Workbook();

        sourceWorkbook.LoadDocument(inputStream);

        List<Worksheet> worksheetsToSplit;
        if(splitVisibleOnly)
            worksheetsToSplit = sourceWorkbook.Worksheets.Where(worksheet => worksheet.Visible).ToList();
        else
            worksheetsToSplit = sourceWorkbook.Worksheets.Where(worksheet => worksheet.VisibilityType != WorksheetVisibilityType.VeryHidden).ToList();

        var documentStreams = new List<Stream>();
        foreach(var sourceWorksheet in worksheetsToSplit) {
            using var targetWorkbook = new Workbook();
            var targetWorksheet = targetWorkbook.Worksheets[0];
            targetWorksheet.Name = sourceWorksheet.Name;
            targetWorksheet.CopyFrom(sourceWorksheet);
            targetWorksheet.VisibilityType = WorksheetVisibilityType.Visible;

            var outputStream = new MemoryStream();
            targetWorkbook.SaveDocument(outputStream, outputFormat);
            outputStream.Position = 0;
            documentStreams.Add(outputStream);
        }

        return documentStreams.AsReadOnly();
    }
}