using System;
using System.ComponentModel.DataAnnotations;

namespace LinnenWebApp_v1.Models;
public class Customer
{
    [Required(ErrorMessage = "Customer ID is required.")]
    [StringLength(5, ErrorMessage = "Customer ID cannot exceed 5 characters.")]
    public string CustomerID { get; set; }

    [Required(ErrorMessage = "Company Name is required.")]
    [StringLength(40, ErrorMessage = "Company Name cannot exceed 40 characters.")]
    public string CompanyName { get; set; }

    [StringLength(40, ErrorMessage = "Contact Name cannot exceed 40 characters.")]
    public string ContactName { get; set; }

    [StringLength(30, ErrorMessage = "Contact Title cannot exceed 30 characters.")]
    public string ContactTitle { get; set; }

    [StringLength(60, ErrorMessage = "Address cannot exceed 60 characters.")]
    public string Address { get; set; }

    [StringLength(15, ErrorMessage = "City cannot exceed 15 characters.")]
    public string City { get; set; }

    [StringLength(15, ErrorMessage = "Region cannot exceed 15 characters.")]
    public string Region { get; set; }

    [StringLength(24, ErrorMessage = "Phone cannot exceed 24 characters.")]
    public string Phone{get;set;}

    [StringLength(10, ErrorMessage = "Postal Code cannot exceed 10 characters.")]
    public string PostalCode { get; set; }

    [StringLength(24, ErrorMessage = "Country cannot exceed 24 characters.")]
    public string Country { get; set; }

    [StringLength(24, ErrorMessage = "Fax cannot exceed 24 characters.")]
    public string Fax { get; set; }

    public Customer()
    {
        CustomerID = string.Empty;
        CompanyName = string.Empty;
        ContactName = string.Empty;
        ContactTitle = string.Empty;
        Address = string.Empty;
        City = string.Empty;
        Region = string.Empty;
        PostalCode = string.Empty;
        Phone = string.Empty;
        Country = string.Empty;
        Fax = string.Empty;
    }
}