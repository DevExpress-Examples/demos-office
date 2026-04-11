using DevExpress.Pdf;

internal static class Program {
    const string InputFilePath = @"Documents\PDF\Sample_Main.pdf";
    const string OutputFileName = "page_image";
    const string OutputDirectoryName = "Image";
    const string ImageFormat = "bmp";
    const int Resolution = 300;
    const RangeExportMode ExportMode = RangeExportMode.All;
    const string PageRange = "";

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);

            using var documentStream = File.OpenRead(inputPath);

            var pageRange = RangeHelper.GetIndexesByRange(ExportMode, PageRange).Select(index => index + 1);
            var imageStreams = ConvertToImages(documentStream, ImageFormat, Resolution, pageRange);

            if(imageStreams.Count == 0)
                throw new InvalidOperationException("Failed to convert document to images.");

            if(imageStreams.Count == 1) {
                var extension = GetOutputExtension(ImageFormat);
                var outputPath = Path.Combine(AppContext.BaseDirectory, $"{OutputFileName}.{extension}");
                using var fileStream = File.Create(outputPath);
                imageStreams[0].CopyTo(fileStream);
                imageStreams[0].Dispose();
                Console.WriteLine($"Created {outputPath}");
                return 0;
            }

            var outputDirectory = Path.Combine(AppContext.BaseDirectory, OutputDirectoryName);
            Directory.CreateDirectory(outputDirectory);

            for(var index = 0; index < imageStreams.Count; index++) {
                var outputPath = Path.Combine(outputDirectory, $"page_{index + 1}.{GetOutputExtension(ImageFormat)}");
                using var fileStream = File.Create(outputPath);
                using var imageStream = imageStreams[index];
                imageStream.CopyTo(fileStream);
            }

            Console.WriteLine($"Created {outputDirectory}");
            return 0;
        }
        catch(Exception exception) {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    static IReadOnlyList<MemoryStream> ConvertToImages(Stream documentStream, string imageFormat, int resolution, IEnumerable<int> pageRange) {
        using var processor = new PdfDocumentProcessor();
        processor.LoadDocument(documentStream);

        var pageCount = processor.Document.Pages.Count;
        var pages = pageRange.Any() ? pageRange : Enumerable.Range(1, pageCount);
        var result = new List<MemoryStream>();

        if(imageFormat == "tiff") {
            var tiffStream = new MemoryStream();
            processor.CreateTiff(tiffStream, pages.Where(page => page >= 1 && page <= pageCount), resolution);
            tiffStream.Position = 0;
            result.Add(tiffStream);
            return result;
        }

        var renderingParameters = PdfPageRenderingParameters.CreateWithResolution(resolution);
        foreach(var page in pages) {
            if(page < 1 || page > pageCount)
                continue;

            var outputStream = new MemoryStream();

            switch(imageFormat) {
                case "bmp":
                    using(var bitmap = processor.CreateDXBitmap(page, renderingParameters))
                        bitmap.Save(outputStream);
                    break;
                case "png":
                    using(var bitmap = processor.CreateDXBitmap(page, renderingParameters))
                        bitmap.Save(outputStream, DevExpress.Drawing.DXImageFormat.Png);
                    break;
                case "svg":
                    using(var svg = processor.CreateSvgImage(page, renderingParameters))
                        svg.Save(outputStream);
                    break;
                default:
                    throw new ArgumentException("Incorrect image format", nameof(imageFormat));
            }

            outputStream.Position = 0;
            result.Add(outputStream);
        }

        return result;
    }

    static string GetOutputExtension(string imageFormat) {
        return imageFormat.ToLowerInvariant() switch {
            "bmp" => "bmp",
            "png" => "png",
            "tiff" => "tiff",
            "svg" => "svg",
            _ => throw new ArgumentException("Incorrect image format", nameof(imageFormat))
        };
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
                    var startStr = part.Substring(0, dashIndex).Trim();
                    var endStr = part.Substring(dashIndex + 1).Trim();

                    if(int.TryParse(startStr, out var start) && int.TryParse(endStr, out var end)) {
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