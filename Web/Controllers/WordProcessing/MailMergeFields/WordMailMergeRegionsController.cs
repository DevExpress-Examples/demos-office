#region Code
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using System.Text.Json;
#endregion Code
using BlazorDemo.DemoData;
using BlazorDemo.OfficeFileApi.Models.WordProcessing.MailMergeFields;
using Microsoft.AspNetCore.Mvc;

namespace BlazorDemo.OfficeFileApi.Controllers.WordProcessing.MailMergeFields {
    [ApiController]
    [Route("api/wordprocessing/mail-merge-regions")]
    public class WordMailMergeRegionsController : WordControllerBase {
        [HttpPost("process")]
        public async Task<IActionResult> CreateMailMergedDocument(IFormFile file, [FromForm] WordMailMergeRegionsModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            var targetFormat = WordFormatHelper.GetDocumentFormat(model.OutputFormat);

            var outputStreams = MailMergeDocument(inputStream, model.MergeMode, model.RecordExportMode == RecordExportMode.ActiveRecord, model.ActiveRecordIndex, model.DocumentExportMode == DocumentExportMode.SingleFile, model.IsPdfOutput, targetFormat);

            return CreateZipOutputFile(outputStreams, model.OutputFormat, "record", model.OutputName);
        }
        #region Code
        List<Stream> MailMergeDocument(Stream stream, MergeMode mergeMode, bool exportActiveRecord, int activeRecordIndex,
            bool exportAsOneDocument, bool isPdfOutput, DocumentFormat targetFormat) {
            using var wordProcessor = new RichEditDocumentServer();

            wordProcessor.LoadDocument(stream);

            var options = wordProcessor.CreateMailMergeOptions();
            var dataSource = PrepareDataSource();

            var firstRecordIndex = exportActiveRecord ? activeRecordIndex : 0;
            var lastRecordIndex = exportActiveRecord ? activeRecordIndex : dataSource.Count - 1;

            options.DataSource = dataSource;
            options.MergeMode = mergeMode;
            options.FirstRecordIndex = firstRecordIndex;
            options.LastRecordIndex = lastRecordIndex;

            using var tempWordProcessor = new RichEditDocumentServer();
            var outputStreams = new List<Stream>();

            if(exportAsOneDocument) {
                var result = MailMerge(wordProcessor, tempWordProcessor, options, isPdfOutput, targetFormat);
                outputStreams.Add(result);
            } else {
                for(int i = firstRecordIndex; i <= lastRecordIndex; i++) {
                    tempWordProcessor.CreateNewDocument();
                    options.FirstRecordIndex = i;
                    options.LastRecordIndex = i;
                    var result = MailMerge(wordProcessor, tempWordProcessor, options, isPdfOutput, targetFormat);
                    outputStreams.Add(result);
                }
            }

            return outputStreams;
        }

        List<NWind.Customer> PrepareDataSource() {
            try {
                using var dsStream = new FileStream(DemoDocumentPaths.NWindDataSource, FileMode.Open, FileAccess.Read);
                var nwind = JsonSerializer.Deserialize<NWind>(dsStream);
                return nwind?.Customers ?? new List<NWind.Customer>();
            } catch(Exception ex) {
                Console.Error.WriteLine($"Deserialization failed: {ex}");
                return new List<NWind.Customer>();
            }
        }

        Stream MailMerge(RichEditDocumentServer wordProcessor, RichEditDocumentServer targetProcessor, MailMergeOptions options, bool isPdfOutput, DocumentFormat targetFormat) {
            wordProcessor.MailMerge(options, targetProcessor.Document);

            var outputStream = new MemoryStream();

            if(isPdfOutput)
                targetProcessor.ExportToPdf(outputStream);
            else
                targetProcessor.SaveDocument(outputStream, targetFormat);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
