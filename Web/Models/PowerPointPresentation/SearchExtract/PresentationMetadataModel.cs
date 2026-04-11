namespace BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.SearchExtract
{
    public class PresentationMetadataModel : PresentationModelBase
    {
        public bool ReadDocumentProperties { get; set; } = true;
        
        public bool ReadCustomProperties { get; set; } = true;
        public override string OutputName { get; set; } = "Document_Metadata";
        public override string OutputFormat { get; set; }= "txt"; 
    }
}