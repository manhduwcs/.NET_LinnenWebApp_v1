using System.ComponentModel.DataAnnotations;

namespace LinnenWebApp_v1.Models
{
    public class Shipper
    {
        public int ShipperID { get; set; }
        [Required(ErrorMessage = "Company Name Name is required.")]
        [StringLength(40, ErrorMessage = "Company Name cannot be longer than 40 characters.")]
        public string CompanyName { get; set; }
        public string Phone { get; set; }
    }
}
