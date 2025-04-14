using System.Data;
using System.Diagnostics;
using System.Reflection;
using LinnenWebApp_v1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

public class ProductController : Controller
{
    // GET: Products
    public List<Product> productList = new List<Product>();
    private static string connectionString;

    public ProductController(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    public IActionResult Index()
    {
        productList = GetAllProducts();
        productList.Reverse();
        return View(productList);
    }

    // GET: Products/Details/5
    public IActionResult Details(int id)
    {
        Product pr = GetProductByID(id);
        return View(pr);
    }

    // GET: Products/Create
    public IActionResult Create()
    {
        ViewBag.Suppliers = GetSupplierChoices();
        ViewBag.Categories = GetCategoryChoices();
        return View();
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product, IFormCollection form)
    {
        try
        {
            // if (!ModelState.IsValid)
            // {
            //     ViewBag.Suppliers = GetSupplierChoices();
            //     ViewBag.Categories = GetCategoryChoices();
            //     return View(product);
            // }

            string productName = form["ProductName"];
            int supplierID = int.Parse(form["SupplierID"]);
            int categoryID = int.Parse(form["CategoryID"]);
            string quantityPerUnit = form["QuantityPerUnit"];
            decimal unitPrice = decimal.Parse(form["UnitPrice"]);
            int unitsInStock = int.Parse(form["UnitsInStock"]);
            int unitsOnOrder = int.Parse(form["UnitsOnOrder"]);
            int reorderLevel = int.Parse(form["ReorderLevel"]);
            bool discontinued = bool.TryParse(form["Discontinued"], out discontinued);

            Product newProduct = new Product
            {
                ProductName = productName,
                SupplierID = supplierID,
                CategoryID = categoryID,
                QuantityPerUnit = quantityPerUnit,
                UnitPrice = unitPrice,
                UnitsInStock = unitsInStock,
                UnitsOnOrder = unitsOnOrder,
                ReorderLevel = reorderLevel,
                Discontinued = discontinued
            };

            if (CreateProduct(newProduct))
            {
                return RedirectToAction("Index");
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }
        ViewBag.Suppliers = GetSupplierChoices();
        ViewBag.Categories = GetCategoryChoices();
        return View(product);
    }

    // GET: Products/Edit/5
    public IActionResult Edit(int id)
    {
        Product product = GetProductByID(id);
        ViewBag.Suppliers = GetSupplierChoices();
        ViewBag.Categories = GetCategoryChoices();
        return View(product);
    }

    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Product product, IFormCollection form)
    {
        int prID = -1;
        if (!int.TryParse(form["ProductID"], out prID))
        {
            ModelState.AddModelError("ProductName", "There's an error with this product's ProductID !");
            return View(product);
        }

        Console.WriteLine("Edit Product ID = " + prID);
        try
        {
            // We can still validate directly in the form without using IsValid check
            // if (!ModelState.IsValid)
            // {
            //     Console.WriteLine("Run to this line : Invalid model state ");
            //     ViewBag.Suppliers = GetSupplierChoices();
            //     ViewBag.Categories = GetCategoryChoices();
            //     return View(product);
            // }

            string productName = form["ProductName"];
            int supplierID = int.Parse(form["SupplierID"]);
            int categoryID = int.Parse(form["CategoryID"]);
            string quantityPerUnit = form["QuantityPerUnit"];
            decimal unitPrice = decimal.Parse(form["UnitPrice"]);
            int unitsInStock = int.Parse(form["UnitsInStock"]);
            int unitsOnOrder = int.Parse(form["UnitsOnOrder"]);
            int reorderLevel = int.Parse(form["ReorderLevel"]);
            bool discontinued = form.ContainsKey("Discontinued");

            Product newProduct = new Product
            {
                ProductID = prID,
                ProductName = productName,
                SupplierID = supplierID,
                CategoryID = categoryID,
                QuantityPerUnit = quantityPerUnit,
                UnitPrice = unitPrice,
                UnitsInStock = unitsInStock,
                UnitsOnOrder = unitsOnOrder,
                ReorderLevel = reorderLevel,
                Discontinued = discontinued
            };

            if (EditProduct(newProduct))
            {
                Console.WriteLine("Run to this line : Edit Product ");
                return RedirectToAction("Index");
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Run to the error outer line ");
        ViewBag.Suppliers = GetSupplierChoices();
        ViewBag.Categories = GetCategoryChoices();
        return View(product);
    }

    // GET: Products/Delete/5
    public IActionResult Delete(int id)
    {
        // TODO: Retrieve product by id
        return View();
    }

    // POST: Products/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id, IFormCollection form)
    {
        // TODO: Remove the product from the database
        return RedirectToAction(nameof(Index));
    }

    public List<Category> GetCategoryChoices()
    {
        List<Category> catlist = new List<Category>();
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            if (con.State == ConnectionState.Closed) con.Open();
            string query = "SELECT CategoryID, CategoryName from Categories";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Category cat = new Category
                        {
                            CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                            CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                        };
                        catlist.Add(cat);
                    }
                }
            }
        }
        return catlist;
    }
    public List<Supplier> GetSupplierChoices()
    {
        List<Supplier> supList = new List<Supplier>();
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            if (con.State == ConnectionState.Closed) con.Open();
            string query = "select SupplierID, CompanyName from Suppliers";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Supplier sup = new Supplier
                        {
                            SupplierID = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                            CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                        };
                        supList.Add(sup);
                    }
                }
            }
        }
        return supList;
    }

    public bool CreateProduct(Product product)
    {
        try
        {
            string query = "INSERT INTO dbo.Products (SupplierID, CategoryID, ProductName, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued) VALUES(@SupplierID, @CategoryID, @ProductName, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel, @Discontinued); ";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
                    cmd.Parameters.AddWithValue("@SupplierID", product.SupplierID);
                    cmd.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@QuantityPerUnit", product.QuantityPerUnit);
                    cmd.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
                    cmd.Parameters.AddWithValue("@UnitsInStock", product.UnitsInStock);
                    cmd.Parameters.AddWithValue("@UnitsOnOrder", product.UnitsOnOrder);
                    cmd.Parameters.AddWithValue("@ReorderLevel", product.ReorderLevel);
                    cmd.Parameters.AddWithValue("@Discontinued", product.Discontinued);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }
        return false;
    }

    public bool EditProduct(Product product)
    {
        try
        {
            string query = "UPDATE dbo.Products SET SupplierID = @SupplierID, CategoryID = @CategoryID, ProductName = @ProductName, QuantityPerUnit = @QuantityPerUnit, UnitPrice = @UnitPrice, UnitsInStock = @UnitsInStock, UnitsOnOrder = @UnitsOnOrder, ReorderLevel = @ReorderLevel, Discontinued = @Discontinued WHERE ProductID = @ProductID;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
                    cmd.Parameters.AddWithValue("@SupplierID", product.SupplierID);
                    cmd.Parameters.AddWithValue("@CategoryID", product.CategoryID);
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@QuantityPerUnit", product.QuantityPerUnit);
                    cmd.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
                    cmd.Parameters.AddWithValue("@UnitsInStock", product.UnitsInStock);
                    cmd.Parameters.AddWithValue("@UnitsOnOrder", product.UnitsOnOrder);
                    cmd.Parameters.AddWithValue("@ReorderLevel", product.ReorderLevel);
                    cmd.Parameters.AddWithValue("@Discontinued", product.Discontinued);

                    Console.WriteLine($"ProductID: {product.ProductID}");
                    Console.WriteLine($"SupplierID: {product.SupplierID}");
                    Console.WriteLine($"CategoryID: {product.CategoryID}");
                    Console.WriteLine($"ProductName: {product.ProductName}");
                    Console.WriteLine($"QuantityPerUnit: {product.QuantityPerUnit}");
                    Console.WriteLine($"UnitPrice: {product.UnitPrice}");
                    Console.WriteLine($"UnitsInStock: {product.UnitsInStock}");
                    Console.WriteLine($"UnitsOnOrder: {product.UnitsOnOrder}");
                    Console.WriteLine($"ReorderLevel: {product.ReorderLevel}");
                    Console.WriteLine($"Discontinued: {product.Discontinued}");

                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine("Rows Affected Update Product = "+rows);
                    return rows > 0;
                }
            }
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }
        return false;
    }

    public Product GetProductByID(int id)
    {
        Product pr = new();
        string query = "select pr.*, sup.CompanyName, cat.CategoryName from dbo.Products pr join dbo.Suppliers sup on pr.SupplierID = sup.SupplierID join dbo.Categories cat on pr.CategoryID = cat.CategoryID WHERE ProductID=@ProductID";
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            if (con.State == ConnectionState.Closed) con.Open();
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ProductID", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pr = new Product
                        {
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            SupplierID = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                            CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")), // From Suppliers
                            CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")), // From Categories
                            QuantityPerUnit = reader.IsDBNull(reader.GetOrdinal("QuantityPerUnit")) ? "" : reader.GetString(reader.GetOrdinal("QuantityPerUnit")),
                            UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                            UnitsInStock = reader.GetInt16(reader.GetOrdinal("UnitsInStock")),
                            UnitsOnOrder = reader.GetInt16(reader.GetOrdinal("UnitsOnOrder")),
                            ReorderLevel = reader.GetInt16(reader.GetOrdinal("ReorderLevel")),
                            Discontinued = reader.GetBoolean(reader.GetOrdinal("Discontinued"))
                        };

                    }
                }
            }
        }
        return pr;
    }

    public List<Product> GetAllProducts()
    {
        List<Product> products = new List<Product>();
        string query = "select pr.*, sup.CompanyName, cat.CategoryName from dbo.Products pr join dbo.Suppliers sup on pr.SupplierID = sup.SupplierID join dbo.Categories cat on pr.CategoryID = cat.CategoryID;";
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            if (con.State == ConnectionState.Closed) con.Open();
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product pr = new Product
                        {
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            SupplierID = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                            CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")), // From Suppliers
                            CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")), // From Categories
                            QuantityPerUnit = reader.IsDBNull(reader.GetOrdinal("QuantityPerUnit")) ? "" : reader.GetString(reader.GetOrdinal("QuantityPerUnit")),
                            UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                            UnitsInStock = reader.GetInt16(reader.GetOrdinal("UnitsInStock")),
                            UnitsOnOrder = reader.GetInt16(reader.GetOrdinal("UnitsOnOrder")),
                            ReorderLevel = reader.GetInt16(reader.GetOrdinal("ReorderLevel")),
                            Discontinued = reader.GetBoolean(reader.GetOrdinal("Discontinued"))
                        };

                        products.Add(pr);
                    }
                }
            }
        }
        return products;
    }
}