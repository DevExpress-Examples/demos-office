using DevExpress.Docs;
using DevExpress.Docs.Presentation;
using DevExpress.Drawing;

class Program {
    const string InputFilePath = @"Documents\Sample.pptx";
    const string OutputDirectoryName = "Presentation_Images";
    static readonly DXImageFormat ImageFormat = DXImageFormat.Png;
    const RangeExportMode ImageExtractMode = RangeExportMode.All;
    const string SlideRange = "";

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputDirectory = Path.Combine(AppContext.BaseDirectory, OutputDirectoryName);

            if(Directory.Exists(outputDirectory))
                Directory.Delete(outputDirectory, recursive: true);
            Directory.CreateDirectory(outputDirectory);

            using var inputStream = File.OpenRead(inputPath);
            var slideRange = RangeHelper.GetIndexesByRange(ImageExtractMode, SlideRange);
            var imageStreams = ExtractImages(inputStream, ImageFormat, slideRange);

            if(imageStreams.Count == 0)
                throw new InvalidOperationException("Failed to extract document images.");

            for(var index = 0; index < imageStreams.Count; index++) {
                var outputPath = Path.Combine(outputDirectory, $"image_{index + 1}.{GetOutputExtension(ImageFormat)}");
                using var fileStream = File.Create(outputPath);
                using var outputStream = imageStreams[index];
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

    static IReadOnlyList<Stream> ExtractImages(Stream inputStream, DXImageFormat imageFormat, List<int> slideRange) {
        using var presentation = new Presentation(inputStream);

        var imageStreams = new List<Stream>();
        var shouldExportFromAllSlides = slideRange.Count == 0;
        for(var index = 0; index < presentation.Slides.Count; index++) {
            if(shouldExportFromAllSlides || slideRange.Contains(index)) {
                var slide = presentation.Slides[index];
                foreach(var shape in slide.Shapes)
                    ExtractShapeImages(shape, imageStreams, imageFormat);
            }
        }

        return imageStreams.AsReadOnly();
    }

    static void ExtractShapeImages(ShapeBase shape, IList<Stream> imageStreams, DXImageFormat imageFormat) {
        switch(shape) {
            case PictureShape pictureShape:
                var officeImage = pictureShape.Image as OfficeImage;
                var dxImage = officeImage?.DXImage;
                if(dxImage != null) {
                    var imageStream = new MemoryStream();
                    dxImage.Save(imageStream, imageFormat);
                    imageStream.Position = 0;
                    imageStreams.Add(imageStream);
                }
                break;
            case GroupShape groupShape:
                foreach(var innerShape in groupShape.Shapes)
                    ExtractShapeImages(innerShape, imageStreams, imageFormat);
                break;
        }
    }

    static string GetOutputExtension(DXImageFormat imageFormat) {
        if(imageFormat == DXImageFormat.Png)
            return "png";
        if(imageFormat == DXImageFormat.Jpeg)
            return "jpeg";
        if(imageFormat == DXImageFormat.Bmp)
            return "bmp";
        throw new ArgumentException("Unsupported image format.", nameof(imageFormat));
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