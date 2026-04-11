#region Code
using DevExpress.Office.Services;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
#endregion Code
using Microsoft.AspNetCore.Mvc;
using BlazorDemo.OfficeFileApi.Models.WordProcessing.MailMergeFields;

namespace BlazorDemo.OfficeFileApi.Controllers.WordProcessing.MailMergeFields {
    [ApiController]
    [Route("api/wordprocessing/mail-merge")]
    public class WordMailMergeController : WordControllerBase {
        [HttpPost("process")]
        public async Task<IActionResult> CreateMailMergedDocument(IFormFile file, [FromForm] WordMailMergeModel model) {
            using var inputStream = new MemoryStream();
            await file.CopyToAsync(inputStream);
            inputStream.Position = 0;

            var targetFormat = WordFormatHelper.GetDocumentFormat(model.OutputFormat);
            var selectedEmployee = model.Employees.FirstOrDefault(employee => employee.EmployeeID == model.EmployeeId);
            if(selectedEmployee != null) {
                var selectedCustomer = model.Customers.FirstOrDefault(customer => customer.CustomerID == model.CustomerId);
                if(selectedCustomer != null) {
                    selectedEmployee.Customer = selectedCustomer;
                }
            }

            var outputStream = MailMergeDocument(inputStream, selectedEmployee, model.IsPdfOutput, targetFormat);

            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }
        #region Code
        MemoryStream MailMergeDocument(Stream stream, Employee selectedEmployee, bool isPdfOutput, DocumentFormat targetFormat) {
            using var wordProcessor = new RichEditDocumentServer();

            var uriService = (IUriStreamService)wordProcessor.GetService(typeof(IUriStreamService));
            uriService.RegisterProvider(new WordMailMergeUriStreamProvider());

            wordProcessor.LoadDocument(stream);

            if(selectedEmployee == null)
                return null;

            var dataSource = new List<Employee>() { selectedEmployee };
            var options = wordProcessor.CreateMailMergeOptions();
            options.DataSource = dataSource;
            options.FirstRecordIndex = 0;
            options.LastRecordIndex = 0;

            using var tempWordProcessor = new RichEditDocumentServer();
            wordProcessor.MailMerge(options, tempWordProcessor.Document);

            var outputStream = new MemoryStream();
            if(isPdfOutput)
                tempWordProcessor.ExportToPdf(outputStream);
            else
                tempWordProcessor.SaveDocument(outputStream, targetFormat);

            outputStream.Position = 0;
            return outputStream;
        }
        public class WordMailMergeUriStreamProvider : IUriStreamProvider {
            const string Prefix = "dbimg://";
            Stream IUriStreamProvider.GetStream(string uri) {
                uri = uri.Trim();
                if(!uri.StartsWith(Prefix))
                    return null;
                var strId = uri.Substring(Prefix.Length).Trim();
                int id;
                if(!int.TryParse(strId, out id))
                    return null;
                var fileName = $"Documents/Photo{id}.jpeg";
                var bytes = new byte[0];
                using(FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {
                    var length = (int)fs.Length;
                    bytes = new byte[length];
                    fs.ReadExactly(bytes, 0, length);
                }
                return new MemoryStream(bytes);
            }
        }
        #endregion Code
    }
}
