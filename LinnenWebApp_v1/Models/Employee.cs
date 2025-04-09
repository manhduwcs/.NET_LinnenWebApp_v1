namespace LinnenWebApp_v1.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }  // Primary Key

        public string FirstName { get; set; }  // nvarchar(20)

        public string LastName { get; set; }  // nvarchar(20)

        public string Title { get; set; }  // nvarchar(25), nullable

        public string TitleOfCourtesy { get; set; }  // nvarchar(25), nullable

        public DateTime? BirthDate { get; set; }  // datetime, nullable

        public DateTime? HireDate { get; set; }  // datetime, nullable

        public string Address { get; set; }  // nvarchar(255), nullable

        public string City { get; set; }  // nvarchar(15), nullable

        public string Region { get; set; }  // nvarchar(15), nullable

        public string PostalCode { get; set; }  // nvarchar(10), nullable

        public string Country { get; set; }  // nvarchar(15), nullable

        public string HomePhone { get; set; }  // nvarchar(24), nullable

        public string Extension { get; set; }  // nvarchar(4), nullable

        public string Notes { get; set; }  // text, nullable

        public string PhotoPath { get; set; }  // nvarchar(255), nullable

        public int? ReportsTo { get; set; }  // Foreign Key, nullable
    }
}
