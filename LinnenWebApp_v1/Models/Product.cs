using System;
using System.ComponentModel.DataAnnotations;

namespace LinnenWebApp_v1.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Supplier ID is required.")]
        public int SupplierID { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Product Name must be between 3 and 80 characters.")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Company Name is required.")]
        public string CompanyName { get; set; } // Added from Suppliers
        [Required(ErrorMessage = "Category Name is required.")]
        public string CategoryName { get; set; } // Added from Categories

        [Required(ErrorMessage = "Quantity Per Unit is required.")]
        public string QuantityPerUnit { get; set; }

        [Required(ErrorMessage = "Unit Price is required.")]
        [Range(0.01, (double) decimal.MaxValue, ErrorMessage = "Unit Price must be greater than 0.")]
        public decimal UnitPrice { get; set; }

        [Range(0, 10000, ErrorMessage = "Units In Stock must be between 0 and 10000.")]
        public int UnitsInStock { get; set; }

        [Range(0, 10000, ErrorMessage = "Units On Order must be between 0 and 10000.")]
        public int UnitsOnOrder { get; set; }

        [Range(0, 10000, ErrorMessage = "Reorder Level must be between 0 and 10000.")]
        public int ReorderLevel { get; set; }

        public bool Discontinued { get; set; }


        // Default constructor
        public Product()
        {
            ProductID = -1;
            SupplierID = -1;
            CategoryID = -1;
            ProductName = "";
            CompanyName = "";
            CategoryName = "";
            QuantityPerUnit = "";
            UnitPrice = 0.0m;
            UnitsInStock = 0;
            ReorderLevel = 0;
            Discontinued = false;
        }
    }
}