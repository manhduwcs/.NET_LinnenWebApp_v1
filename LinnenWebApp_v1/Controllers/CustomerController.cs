using System.Reflection.Metadata.Ecma335;
using System.Data;
using LinnenWebApp_v1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace LinnenWebApp_v1.Controllers
{
    public class CustomerController : Controller
    {
        private List<Customer> customers = new List<Customer>();
        private readonly string connectionString = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";

        // GET: CustomerController
        public ActionResult Index()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Customers";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer
                            {
                                CustomerID = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                CompanyName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                ContactName = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                ContactTitle = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                Address = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                City = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                Region = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                PostalCode = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                Country = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                Phone = reader.IsDBNull(9) ? "" : reader.GetString(9),
                                Fax = reader.IsDBNull(10) ? "" : reader.GetString(10)
                            };

                            customers.Add(customer);
                        }
                    }
                }
            }
            return View(customers);
        }

        // GET: CustomerController/Details/5
        public ActionResult Details(string id)
        {
            Customer customer = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customer = new Customer
                            {
                                CustomerID = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                CompanyName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                ContactName = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                ContactTitle = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                Address = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                City = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                Region = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                PostalCode = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                Country = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                Phone = reader.IsDBNull(9) ? "" : reader.GetString(9),
                                Fax = reader.IsDBNull(10) ? "" : reader.GetString(10)
                            };
                        }
                    }
                }
            }
            return View(customer);
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Customer customer = new Customer
                {
                    CustomerID = collection["CustomerID"],
                    CompanyName = collection["CompanyName"],
                    ContactName = collection["ContactName"],
                    ContactTitle = collection["ContactTitle"],
                    Address = collection["Address"],
                    City = collection["City"],
                    Region = collection["Region"],
                    PostalCode = collection["PostalCode"],
                    Country = collection["Country"],
                    Phone = collection["Phone"],
                    Fax = collection["Fax"]
                };

                if (string.IsNullOrEmpty(customer.CustomerID) ||
                    string.IsNullOrEmpty(customer.CompanyName) ||
                    string.IsNullOrEmpty(customer.ContactName))
                {
                    ModelState.AddModelError(string.Empty, "Please fill in all required fields.");
                    return View(customer);
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "INSERT INTO Customers (CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax) " +
                                   "VALUES (@CustomerID, @CompanyName, @ContactName, @ContactTitle, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Fax)"; using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                        cmd.Parameters.AddWithValue("@CompanyName", customer.CompanyName);
                        cmd.Parameters.AddWithValue("@ContactName", customer.ContactName);
                        cmd.Parameters.AddWithValue("@ContactTitle", customer.ContactTitle);
                        cmd.Parameters.AddWithValue("@Address", customer.Address);
                        cmd.Parameters.AddWithValue("@City", customer.City);
                        cmd.Parameters.AddWithValue("@Region", customer.Region);
                        cmd.Parameters.AddWithValue("@PostalCode", customer.PostalCode);
                        cmd.Parameters.AddWithValue("@Country", customer.Country);
                        cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                        cmd.Parameters.AddWithValue("@Fax", customer.Fax);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return RedirectToAction(nameof(Index));
                        }  
                    }
                }
                ModelState.AddModelError("", "An unexpected error occured when trying to add new Customer.");
                return View(customer);
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(string id)
        {
            Customer customer = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customer = new Customer
                            {
                                CustomerID = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                CompanyName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                ContactName = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                ContactTitle = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                Address = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                City = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                Region = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                PostalCode = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                Country = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                Phone = reader.IsDBNull(9) ? "" : reader.GetString(9),
                                Fax = reader.IsDBNull(10) ? "" : reader.GetString(10)
                            };
                        }
                    }
                }
            }
            return View(customer);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IFormCollection collection)
        {
            try
            {
                Customer customer = new Customer
                {
                    CustomerID = collection["CustomerID"],
                    CompanyName = collection["CompanyName"],
                    ContactName = collection["ContactName"],
                    ContactTitle = collection["ContactTitle"],
                    Address = collection["Address"],
                    City = collection["City"],
                    Region = collection["Region"],
                    PostalCode = collection["PostalCode"],
                    Country = collection["Country"],
                    Phone = collection["Phone"],
                    Fax = collection["Fax"]
                };

                if (string.IsNullOrEmpty(customer.CustomerID) ||
                    string.IsNullOrEmpty(customer.CompanyName) ||
                    string.IsNullOrEmpty(customer.ContactName))
                {
                    ModelState.AddModelError(string.Empty, "Please fill in all required fields.");
                    return View(customer);
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "UPDATE Customers SET CompanyName = @CompanyName, ContactName = @ContactName, ContactTitle = @ContactTitle, Address = @Address, City = @City, Region = @Region, PostalCode = @PostalCode, Country = @Country, Phone = @Phone, Fax = @Fax WHERE CustomerID = @CustomerID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                        cmd.Parameters.AddWithValue("@CompanyName", customer.CompanyName);
                        cmd.Parameters.AddWithValue("@ContactName", customer.ContactName);
                        cmd.Parameters.AddWithValue("@ContactTitle", customer.ContactTitle);
                        cmd.Parameters.AddWithValue("@Address", customer.Address);
                        cmd.Parameters.AddWithValue("@City", customer.City);
                        cmd.Parameters.AddWithValue("@Region", customer.Region);
                        cmd.Parameters.AddWithValue("@PostalCode", customer.PostalCode);
                        cmd.Parameters.AddWithValue("@Country", customer.Country);
                        cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                        cmd.Parameters.AddWithValue("@Fax", customer.Fax);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                ModelState.AddModelError("", "An unexpected error occured when trying to update Customer.");
                return View(customer);
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(string id)
        {
            Customer customer = new();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                string query = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customer = new Customer
                            {
                                CustomerID = reader.IsDBNull(0) ? "" : reader.GetString(0),
                                CompanyName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                ContactName = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                ContactTitle = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                Address = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                City = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                Region = reader.IsDBNull(6) ? "" : reader.GetString(6),
                                PostalCode = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                Country = reader.IsDBNull(8) ? "" : reader.GetString(8),
                                Phone = reader.IsDBNull(9) ? "" : reader.GetString(9),
                                Fax = reader.IsDBNull(10) ? "" : reader.GetString(10)
                            };
                        }
                    }
                }
            }
            return View(customer);
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(IFormCollection collection)
        {
            try
            {
                string id = collection["CustomerID"];
                if (string.IsNullOrEmpty(id))
                {
                    ModelState.AddModelError("", "Error in CustomerID when trying to delete this customer !");
                    return View();
                }
                
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State == ConnectionState.Closed) con.Open();
                    string query = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", id);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                ModelState.AddModelError("", "Error when trying to delete this customer !");
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
