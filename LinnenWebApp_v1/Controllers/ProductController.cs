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
        return View();
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            // TODO: Insert the new product into the database
            return RedirectToAction(nameof(Index));
        }
        // TODO: Populate ViewBag for Suppliers and Categories if ModelState is invalid
        return View(product);
    }

    // GET: Products/Edit/5
    public IActionResult Edit(int id)
    {
        // TODO: Retrieve product by id
        // TODO: Populate ViewBag for Suppliers and Categories
        return View();
    }

    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            // TODO: Update the product in the database
            return RedirectToAction(nameof(Index));
        }
        // TODO: Populate ViewBag for Suppliers and Categories if ModelState is invalid
        return View(product);
    }

    // GET: Products/Delete/5
    public IActionResult Delete(int id)
    {
        // TODO: Retrieve product by id
        return View();
    }

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        // TODO: Remove the product from the database
        return RedirectToAction(nameof(Index));
    }

    public bool CreateProduct(Product product)
    {
        try
        {
            string query = "INSERT INTO dbo.Products (SupplierID, CategoryID, ProductName, QuantityPerUnit, UnitPrice, UnitsInStock, ReorderLevel, Discontinued) VALUES(@SupplierID, @CategoryID, @ProductName, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @ReorderLevel, @Discontinued); ";
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
                    cmd.Parameters.AddWithValue("@ReorderLevel", product.ReorderLevel);
                    cmd.Parameters.AddWithValue("@Discontinued", product.Discontinued);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.WriteLine(e);
        }
        return false;
    }

    public bool EditProduct(Product product)
    {
        try
        {
            string query = "UPDATE dbo.Products SET SupplierID = @SupplierID, CategoryID = @CategoryID, ProductName = @ProductName, QuantityPerUnit = @QuantityPerUnit, UnitPrice = @UnitPrice, UnitsInStock = @UnitsInStock, ReorderLevel = @ReorderLevel, Discontinued = @Discontinued WHERE ProductID = @ProductID;";
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
                    cmd.Parameters.AddWithValue("@ReorderLevel", product.ReorderLevel);
                    cmd.Parameters.AddWithValue("@Discontinued", product.Discontinued);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.WriteLine(e);
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