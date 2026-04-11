using DevExpress.Spreadsheet;

class Program {
    const string InputFilePath = @"Documents\InvestmentPortfolio.xlsx";
    const string OutputDirectoryName = "Document_Images";
    const ImageFileFormat ImageFormat = ImageFileFormat.Png;
    const int Resolution = 300;
    const bool ExportActiveWorksheetOnly = false;
    const bool ExportGridlines = false;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputDirectory = Path.Combine(AppContext.BaseDirectory, OutputDirectoryName);

            if(Directory.Exists(outputDirectory))
                Directory.Delete(outputDirectory, recursive: true);
            Directory.CreateDirectory(outputDirectory);

            using var inputStream = File.OpenRead(inputPath);
            var imageStreams = ConvertToImages(inputStream, ImageFormat, ExportActiveWorksheetOnly, Resolution, ExportGridlines);

            if(imageStreams.Count == 0)
                throw new InvalidOperationException("Failed to convert workbook to images.");

            for(var index = 0; index < imageStreams.Count; index++) {
                var outputPath = Path.Combine(outputDirectory, $"sheet_{index + 1}.{GetOutputExtension(ImageFormat)}");
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

    static IReadOnlyList<Stream> ConvertToImages(
        Stream inputStream,
        ImageFileFormat imageFormat,
        bool exportActiveWorksheetOnly,
        int resolution,
        bool exportGridlines) {
        using var workbook = new Workbook();

        workbook.LoadDocument(inputStream);

        var imageExportOptions = new RangeImageOptions {
            Resolution = resolution,
            ExportGridlines = exportGridlines,
            GridlineColor = System.Drawing.Color.Gray
        };

        var worksheetsToExport = new List<Worksheet>();
        if(exportActiveWorksheetOnly)
            worksheetsToExport.Add(workbook.Worksheets.ActiveWorksheet);
        else
            worksheetsToExport = workbook.Worksheets.ToList();

        var imageStreams = new List<Stream>();
        foreach(var worksheet in worksheetsToExport) {
            var worksheetImageStream = new MemoryStream();
            worksheet.GetUsedRange().ExportToImage(worksheetImageStream, imageFormat, imageExportOptions);
            worksheetImageStream.Position = 0;
            imageStreams.Add(worksheetImageStream);
        }

        return imageStreams.AsReadOnly();
    }

    static string GetOutputExtension(ImageFileFormat imageFormat) {
        return imageFormat switch {
            ImageFileFormat.Png => "png",
            ImageFileFormat.Jpeg => "jpeg",
            ImageFileFormat.Bmp => "bmp",
            ImageFileFormat.Tiff => "tiff",
            ImageFileFormat.Svg => "svg",
            _ => throw new ArgumentException("Unsupported image format.", nameof(imageFormat))
        };
    }
}