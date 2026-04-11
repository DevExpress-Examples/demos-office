namespace BlazorDemo.OfficeFileApi.Models.PdfProcessing.MergeSplit {
    public class PdfSplitDocumentsModel : PdfPageRangeModelBase {
        public override string OutputFormat { get => "zip"; }
        public override string OutputName { get => "Documents"; }
    }
}
