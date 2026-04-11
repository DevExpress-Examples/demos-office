using DevExpress.Docs.Presentation;

class Program {
    const string InputFilePath = @"Documents\Sample.pptx";
    const string OutputDirectoryName = "Split_Presentation";
    const string SlideRange = "1-3";
    const SlideExtractMode ExtractMode = SlideExtractMode.SeparateFiles;
    const bool IsPdfOutput = false;
    static readonly DocumentFormat OutputFormat = DocumentFormat.Pptx;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputDirectory = Path.Combine(AppContext.BaseDirectory, OutputDirectoryName);

            if(Directory.Exists(outputDirectory))
                Directory.Delete(outputDirectory, recursive: true);
            Directory.CreateDirectory(outputDirectory);

            using var inputStream = File.OpenRead(inputPath);
            var slideIndexes = ParseSlideRange(SlideRange);
            var slideStreams = ExtractSlides(inputStream, slideIndexes, ExtractMode == SlideExtractMode.SingleFile, IsPdfOutput, OutputFormat);

            if(slideStreams.Count == 0)
                throw new InvalidOperationException("Failed to split presentation.");

            for(var index = 0; index < slideStreams.Count; index++) {
                var extension = IsPdfOutput ? "pdf" : "pptx";
                var outputPath = Path.Combine(outputDirectory, $"slide_{index + 1}.{extension}");
                using var fileStream = File.Create(outputPath);
                using var outputStream = slideStreams[index];
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

    static IEnumerable<int> ParseSlideRange(string slideRange) {
        if(string.IsNullOrWhiteSpace(slideRange))
            return Enumerable.Empty<int>();

        var result = new SortedSet<int>();
        var tokens = slideRange.Split(new[] { ',', ';', ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
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

    static IReadOnlyList<Stream> ExtractSlides(Stream inputStream, IEnumerable<int> slideIndexes, bool isSingleFileOutput,
        bool isPdfOutput, DocumentFormat documentFormat) {
        using var presentation = new Presentation(inputStream);
        return isSingleFileOutput
            ? ExtractSlidesSingleFile(slideIndexes, isPdfOutput, documentFormat, presentation)
            : ExtractSlidesDifferentFiles(slideIndexes, isPdfOutput, documentFormat, presentation);
    }

    static IReadOnlyList<Stream> ExtractSlidesDifferentFiles(IEnumerable<int> slideIndexes, bool isPdfOutput, DocumentFormat documentFormat, Presentation presentation) {
        var outputStreams = new List<Stream>();

        SlideCollection slides = presentation.Slides;
        if(!slideIndexes.Any())
            slideIndexes = Enumerable.Range(0, slides.Count);

        foreach(var slideIndex in slideIndexes) {
            if(slideIndex < 0 || slideIndex >= slides.Count)
                continue;
            using var singleSlidePresentation = new Presentation();
            singleSlidePresentation.Slides.Clear();
            singleSlidePresentation.SlideSize = presentation.SlideSize;
            singleSlidePresentation.Slides.Add(slides[slideIndex]);

            var outputStream = new MemoryStream();
            if(isPdfOutput)
                singleSlidePresentation.ExportToPdf(outputStream);
            else
                singleSlidePresentation.SaveDocument(outputStream, documentFormat);

            outputStream.Position = 0;
            outputStreams.Add(outputStream);
        }

        return outputStreams.AsReadOnly();
    }
    static IReadOnlyList<Stream> ExtractSlidesSingleFile(IEnumerable<int> slideIndexes, bool isPdfOutput, DocumentFormat documentFormat, Presentation presentation) {
        var outputStreams = new List<Stream>();

        SlideCollection slides = presentation.Slides;
        if(!slideIndexes.Any())
            slideIndexes = Enumerable.Range(0, slides.Count);

        for(var index = slides.Count - 1; index >= 0; index--) {
            if(!slideIndexes.Contains(index))
                presentation.Slides.RemoveAt(index);
        }

        var outputStream = new MemoryStream();
        if(isPdfOutput)
            presentation.ExportToPdf(outputStream);
        else
            presentation.SaveDocument(outputStream, documentFormat);

        outputStream.Position = 0;
        outputStreams.Add(outputStream);
        return outputStreams.AsReadOnly();
    }

    enum SlideExtractMode {
        SeparateFiles,
        SingleFile
    }
}
