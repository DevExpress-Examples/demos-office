using DevExpress.Docs.Presentation;

class Program {
    static readonly string[] InputFilePaths = {
        @"Documents\Sample_first.pptx",
        @"Documents\Sample_second.pptx"
    };
    const string OutputFileName = "Merged_Presentation.pdf";
    const bool IsPdfOutput = true;
    static readonly DocumentFormat OutputFormat = DocumentFormat.Pptx;

    static int Main() {
        try {
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);
            var inputStreams = InputFilePaths
                .Select(path => Path.Combine(AppContext.BaseDirectory, path))
                .Select(File.OpenRead)
                .Cast<Stream>()
                .ToList();

            try {
                using var outputStream = MergePresentations(inputStreams, OutputFormat, IsPdfOutput);
                using var fileStream = File.Create(outputPath);
                outputStream.CopyTo(fileStream);
            }
            finally {
                foreach(var inputStream in inputStreams)
                    inputStream.Dispose();
            }

            Console.WriteLine($"Created {outputPath}");
            return 0;
        }
        catch(Exception exception) {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    static Stream MergePresentations(List<Stream> inputStreams, DocumentFormat targetFormat, bool pdfOutput) {
        var outputStream = new MemoryStream();

        if(inputStreams.Count > 0) {
            using var presentation = new Presentation(inputStreams[0]);
            for(var index = 1; index < inputStreams.Count; index++) {
                using var tempPresentation = new Presentation(inputStreams[index]);
                foreach(var slide in tempPresentation.Slides)
                    presentation.Slides.Add(slide);
            }

            if(pdfOutput)
                presentation.ExportToPdf(outputStream);
            else
                presentation.SaveDocument(outputStream, targetFormat);
        }

        outputStream.Position = 0;
        return outputStream;
    }
}