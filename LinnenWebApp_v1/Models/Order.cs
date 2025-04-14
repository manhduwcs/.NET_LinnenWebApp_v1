using System;
using System.ComponentModel.DataAnnotations;

namespace LinnenWebApp_v1.Models;
public class Order
{
    public int OrderID { get; set; }
    public string CustomerID { get; set; }
    public string CustomerCompanyName { get; set; }
    public int EmployeeID { get; set; }
    public string EmployeeFirstName { get; set; }
    public string EmployeeLastName { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public int ShipVia { get; set; }
    public string ShipperCompanyName { get; set; }
    public decimal Freight { get; set; }

    [MaxLength(40, ErrorMessage = "Ship Name cannot exceed 40 characters.")]
    public string ShipName { get; set; }

    [MaxLength(40, ErrorMessage = "Ship Address cannot exceed 40 characters.")]
    public string ShipAddress { get; set; }

    [MaxLength(15, ErrorMessage = "Ship City cannot exceed 15 characters.")]
    public string ShipCity { get; set; }

    [MaxLength(15, ErrorMessage = "Ship Region cannot exceed 15 characters.")]
    public string ShipRegion { get; set; }

    [MaxLength(10, ErrorMessage = "Ship Postal Code cannot exceed 10 characters.")]
    public string ShipPostalCode { get; set; }

    [MaxLength(15, ErrorMessage = "Ship Country cannot exceed 15 characters.")]
    public string ShipCountry { get; set; }

    public Order()
    {
        OrderID = 0;
        CustomerID = string.Empty;
        CustomerCompanyName = string.Empty;
        EmployeeID = 0;
        EmployeeFirstName = string.Empty;
        EmployeeLastName = string.Empty;
        EmployeeFullName = EmployeeFirstName + " " + EmployeeLastName;
        OrderDate = DateTime.Now;
        RequiredDate = null;
        ShippedDate = null;
        ShipVia = 0;
        ShipperCompanyName = string.Empty;
        Freight = 0.0m;
        ShipName = string.Empty;
        ShipAddress = string.Empty;
        ShipCity = string.Empty;
        ShipRegion = string.Empty;
        ShipPostalCode = string.Empty;
        ShipCountry = string.Empty;
    }
}