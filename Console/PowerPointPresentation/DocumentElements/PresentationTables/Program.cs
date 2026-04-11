using DevExpress.Docs.Presentation;
using DevExpress.Office.Utils;

class Program {
    const string OutputFileName = "Table_Result.pptx";
    const int NumberOfRows = 5;
    const int NumberOfColumns = 4;
    const float ColumnWidth = 1.5f;
    const TableStyleType TableStyle = TableStyleType.DarkStyle1Accent1;
    const bool HasHeader = true;
    const bool HasBandedRows = true;
    const bool HasBandedColumns = false;
    const bool IsPdfOutput = false;
    static readonly DocumentFormat OutputFormat = DocumentFormat.Pptx;

    static int Main() {
        try {
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var outputStream = CreatePresentationTable(
                NumberOfRows,
                NumberOfColumns,
                ColumnWidth,
                TableStyle,
                HasHeader,
                HasBandedRows,
                HasBandedColumns,
                IsPdfOutput,
                OutputFormat);
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

    static Stream CreatePresentationTable(
        int rows,
        int columns,
        float columnWidthInInches,
        TableStyleType styleType,
        bool hasHeader,
        bool hasBandedRows,
        bool hasBandedColumns,
        bool isPdfOutput,
        DocumentFormat format) {
        using var presentation = new Presentation();

        presentation.Slides.Clear();
        var slide = new Slide(SlideLayoutType.Blank);
        presentation.Slides.Add(slide);

        var tableWidth = Units.InchesToDocumentsF(columns * columnWidthInInches);
        var table = new Table(rows, columns, 100, 100, tableWidth) {
            Style = new ThemedTableStyle(styleType),
            HasHeaderRow = hasHeader,
            HasBandedRows = hasBandedRows,
            HasBandedColumns = hasBandedColumns
        };
        slide.Shapes.Add(table);

        var outputStream = new MemoryStream();
        if(isPdfOutput)
            presentation.ExportToPdf(outputStream);
        else
            presentation.SaveDocument(outputStream, format);

        outputStream.Position = 0;
        return outputStream;
    }
}