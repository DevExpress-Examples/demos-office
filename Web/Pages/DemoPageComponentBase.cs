using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using BlazorDemo.OfficeFileApi.Shared;
using DevExpress.Data.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazorDemo.OfficeFileApi.Pages {
    /// <summary>
    /// Base class for demo pages that handle file upload and download operations.
    /// </summary>
    public abstract class DemoPageComponentBase : ComponentBase {
#pragma warning disable DX0006 // Used via AsyncDownloader
        [Inject] protected HttpClient Http { get; set; } = default!;
#pragma warning restore DX0006
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

        protected bool loading = false;
        protected DemoUploader fileUploadRef;
        protected DownloadButton downloadButton;

        /// <summary>
        /// Creates multipart form content with file content, uploaded file and common parameters.
        /// </summary>
        protected MultipartFormDataContent CreateMultipartContent(byte[] fileBytes, string fileFieldName = "File", string fileName = "FileName") {
            if(fileBytes == null || fileBytes.Length == 0)
                throw new InvalidOperationException("No file uploaded");

            var content = new MultipartFormDataContent();

            // Add file
            var streamContent = new StreamContent(new MemoryStream(fileBytes));
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Add(streamContent, fileFieldName, fileName);

            return content;
        }

        /// <summary>
        /// Creates multipart form content with uploaded file and common parameters.
        /// </summary>
        protected MultipartFormDataContent CreateMultipartContent(string fileFieldName = "File", string fileName = "FileName") {
            return CreateMultipartContent(fileUploadRef?.FileBytes, fileFieldName, fileName);
        }
        /// <summary>
        /// Adds common form parameters to multipart content.
        /// </summary>
        protected void AddCommonParameters(MultipartFormDataContent content, string outputFormat, string outputName) {
            content.Add(new StringContent(outputFormat), "OutputFormat");
            content.Add(new StringContent(outputName), "OutputName");
        }
        protected void AddDocumentsContent(MultipartFormDataContent content) {
            if(fileUploadRef == null)
                throw new InvalidOperationException("DemoUploader is not initialized");

            if(fileUploadRef.UseDefaultFile) {
                foreach(var file in fileUploadRef.DefaultFileInfoList)
                    AddDocumentContent(content, file.Content);
            } else {
                foreach(var file in fileUploadRef.FileBytesList)
                    AddDocumentContent(content, file.Value);
            }
        }
        void AddDocumentContent(MultipartFormDataContent content, byte[] fileBytes) {
            var streamContent = new StreamContent(new MemoryStream(fileBytes));
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Add(streamContent, "files", "FileName");
        }

        /// <summary>
        /// Executes an API call with multipart content and downloads the result.
        /// </summary>
        protected async Task<bool> ExecuteDownloadOperation(string apiUrl, MultipartFormDataContent content, string outputName, string outputFormat) {
            try {
                string absoluteApiUrl = NavigationManager.ToAbsoluteUri(apiUrl).ToString();
                var result = await ExecuteDownloadOperation(absoluteApiUrl, content);
                if(result.ExceptionInfo != null)
                    result.ExceptionInfo.Throw();
                string finalFormat = result.MediaType.Contains("zip") ? "zip" : outputFormat;
                await downloadButton!.Download($"{outputName}.{finalFormat}", result.Stream);
                return true;
            } catch(Exception) {
                throw;
            }
        }

        async Task<ResponseResult> ExecuteDownloadOperation(string apiUrl, MultipartFormDataContent dataContent) {
            var lifeTime = AsyncDownloader<ResponseResult>.CreateLifeTime(Http);
            return await AsyncDownloader<ResponseResult>.LoadAsync(
                apiUrl,
                handler: (o, e, s) => new ResponseResult(o, e, s),
                cancellationToken: default,
                options: new AsyncDownloader<ResponseResult>.PostMethodOptions(() => dataContent, null, lifeTime));
        }

        sealed class ResponseResult {
            readonly string mediaType;
            readonly ExceptionDispatchInfo exceptionInfo;
            readonly Stream stream;

            public ResponseResult(object response, ExceptionDispatchInfo exceptionInfo, Stream stream) {
                var responseMessage = response as HttpResponseMessage;
                mediaType = responseMessage?.Content.Headers.ContentType?.MediaType ?? string.Empty;
                this.exceptionInfo = exceptionInfo;
                this.stream = stream;
            }

            public string MediaType => mediaType;
            public ExceptionDispatchInfo ExceptionInfo => exceptionInfo;
            public Stream Stream => stream;
        }

        /// <summary>
        /// Standard processing wrapper for download operations.
        /// </summary>
        protected virtual async Task ProcessDownloadWithLoading(Func<Task> operation) {
            try {
                loading = true;
                await operation();
            } catch(Exception) {
                throw;
            } finally {
                loading = false;
                StateHasChanged();
            }
        }
    }
}
