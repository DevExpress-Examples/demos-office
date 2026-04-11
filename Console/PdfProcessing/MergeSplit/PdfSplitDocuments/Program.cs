using System.IO.Compression;
using DevExpress.Pdf;

internal static class Program {
    const string InputFilePath = @"Documents\PDF\Sample_Main.pdf";
    const string OutputDirectoryName = "Documents";
    const string OutputArchiveName = "Documents.zip";
    const RangeExportMode ExportMode = RangeExportMode.All;
    const string PageRange = "";

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputDirectory = Path.Combine(AppContext.BaseDirectory, OutputDirectoryName);
            var outputArchivePath = Path.Combine(AppContext.BaseDirectory, OutputArchiveName);

            if(Directory.Exists(outputDirectory))
                Directory.Delete(outputDirectory, recursive: true);
            if(File.Exists(outputArchivePath))
                File.Delete(outputArchivePath);

            Directory.CreateDirectory(outputDirectory);

            using var documentStream = File.OpenRead(inputPath);
            var pageRange = RangeHelper.GetIndexesByRange(ExportMode, PageRange);
            var outputStreams = SplitDocuments(documentStream, pageRange);

            if(outputStreams.Count == 0)
                throw new InvalidOperationException("No pages were extracted from the input document.");

            for(var index = 0; index < outputStreams.Count; index++) {
                var outputPath = Path.Combine(outputDirectory, $"document_{index + 1}.pdf");
                using var fileStream = File.Create(outputPath);
                using var outputStream = outputStreams[index];
                outputStream.CopyTo(fileStream);
            }

            ZipFile.CreateFromDirectory(outputDirectory, outputArchivePath);

            Console.WriteLine($"Created {outputDirectory}");
            Console.WriteLine($"Created {outputArchivePath}");
            return 0;
        }
        catch(Exception exception) {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    static IReadOnlyList<Stream> SplitDocuments(Stream documentStream, IEnumerable<int> pageRange) {
        using var processor = new PdfDocumentProcessor();
        processor.LoadDocument(documentStream);

        if(!pageRange.Any())
            pageRange = Enumerable.Range(0, processor.Document.Pages.Count);

        var result = new List<Stream>();
        foreach(var index in pageRange) {
            if(index < 0 || index >= processor.Document.Pages.Count)
                continue;

            using var pageProcessor = new PdfDocumentProcessor();
            pageProcessor.CreateEmptyDocument();
            pageProcessor.Document.Pages.Add(processor.Document.Pages[index]);

            var outputStream = new MemoryStream();
            pageProcessor.SaveDocument(outputStream);
            outputStream.Position = 0;
            result.Add(outputStream);
        }

        return result;
    }

    enum RangeExportMode {
        First,
        All,
        CustomRange
    }

    static class RangeHelper {
        public static List<int> GetIndexesByRange(RangeExportMode exportMode, string range) {
            return exportMode switch {
                RangeExportMode.First => new List<int> { 0 },
                RangeExportMode.All => new List<int>(),
                RangeExportMode.CustomRange => new List<int>(ParseRange(range)),
                _ => new List<int>()
            };
        }

        static IEnumerable<int> ParseRange(string range) {
            if(string.IsNullOrWhiteSpace(range))
                return Enumerable.Empty<int>();

            var result = new SortedSet<int>();
            var tokens = range.Split(new[] { ',', ';', ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach(var raw in tokens) {
                var part = raw.Trim();
                if(part.Length == 0)
                    continue;

                var dashIndex = part.IndexOf('-');
                if(dashIndex > 0) {
                    var startText = part[..dashIndex].Trim();
                    var endText = part[(dashIndex + 1)..].Trim();

                    if(int.TryParse(startText, out var start) && int.TryParse(endText, out var end)) {
                        if(start <= 0 || end <= 0)
                            continue;

                        if(end < start)
                            (start, end) = (end, start);

                        for(var value = start; value <= end; value++)
                            result.Add(value - 1);
                    }

                    continue;
                }

                if(int.TryParse(part, out var single) && single > 0)
                    result.Add(single - 1);
            }

            return result;
        }
    }
}