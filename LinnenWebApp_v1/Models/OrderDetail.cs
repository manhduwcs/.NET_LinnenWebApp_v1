using System;
using System.ComponentModel.DataAnnotations;

namespace LinnenWebApp_v1.Models;

public class OrderDetail
{
    [Required(ErrorMessage = "Order ID is required.")]
    public int OrderID { get; set; }

    [Required(ErrorMessage = "Product ID is required.")]
    public int ProductID { get; set; }

    [Required(ErrorMessage = "Unit Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit Price must be greater than zero.")]
    public decimal UnitPrice { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, short.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public short Quantity { get; set; }

    [Range(0.0, 1.0, ErrorMessage = "Discount must be between 0 and 1.")]
    public double Discount { get; set; }

    public OrderDetail()
    {
        OrderID = 0;
        ProductID = 0;
        UnitPrice = 0.0m;
        Quantity = 1;
        Discount = 0.0;
    }
}