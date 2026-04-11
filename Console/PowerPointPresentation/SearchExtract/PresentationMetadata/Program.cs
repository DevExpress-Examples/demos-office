using System.Text;
using DevExpress.Docs.Presentation;

class Program {
    const string InputFilePath = @"Documents\Sample.pptx";
    const string OutputFileName = "Document_Metadata.txt";
    const bool ReadDocumentProperties = true;
    const bool ReadCustomProperties = true;

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);

            using var inputStream = File.OpenRead(inputPath);
            var metadataText = ReadDocumentPropertiesText(inputStream, ReadDocumentProperties, ReadCustomProperties);
            File.WriteAllText(outputPath, metadataText, Encoding.UTF8);

            Console.WriteLine($"Created {outputPath}");
            return 0;
        }
        catch(Exception exception) {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    static string ReadDocumentPropertiesText(Stream inputStream, bool readDocumentProperties, bool readCustomProperties) {
        var output = new StringBuilder();

        using var presentation = new Presentation(inputStream);

        if(readDocumentProperties) {
            var coreProperties = presentation.DocumentProperties;
            output.AppendLine("CORE DOCUMENT PROPERTIES:");
            output.AppendLine(new string('-', 30));
            output.AppendLine($"Title: {coreProperties.Title ?? string.Empty}");
            output.AppendLine($"Author: {coreProperties.Author ?? string.Empty}");
            output.AppendLine($"Subject: {coreProperties.Subject ?? string.Empty}");
            output.AppendLine($"Keywords: {coreProperties.Keywords ?? string.Empty}");
            output.AppendLine($"Category: {coreProperties.Category ?? string.Empty}");
            output.AppendLine($"Company: {coreProperties.Company ?? string.Empty}");
            output.AppendLine($"Manager: {coreProperties.Manager ?? string.Empty}");
            output.AppendLine($"Created Date: {coreProperties.Created:yyyy-MM-dd HH:mm:ss}");
            output.AppendLine($"Last Printed: {coreProperties.Printed:yyyy-MM-dd HH:mm:ss}");
            output.AppendLine();
        }

        if(readCustomProperties) {
            var customProperties = presentation.DocumentProperties.CustomProperties;
            output.AppendLine("CUSTOM PROPERTIES:");
            output.AppendLine(new string('-', 30));

            var hasCustomProperties = false;
            foreach(string propertyName in customProperties.Keys) {
                hasCustomProperties = true;
                var propertyValue = customProperties[propertyName];
                output.AppendLine($"{propertyName}: {propertyValue} ({propertyValue.GetType().Name})");
            }

            if(!hasCustomProperties)
                output.AppendLine("No custom properties found.");

            output.AppendLine();
        }

        return output.ToString();
    }
}