using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using LinnenWebApp_v1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace LinnenWebApp_v1.Controllers
{
    public class SupplierController : Controller
    {
        private List<Supplier> supplierList = new List<Supplier>();
        private readonly string connectionString;

        private Supplier currentSupplier = new(); 
        public SupplierController(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // GET: SupplierController
        public ActionResult Index()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Suppliers";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Supplier supplier = new Supplier
                            {
                                SupplierID = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                                CompanyName = reader["CompanyName"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("CompanyName")) : string.Empty,
                                ContactName = reader["ContactName"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("ContactName")) : string.Empty,
                                ContactTitle = reader["ContactTitle"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("ContactTitle")) : string.Empty,
                                Address = reader["Address"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Address")) : string.Empty,
                                City = reader["City"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("City")) : string.Empty,
                                Region = reader["Region"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Region")) : string.Empty,
                                PostalCode = reader["PostalCode"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("PostalCode")) : string.Empty,
                                Country = reader["Country"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Country")) : string.Empty,
                                Phone = reader["Phone"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Phone")) : string.Empty,
                                Fax = reader["Fax"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Fax")) : string.Empty,
                                HomePage = reader["HomePage"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("HomePage")) : string.Empty,
                            };
                            supplierList.Add(supplier);
                        }
                    }
                }
            }
            return View(supplierList);
        }

        // GET: SupplierController/Details/5
        public ActionResult Details(int id)
        {
            Supplier supplier = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Suppliers WHERE SupplierID = @SupplierID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SupplierID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            supplier = new Supplier
                            {
                                SupplierID = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                                CompanyName = reader["CompanyName"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("CompanyName")) : string.Empty,
                                ContactName = reader["ContactName"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("ContactName")) : string.Empty,
                                ContactTitle = reader["ContactTitle"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("ContactTitle")) : string.Empty,
                                Address = reader["Address"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Address")) : string.Empty,
                                City = reader["City"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("City")) : string.Empty,
                                Region = reader["Region"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Region")) : string.Empty,
                                PostalCode = reader["PostalCode"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("PostalCode")) : string.Empty,
                                Country = reader["Country"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Country")) : string.Empty,
                                Phone = reader["Phone"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Phone")) : string.Empty,
                                Fax = reader["Fax"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Fax")) : string.Empty,
                                HomePage = reader["HomePage"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("HomePage")) : string.Empty,
                            };
                        }
                    }
                }
            }
            return View(supplier);
        }

        // GET: SupplierController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupplierController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Supplier supplier = new Supplier
                {
                    CompanyName = collection["CompanyName"],
                    ContactName = collection["ContactName"],
                    ContactTitle = collection["ContactTitle"],
                    Address = collection["Address"],
                    City = collection["City"],
                    Region = collection["Region"],
                    PostalCode = collection["PostalCode"],
                    Country = collection["Country"],
                    Phone = collection["Phone"],
                    Fax = collection["Fax"],
                    HomePage = collection["HomePage"],
                };

                if (string.IsNullOrEmpty(supplier.CompanyName))
                {
                    ModelState.AddModelError("", "Company Name must not be empty !");
                    return View(supplier);
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "INSERT INTO Suppliers (CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax, HomePage) VALUES(@CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax, @HomePage);";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CompanyName", supplier.CompanyName);
                        cmd.Parameters.AddWithValue("@ContactName", supplier.ContactName);
                        cmd.Parameters.AddWithValue("@ContactTitle", supplier.ContactTitle);
                        cmd.Parameters.AddWithValue("@Address", supplier.Address);
                        cmd.Parameters.AddWithValue("@City", supplier.City);
                        cmd.Parameters.AddWithValue("@Region", supplier.Region);
                        cmd.Parameters.AddWithValue("@PostalCode", supplier.PostalCode);
                        cmd.Parameters.AddWithValue("@Country", supplier.Country);
                        cmd.Parameters.AddWithValue("@Phone", supplier.Phone);
                        cmd.Parameters.AddWithValue("@Fax", supplier.Fax);
                        cmd.Parameters.AddWithValue("@HomePage", supplier.HomePage);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                ModelState.AddModelError("", "An unexpected error happen. Try again later !");
                return View(supplier);
            }
            catch
            {
                return View();
            }
        }

        // GET: SupplierController/Edit/5
        public ActionResult Edit(int id)
        {
            Supplier supplier = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Suppliers WHERE SupplierID = @SupplierID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SupplierID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            supplier = new Supplier
                            {
                                SupplierID = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                                CompanyName = reader["CompanyName"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("CompanyName")) : string.Empty,
                                ContactName = reader["ContactName"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("ContactName")) : string.Empty,
                                ContactTitle = reader["ContactTitle"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("ContactTitle")) : string.Empty,
                                Address = reader["Address"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Address")) : string.Empty,
                                City = reader["City"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("City")) : string.Empty,
                                Region = reader["Region"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Region")) : string.Empty,
                                PostalCode = reader["PostalCode"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("PostalCode")) : string.Empty,
                                Country = reader["Country"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Country")) : string.Empty,
                                Phone = reader["Phone"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Phone")) : string.Empty,
                                Fax = reader["Fax"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Fax")) : string.Empty,
                                HomePage = reader["HomePage"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("HomePage")) : string.Empty,
                            };
                            
                        }
                        currentSupplier = supplier;
                        
                    }
                }
            }
            return View(supplier);
        }

        // POST: SupplierController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IFormCollection collection)
        {
            try
            {
                int id = -1;
                if (!int.TryParse(collection["SupplierID"], out id))
                {
                    ModelState.AddModelError("", "Company Name must not be empty !");
                    // possible null pointer
                    return View();
                }

                Supplier supplier = new Supplier
                {
                    CompanyName = string.IsNullOrEmpty(collection["CompanyName"]) ? currentSupplier.CompanyName : collection["CompanyName"],
                    ContactName = string.IsNullOrEmpty(collection["ContactName"]) ? currentSupplier.ContactName : collection["ContactName"],
                    ContactTitle = string.IsNullOrEmpty(collection["ContactTitle"]) ? currentSupplier.ContactTitle : collection["ContactTitle"],
                    Address = string.IsNullOrEmpty(collection["Address"]) ? currentSupplier.Address : collection["Address"],
                    City = string.IsNullOrEmpty(collection["City"]) ? currentSupplier.City : collection["City"],
                    Region = string.IsNullOrEmpty(collection["Region"]) ? currentSupplier.Region : collection["Region"],
                    PostalCode = string.IsNullOrEmpty(collection["PostalCode"]) ? currentSupplier.PostalCode : collection["PostalCode"],
                    Country = string.IsNullOrEmpty(collection["Country"]) ? currentSupplier.Country : collection["Country"],
                    Phone = string.IsNullOrEmpty(collection["Phone"]) ? currentSupplier.Phone : collection["Phone"],
                    Fax = string.IsNullOrEmpty(collection["Fax"]) ? currentSupplier.Fax : collection["Fax"],
                    HomePage = string.IsNullOrEmpty(collection["HomePage"]) ? currentSupplier.HomePage : collection["HomePage"],                 
                };

                Debug.WriteLine($"SupplierID : {id}");
                Debug.WriteLine($"CompanyName: {supplier.CompanyName}");
                Debug.WriteLine($"ContactName: {supplier.ContactName}");
                Debug.WriteLine($"ContactTitle: {supplier.ContactTitle}");
                Debug.WriteLine($"Address: {supplier.Address}");
                Debug.WriteLine($"City: {supplier.City}");
                Debug.WriteLine($"Region: {supplier.Region}");
                Debug.WriteLine($"PostalCode: {supplier.PostalCode}");
                Debug.WriteLine($"Country: {supplier.Country}");
                Debug.WriteLine($"Phone: {supplier.Phone}");
                Debug.WriteLine($"Fax: {supplier.Fax}");
                Debug.WriteLine($"HomePage: {supplier.HomePage}");

                if (string.IsNullOrEmpty(supplier.CompanyName))
                {
                    ModelState.AddModelError("", "Company Name must not be empty !");
                    return View(supplier);
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "UPDATE Suppliers SET CompanyName = @CompanyName, ContactName = @ContactName, ContactTitle = @ContactTitle, Address = @Address, City = @City, Region = @Region, PostalCode = @PostalCode, Country = @Country, Phone = @Phone, Fax = @Fax, HomePage = @HomePage WHERE SupplierID = @SupplierID";
                    
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@SupplierID", id);
                        cmd.Parameters.AddWithValue("@CompanyName", supplier.CompanyName);
                        cmd.Parameters.AddWithValue("@ContactName", supplier.ContactName);
                        cmd.Parameters.AddWithValue("@ContactTitle", supplier.ContactTitle);
                        cmd.Parameters.AddWithValue("@Address", supplier.Address);
                        cmd.Parameters.AddWithValue("@City", supplier.City);
                        cmd.Parameters.AddWithValue("@Region", supplier.Region);
                        cmd.Parameters.AddWithValue("@PostalCode", supplier.PostalCode);
                        cmd.Parameters.AddWithValue("@Country", supplier.Country);
                        cmd.Parameters.AddWithValue("@Phone", supplier.Phone);
                        cmd.Parameters.AddWithValue("@Fax", supplier.Fax);
                        cmd.Parameters.AddWithValue("@HomePage", supplier.HomePage);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                ModelState.AddModelError("", "An unexpected error happen. Try again later !");
                return View(supplier);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return View();
            }
        }

        // GET: SupplierController/Delete/5
        public ActionResult Delete(int id)
        {
            Supplier supplier = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Suppliers WHERE SupplierID = @SupplierID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SupplierID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            supplier = new Supplier
                            {
                                SupplierID = reader.GetInt32(reader.GetOrdinal("SupplierID")),
                                CompanyName = reader["CompanyName"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("CompanyName")) : string.Empty,
                                ContactName = reader["ContactName"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("ContactName")) : string.Empty,
                                ContactTitle = reader["ContactTitle"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("ContactTitle")) : string.Empty,
                                Address = reader["Address"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Address")) : string.Empty,
                                City = reader["City"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("City")) : string.Empty,
                                Region = reader["Region"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Region")) : string.Empty,
                                PostalCode = reader["PostalCode"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("PostalCode")) : string.Empty,
                                Country = reader["Country"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Country")) : string.Empty,
                                Phone = reader["Phone"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Phone")) : string.Empty,
                                Fax = reader["Fax"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Fax")) : string.Empty,
                                HomePage = reader["HomePage"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("HomePage")) : string.Empty,
                            };
                        }
                    }
                }
            }
            return View(supplier);
        }

        // POST: SupplierController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(IFormCollection collection)
        {
            try
            {
                int id = -1;
                if (!int.TryParse(collection["SupplierID"], out id))
                {
                    ModelState.AddModelError("", "Company Name must not be empty !");
                    // possible null pointer
                    return View();
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "DELETE FROM Suppliers WHERE SupplierID = @SupplierID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@SupplierID", id);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
