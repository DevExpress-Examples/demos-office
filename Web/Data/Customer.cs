namespace BlazorDemo.OfficeFileApi;

public class Customer {

    public Customer(string customerId, string companyName, string contactName, string contactTitle, string address, string city, string postalCode) {
        CustomerID = customerId;
        CompanyName = companyName;
        ContactName = contactName;
        ContactTitle = contactTitle;
        Address = address;
        City = city;
        PostalCode = postalCode;
    }

    public string CustomerID { get; set; }
    public string CompanyName { get; set; }
    public string ContactName { get; set; }
    public string ContactTitle { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }

}
