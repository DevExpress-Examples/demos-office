using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Layout;
using DevExpress.XtraRichEdit.API.Native;

class Program {
    const string InputFilePath = @"Documents\Sample.docx";
    const string OutputDirectoryName = "Split_Document";
    const string OutputFormat = "pdf";
    const bool IsPdfOutput = true;
    const DocumentSplitMode SplitMode = DocumentSplitMode.BySection;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputDirectory = Path.Combine(AppContext.BaseDirectory, OutputDirectoryName);

            if(Directory.Exists(outputDirectory))
                Directory.Delete(outputDirectory, recursive: true);
            Directory.CreateDirectory(outputDirectory);

            using var inputStream = File.OpenRead(inputPath);
            IReadOnlyList<Stream> outputStreams = SplitMode switch {
                DocumentSplitMode.ByPage => SplitByPage(inputStream, GetDocumentFormat(OutputFormat), IsPdfOutput),
                DocumentSplitMode.ByHeading => SplitByHeading(inputStream, GetDocumentFormat(OutputFormat), IsPdfOutput),
                _ => SplitBySection(inputStream, GetDocumentFormat(OutputFormat), IsPdfOutput)
            };
            var entryName = SplitMode switch {
                DocumentSplitMode.ByPage => "page",
                DocumentSplitMode.ByHeading => "heading_section",
                _ => "section"
            };

            if(outputStreams == null || outputStreams.Count == 0)
                throw new InvalidOperationException("No output documents were created.");

            for(var index = 0; index < outputStreams.Count; index++) {
                var extension = IsPdfOutput ? "pdf" : OutputFormat;
                var outputPath = Path.Combine(outputDirectory, $"{entryName}_{index + 1}.{extension}");
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

    static IReadOnlyList<Stream> SplitBySection(Stream inputStream, DocumentFormat targetFormat, bool pdfOutput) {
        using var wordProcessor = new RichEditDocumentServer();
        wordProcessor.LoadDocument(inputStream);

        var document = wordProcessor.Document;
        using var sectionProcessor = new RichEditDocumentServer();
        var outputStreams = new List<Stream>();

        foreach(Section section in document.Sections) {
            sectionProcessor.DocxBytes = document.GetDocxBytes(section.Range);
            var sectionStream = new MemoryStream();
            if(pdfOutput)
                sectionProcessor.ExportToPdf(sectionStream);
            else
                sectionProcessor.SaveDocument(sectionStream, targetFormat);
            sectionStream.Position = 0;
            outputStreams.Add(sectionStream);
        }

        return outputStreams;
    }

    static IReadOnlyList<Stream> SplitByHeading(Stream inputStream, DocumentFormat targetFormat, bool pdfOutput) {
        const string StyleName = "Heading 1";

        using var wordProcessor = new RichEditDocumentServer();
        wordProcessor.LoadDocument(inputStream);

        var document = wordProcessor.Document;
        if(document.ParagraphStyles[StyleName] == null)
            return Array.Empty<Stream>();

        var headingParagraphs = document.Paragraphs.Where(paragraph => paragraph.Style.Name == StyleName).ToList();
        using var headingProcessor = new RichEditDocumentServer();
        var outputStreams = new List<Stream>();

        for(var index = 0; index < headingParagraphs.Count; index++) {
            var startPosition = headingParagraphs[index].Range.Start;
            DocumentPosition endPosition = index == headingParagraphs.Count - 1
                ? document.Range.End
                : headingParagraphs[index + 1].Range.Start;
            var headingRange = document.CreateRange(startPosition, endPosition.ToInt() - startPosition.ToInt());

            headingProcessor.DocxBytes = document.GetDocxBytes(headingRange);
            var headingStream = new MemoryStream();
            if(pdfOutput)
                headingProcessor.ExportToPdf(headingStream);
            else
                headingProcessor.SaveDocument(headingStream, targetFormat);
            headingStream.Position = 0;
            outputStreams.Add(headingStream);
        }

        return outputStreams;
    }

    static IReadOnlyList<Stream> SplitByPage(Stream inputStream, DocumentFormat targetFormat, bool pdfOutput) {
        using var wordProcessor = new RichEditDocumentServer();
        wordProcessor.LoadDocument(inputStream);

        var pageCount = wordProcessor.DocumentLayout.GetPageCount();
        var document = wordProcessor.Document;
        using var pageProcessor = new RichEditDocumentServer();
        var outputStreams = new List<Stream>();

        for(var index = 0; index < pageCount; index++) {
            LayoutPage page = wordProcessor.DocumentLayout.GetPage(index);
            var pageRange = document.CreateRange(page.MainContentRange.Start, page.MainContentRange.Length);
            pageProcessor.DocxBytes = document.GetDocxBytes(pageRange);
            var pageStream = new MemoryStream();
            if(pdfOutput)
                pageProcessor.ExportToPdf(pageStream);
            else
                pageProcessor.SaveDocument(pageStream, targetFormat);
            pageStream.Position = 0;
            outputStreams.Add(pageStream);
        }

        return outputStreams;
    }

    static DocumentFormat GetDocumentFormat(string format) {
        return format.ToLowerInvariant() switch {
            "doc" => DocumentFormat.Doc,
            "docx" => DocumentFormat.Docx,
            "docm" => DocumentFormat.Docm,
            "dotx" => DocumentFormat.Dotx,
            "dotm" => DocumentFormat.Dotm,
            "rtf" => DocumentFormat.Rtf,
            _ => DocumentFormat.Docx
        };
    }

    enum DocumentSplitMode {
        BySection,
        ByPage,
        ByHeading
    }
}