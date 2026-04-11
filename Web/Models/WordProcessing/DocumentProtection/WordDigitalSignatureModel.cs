using DevExpress.Office.DigitalSignatures;

namespace BlazorDemo.OfficeFileApi.Models.WordProcessing.DocumentProtection {
    public class WordDigitalSignatureModel : WordFileModelBase {
        public override string OutputName { get; set; } = "Signed_Document";
        public CommitmentType Reason { get; set; } = CommitmentType.ProofOfApproval;
        public string Role { get; set; } = "Sales Representative";
        public string Comments { get; set; } = "Demo Digital Signature";
        public string Country { get; set; } = "USA";
        public string State { get; set; } = "WA";
        public string City { get; set; } = "Seattle";
        public string Address1 { get; set; } = "507 - 20th Ave. E.";
        public string Address2 { get; set; } = "Apt. 2A";
        public string PostalCode { get; set; } = "98122";
        public string TsaServer { get; set; } = "https://freetsa.org/tsr";
        public HashAlgorithmType HashAlgorithm { get; set; } = HashAlgorithmType.SHA256;
        internal readonly List<object> HashAlgorithms = new()
    {
        new { Text = "SHA256", Value = HashAlgorithmType.SHA256 },
        new { Text = "SHA384", Value = HashAlgorithmType.SHA384 },
        new { Text = "SHA512", Value = HashAlgorithmType.SHA512 }
    };
        internal readonly List<object> Reasons = new()
    {
        new { Text = "Approved this document", Value = CommitmentType.ProofOfApproval },
        new { Text = "Created this document", Value = CommitmentType.ProofOfCreation },
        new { Text = "Created and Approved this document", Value = CommitmentType.ProofOfOrigin }
    };
    }
}
