using DevExpress.Pdf;

internal static class Program {
    const string InputFilePath = @"Documents\PDF\Sample_Main.pdf";
    const string OutputFileName = "Document.pdf";
    const string SearchText = "DevExpress";
    const string RedactionText = "REDACTED";
    const bool ApplyRedaction = true;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var documentStream = File.OpenRead(inputPath);
            using var outputStream = AddRedaction(documentStream, SearchText, RedactionText, ApplyRedaction);
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

    static Stream AddRedaction(Stream documentStream, string searchText, string redactionText, bool applyRedaction) {
        using var processor = new PdfDocumentProcessor();
        processor.LoadDocument(documentStream);

        PdfTextSearchResults search;
        do {
            search = processor.FindText(
                searchText,
                new PdfTextSearchParameters {
                    CaseSensitive = false,
                    WholeWords = false
                });

            if(search.Status == PdfTextSearchStatus.Found) {
                var annotation = processor.DocumentFacade.Pages[search.Page.GetPageIndex()].AddRedactAnnotation(search.Rectangles);
                annotation.Author = "DX Demo";
                annotation.FillColor = new PdfRGBColor(0, 0, 0);
                annotation.FontColor = new PdfRGBColor(1, 1, 1);
                if(!string.IsNullOrEmpty(redactionText))
                    annotation.OverlayText = redactionText;
                annotation.CreationDate = DateTime.UtcNow;
                annotation.TextJustification = PdfTextJustification.LeftJustified;
                annotation.RepeatText = true;
                annotation.FontSize = 0;
            }
        } while(search.Status == PdfTextSearchStatus.Found);

        if(applyRedaction)
            processor.DocumentFacade.ApplyRedactAnnotations();

        var outputStream = new MemoryStream();
        processor.SaveDocument(outputStream);
        outputStream.Position = 0;
        return outputStream;
    }
}