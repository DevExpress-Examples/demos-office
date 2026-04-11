using DevExpress.Spreadsheet;

class Program {
    const string InputFilePath = @"Documents\InvestmentPortfolio.xlsx";
    const string OutputFileName = "Encrypted_Document.xlsx";
    static readonly DocumentFormat OutputFormat = DocumentFormat.Xlsx;
    const string Password = "123";
    const EncryptionType EncryptionType = DevExpress.Spreadsheet.EncryptionType.Strong;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var inputStream = File.OpenRead(inputPath);
            using var outputStream = EncryptDocumentWithPassword(inputStream, OutputFormat, Password, EncryptionType);
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
        using var workbook = new Workbook();

        workbook.LoadDocument(inputStream);

        var encryptionSettings = new EncryptionSettings {
            Password = password,
            Type = encryptionType
        };

        var outputStream = new MemoryStream();
        workbook.SaveDocument(outputStream, outputFormat, encryptionSettings);
        outputStream.Position = 0;
        return outputStream;
    }
}