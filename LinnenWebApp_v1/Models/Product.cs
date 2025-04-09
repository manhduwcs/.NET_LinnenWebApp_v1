using System;

namespace LinnenWebApp_v1.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public int SupplierID { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
        public string CompanyName { get; set; } // Added from Suppliers
        public string CategoryName { get; set; } // Added from Categories
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
        public short ReorderLevel { get; set; }
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