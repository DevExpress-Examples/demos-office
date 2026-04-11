using System.Drawing;
using DevExpress.Docs;
using DevExpress.Docs.Presentation;

class Program {
    const string InputFilePath = @"Documents\Sample.pptx";
    const string OutputFileName = "Modified_Presentation.pptx";
    const string SearchText = "DevExpress";
    const string ReplaceText = "Updated Presentation";
    const string HighlightColor = "#FFFF00";
    const FindReplaceActionType Operation = FindReplaceActionType.Replace;
    static readonly DocumentFormat OutputFormat = DocumentFormat.Pptx;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var inputStream = File.OpenRead(inputPath);
            using var outputStream = FindReplacePresentationText(
                inputStream,
                Operation,
                SearchText,
                ReplaceText,
                ColorTranslator.FromHtml(HighlightColor),
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

    static Stream FindReplacePresentationText(
        Stream inputStream,
        FindReplaceActionType actionType,
        string findText,
        string replaceText,
        Color highlightColor,
        DocumentFormat outputFormat) {
        using var presentation = new Presentation(inputStream);

        var searchResult = presentation.FindText(findText);
        switch(actionType) {
            case FindReplaceActionType.Replace:
                presentation.ReplaceText(searchResult, replaceText);
                break;
            case FindReplaceActionType.Remove:
                presentation.RemoveText(searchResult);
                break;
            case FindReplaceActionType.Highlight:
                presentation.ModifyTextProperties(searchResult, new TextProperties { HighlightColor = new OfficeColor(highlightColor) });
                break;
        }

        var outputStream = new MemoryStream();
        presentation.SaveDocument(outputStream, outputFormat);
        outputStream.Position = 0;
        return outputStream;
    }

    enum FindReplaceActionType {
        Replace,
        Remove,
        Highlight
    }
}