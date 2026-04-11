using Microsoft.AspNetCore.Mvc;

namespace BlazorDemo.OfficeFileApi.Controllers {
    public class FileStreamControllerBase : ControllerBase {
        internal FileContentResult CreateOutputFile(Stream stream, string outputFormat, string fileName) {
            if(stream == null || stream.Length == 0)
                throw new ArgumentException("Stream cannot be null or empty", nameof(stream));
            var memoryStream = stream as MemoryStream ?? throw new ArgumentException("Stream must be a MemoryStream", nameof(stream));

            var safeFileName = Path.GetFileName(fileName);
            var outputFileName = Path.ChangeExtension(safeFileName, outputFormat);

            var contentType = GetMimeContentType(outputFormat);
            return new FileContentResult(memoryStream.ToArray(), contentType) {
                FileDownloadName = outputFileName
            };
        }
        internal FileContentResult CreateZipOutputFile(IReadOnlyList<Stream> streams, string entryFormat, string entryPrefix, string fileName) {
            using var zipStream = new MemoryStream();
            using(var archive = new System.IO.Compression.ZipArchive(zipStream, System.IO.Compression.ZipArchiveMode.Create)) {
                for(int i = 0; i < streams.Count; i++) {
                    var entryName = $"{entryPrefix}_{i + 1:D3}.{entryFormat.ToLowerInvariant()}";
                    var entry = archive.CreateEntry(entryName);

                    using var entryStream = entry.Open();
                    var imageData = ReadStreamToBytes(streams[i]);
                    entryStream.Write(imageData, 0, imageData.Length);
                }
            }
            return new FileContentResult(zipStream.ToArray(), "application/zip") {
                FileDownloadName = fileName
            };
        }
        byte[] ReadStreamToBytes(Stream stream) {
            if(stream == null)
                return Array.Empty<byte>();
            stream.Position = 0;
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
        public virtual string GetMimeContentType(string format) => "application/octet-stream";
    }
}
