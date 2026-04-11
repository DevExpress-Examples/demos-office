using System.Text;
using DevExpress.Docs.Presentation;

class Program {
    const string InputFilePath = @"Documents\Sample.pptx";
    const string OutputFileName = "Presentation_Images.txt";
    const RangeExportMode ImageExtractMode = RangeExportMode.First;
    const string SlideRange = "";
    const bool ExtractFromShapes = true;
    const bool ExtractFromTables = true;
    const bool ExtractNotes = false;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var inputStream = File.OpenRead(inputPath);
            var slideRange = RangeHelper.GetIndexesByRange(ImageExtractMode, SlideRange);
            var presentationText = ExtractText(inputStream, slideRange, ExtractFromShapes, ExtractFromTables, ExtractNotes);
            File.WriteAllText(outputPath, presentationText, Encoding.UTF8);

            Console.WriteLine($"Created {outputPath}");
            return 0;
        }
        catch(Exception exception) {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    static string ExtractText(Stream inputStream, List<int> slideRange, bool extractFromShapes, bool extractFromTables, bool extractNotes) {
        using var presentation = new Presentation(inputStream);
        var shouldExportFromAllSlides = slideRange.Count == 0;
        var builder = new StringBuilder();

        for(var index = 0; index < presentation.Slides.Count; index++) {
            if(shouldExportFromAllSlides || slideRange.Contains(index)) {
                var slide = presentation.Slides[index];
                builder.AppendLine($"## Slide {index + 1}");
                ExtractShapeText(builder, slide.Shapes, extractFromShapes, extractFromTables);
                if(extractNotes) {
                    var notesText = ExtractTextFromNotes(slide);
                    if(!string.IsNullOrWhiteSpace(notesText)) {
                        builder.AppendLine("### Notes");
                        builder.AppendLine(notesText);
                    }
                }
                builder.AppendLine();
            }
        }

        return builder.ToString();
    }

    static void ExtractShapeText(StringBuilder builder, ShapeCollection shapes, bool extractFromShapes, bool extractFromTables) {
        var sortedShapes = shapes.OrderBy(shape => shape.Y).ThenBy(shape => shape.X);
        foreach(var shape in sortedShapes) {
            switch(shape) {
                case Shape textShape:
                    if(extractFromShapes && textShape.TextArea != null)
                        builder.AppendLine(textShape.TextArea.Text);
                    break;
                case Table table:
                    if(extractFromTables)
                        builder.Append(ExtractTextFromTable(table));
                    break;
                case GroupShape groupShape:
                    ExtractShapeText(builder, groupShape.Shapes, extractFromShapes, extractFromTables);
                    break;
            }
        }
    }

    static string ExtractTextFromNotes(Slide slide) {
        var notesText = string.Empty;
        if(slide.Notes != null) {
            var notesBody = slide.Notes.Shapes
                .Where(shape => shape is Shape noteShape && noteShape.PlaceholderSettings.Type == PlaceholderType.Body)
                .FirstOrDefault();
            notesText += notesBody != null ? ((Shape)notesBody).TextArea.Text : string.Empty;
        }

        return notesText;
    }

    static string ExtractTextFromTable(Table table) {
        var builder = new StringBuilder();
        for(var row = 0; row < table.Rows.Count; row++) {
            for(var column = 0; column < table.Columns.Count; column++) {
                var cell = table[row, column];
                var cellText = cell.TextArea?.Text ?? string.Empty;
                builder.Append(cellText);
                if(column != table.Columns.Count - 1)
                    builder.Append('\t');
            }
            builder.AppendLine();
        }

        return builder.ToString();
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