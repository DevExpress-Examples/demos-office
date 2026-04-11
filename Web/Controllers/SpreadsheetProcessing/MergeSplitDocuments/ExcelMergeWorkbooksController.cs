#region Code
using DevExpress.Spreadsheet;
#endregion Code
using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Controllers.Spreadsheet;
using BlazorDemo.OfficeFileApi.Models.Spreadsheet.MergeSplitDocuments;
using BlazorDemo.OfficeFileApi.Services;

namespace BlazorDemo.OfficeFileApi.Controllers.SpreadsheetProcessing.MergeSplitDocuments {
    [ApiController]
    [Route("api/spreadsheetprocessing")]
    public class ExcelMergeWorkbooksController : SpreadsheetControllerBase {
        [HttpPost("merge-workbooks/merge")]
        public async Task<IActionResult> Merge(IFormFile[] files, [FromForm] ExcelMergeWorkbooksModel model) {
            if(files == null || files.Length == 0) {
                return BadRequest("No files are uploaded");
            }

            var inputStreams = new List<Stream>();
            foreach(var file in files) {
                var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                ms.Position = 0;
                inputStreams.Add(ms);
            }

            DocumentFormat outputFormat = ExcelFormatHelper.GetDocumentFormat(model.OutputFormat);

            var documentStream = MergeWorkbooks(inputStreams, outputFormat);

            return CreateOutputFile(documentStream, model.OutputFormat, model.OutputName);
        }
        #region Code
        Stream MergeWorkbooks(List<Stream> inputStreams, DocumentFormat outputFormat) {
            using var finalWorkbook = new Workbook();

            finalWorkbook.LoadDocument(inputStreams[0]);

            for(int i = 1; i < inputStreams.Count; i++) {
                using var currentWorkbook = new Workbook();
                currentWorkbook.LoadDocument(inputStreams[i]);
                finalWorkbook.Append(currentWorkbook);
            }

            var outputStream = new MemoryStream();

            finalWorkbook.SaveDocument(outputStream, outputFormat);

            outputStream.Position = 0;
            return outputStream;
        }
        #endregion Code
    }
}
