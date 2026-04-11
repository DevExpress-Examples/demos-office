using DevExpress.Drawing;
using DevExpress.Pdf;

internal static class Program {
    const string InputFilePath = @"Documents\PDF\Sample_Main.pdf";
    const string OutputFileName = "Document.pdf";
    const string WatermarkText = "WATERMARK";
    const bool Rotate = false;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var documentStream = File.OpenRead(inputPath);
            using var outputStream = ApplyWatermark(documentStream, WatermarkText, Rotate);
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

    static Stream ApplyWatermark(Stream documentStream, string text, bool rotate) {
        using var processor = new PdfDocumentProcessor();
        processor.LoadDocument(documentStream);

        using var brush = new DXSolidBrush(System.Drawing.Color.FromArgb(50, System.Drawing.Color.LightSkyBlue));

        const string fontName = "Arial Black";
        const float fontSize = 12;
        var font = new DXFont(fontName, fontSize);

        var stringFormat = PdfStringFormat.GenericTypographic;
        stringFormat.Alignment = PdfStringAlignment.Center;
        stringFormat.LineAlignment = PdfStringAlignment.Center;

        foreach(var page in processor.Document.Pages) {
            using var graphics = processor.CreateGraphicsPageSystem();

            var watermarkSize = page.CropBox.Width * 0.75;
            var stringSize = graphics.MeasureString(text, font);
            var scale = (float)(watermarkSize / stringSize.Width);

            graphics.TranslateTransform((float)(page.CropBox.Width * 0.5), (float)(page.CropBox.Height * 0.5));
            if(rotate)
                graphics.RotateTransform(-45f);
            graphics.TranslateTransform(
                (float)(-stringSize.Width * scale * 0.5),
                (float)(-stringSize.Height * scale * 0.5));

            var actualFont = new DXFont(fontName, fontSize * scale);
            var rect = new System.Drawing.RectangleF(0, 0, stringSize.Width * scale, stringSize.Height * scale);
            graphics.DrawString(text, actualFont, brush, rect, stringFormat);
            graphics.AddToPageForeground(page);
        }

        var outputStream = new MemoryStream();
        processor.SaveDocument(outputStream);
        outputStream.Position = 0;
        return outputStream;
    }
}