using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

class Program {
    static readonly string[] InputFilePaths = {
        @"Documents\Sample.docx",
        @"Documents\DocumentForProtection.docx"
    };

    const string OutputFileName = "Merged_Document.pdf";
    const string OutputFormat = "pdf";
    const bool IsPdfOutput = true;

    static int Main() {
        try {
            var inputPaths = InputFilePaths.Select(path => Path.Combine(AppContext.BaseDirectory, path)).ToArray();
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);
            var inputStreams = inputPaths.Select(File.OpenRead).Cast<Stream>().ToList();

            try {
                using var outputStream = MergeDocuments(inputStreams, GetDocumentFormat(OutputFormat), IsPdfOutput);
                using var fileStream = File.Create(outputPath);
                outputStream.CopyTo(fileStream);
            }
            finally {
                foreach(var stream in inputStreams)
                    stream.Dispose();
            }

            Console.WriteLine($"Created {outputPath}");
            return 0;
        }
        catch(Exception exception) {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    static Stream MergeDocuments(List<Stream> inputStreams, DocumentFormat targetFormat, bool pdfOutput) {
        using var sourceWordProcessor = new RichEditDocumentServer();
        using var targetWordProcessor = new RichEditDocumentServer();
        var sourceDocument = sourceWordProcessor.Document;
        var targetDocument = targetWordProcessor.Document;

        for(var index = 0; index < inputStreams.Count; index++) {
            sourceWordProcessor.LoadDocument(inputStreams[index]);

            var targetSection = targetDocument.Sections[targetDocument.Sections.Count - 1];
            targetSection.UnlinkHeaderFromPrevious();
            targetSection.UnlinkFooterFromPrevious();
            targetSection.UnlinkHeaderFromPrevious(HeaderFooterType.First);
            targetSection.UnlinkFooterFromPrevious(HeaderFooterType.First);
            targetSection.UnlinkHeaderFromPrevious(HeaderFooterType.Even);
            targetSection.UnlinkFooterFromPrevious(HeaderFooterType.Even);

            AppendDocumentSection(sourceDocument, targetDocument);

            if(index < inputStreams.Count - 1) {
                targetDocument.AppendSection();
                targetDocument.Sections.Last().StartType = SectionStartType.NextPage;
            }
        }

        var outputStream = new MemoryStream();
        if(pdfOutput)
            targetWordProcessor.ExportToPdf(outputStream);
        else
            targetWordProcessor.SaveDocument(outputStream, targetFormat);

        outputStream.Position = 0;
        return outputStream;
    }

    static void AppendDocumentSection(Document source, Document target) {
        var lastSectionIndexBeforeAppending = target.Sections.Count - 1;
        var sourceSectionCount = source.Sections.Count;

        var appendedRange = target.AppendDocumentContent(source.Range);
        target.Delete(target.Paragraphs.Get(target.CreatePosition(appendedRange.Start.ToInt() - 1)).Range);

        target.DifferentOddAndEvenPages = source.DifferentOddAndEvenPages;

        for(var index = 0; index < sourceSectionCount; index++) {
            var sourceSection = source.Sections[index];
            var targetSection = target.Sections[lastSectionIndexBeforeAppending + index];

            AppendHeader(sourceSection, targetSection, HeaderFooterType.Odd);
            AppendFooter(sourceSection, targetSection, HeaderFooterType.Odd);
            AppendHeader(sourceSection, targetSection, HeaderFooterType.Even);
            AppendFooter(sourceSection, targetSection, HeaderFooterType.Even);
            AppendHeader(sourceSection, targetSection, HeaderFooterType.First);
            AppendFooter(sourceSection, targetSection, HeaderFooterType.First);
        }
    }

    static void AppendHeader(Section sourceSection, Section targetSection, HeaderFooterType headerFooterType) {
        if(!sourceSection.HasHeader(headerFooterType)) {
            if(targetSection.HasHeader(headerFooterType)) {
                var targetHeader = targetSection.BeginUpdateHeader(headerFooterType);
                targetHeader.Delete(targetHeader.Range);
                targetSection.EndUpdateHeader(targetHeader);
            }
            return;
        }

        var source = sourceSection.BeginUpdateHeader(headerFooterType);
        var target = targetSection.BeginUpdateHeader(headerFooterType);
        target.Delete(target.Range);
        target.InsertDocumentContent(target.Range.Start, source.Range, InsertOptions.KeepSourceFormatting);
        var emptyParagraph = target.CreateRange(target.Range.End.ToInt() - 1, 1);
        target.Delete(emptyParagraph);
        sourceSection.EndUpdateHeader(source);
        targetSection.EndUpdateHeader(target);
    }

    static void AppendFooter(Section sourceSection, Section targetSection, HeaderFooterType headerFooterType) {
        if(!sourceSection.HasFooter(headerFooterType)) {
            if(targetSection.HasFooter(headerFooterType)) {
                var targetFooter = targetSection.BeginUpdateFooter(headerFooterType);
                targetFooter.Delete(targetFooter.Range);
                targetSection.EndUpdateFooter(targetFooter);
            }
            return;
        }

        var source = sourceSection.BeginUpdateFooter(headerFooterType);
        var target = targetSection.BeginUpdateFooter(headerFooterType);
        target.Delete(target.Range);
        target.InsertDocumentContent(target.Range.Start, source.Range, InsertOptions.KeepSourceFormatting);
        var emptyParagraph = target.CreateRange(target.Range.End.ToInt() - 1, 1);
        target.Delete(emptyParagraph);
        sourceSection.EndUpdateFooter(source);
        targetSection.EndUpdateFooter(target);
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
}