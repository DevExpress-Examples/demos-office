using System.Text.Json;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

class Program {
    const string InputFilePath = @"Documents\MailMergeMasterDetail_Template.docx";
    const string DataSourcePath = @"Documents\nwind_data.json";
    const string OutputFileName = "MailMerge_Result.pdf";
    const string OutputDirectoryName = "MailMerge_Result";
    const string OutputFormat = "pdf";
    const bool IsPdfOutput = true;
    const MergeMode MergeMode = DevExpress.XtraRichEdit.API.Native.MergeMode.NewSection;
    const RecordExportMode DefaultRecordExportMode = RecordExportMode.AllRecords;
    const DocumentExportMode DefaultDocumentExportMode = DocumentExportMode.SingleFile;
    const int ActiveRecordIndex = 0;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            using var inputStream = File.OpenRead(inputPath);

            var outputStreams = MailMergeDocument(
                inputStream,
                MergeMode,
                DefaultRecordExportMode == RecordExportMode.ActiveRecord,
                ActiveRecordIndex,
                DefaultDocumentExportMode == DocumentExportMode.SingleFile,
                IsPdfOutput,
                GetDocumentFormat(OutputFormat));

            if(outputStreams.Count == 0)
                throw new InvalidOperationException("No output documents were created.");

            if(outputStreams.Count == 1) {
                var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);
                using var fileStream = File.Create(outputPath);
                using var outputStream = outputStreams[0];
                outputStream.CopyTo(fileStream);
                Console.WriteLine($"Created {outputPath}");
                return 0;
            }

            var outputDirectory = Path.Combine(AppContext.BaseDirectory, OutputDirectoryName);
            if(Directory.Exists(outputDirectory))
                Directory.Delete(outputDirectory, recursive: true);
            Directory.CreateDirectory(outputDirectory);

            for(var index = 0; index < outputStreams.Count; index++) {
                var extension = IsPdfOutput ? "pdf" : OutputFormat;
                var outputPath = Path.Combine(outputDirectory, $"record_{index + 1}.{extension}");
                using var fileStream = File.Create(outputPath);
                using var outputStream = outputStreams[index];
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

    static List<Stream> MailMergeDocument(
        Stream stream,
        MergeMode mergeMode,
        bool exportActiveRecord,
        int activeRecordIndex,
        bool exportAsOneDocument,
        bool isPdfOutput,
        DocumentFormat targetFormat) {
        using var wordProcessor = new RichEditDocumentServer();
        wordProcessor.LoadDocument(stream);

        var options = wordProcessor.CreateMailMergeOptions();
        var dataSource = PrepareDataSource();

        var firstRecordIndex = exportActiveRecord ? activeRecordIndex : 0;
        var lastRecordIndex = exportActiveRecord ? activeRecordIndex : dataSource.Count - 1;

        options.DataSource = dataSource;
        options.MergeMode = mergeMode;
        options.FirstRecordIndex = firstRecordIndex;
        options.LastRecordIndex = lastRecordIndex;

        using var tempWordProcessor = new RichEditDocumentServer();
        var outputStreams = new List<Stream>();

        if(exportAsOneDocument) {
            var result = MailMerge(wordProcessor, tempWordProcessor, options, isPdfOutput, targetFormat);
            outputStreams.Add(result);
        }
        else {
            for(var index = firstRecordIndex; index <= lastRecordIndex; index++) {
                tempWordProcessor.CreateNewDocument();
                options.FirstRecordIndex = index;
                options.LastRecordIndex = index;
                var result = MailMerge(wordProcessor, tempWordProcessor, options, isPdfOutput, targetFormat);
                outputStreams.Add(result);
            }
        }

        return outputStreams;
    }

    static List<NWind.Customer> PrepareDataSource() {
        try {
            var dataSourcePath = Path.Combine(AppContext.BaseDirectory, DataSourcePath);
            using var dataStream = new FileStream(dataSourcePath, FileMode.Open, FileAccess.Read);
            var nwind = JsonSerializer.Deserialize<NWind>(dataStream);
            return nwind?.Customers ?? new List<NWind.Customer>();
        }
        catch(Exception exception) {
            Console.Error.WriteLine($"Deserialization failed: {exception}");
            return new List<NWind.Customer>();
        }
    }

    static Stream MailMerge(RichEditDocumentServer wordProcessor, RichEditDocumentServer tempWordProcessor, MailMergeOptions options, bool isPdfOutput, DocumentFormat targetFormat) {
        wordProcessor.MailMerge(options, tempWordProcessor.Document);

        var outputStream = new MemoryStream();
        if(isPdfOutput)
            tempWordProcessor.ExportToPdf(outputStream);
        else
            tempWordProcessor.SaveDocument(outputStream, targetFormat);

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
            "pdf" => DocumentFormat.Undefined,
            _ => DocumentFormat.Docx
        };
    }

    class NWind {
        public List<Customer> Customers { get; set; } = new();

        public record Customer(
            string CustomerId,
            string CompanyName,
            string ContactName,
            string Country,
            string Address,
            string City,
            string Phone,
            List<Order> Orders);

        public record Order(
            string OrderID,
            DateTime? OrderDate,
            string ShipCountry,
            double Freight,
            List<OrderDetails> OrderDetails);

        public record OrderDetails(
            int ProductId,
            string ProductName,
            int Quantity,
            decimal UnitPrice,
            double Discount);
    }

    enum RecordExportMode {
        AllRecords,
        ActiveRecord
    }

    enum DocumentExportMode {
        SeparateFiles,
        SingleFile
    }
}
