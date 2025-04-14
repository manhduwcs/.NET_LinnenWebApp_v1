using System.ComponentModel.DataAnnotations;

namespace LinnenWebApp_v1.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(40, ErrorMessage = "Company Name cannot be longer than 40 characters.")]
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string HomePage { get; set; }

        public Supplier()
        {
            SupplierID = -1;
            CompanyName = "";
            ContactName = "";
            ContactTitle = "";
            Address = "";
            City = "";
            Region = "";
            PostalCode = "";
            Country = "";
            Phone = "";
            Fax = "";
            HomePage = "";
        }

    }
}
