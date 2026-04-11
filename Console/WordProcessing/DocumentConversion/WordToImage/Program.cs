using DevExpress.Drawing;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Export.Image;

class Program {
    const string InputFilePath = @"Documents\Sample.docx";
    const string OutputDirectoryName = "Document_Images";
    static readonly DXImageFormat ImageFormat = DXImageFormat.Png;
    const int Resolution = 300;
    const RangeExportMode ExportMode = RangeExportMode.All;
    const string PageRange = "";

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputDirectory = Path.Combine(AppContext.BaseDirectory, OutputDirectoryName);

            if(Directory.Exists(outputDirectory))
                Directory.Delete(outputDirectory, recursive: true);
            Directory.CreateDirectory(outputDirectory);

            using var inputStream = File.OpenRead(inputPath);
            var correctedPageRange = GetPagesToExport(ExportMode, PageRange);
            var imageStreams = ConvertToImages(inputStream, ImageFormat, Resolution, correctedPageRange);

            if(imageStreams.Count == 0)
                throw new InvalidOperationException("Failed to convert document to images.");

            for(var index = 0; index < imageStreams.Count; index++) {
                var extension = GetOutputExtension(ImageFormat);
                var outputPath = Path.Combine(outputDirectory, $"page_{index + 1}.{extension}");
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

    static string GetPagesToExport(RangeExportMode exportMode, string pageRange) {
        return exportMode switch {
            RangeExportMode.First => "1",
            RangeExportMode.All => string.Empty,
            RangeExportMode.CustomRange => pageRange,
            _ => "1"
        };
    }

    static IReadOnlyList<Stream> ConvertToImages(Stream inputStream, DXImageFormat imageFormat, int resolution, string pageRange) {
        using var wordProcessor = new RichEditDocumentServer();

        wordProcessor.LoadDocument(inputStream);

        var imageExportOptions = new RichEditImageExportOptions {
            ExportMode = RichEditImageExportMode.DifferentFiles,
            Resolution = resolution,
            PageRange = pageRange,
            Format = imageFormat
        };

        return wordProcessor.Document.ExportToImage(imageExportOptions);
    }

    static string GetOutputExtension(DXImageFormat imageFormat) {
        if(imageFormat == DXImageFormat.Jpeg)
            return "jpeg";
        if(imageFormat == DXImageFormat.Png)
            return "png";
        if(imageFormat == DXImageFormat.Bmp)
            return "bmp";
        if(imageFormat == DXImageFormat.Svg)
            return "svg";

        throw new ArgumentException("Unsupported image format.", nameof(imageFormat));
    }

    enum RangeExportMode {
        First,
        All,
        CustomRange
    }
}