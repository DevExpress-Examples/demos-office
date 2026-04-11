namespace BlazorDemo.OfficeFileApi;

public class Employee {

    public Employee(int employeeId, string firstName, string lastName, string address, string city, string postalCode, string title, string homePhone, byte[] picture = null) {
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
    public Customer Customer { get; set; }
    public byte[] Picture { get; set; }
}
