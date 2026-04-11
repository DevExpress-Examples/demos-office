namespace BlazorDemo.OfficeFileApi.Models.Spreadsheet.DocumentProtection {
    public class ExcelDigitalSignatureModel : ExcelFileModelBase {
        public override string OutputName { get; set; } = "Signed_Document";
        public string TemplateName => "Template.xlsx";

        public string Reason { get; set; } = "approved";
        public string Role { get; set; } = "Sales Representative";
        public string Comments { get; set; } = "Demo Digital Signature";
        public string Country { get; set; } = "USA";
        public string State { get; set; } = "WA";
        public string City { get; set; } = "Seattle";
        public string Address1 { get; set; } = "507 - 20th Ave. E.";
        public string Address2 { get; set; } = "Apt. 2A";
        public string PostalCode { get; set; } = "98122";
        public string TsaServer { get; } = "https://freetsa.org/tsr";
        public string HashAlgorithm { get; set; } = "SHA256";

        internal readonly List<object> HashAlgorithms = new()
    {
        new { Text = "SHA256", Value = "SHA256" },
        new { Text = "SHA384", Value = "SHA384" },
        new { Text = "SHA512", Value = "SHA512" }
    };
        internal readonly List<object> Reasons = new()
    {
        new { Text = "Approved this document", Value = "approved" },
        new { Text = "Created this document", Value = "created" },
        new { Text = "Created and Approved this document", Value = "created_approved" }
    };
    }
}
