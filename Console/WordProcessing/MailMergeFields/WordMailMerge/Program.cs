using System.Text.Json;
using DevExpress.Office.Services;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;

class Program {
    const string InputFilePath = @"Documents\MailMerge_Template.docx";
    const string DataSourcePath = @"Documents\nwind_data.json";
    const string OutputFileName = "MailMerge_Result.pdf";
    const string OutputFormat = "pdf";
    const bool IsPdfOutput = true;
    const int EmployeeId = 1;
    const string CustomerId = "ALFKI";

    static readonly Employee[] Employees = {
        new(1, "Nancy", "Davolio", "507 - 20th Ave. E.\nApt. 2A", "Seattle", "98122", "Sales Representative", "(206) 555-9857")
    };

    static int Main() {
        try {
            var inputPath = Path.Combine(AppContext.BaseDirectory, InputFilePath);
            var outputPath = Path.Combine(AppContext.BaseDirectory, OutputFileName);
            var selectedEmployee = Employees.FirstOrDefault(employee => employee.EmployeeID == EmployeeId);
            if(selectedEmployee != null) {
                var selectedCustomer = PrepareDataSource().FirstOrDefault(customer => customer.CustomerId == CustomerId);
                if(selectedCustomer != null)
                    selectedEmployee.Customer = selectedCustomer;
            }

            using var inputStream = File.OpenRead(inputPath);
            using var outputStream = MailMergeDocument(inputStream, selectedEmployee, IsPdfOutput, GetDocumentFormat(OutputFormat));
            using var fileStream = File.Create(outputPath);
            outputStream.CopyTo(fileStream);

            Console.WriteLine($"Created {outputPath}");
            return 0;
        }
        catch(Exception exception) {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    static MemoryStream MailMergeDocument(Stream stream, Employee? selectedEmployee, bool isPdfOutput, DocumentFormat targetFormat) {
        using var wordProcessor = new RichEditDocumentServer();

        var uriService = (IUriStreamService)wordProcessor.GetService(typeof(IUriStreamService));
        uriService.RegisterProvider(new WordMailMergeUriStreamProvider());

        wordProcessor.LoadDocument(stream);

        if(selectedEmployee == null)
            throw new InvalidOperationException("Employee data was not found.");

        var dataSource = new List<Employee> { selectedEmployee };
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

    static List<NWind.Customer> PrepareDataSource() {
        try {
            var dataSourcePath = Path.Combine(AppContext.BaseDirectory, DataSourcePath);
            using var dataStream = new FileStream(dataSourcePath, FileMode.Open, FileAccess.Read);
            var nwind = JsonSerializer.Deserialize<NWind>(dataStream);
            return nwind?.Customers ?? new List<NWind.Customer>();
        }
        catch(Exception exception) {
            Console.Error.WriteLine($"Deserialization failed: {exception}");
            return new List<NWind.Customer>();
        }
    }

    static DocumentFormat GetDocumentFormat(string format) {
        return format.ToLowerInvariant() switch {
            "doc" => DocumentFormat.Doc,
            "docx" => DocumentFormat.Docx,
            "docm" => DocumentFormat.Docm,
            "dotx" => DocumentFormat.Dotx,
            "dotm" => DocumentFormat.Dotm,
            "rtf" => DocumentFormat.Rtf,
            "pdf" => DocumentFormat.Undefined,
            _ => DocumentFormat.Docx
        };
    }

    class WordMailMergeUriStreamProvider : IUriStreamProvider {
        const string Prefix = "dbimg://";

        public Stream? GetStream(string uri) {
            uri = uri.Trim();
            if(!uri.StartsWith(Prefix))
                return null;

            var idText = uri[Prefix.Length..].Trim();
            if(!int.TryParse(idText, out var id))
                return null;

            var filePath = Path.Combine(AppContext.BaseDirectory, "Documents", $"Photo{id}.jpeg");
            return File.Exists(filePath) ? File.OpenRead(filePath) : null;
        }
    }

    class Employee {
        public Employee(int employeeId, string firstName, string lastName, string address, string city, string postalCode, string title, string homePhone, byte[]? picture = null) {
            EmployeeID = employeeId;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            City = city;
            PostalCode = postalCode;
            Title = title;
            HomePhone = homePhone;
            Picture = picture;
        }

        public int EmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Title { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string HomePhone { get; set; }
        public NWind.Customer? Customer { get; set; }
        public byte[]? Picture { get; set; }
    }

    class NWind {
        public List<Customer> Customers { get; set; } = new();

        public record Customer(
            string CustomerId,
            string CompanyName,
            string ContactName,
            string Country,
            string Address,
            string City,
            string Phone,
            List<Order> Orders);

        public record Order(
            string OrderID,
            DateTime? OrderDate,
            string ShipCountry,
            double Freight,
            List<OrderDetails> OrderDetails);

        public record OrderDetails(
            int ProductId,
            string ProductName,
            int Quantity,
            decimal UnitPrice,
            double Discount);
    }
}